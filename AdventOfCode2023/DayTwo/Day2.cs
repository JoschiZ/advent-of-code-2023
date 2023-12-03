using System.Text.RegularExpressions;
using AdventOfCode2023.Core;

namespace AdventOfCode2023.DayTwo;

public partial class Day2
{

    public static int SolveFirst()
    {
        var inputFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "DayTwo/First.txt"));

        var resultSum = 0;
        foreach (var inputString in inputFile.GetInputStrings())
        {
            var game = Game.CreateMaximumGame(inputString);
            if (game.IsValid())
            {
                resultSum += game.Id;
            }
        }

        return resultSum;
    }

    public static int SolveSecond()
    {
        var inputFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "DayTwo/First.txt"));
        
        var resultSum = 0;
        foreach (var inputString in inputFile.GetInputStrings())
        {
            var game = Game.CreateMaximumGame(inputString);
            resultSum += game.Power;
        }

        return resultSum;
    }
    

    public partial class Game
    {
        [GeneratedRegex("\\d+(?= red)")]
        private static partial Regex MatchRedCounts();
        
        [GeneratedRegex("\\d+(?= green)")]
        private static partial Regex MatchGreenCounts();
        
        [GeneratedRegex("\\d+(?= blue)")]
        private static partial Regex MatchBlueCounts();

        [GeneratedRegex("(?<=Game )\\d+")]
        private static partial Regex MatchGameId();
        
        private Game(int id, int maxRed, int maxGreen, int maxBlue)
        {
            Id = id;
            MaxRed = maxRed;
            MaxGreen = maxGreen;
            MaxBlue = maxBlue;
        }

        // Game 15: 19 blue, 1 green; 1 red, 5 blue; 3 green, 8 blue; 1 red, 13 blue, 3 green
        public static Game CreateMaximumGame(string input)
        {
            return new Game
            (
                // Not as extensible compared to splitting everything, but quicker.
                MatchGameId().ExtractNumber(input),
                MatchRedCounts().ExtractNumbers(input).Max(),
                MatchGreenCounts().ExtractNumbers(input).Max(),
                MatchBlueCounts().ExtractNumbers(input).Max()
            );
        }

        public bool IsValid(int redMaximum = 12, int greenMaximum = 13, int blueMaximum = 14)
        {
            // only 12 red cubes, 13 green cubes, and 14 blue cubes?
            if (MaxRed > redMaximum || MaxGreen > greenMaximum || MaxBlue > blueMaximum)
            {
                return false;
            }

            return true;
        }

        public int Power => MaxRed * MaxGreen * MaxBlue;

        public int MaxRed { get; }
        public int MaxGreen { get; }
        public int MaxBlue { get; }
        
        
        private List<ColorDraw> ColorDraws { get; set; } = new();
        public int Id { get; }
    }
    
    
    public record ColorDraw
    {
        public Dictionary<string, int> Balls { get; } = new Dictionary<string, int>();
        
        public ColorDraw(string input)
        {
            // Input Example "1 green, 1 blue, 1 red"
            var splitIntoColors = input.Split(",");
            foreach (var splitIntoColor in splitIntoColors)
            {
                var split = splitIntoColor.Split(" ");
                if (Balls.TryAdd(split[1], int.Parse(split[0])))
                {
                    // I don't expect this, but if it's present we need to know and then implement a graceful handling
                    throw new Exception($"Duplicate Color in input: {input}");
                }
            }
        }
    }
    
}
