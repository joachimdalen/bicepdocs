namespace LandingZones.Tools.BicepDocs.Destination.Confluence;

public static class Endpoints
{
    public static string RootFormat(string instanceName) => $"https://{instanceName}.atlassian.net";
    public static string GetPageById(int pageId) => $"/wiki/api/v2/pages/{pageId}";
}