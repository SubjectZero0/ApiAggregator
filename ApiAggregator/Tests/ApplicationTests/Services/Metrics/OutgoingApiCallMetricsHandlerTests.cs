using Application.Services.Metrics;
using FluentAssertions;
using System.Net;

namespace ApplicationTests.Services.Metrics;

[Collection("MetricsStaticState")]
public class OutgoingApiCallMetricsHandlerTests : IDisposable
{
	public OutgoingApiCallMetricsHandlerTests() => OutgoingApiCallMetricsHandler.Records.Clear();

	public void Dispose() => OutgoingApiCallMetricsHandler.Records.Clear();

	[Fact]
	public async Task SendAsync_RecordsApiHostAndDuration()
	{
		//Arrange
		var fakeInner = new FakeHttpMessageHandler(HttpStatusCode.OK);
		var handler = new OutgoingApiCallMetricsHandler { InnerHandler = fakeInner };
		var client = new HttpClient(handler);

		//Act
		await client.GetAsync("http://api.testhost.com/data");

		//Assert
		OutgoingApiCallMetricsHandler.Records.Should().ContainSingle();

		var (api, duration) = OutgoingApiCallMetricsHandler.Records.Single();

		api.Should().Be("api.testhost.com");
		duration.Should().BeGreaterThanOrEqualTo(0);
	}

	[Fact]
	public async Task SendAsync_WhenRequestUriHasNoHost_ThrowsException()
	{
		//Arrange
		var fakeInner = new FakeHttpMessageHandler(HttpStatusCode.OK);
		var handler = new OutgoingApiCallMetricsHandler { InnerHandler = fakeInner };

		var request = new HttpRequestMessage { RequestUri = null };
		var invoker = new HttpMessageInvoker(handler);

		//Act
		var act = () => invoker.SendAsync(request, CancellationToken.None);

		//Assert
		await act.Should().ThrowAsync<Exception>();
	}

	private sealed class FakeHttpMessageHandler(HttpStatusCode statusCode) : HttpMessageHandler
	{
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
			=> Task.FromResult(new HttpResponseMessage(statusCode));
	}
}