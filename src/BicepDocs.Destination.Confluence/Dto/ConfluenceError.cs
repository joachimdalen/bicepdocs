namespace LandingZones.Tools.BicepDocs.Destination.Confluence.Dto;

public record ConfluenceError(int StatusCode, string Message, ConfluenceErrorData Data);

public record ConfluenceErrorData(bool? Authorized, bool? Valid, object[]? Errors, bool? Successful);