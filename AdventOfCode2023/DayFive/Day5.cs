using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using AdventOfCode2023.Core;

namespace AdventOfCode2023.DayFive;

// NOTE: A possible solution to this would be more algorithmically. The maps in the file are already sorted
// And such we could determine the path while going through the file, but that's not very extensible.

// Another solution would be building large immutable dictionaries representing the maps.
// That would trade up front performance of building those for lookup performance later on

public partial class Day5
{
    [GeneratedRegex(@"\d+")]
    private static partial Regex MatchDigits();
    
    private List<long> _seeds = [];
    private Dictionary<string, Map> _maps = new();

    public static long SolveFirst()
    {
        var inputFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "DayFive/First.txt"));

        var day5 = new Day5();
        day5.BuildMaps(inputFile.GetInputStrings());
        return day5.FindShortestPathSeed();
    }
    

    private IEnumerable<long> CreateRange(long start, long count)
    {
        var limit = start + count;

        while (start < limit)
        {
            yield return start;
            start++;
        }
    }

    public Dictionary<string, Map> BuildMaps(IEnumerable<string> input)
    {
        Dictionary<string, Map> maps = new();
        Map? currentMap = null;
        foreach (var line in input)
        {
            if (_seeds.Count == 0 && line.Contains("seeds"))
            {
                _seeds = MatchDigits().ExtractBigNumbers(line).ToList();
                
                continue;
            }
            
            if (line.Contains("map"))
            {
                currentMap = new Map(line);
                maps.Add(currentMap.Source, currentMap);
                continue;
            }

            if (!string.IsNullOrEmpty(line))
            {
                currentMap?.AddRange(line);    
            }
        }
        // This should be done in a static create method with private constructor of Day5 for safety but not important here
        _maps = maps;
        return maps;
    }
    
    public Dictionary<string, Map> BuildLongMaps(IEnumerable<string> input)
    {
        Dictionary<string, Map> maps = new();
        Map? currentMap = null;
        foreach (var line in input)
        {
            if (_seeds.Count == 0 && line.Contains("seeds"))
            {
                var seeds = MatchDigits().ExtractBigNumbers(line).ToList();
                while (seeds.Count > 1)
                {
                    var start = seeds[0];
                    seeds.RemoveAt(0);
                    
                    var range = seeds[0];
                    seeds.RemoveAt(0);
                    Console.WriteLine($"Creating Seed Range {start} - {range}");
                    _seeds.AddRange(CreateRange(start, range));
                }
                _seeds.Sort(); // This may allow us some early breaks from search loops later on.
                continue;
            }
            
            if (line.Contains("map"))
            {
                currentMap = new Map(line);
                maps.Add(currentMap.Source, currentMap);
                continue;
            }

            if (!string.IsNullOrEmpty(line))
            {
                currentMap?.AddRange(line);    
            }
        }
        // This should be done in a static create method with private constructor of Day5 for safety but not important here
        _maps = maps;
        return maps;
    }


    public long FindShortestPathSeed()
    {
        var paths = FindPaths(_seeds, _maps["seed"]).ToArray();
        return paths.Min();
    }

    private IEnumerable<long> FindPaths(List<long> startNumbers, Map searchMap)
    {
        Console.WriteLine($"{searchMap.Source} -> {searchMap.Destination}");
        var resultNumbers = 
            startNumbers
                .Select(searchMap.FindNext)
                .ToList();

        if (_maps.TryGetValue(searchMap.Destination, out var nextMap))
        {
            return FindPaths(resultNumbers, nextMap);
        }
        return resultNumbers;
    }

    public record Range(long StartSource, long StartDestination, long Length)
    {
        public bool TryGetDestination(long sourceNumber, [NotNullWhen(returnValue: true)] out long? destination)
        {
            destination = null;
            
            if (!IsInSourceRange(sourceNumber)) 
                return false;
            
            var offSet = StartDestination - StartSource;
            Console.WriteLine("Offest: " + offSet);
            destination = sourceNumber + offSet;
            Console.WriteLine($"{sourceNumber} + {offSet} = {destination}");
            return true;
        }
        
        public bool IsInSourceRange(long sourceNumber)
        {
            return sourceNumber >= StartSource && sourceNumber < StartSource + Length;
        }
    };

    public partial class Map
    {
        [GeneratedRegex(@"[^- ]+")]
        private static partial Regex MatchWords();
        
        
        public Map(string source, string destination)
        {
            Source = source;
            Destination = destination;
        }

        public Map(string line)
        {
            // Example line "seed-to-soil map:"
            if (!line.Contains("map"))
            {
                throw new ArgumentException("Not a valid starting line for a map", nameof(line));
            }

            var words = MatchWords().Matches(line);
            Source = words[0].ToString();
            Destination = words[2].ToString();
        }

        public Map AddRange(string rangeLine)
        {
            var numbers = MatchDigits().ExtractBigNumbers(rangeLine).ToArray();
            if (numbers.Length < 3)
            {
                return this;
            }
            var range = new Range(numbers[1], numbers[0], numbers[2]);
            Ranges.Add(range.StartSource, range);
            return this;
        }

        public long FindNext(long sourceNumber)
        {
            Console.WriteLine($"Searching for {sourceNumber} in {Source} -> {Destination} map");
            foreach (var (start, range) in Ranges)
            {
                Console.WriteLine(range);
                
                
                if (range.TryGetDestination(sourceNumber, out var nextNumber))
                {
                    Console.WriteLine($"Next Number {nextNumber}");
                    return (long)nextNumber;
                }
            }

            Console.WriteLine($"Return self");
            return sourceNumber;
        }

        /// <summary>
        /// Using a sorted list allows us some shortcuts while searching for keys later on.
        /// We could also just use a list and sort it, but this enforeces a sorting
        /// </summary>
        public SortedList<long, Range> Ranges { get; } = [];
        public string Source { get; }
        public string Destination { get; }
    }
}