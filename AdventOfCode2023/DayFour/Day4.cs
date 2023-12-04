using System.Text.RegularExpressions;
using AdventOfCode2023.Core;

namespace AdventOfCode2023.DayFour;

public partial class Day4
{
    [GeneratedRegex(@"\d+")]
    private static partial Regex MatchDigits();

    /// <summary>
    /// 
    /// </summary>
    private const int NumberCount = 35;
    
    public static double SolveFirst(IEnumerable<string>? input = null)
    {
        if (input is null)
        {
            var inputFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "DayFour/First.txt"));
            input = inputFile.GetInputStrings();    
        }

        double answer = 0;
        foreach (var line in input)
        {
            var splitOffGame = line.Split(":");
            var numbers = splitOffGame[1].Split("|");
            var winningNumbers = MatchDigits().ExtractNumbers(numbers[0]).ToHashSet();
            var haveNumbers = MatchDigits().ExtractNumbers(numbers[1]).ToHashSet();

            var intersections = winningNumbers.Intersect(haveNumbers);
            answer += Math.Floor(1 * Math.Pow(2, intersections.Count() - 1));
        }
        
        return answer;
    }
    
    public static double SolveSecond(IEnumerable<string>? input = null)
    {
        if (input is null)
        {
            var inputFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "DayFour/First.txt"));
            input = inputFile.GetInputStrings();    
        }

        double answer = 0;
        var cardCounts = new Dictionary<int, int>();
        foreach (var line in input)
        {
            var splitOffGame = line.Split(":");
            var id = MatchDigits().ExtractNumber(splitOffGame[0]);
            var numbers = splitOffGame[1].Split("|");
            var winningNumbers = MatchDigits().ExtractNumbers(numbers[0]).ToHashSet();
            var haveNumbers = MatchDigits().ExtractNumbers(numbers[1]).ToHashSet();

            var intersections = winningNumbers.Intersect(haveNumbers).Count();
            if (!cardCounts.TryAdd(id, 1))
            {
                cardCounts[id] += 1;
            }

            
            var cardCountToAdd = cardCounts.GetValueOrDefault(id, 1);
            for (var i = 1; i < intersections + 1; i++)
            {
                cardCounts.TryAdd(i + id, 0);
                cardCounts[id + i] += cardCountToAdd;
            }
        }
        
        return cardCounts.Values.Sum();
    }
}