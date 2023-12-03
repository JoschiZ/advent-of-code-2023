namespace AoC2023.Tests;

public class Day3
{
    [Fact]
    public void ShouldSolveDay3First()
    {
        var sampleInput = """
                          467..114..
                          ...*......
                          ..35..633.
                          ......#...
                          617*......
                          .....+.58.
                          ..592.....
                          ......755.
                          ...$.*....
                          .664.598..
                          """;
        var solution = 4361;

        var lineByLine = sampleInput.Split("\n");

        var result = AdventOfCode2023.DayThree.Day3.SolveFirst(lineByLine);
        Assert.Equal(solution, result);
    }
    
    [Fact]
    public void ShouldSolveDay3Second()
    {
        var sampleInput = """
                          467..114..
                          ...*......
                          ..35..633.
                          ......#...
                          617*......
                          .....+.58.
                          ..592.....
                          ......755.
                          ...$.*....
                          .664.598..
                          """;
        var solution = 467835;

        var lineByLine = sampleInput.Split("\n");

        var result = AdventOfCode2023.DayThree.Day3.SolveSecond(lineByLine);
        Assert.Equal(solution, result);
    }
}