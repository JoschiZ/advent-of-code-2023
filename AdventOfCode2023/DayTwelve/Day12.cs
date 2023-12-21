using System.Text.RegularExpressions;
using AdventOfCode2023.Core;

namespace AdventOfCode2023.DayTwelve;

public partial class Day12
{
    public const char Broken = '#';
    public const char Operational = '.';
    public const char Unknown = '?';

    [GeneratedRegex(@"\.+")]
    private static partial Regex MatchOperational();

    [GeneratedRegex("#+")]
    private static partial Regex MatchBroken();

    [GeneratedRegex(@"\d+")]
    private static partial Regex MatchDigits();

    public static int SolveFirst()
    {
        var day12 = new Day12();
        
        var inputFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "DayTwelve/First.txt"));
        
        var solution = 0;
        foreach (var line in inputFile.GetInputStrings())
        {
            solution += day12.SolveInputString(line);
        }

        return solution;
    }

    public int SolveInputString(string input)
    {
        var shouldArray = MatchDigits().ExtractNumbers(input);
        var splitString = input.Split(' ');
        return GetValidCombinations(splitString[0], shouldArray).Count();
    }
    

    public bool IsValidArrangement(string input, IEnumerable<int> shouldArrangement)
    {
        if (input.Contains('?'))
        {
            return false;
        }
        
        var condensedInput = MatchOperational().Replace(input, ".");
        var arrangement = MatchBroken().Matches(condensedInput).Select(match => match.Length);

        return arrangement.SequenceEqual(shouldArrangement);
    }

    public IEnumerable<string> GetValidCombinations(string input, IEnumerable<int> shouldArrangement)
    {
        // this is a quick and hacky brute force solution with no regards to performance.
        // It will not work for part 2
        
        var combinations = GenerateCombinations(input, 0);

        // Enumerate this just once
        shouldArrangement = shouldArrangement.ToArray();
        foreach (var combination in combinations)
        {
            if (IsValidArrangement(combination, shouldArrangement))
            {
                yield return combination;
            }
        }
    }
    
    public IEnumerable<string> GenerateCombinations(string str, int index)
    {
        if (index == str.Length)
        {
            yield return str;
        }
        else
        {
            if (str[index] == '?')
            {
                // Replace '?' by '#'
                foreach (var s in GenerateCombinations(str.Remove(index, 1).Insert(index, "#"), index + 1))
                {
                    yield return s;
                }

                // Replace '?' by '.'
                foreach (var s in GenerateCombinations(str.Remove(index, 1).Insert(index, "."), index + 1))
                {
                    yield return s;
                }
            }
            else
            {
                foreach (var s in GenerateCombinations(str, index + 1))
                {
                    yield return s;
                }
            }
        }
    }

    private string ReplaceFirst(string text, char search, char replace)
    {
        var pos = text.IndexOf(search, StringComparison.Ordinal);
        return pos < 0 ? 
            text : 
            string.Concat(text.AsSpan(0, pos), replace.ToString(), text.AsSpan(pos + 1));
    }
}