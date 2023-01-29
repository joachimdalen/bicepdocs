using System.Buffers.Text;
using System.Net.Http.Headers;
using System.Text;
using LandingZones.Tools.BicepDocs.Destination.Confluence.Abstractions;

namespace LandingZones.Tools.BicepDocs.Destination.Confluence.Services;

public class ConfluenceServiceFactory : IConfluenceServiceFactory
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ConfluenceServiceFactory(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public ConfluenceService GetConfluenceClient(string instanceName, string email, string token)
    {
        var byteArray = Encoding.ASCII.GetBytes($"{email}:{token}");
        var encodeString = Convert.ToBase64String(byteArray);
        var client = _httpClientFactory.CreateClient();
        
        client.BaseAddress = new Uri(Endpoints.RootFormat(instanceName));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodeString);
        return new ConfluenceService(client);
    }
}