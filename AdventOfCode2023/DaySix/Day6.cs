using System.Text.RegularExpressions;
using AdventOfCode2023.Core;
using RaceRecord = (long time, long minDistance);
using Race = (long buttonPressTime, long distanceTraveled);

namespace AdventOfCode2023.DaySix;

public partial class Day6
{
    [GeneratedRegex(@"\d+")]
    private static partial Regex MatchDigits();

    private static IEnumerable<RaceRecord> ParseInput(IEnumerable<string> input)
    {
        var inputList = input.ToArray();
        var times = MatchDigits().ExtractBigNumbers(inputList[0]);
        var minDistances = MatchDigits().ExtractBigNumbers(inputList[1]);

        return times.Zip(minDistances);
    }
    
    private static IEnumerable<RaceRecord> ParseInputForSecond(IEnumerable<string> input)
    {
        var inputList = input.Select(s => s.Replace(" ", "")).ToArray();
        var times = MatchDigits().ExtractBigNumbers(inputList[0]);
        var minDistances = MatchDigits().ExtractBigNumbers(inputList[1]);

        return times.Zip(minDistances);
    }

    private static long GetDistanceTraveled(long overallTime, long buttonPressTime) =>
        (overallTime - buttonPressTime) * buttonPressTime;

    private static IEnumerable<Race> GetAllPossibleTimes(RaceRecord race)
    {
        for (var i = 1; i <= race.time; i++)
        {
            yield return (i, GetDistanceTraveled(race.time, i));
        }
    }

    private static IEnumerable<long> GetWinningTimes(IEnumerable<Race> races, RaceRecord record)
    {
        return 
            from race in races 
            where race.distanceTraveled > record.minDistance 
            select race.buttonPressTime;
    }

    public static int SolveRace(RaceRecord record)
    {
        var possibleTimes = GetAllPossibleTimes(record);
        var winningTimes = GetWinningTimes(possibleTimes, record);
        return winningTimes.Count();
    }

    public static int GetSolution(IEnumerable<RaceRecord> records)
    {
        return records.Select(SolveRace).Aggregate((x, y) => x * y);
    }

    public static int SolveFirst()
    {
        var inputFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "DaySix/First.txt"));
        var races = ParseInput(inputFile.GetInputStrings());
        return GetSolution(races);
    }
    
    public static int SolveSecond()
    {
        var inputFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "DaySix/First.txt"));
        var races = ParseInputForSecond(inputFile.GetInputStrings());
        return GetSolution(races);
    }
}