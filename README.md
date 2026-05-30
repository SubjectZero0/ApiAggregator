# ApiAggregator

A .NET 8 dashboard API that aggregates real-time data from three independent external sources — current weather, top news headlines, and daily financial market summaries — into a single endpoint response. Built with Clean Architecture and .NET Aspire.

---

## What It Does

**`GET /api/dashboard`** — Returns weather, news, and market data in one call, fetched in parallel from:

- [OpenWeatherMap](https://openweathermap.org/api) — current weather by city/country
- [NewsAPI](https://newsapi.org/) — top headlines by category
- [Massive](https://massive.com) — daily stock market summary

**`GET /api/dashboard/metrics`** — Returns timing statistics for every outgoing HTTP call the application has made, grouped by API host with fast/average/slow bucketing.

---

## Architecture

```
┌─────────────────────────────────────────────┐
│               ApiAggregator                 │  ASP.NET Core Web API
│         Controllers · Middleware            │  Entry point, routing, exception handling
└───────────────────┬─────────────────────────┘
                    │
┌───────────────────▼─────────────────────────┐
│               Application                   │  Business logic layer
│     Services · Interfaces · Models          │  Defines contracts, orchestrates services
└───────────────────┬─────────────────────────┘
          ┌─────────┴──────────┐
          │                    │
┌─────────▼──────────┐  ┌──────▼──────────────┐
│      Gateway        │  │   Infrastructure    │
│  Clients · Providers│  │  Caching · Decorators│
│  Mappers · Sorter   │  │  HybridCacheProvider│
└─────────┬──────────┘  └─────────────────────┘
          │
┌─────────▼──────────────────────────────────┐
│          External APIs                      │
│    OpenWeatherMap · NewsAPI · Massive        │
└─────────────────────────────────────────────┘
```

### Layer Responsibilities

| Layer              | Responsibility                                                   |
| ------------------ | ---------------------------------------------------------------- |
| **ApiAggregator**  | HTTP routing, model validation, global exception handling        |
| **Application**    | Business logic, service interfaces, request/response models      |
| **Gateway**        | External API clients, data providers, mappers, sorting utilities |
| **Infrastructure** | Caching decorators, HybridCache provider, cache options factory  |
| **Common**         | Shared constants (cache names) with no external dependencies     |

Dependencies only flow downward. `Application` defines interfaces; `Gateway` and `Infrastructure` implement them. Nothing in `Application` knows about caching or HTTP clients.

---

## Why .NET Aspire

This project uses [.NET Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview) as its orchestration layer. It was chosen deliberately for four reasons:

### 1. Orchestration and Future Scalability

The `AppHost` project is the single place where services, dependencies, and configuration are wired together. Adding a Redis instance, a database, or a second API service in the future requires adding a reference in `AppHost.cs` — not restructuring the application. Running multiple instances of the API is equally trivial.

### 2. Built-in Developer Dashboard

Aspire ships a dashboard that provides structured logs, distributed traces, and runtime metrics for every service in the application — with zero additional configuration. During development this replaces the need to set up Grafana, Jaeger, or a separate logging UI. Every outgoing HTTP call, startup event, and error is visible immediately.

### 3. Standard Resilience and Health Checks via ServiceDefaults

The `ServiceDefaults` project configures retry policies, circuit breakers, and health check endpoints for every registered HTTP client using a single call. This is applied uniformly across all four external API clients without any per-client boilerplate. Health check endpoints (`/health`, `/alive`) are also wired up automatically.

### 4. Ready for Distributed Caching

The application currently uses `HybridCache` backed by in-memory storage. Switching to Redis as the distributed L2 cache requires two lines in `AppHost.cs` and one service reference in `Program.cs` — the caching abstraction (`ICachingProvider`) and all decorator logic remain completely unchanged.

---

## Key Design Decisions

### What Was Done Intentionally

**Decorator pattern for caching**
Caching is applied as a decorator over each provider (`MarketCacheDecorator`, `NewsCacheDecorator`, `WeatherCacheDecorator`) registered via [Scrutor](https://github.com/khellang/Scrutor). The business logic in `Gateway` providers has no awareness of caching. Adding, removing, or changing cache behaviour requires no changes to the providers themselves.

**`Task.WhenAll` for parallel upstream calls**
`DashBoardService` fires all three upstream API calls simultaneously rather than sequentially. Since the three data sources are fully independent, the response time of the dashboard endpoint is bounded by the slowest single call rather than the sum of all three.

**`DelegatingHandler` for metrics**
`OutgoingApiCallMetricsHandler` is a delegating HTTP handler — it sits in the HTTP pipeline and intercepts every outgoing request transparently. This means timing data is captured for all four external clients from a single class, with no changes to the clients themselves.

**`HybridCache` with per-type expiry**
Each data type has a cache duration that reflects its real-world staleness:

- Weather: 8 hours (changes slowly)
- News: 1 hour (refreshes frequently)
- Finance (query results): 2 hours
- Daily market snapshot: expires at midnight UTC (the snapshot is for a fixed calendar date and never changes — expiring at day boundary avoids serving stale data into the next trading day)

**Options pattern with startup validation**
All external API configuration and cache settings use `IOptions<T>` with `[Required]` attributes validated at startup. If a required API key is missing, the application refuses to start rather than failing silently at runtime.

---

### Pain Points and Trade-offs

**Static metrics collection**
`OutgoingApiCallMetricsHandler.Records` is a `static ConcurrentBag<T>`. It is thread-safe and works correctly, but it grows unbounded over the lifetime of the application — a slow memory leak in a long-running production process. This was accepted as a deliberate simplification for a demonstration context. To solve this, I would implement a periodic eviction process.

**Generic exception handling in HTTP clients**
All four clients catch `Exception` broadly and return `null` on any failure, with the error logged. This keeps the application resilient to upstream outages — a partial dashboard response is returned rather than a 500. The trade-off is that different failure modes (network timeout, auth failure, rate limit) are treated identically, which reduces observability. In a production system these would be separated so rate-limit errors and auth failures could be surfaced and alerted on distinctly.

**No `CancellationToken` propagation**
The entire async call chain from controller to HTTP client does not accept or propagate `CancellationToken`. This means a cancelled client request (browser tab closed, load balancer timeout) cannot cancel the in-flight upstream API calls. This was the first thing identified as a gap and would be the first change made before production deployment.

---

## Running the Project

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download) — select .NET 8 (LTS)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) — required by Aspire for container orchestration
- .NET Aspire workload — if you haven't used Aspire before, install it with:

```bash
dotnet workload install aspire
```

To verify it's installed:

```bash
dotnet workload list
```

You should see `aspire` in the output.

### API Keys

All API keys are already included in `ApiAggregator/appsettings.Development.json` — no configuration needed. The keys are free-tier and ready to use out of the box.

The application integrates with:

- [OpenWeatherMap](https://openweathermap.org/api) — current weather and geocoding
- [NewsAPI](https://newsapi.org/) — top headlines
- [Massive](https://massive.com) — daily stock market data

### Running

```bash
# From the solution root
dotnet run --project ApiAggregator.AppHost
```

Aspire will print two URLs in the terminal:

- **Dashboard** — open this to see logs, traces, and metrics for all services
- **ApiAggregator** — this is the base URL for the API

### Calling the API

```
GET {base-url}/api/dashboard?cityName=London&countryCode=GB&newsCategory=Technology&newsPageSize=5&numberOfMarkets=10&marketFieldOrdering=Descending&marketFieldToSort=HighestPrice
```

```
GET {base-url}/api/dashboard/metrics
```

Swagger UI is available at `{base-url}/swagger` when running in Development mode.

---

## Running Tests

```bash
# From the solution root
dotnet test
```

Three test projects cover the non-orchestrator logic across all layers:

| Project               | Covers                                                |
| --------------------- | ----------------------------------------------------- |
| `ApplicationTests`    | Service classes, metrics handler, metrics aggregation |
| `GatewayTests`        | Providers, mappers, market sorter                     |
| `InfrastructureTests` | Cache decorators, cache options factory               |

Tests use **xUnit**, **Moq**, and **FluentAssertions**. Internal types are exposed to test projects via `InternalsVisibleTo`.

---

## Project Structure

```
ApiAggregator/
├── ApiAggregator/          # Web API entry point
├── ApiAggregator.AppHost/  # .NET Aspire orchestration host
├── ApiAggregator.ServiceDefaults/ # Shared resilience and health check config
├── Application/            # Business logic and interfaces
├── Gateway/                # External API clients and providers
├── Infrastructure/         # Caching decorators and HybridCache provider
├── Common/                 # Shared constants
└── Tests/
    ├── ApplicationTests/
    ├── GatewayTests/
    └── InfrastructureTests/
```
