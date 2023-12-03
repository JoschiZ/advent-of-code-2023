using System.Text.RegularExpressions;

namespace AdventOfCode2023.Core;

public static class RegexExtensions
{
    /// <summary>
    /// Takes in a regex that should match a valid number and parses it to int
    /// </summary>
    /// <param name="input"></param>
    /// <param name="regex"></param>
    /// <returns></returns>
    public static int ExtractNumber(this Regex regex, string input)
    {
        var match = regex.Match(input);
        return int.Parse(match.ToString()); // We wanna fix this if there are problems in the input so no tryparse handling now
    }

    /// <summary>
    /// Matches one or more numbers with a provided regex and parses it to int
    /// </summary>
    /// <param name="input"></param>
    /// <param name="regex"></param>
    /// <returns></returns>
    public static IEnumerable<int> ExtractNumbers(this Regex regex, string input)
    {
        var matches = regex.Matches(input);
        return matches.Select(match => int.Parse(match.ToString()));
    }
}