using System.Net;
using System.Text;

namespace PodatkovniSloj.Tests;

/// <summary>Stands in for the network: records every request and answers with whatever the test chose.</summary>
internal sealed class FakeHttpMessageHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> respond)
    : HttpMessageHandler
{
    public List<Uri> RequestedUris { get; } = [];

    public static FakeHttpMessageHandler Returning(string json, HttpStatusCode status = HttpStatusCode.OK) =>
        new((_, _) => Task.FromResult(new HttpResponseMessage(status)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json"),
        }));

    public static FakeHttpMessageHandler ReturningFixture(string fileName) =>
        Returning(Fixtures.Read(fileName));

    public static FakeHttpMessageHandler Throwing(Exception exception) =>
        new((_, _) => Task.FromException<HttpResponseMessage>(exception));

    /// <summary>Never answers; only the HttpClient timeout or a cancellation ends the request.</summary>
    public static FakeHttpMessageHandler Hanging() =>
        new(async (_, cancellationToken) =>
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            throw new InvalidOperationException("Unreachable: the delay only ends by cancellation.");
        });

    /// <summary>A client set up the way Program sets up the real one, sending through this handler.</summary>
    public HttpClient CreateClient(TimeSpan? timeout = null) =>
        new(this, disposeHandler: false)
        {
            BaseAddress = ApiService.DefaultBaseAddress,
            Timeout = timeout ?? ApiService.DefaultTimeout,
        };

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        RequestedUris.Add(request.RequestUri!);
        return respond(request, cancellationToken);
    }
}
