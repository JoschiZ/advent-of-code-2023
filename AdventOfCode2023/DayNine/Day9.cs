using System.Text.RegularExpressions;
using AdventOfCode2023.Core;

namespace AdventOfCode2023.DayNine;

public static partial class Day9
{
    [GeneratedRegex(@"-?\d+")]
    public static partial Regex MatchNumbers();

    public static int SolveFirst()
    {
        var inputFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "DayNine/First.txt"));

        var answer = 0;
        foreach (var inputString in inputFile.GetInputStrings())
        {
            var numbers = MatchNumbers().ExtractNumbers(inputString).ToList();
            var differences = GetAllDifferences(numbers);
            answer += RegenerateAllNumbers(differences);
        }

        return answer;
    }
    
    public static int SolveSecond()
    {
        var inputFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "DayNine/First.txt"));

        var answer = 0;
        foreach (var inputString in inputFile.GetInputStrings())
        {
            var numbers = MatchNumbers().ExtractNumbers(inputString).ToList();
            var differences = GetAllDifferences(numbers);
            answer += RegenerateAllFirstNumbers(differences);
        }

        return answer;
    }


    private static List<int> GetDifferences(IEnumerable<int> startNumbers)
    {
        var differences = new List<int>();

        var previousNumber = 0;
        foreach (var number in startNumbers)
        {
            differences.Add(number - previousNumber);
            previousNumber = number;
        }
        // We don't actually want the first number
        differences.RemoveAt(0);
        return differences;
    }

    public static List<List<int>> GetAllDifferences(List<int> numbers)
    {
        var currentDiff = GetDifferences(numbers);
        var returnList = new List<List<int>>() { numbers, currentDiff };

        while (currentDiff.Any(i => i != 0))
        {
            currentDiff = GetDifferences(currentDiff);
            returnList.Add(currentDiff);
        }

        return returnList;
    }

    private static int RegenerateNumber(IEnumerable<int> firstList, IEnumerable<int> secondList)
    {
        var extrapolatedNumber = firstList.Last() + secondList.Last();
        return extrapolatedNumber;
    }
    
    private static int RegenerateFirstNumber(IEnumerable<int> firstList, IEnumerable<int> secondList)
    {
        var extrapolatedNumber = secondList.First() - firstList.First();
        return extrapolatedNumber;
    }

    public static int RegenerateAllNumbers(List<List<int>> input)
    {
        var currentList = input.Last();
        currentList.Add(0);
        input.RemoveAt(input.Count - 1);
        
        var regeneratedNumber = 0;

        while (input.Count != 0)
        {
            var nextList = input.Last();
            regeneratedNumber = RegenerateNumber(currentList, nextList);
            
            currentList = nextList;
            currentList.Add(regeneratedNumber);
            input.RemoveAt(input.Count - 1);
        }

        return regeneratedNumber;
    }
    
    public static int RegenerateAllFirstNumbers(List<List<int>> input)
    {
        var currentList = input.Last();
        currentList.Insert(0,0);
        input.RemoveAt(input.Count - 1);
        
        var regeneratedNumber = 0;

        while (input.Count != 0)
        {
            var nextList = input.Last();
            regeneratedNumber = RegenerateFirstNumber(currentList, nextList);
            
            currentList = nextList;
            currentList.Insert(0, regeneratedNumber);
            input.RemoveAt(input.Count - 1);
        }

        return regeneratedNumber;
    }
}