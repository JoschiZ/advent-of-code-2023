using AdventOfCode2023.DayFive;
using Xunit;

namespace AoC2023.Tests;

public class Day5
{
    [Theory]
    [InlineData(79, 82)]
    [InlineData(14, 43)]
    [InlineData(55, 86)]
    [InlineData(13, 35)]
    public void Should_Map_Correct_Path(int seed, int result)
    { 
        var input = $"""
                     seeds: {seed}

                     seed-to-soil map:
                     50 98 2
                     52 50 48

                     soil-to-fertilizer map:
                     0 15 37
                     37 52 2
                     39 0 15

                     fertilizer-to-water map:
                     49 53 8
                     0 11 42
                     42 0 7
                     57 7 4

                     water-to-light map:
                     88 18 7
                     18 25 70

                     light-to-temperature map:
                     45 77 23
                     81 45 19
                     68 64 13

                     temperature-to-humidity map:
                     0 69 1
                     1 0 69

                     humidity-to-location map:
                     60 56 37
                     56 93 4
                     """;
        
        var d5 = new AdventOfCode2023.DayFive.Day5();
        d5.BuildMaps(input.Split("\n"));
        var shortedPathSeed = d5.FindShortestPathSeed();
        Assert.Equal(result, shortedPathSeed);
    }

    [Fact]
    public void Should_Solve_First()
    {
        const string input = """
                             seeds: 79 14 55 13

                             seed-to-soil map:
                             50 98 2
                             52 50 48

                             soil-to-fertilizer map:
                             0 15 37
                             37 52 2
                             39 0 15

                             fertilizer-to-water map:
                             49 53 8
                             0 11 42
                             42 0 7
                             57 7 4

                             water-to-light map:
                             88 18 7
                             18 25 70

                             light-to-temperature map:
                             45 77 23
                             81 45 19
                             68 64 13

                             temperature-to-humidity map:
                             0 69 1
                             1 0 69

                             humidity-to-location map:
                             60 56 37
                             56 93 4
                             """;
        
        var d5 = new AdventOfCode2023.DayFive.Day5();
        var maps = d5.BuildMaps(input.Split("\n"));
        var shortedPathSeed = d5.FindShortestPathSeed();
        Assert.Equal(35, shortedPathSeed);
    }
    
    
    [Theory]
    [InlineData(79, 82)]
    [InlineData(14, 43)]
    [InlineData(55, 86)]
    [InlineData(13, 35)]
    public void Should_Map_Correct_Path_Second(int seed, int result)
    { 
        var input = $"""
                     seeds: {seed} 1

                     seed-to-soil map:
                     50 98 2
                     52 50 48

                     soil-to-fertilizer map:
                     0 15 37
                     37 52 2
                     39 0 15

                     fertilizer-to-water map:
                     49 53 8
                     0 11 42
                     42 0 7
                     57 7 4

                     water-to-light map:
                     88 18 7
                     18 25 70

                     light-to-temperature map:
                     45 77 23
                     81 45 19
                     68 64 13

                     temperature-to-humidity map:
                     0 69 1
                     1 0 69

                     humidity-to-location map:
                     60 56 37
                     56 93 4
                     """;
        
        var d5 = new Day5Second();
        d5.BuildMaps(input.Split("\n"));
        var shortedPathSeed = d5.GetMinimumLand();
        Assert.Equal(result, shortedPathSeed);
    }
    
    
    [Fact]
    public void Should_Solve_Second()
    {
        const string input = """
                             seeds: 79 14 55 13

                             seed-to-soil map:
                             50 98 2
                             52 50 48

                             soil-to-fertilizer map:
                             0 15 37
                             37 52 2
                             39 0 15

                             fertilizer-to-water map:
                             49 53 8
                             0 11 42
                             42 0 7
                             57 7 4

                             water-to-light map:
                             88 18 7
                             18 25 70

                             light-to-temperature map:
                             45 77 23
                             81 45 19
                             68 64 13

                             temperature-to-humidity map:
                             0 69 1
                             1 0 69

                             humidity-to-location map:
                             60 56 37
                             56 93 4
                             """;
        
        var d5 = new AdventOfCode2023.DayFive.Day5Second();
        var maps = d5.BuildMaps(input.Split("\n"));
        var shortedPathSeed = d5.GetMinimumLand();
        Assert.Equal(46, shortedPathSeed);
    }

    [Fact]
    public void Should_Handle_Below_To_Mid_Range()
    {
        var range = new Day5Second.MapRange(10, 20, 6);
        var seeds = Day5Second.SeedRange.CreateInclusiveSeedRange(8, 13);

        var resultRanges = range.GetDestinationRanges(seeds);
        IEnumerable<Day5Second.SeedRange> trueResults = new[]
        {
            Day5Second.SeedRange.CreateInclusiveSeedRange(8, 9), 
            Day5Second.SeedRange.CreateInclusiveSeedRange(20, 23)
        };
        
        Assert.Equivalent(trueResults, resultRanges, strict: true);
    }
    
    [Fact]
    public void Should_Handle_OverArching_Range()
    {
        var range = new Day5Second.MapRange(10, 20, 6);
        var seeds = Day5Second.SeedRange.CreateInclusiveSeedRange(8, 17);

        var resultRanges = range.GetDestinationRanges(seeds);
        IEnumerable<Day5Second.SeedRange> trueResults = new[]
        {
            Day5Second.SeedRange.CreateInclusiveSeedRange(8, 9), 
            Day5Second.SeedRange.CreateInclusiveSeedRange(20, 25),
            Day5Second.SeedRange.CreateInclusiveSeedRange(16, 17)
        };
        
        Assert.Equivalent(trueResults, resultRanges, strict: true);
    }
    
    [Fact]
    public void Should_Handle_Mid_To_Beyond_Range()
    {
        var range = new Day5Second.MapRange(10, 20, 6);
        var seeds = Day5Second.SeedRange.CreateInclusiveSeedRange(13, 17);

        var resultRanges = range.GetDestinationRanges(seeds);
        IEnumerable<Day5Second.SeedRange> trueResults = new[]
        {
            Day5Second.SeedRange.CreateInclusiveSeedRange(23, 25),
            Day5Second.SeedRange.CreateInclusiveSeedRange(16, 17)
        };
        
        Assert.Equivalent(trueResults, resultRanges, strict: true);
    }
    
    [Fact]
    public void Should_Handle_Contained_Range()
    {
        var range = new Day5Second.MapRange(10, 20, 6);
        var seeds = Day5Second.SeedRange.CreateInclusiveSeedRange(11, 15);

        var resultRanges = range.GetDestinationRanges(seeds);
        IEnumerable<Day5Second.SeedRange> trueResults = new[]
        {
            Day5Second.SeedRange.CreateInclusiveSeedRange(21, 25)
        };
        
        Assert.Equivalent(trueResults, resultRanges, strict: true);
    }
    
}