var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ApiAggregator>("apiaggregator");

builder.Build().Run();
