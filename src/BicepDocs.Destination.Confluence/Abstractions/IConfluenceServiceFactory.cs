using LandingZones.Tools.BicepDocs.Destination.Confluence.Services;

namespace LandingZones.Tools.BicepDocs.Destination.Confluence.Abstractions;

public interface IConfluenceServiceFactory
{
    ConfluenceService GetConfluenceClient(string instanceName, string email, string token);
}