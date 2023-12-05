using System.Diagnostics;
using System.Text.RegularExpressions;
using AdventOfCode2023.Core;

namespace AdventOfCode2023.DayFive;

// NOTE: This does fully work for some reason. Two of the starting seeds return 0, but the answer was the next one down (while ignoring the 0)
// And I simply cannot find why


public partial class Day5Second
{
    [GeneratedRegex(@"\d+")]
    private static partial Regex MatchDigits();
    
    private readonly List<SeedRange> _seeds = new();

    private Dictionary<string, Map> _maps = new();
    
    public static long Solve()
    {
        var inputFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "DayFive/First.txt"));

        var day5 = new Day5Second();
        day5.BuildMaps(inputFile.GetInputStrings());
        return day5.GetMinimumLand();
    }

    public Dictionary<string, Map> BuildMaps(IEnumerable<string> input)
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
                    
                    var length = seeds[0];
                    seeds.RemoveAt(0);
                    _seeds.Add(new SeedRange(start, length));
                }
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
    
    public long GetMinimumLand()
    {
        var ranges = FindAllResultRanges();
        return ranges.MinBy(range => range.Start).Start;
    }

    public IEnumerable<SeedRange> FindAllResultRanges()
    {
        var firstMap = _maps["seed"];
        var result = SearchMap(_seeds, firstMap);
        return result;
    }

    public List<SeedRange> SearchMap(IEnumerable<SeedRange> seedRanges, Map map)
    {
        var resultRanges = new List<SeedRange>();

        foreach (var seedRange in seedRanges)
        {
            resultRanges.AddRange(map.FindNextRanges(seedRange));
        }
        
        if (_maps.TryGetValue(map.Destination, out var nextMap))
        {
            return SearchMap(resultRanges, nextMap);
        }

        return resultRanges;
    }
    
    
    
    public record MapRange(long StartSource, long StartDestination, long Length)
    {
        private readonly long _offset = StartDestination - StartSource;

        private readonly long _endSource = StartSource + Length - 1;
        private readonly long _endDestination = StartDestination + Length - 1;
        
        /// <summary>
        /// Takes in a seed range and maps it to the corresponding result ranges
        /// </summary>
        /// <param name="seedRange"></param>
        /// <returns></returns>
        /// <exception cref="UnreachableException"></exception>
        public IEnumerable<SeedRange> GetDestinationRanges(SeedRange seedRange)
        {
            var resultRanges = new List<SeedRange>();
            var workingRange = seedRange;

            // Handle ranges that are complete below this range
            if (workingRange.End < StartSource)
            {
                throw new UnreachableException("This should not be reached here");
                resultRanges.Add(workingRange);
                return resultRanges;
            }
            
            
            if (workingRange.Start - StartSource < 0)
            {
                // Create the range from the start of the seeds until the start of this range
                // Which is mapped to itself
                var found = SeedRange.CreateInclusiveSeedRange(workingRange.Start, StartSource - 1);
                Console.WriteLine($"Found: {found}");
                resultRanges.Add(found);
                
                // "Slice off" the seeds that got mapped to themselves
                workingRange = SeedRange.CreateInclusiveSeedRange(StartSource, workingRange.End);
                Console.WriteLine($"New Working Range: {workingRange}");
            }

            // Case: The seed range is within this range
            if (workingRange.Start >= StartSource && workingRange.End >= _endSource)
            {
                var found = SeedRange.CreateInclusiveSeedRange(workingRange.Start + _offset, _endDestination);
                resultRanges.Add(found);
                Console.WriteLine($"Found 1: {found}");

                if (_endSource + 1 >= workingRange.End)
                {
                    return resultRanges;
                }
                workingRange = SeedRange.CreateInclusiveSeedRange(_endSource + 1, workingRange.End);
                Console.WriteLine($"New Working Range: {workingRange}");
            }

            if (workingRange.Start >= StartSource && workingRange.End < _endSource)
            {
                var found = SeedRange.CreateInclusiveSeedRange(workingRange.Start + _offset,
                    workingRange.End + _offset);
                resultRanges.Add(found);
                Console.WriteLine($"Found 2: {found}");
                // After this case out working range should be empty
                return resultRanges;
            }

            // Handle remaining range that was left 
            if (workingRange.Start > _endSource)
            {
                Console.WriteLine($"Return remaining: {workingRange}");
                resultRanges.Add(workingRange);
            }

            if (resultRanges.Count > 3)
            {
                throw new UnreachableException("");
            }
            return resultRanges;
        }
        
        
        /// <summary>
        /// Returns true if any part of the seed range is in this MapRange
        /// </summary>
        /// <param name="seedRange"></param>
        /// <returns></returns>
        public bool IsInRange(SeedRange seedRange)
        {
            
            if (seedRange.Start >= StartSource && seedRange.Start < StartSource + Length)
            {
                return true;
            }

            if (seedRange.End >= StartSource && seedRange.End < StartSource + Length)
            {
                return true;
            }

            return false;
        }
    }
    
    
    public readonly struct SeedRange: IEquatable<SeedRange>
    {
        public override string ToString()
        {
            return $"Start: {Start} - End: {End} | Length: {Length}";
        }
        
        public long Start { get; }
        
        /// <summary>
        /// End is the Last number in the range that is still considered part of the range
        /// </summary>
        public long End { get; }
        
        /// <summary>
        /// Length is the number of elements if this would have been an array
        /// </summary>
        private long Length { get; }
        
        public SeedRange(long start, long length)
        {
            Start = start;
            Length = length;
            End = start + length - 1; // 3 2 -> 3, 4 Start = 3 End = 4
        }
        
        private SeedRange(long start, long end, long length)
        {
            Start = start;
            End = end;
            Length = length;
        }

        public static SeedRange CreateInclusiveSeedRange(long start, long end)
        {
            return new SeedRange(start, end, start - end + 1);
        }

        public bool Equals(SeedRange other)
        {
            return Start == other.Start && End == other.End;
        }

        public override bool Equals(object? obj)
        {
            return obj is SeedRange other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Start, End, Length);
        }

        public static bool operator ==(SeedRange left, SeedRange right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SeedRange left, SeedRange right)
        {
            return !(left == right);
        }
    }
    
    public partial class Map
    {
        private Dictionary<SeedRange, IEnumerable<SeedRange>> _mappings = new();
        
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

        [GeneratedRegex(@"[^- ]+")]
        private static partial Regex MatchWords();
        public SortedList<long, MapRange> Ranges { get; } = [];
        public Map AddRange(string rangeLine)
        {
            var numbers = MatchDigits().ExtractBigNumbers(rangeLine).ToArray();
            if (numbers.Length < 3)
            {
                return this;
            }
            var range = new MapRange(numbers[1], numbers[0], numbers[2]);
            Ranges.Add(range.StartSource, range);
            return this;
        }
        public string Source { get; }
        public string Destination { get; }


        public IEnumerable<SeedRange> FindNextRanges(SeedRange seedRange)
        {
            Console.WriteLine($"Getting results for {seedRange} in {Source} -> {Destination}");
            var results = new List<SeedRange>();
            if (_mappings.TryGetValue(seedRange, out var ranges))
            {
                return ranges;
            }
            foreach (var (_, range) in Ranges)
            {
                if (range.IsInRange(seedRange))
                {
                    Console.WriteLine(range);
                    results.AddRange(range.GetDestinationRanges(seedRange));
                }
            }


            if (results.Count > 0)
            {            
                _mappings.Add(seedRange, results);
                return results;
            }
            
            Console.WriteLine($"None found returning self: {seedRange}");
            _mappings.Add(seedRange,  new[] { seedRange });
            // If this is not part of any seed range we can just return itself back
            return new[] { seedRange };
        }
    }
}