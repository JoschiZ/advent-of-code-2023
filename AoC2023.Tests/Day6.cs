using Xunit;
namespace AoC2023.Tests;
using RaceRecord = (long time, long minDistance);



public class Day6
{
    [Theory]
    [InlineData(7, 9, 4)]
    [InlineData(15, 40, 8)]
    [InlineData(30, 200, 9)]
    public void Should_Solve_Race(long raceTime, long recordDistance, long solution)
    {
        var toTestSolution = AdventOfCode2023.DaySix.Day6.SolveRace((raceTime, recordDistance));
        Assert.Equal(toTestSolution, solution);
    }

    [Fact]
    public void Should_Solve_All_Races()
    {
        // assert
        var races = new List<RaceRecord>();
        races.Add(new RaceRecord(7, 9));
        races.Add(new RaceRecord(15, 40));
        races.Add(new RaceRecord(30, 200));
        
        var toTestSolution = AdventOfCode2023.DaySix.Day6.GetSolution(races);
        
        Assert.Equal(288, toTestSolution);
    }
}