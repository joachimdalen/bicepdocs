using LandingZones.Tools.BicepDocs.Core.Abstractions;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;

namespace LandingZones.Tools.BicepDocs.Core.Services;

public class MatcherWrapper : IMatcher
{
    private readonly Matcher _matcher;

    public MatcherWrapper()
    {
        _matcher = new Matcher();
    }

    public void AddIncludePatterns(params IEnumerable<string>[] includePatternsGroups)
    {
        foreach (var group in includePatternsGroups)
        {
            foreach (var pattern in group)
            {
                _matcher.AddInclude(pattern);
            }
        }
    }

    public void AddExcludePatterns(params IEnumerable<string>[] excludePatternsGroups)
    {
        foreach (var group in excludePatternsGroups)
        {
            foreach (var pattern in group)
            {
                _matcher.AddExclude(pattern);
            }
        }
    }

    public IEnumerable<string> GetResultsInFullPath(string directoryPath)
    {
        var matches = _matcher.Execute(new DirectoryInfoWrapper(new DirectoryInfo(directoryPath))).Files;
        var result = matches.Select(match => Path.GetFullPath(Path.Combine(directoryPath, match.Path))).ToArray();
        return result;
    }
}