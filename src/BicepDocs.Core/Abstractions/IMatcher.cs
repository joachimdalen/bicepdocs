namespace LandingZones.Tools.BicepDocs.Core.Abstractions;

public interface IMatcher
{
    void AddIncludePatterns(params IEnumerable<string>[] includePatternsGroups);
    void AddExcludePatterns(params IEnumerable<string>[] excludePatternsGroups);
    IEnumerable<string> GetResultsInFullPath(string directoryPath);
}