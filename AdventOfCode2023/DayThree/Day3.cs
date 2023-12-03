using System.Text.RegularExpressions;
using AdventOfCode2023.Core;

namespace AdventOfCode2023.DayThree;

public partial class Day3
{
    [GeneratedRegex(@"[^\d|\.]")]
    private static partial Regex MatchPoints();

    [GeneratedRegex(@"\d+")]
    private static partial Regex MatchNumbers();

    [GeneratedRegex(@"\*")]
    private static partial Regex MatchGears();
    public static int SolveFirst(IEnumerable<string>? lines = null)
    {
        if (lines is null)
        {
            var inputFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "DayThree/First.txt"));
            lines = inputFile.GetInputStrings();    
        }
        
        var symbolSet = new HashSet<Point>();
        var allPartNumbers = new List<PartNumber>();
        var i = 0;
        foreach (var line in lines)
        {
            var symbols = GetCenterPoints(line, i);
            foreach (var symbol in symbols)
            {
                symbolSet.Add(symbol);
            }
            
            var partNumbers = GetPartNumbers(line, i);
            allPartNumbers.AddRange(partNumbers);
            
            i++;
        }

        var resultNumber = 0;
        // We could cur allocating all those numbers to an List and iterating over them
        // by not only loading the current line, but also the symbols of the next line
        // And then directly evaluating if the numbers of that line are valid.
        foreach (var partNumber in allPartNumbers)
        {
            if (partNumber.IsValid(symbolSet))
            {
                resultNumber += partNumber.Number;
            }
        }

        return resultNumber;
    }

    public static int SolveSecond(IEnumerable<string>? lines = null)
    {
        if (lines is null)
        {
            var inputFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "DayThree/First.txt"));
            lines = inputFile.GetInputStrings();    
        }
        
        var gears = new HashSet<Point>();
        var allPartNumbers = new List<PartNumber>();
        var i = 0;
        foreach (var line in lines)
        {
            var symbols = GetGears(line, i);
            foreach (var symbol in symbols)
            {
                gears.Add(symbol);
            }
            
            var partNumbers = GetPartNumbers(line, i);
            allPartNumbers.AddRange(partNumbers);
            
            i++;
        }

        var gearAdjacency = new Dictionary<Point, List<int>>();

        foreach (var partNumber in allPartNumbers)
        {
            var adjacentGears = partNumber.GetAllAdjacentPoints(gears);
            foreach (var adjacentGear in adjacentGears)
            {
                gearAdjacency.TryAdd(adjacentGear, new List<int>());
                gearAdjacency[adjacentGear].Add(partNumber.Number);
            }
        }


        return gearAdjacency
            .Where(pair => pair.Value.Count == 2)
            .Select(pair => pair.Value)
            .Sum(value => value[0] * value[1]);
    }
    
    
    /// <summary>
    /// Returns all the Part Numbers that are contained in a line
    /// </summary>
    /// <param name="inputLine"></param>
    /// <param name="lineNumber"></param>
    /// <returns></returns>
    public static IEnumerable<PartNumber> GetPartNumbers(string inputLine, int lineNumber)
    {
        var matches = MatchNumbers().Matches(inputLine);

        foreach (Match match in matches)
        {
            var position = new Point(match.Index, lineNumber);
            var number = int.Parse(match.ToString());
            yield return new PartNumber(position, number, match.Length);
        }
    }
    
    /// <summary>
    /// Returns all Symbols in a Line that could make a PartNumber valid
    /// </summary>
    /// <param name="inputLine"></param>
    /// <param name="lineNumber"></param>
    /// <returns></returns>
    public static IEnumerable<Point> GetCenterPoints(string inputLine, int lineNumber)
    {
        var matches = MatchPoints().Matches(inputLine);
        foreach (Match match in matches)
        {
            yield return new Point(match.Index, lineNumber);
        }
    }

    public static IEnumerable<Point> GetGears(string inputLine, int lineNumber)
    {
        var matches = MatchGears().Matches(inputLine);
        foreach (Match match in matches)
        {
            yield return new Point(match.Index, lineNumber);
        }
    }
    
    
    public IEnumerable<Point> GetValidPoints(Point centerPoint)
    {
        for (int deltaX = -1; deltaX < 1; deltaX++)
        {
            for (int deltaY = -1; deltaY < 1; deltaY++)
            {
                yield return new Point(centerPoint.X + deltaX, centerPoint.Y + deltaY);
            }
        }
    }

    public record PartNumber(Point StartPosition, int Number, int Length)
    {
        /// <summary>
        /// Returns the Point position of every digit in this part number
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Point> GetSubPoints()
        {
            yield return StartPosition;
            for (int xOffset = 1; xOffset < Length; xOffset++)
            {
                yield return StartPosition with { X = StartPosition.X + xOffset };
            }
        }
        
        /// <summary>
        /// Returns all points, that would make this Part Valid
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Point> GetPossiblePoints()
        {
            var i = 0;
            foreach (var subPoint in GetSubPoints())
            {
                if (i == 0)
                {
                    yield return subPoint with { X = subPoint.X - 1 };
                    yield return new Point(X: subPoint.X - 1, Y: subPoint.Y + 1);
                    yield return new Point(X: subPoint.X - 1, Y: subPoint.Y - 1);
                }

                yield return subPoint with { Y = subPoint.Y + 1 };
                yield return subPoint with { Y = subPoint.Y - 1 };
                
                
                if (i == Length-1)
                {
                    yield return subPoint with { X = subPoint.X + 1 };
                    yield return new Point(X: subPoint.X + 1, Y: subPoint.Y + 1);
                    yield return new Point(X: subPoint.X + 1, Y: subPoint.Y - 1);
                }
                
                i++;
            }
        }

        
        /// <summary>
        /// Gets ALL points from a HasSet that are adjacent to this GearNumber
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public IEnumerable<Point> GetAllAdjacentPoints(HashSet<Point> points)
        {
            foreach (var possiblePoint in GetPossiblePoints())
            {
                if (points.Contains(possiblePoint))
                {
                    yield return possiblePoint;
                }
            }
        }

        public bool IsValid(HashSet<Point> validatingPoints)
        {
            return GetPossiblePoints().Any(validatingPoints.Contains);
        }
    };
    public record Point(int X, int Y);
}