using Microsoft.AspNetCore.Mvc.Testing;

namespace VacationRental.Api.MyTests;

[CollectionDefinition("Integration")]
public sealed class IntegrationFixture : IDisposable, ICollectionFixture<IntegrationFixture>
{
    public HttpClient Client { get; }

    public IntegrationFixture()
    {
        var app = new WebApplicationFactory<Program>();

        Client = app.CreateClient();
    }

    public void Dispose()
    {
        Client.Dispose();
    }
}
