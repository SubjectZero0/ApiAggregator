using Application.Services.Metrics;
using FluentAssertions;

namespace ApplicationTests.Services.Metrics;

[Collection("MetricsStaticState")]
public class MetricsServiceTests : IDisposable
{
	private readonly MetricsService _sut = new();

	public MetricsServiceTests()
		=> OutgoingApiCallMetricsHandler.Records.Clear();

	public void Dispose()
		=> OutgoingApiCallMetricsHandler.Records.Clear();

	[Fact]
	public void GetOutgoingApiCallMetrics_WithNoRecords_ReturnsEmpty()
	{
		//Act
		var result = _sut.GetOutgoingApiCallMetrics();

		//Assert
		result.Should().BeEmpty();
	}

	[Fact]
	public void GetOutgoingApiCallMetrics_GroupsByApi()
	{
		//Arrange
		OutgoingApiCallMetricsHandler.Records.Add(("api.example.com", 100));
		OutgoingApiCallMetricsHandler.Records.Add(("api.example.com", 300));
		OutgoingApiCallMetricsHandler.Records.Add(("api.other.com", 600));

		//Act
		var result = _sut.GetOutgoingApiCallMetrics().ToList();

		//Assert
		result.Should().HaveCount(2);
		result.Should().Contain(m => m.Api == "api.example.com");
		result.Should().Contain(m => m.Api == "api.other.com");
	}

	[Fact]
	public void GetOutgoingApiCallMetrics_CalculatesTotalRequests()
	{
		//Arrange
		OutgoingApiCallMetricsHandler.Records.Add(("api.example.com", 100));
		OutgoingApiCallMetricsHandler.Records.Add(("api.example.com", 200));
		OutgoingApiCallMetricsHandler.Records.Add(("api.example.com", 600));

		//Act
		var result = _sut.GetOutgoingApiCallMetrics().Single();

		//Assert
		result.TotalRequests.Should().Be(3);
	}

	[Fact]
	public void GetOutgoingApiCallMetrics_CalculatesAverageResponseTime()
	{
		//Arrange
		OutgoingApiCallMetricsHandler.Records.Add(("api.example.com", 100));
		OutgoingApiCallMetricsHandler.Records.Add(("api.example.com", 300));

		//Act
		var result = _sut.GetOutgoingApiCallMetrics().Single();

		//Assert
		result.AverageResponseTimeMs.Should().Be(200m);
	}

	[Fact]
	public void GetOutgoingApiCallMetrics_CategorizesPerformanceBuckets()
	{
		//Arrange
		OutgoingApiCallMetricsHandler.Records.Add(("api.example.com", 100));  // fast
		OutgoingApiCallMetricsHandler.Records.Add(("api.example.com", 199));  // fast
		OutgoingApiCallMetricsHandler.Records.Add(("api.example.com", 200));  // average
		OutgoingApiCallMetricsHandler.Records.Add(("api.example.com", 500));  // average
		OutgoingApiCallMetricsHandler.Records.Add(("api.example.com", 501));  // slow
		OutgoingApiCallMetricsHandler.Records.Add(("api.example.com", 1000)); // slow

		//Act
		var result = _sut.GetOutgoingApiCallMetrics().Single();

		//Assert
		result.PerformanceBuckets.FastCount.Should().Be(2);
		result.PerformanceBuckets.Average.Should().Be(2);
		result.PerformanceBuckets.Slow.Should().Be(2);
	}
}