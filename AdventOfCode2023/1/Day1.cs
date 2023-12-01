using System.Globalization;
using System.Text.RegularExpressions;
using AdventOfCode2023.Core;

namespace AdventOfCode2023._1;

public partial class Day1
{
    [GeneratedRegex("\\d")]
    private static partial Regex GetNumbers();
    
    
    public static int SolveDay1First()
    {
        var inputFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "1/First.txt"));
        

        var overallCount = 0;
        foreach (var line in inputFile.GetInputStrings())
        {
            overallCount += ParseString(line);
        }

        return overallCount;
    }
    
    public static int SolveDay1Second()
    {
        var inputFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "1/Second.txt"));


        return inputFile
            .GetInputStrings()
            .Select(TransformString)
            .Select(ParseString)
            .Sum();
    }
    
    public static int ParseString(string input)
    {
        var matches = GetNumbers().Matches(input);

        var resultAsString = matches.Count switch
        {
            0 => throw new ArgumentOutOfRangeException(),
            1 => matches[0].ToString() + matches[0],
            _ => matches.First().ToString() + matches.Last()
        };

        if (int.TryParse(resultAsString, out var result))
        {
            return result;
        }

        throw new InvalidCastException();
    }

    private static readonly Dictionary<string, string> NumberWords = new()
    {
        { "one", "1" },
        { "two", "2" },
        { "three", "3" },
        { "four", "4" },
        { "five", "5" },
        { "six", "6" },
        { "seven", "7" },
        { "eight", "8" },
        { "nine", "9" },
    };

    [GeneratedRegex("\\d | one | two | three | four | five | six | seven | eight | nine", RegexOptions.IgnorePatternWhitespace)]
    private static partial Regex GetNumbersWithNumberWords();
    public static string TransformString(string input)
    {
        var matches = new List<Match> { GetNumbersWithNumberWords().Match(input) };

        while (matches.Last().Success)
        {
            matches.Add(GetNumbersWithNumberWords().Match(input, matches.Last().Index + 1));
        }


        var transformedString = "";
        foreach (var match in matches)
        {
            if (NumberWords.TryGetValue(match.ToString(), out var resultNumber))
            {
                transformedString += resultNumber;
                continue;
            }

            transformedString += match.ToString();
        }

        return transformedString;
    }
} 
