using AdventOfCode2023.DayEleven;

namespace AoC2023.Tests;

public class Day11
{
    [Theory]
    [InlineData(0, 0, 0, 0, 1)]
    [InlineData(3, 0, 4, 0, 1)]
    [InlineData(6, 0, 8, 0, 1)]
    [InlineData(9, 0, 12, 0, 1)]
    [InlineData(0, 4, 0, 5, 1)]
    [InlineData(0, 8, 0, 10, 1)]
    [InlineData(9, 8, 12, 10, 1)]
    [InlineData(3, 0, 13, 0, 10)]
    [InlineData(0, 4, 0, 14, 10)]
    [InlineData(9, 8, 15, 12, 2)]
    public void Should_Expand_Coordinate(int column, int row, int resultColumn, int resultRow, int expandFactor)
    {
        var input = """
                    ...#......
                    .......#..
                    #.........
                    ..........
                    ......#...
                    .#........
                    .........#
                    ..........
                    .......#..
                    #...#.....
                    """;

        var image = Image.CreateFromString(input, expandFactor);

        var coord = new Coordinate(column, row);
        var expandedCoord = image.ExpandCoordinate(coord);
        
        Assert.Equal(new Coordinate(resultColumn, resultRow), expandedCoord);
    }


    
    [Theory]
    [InlineData(374, 1)]
    [InlineData(1030, 9)]
    [InlineData(8410, 99)]
    public void Should_Solve_First(int shouldResult, int expandFactor)
    {
        var input = """
                    ...#......
                    .......#..
                    #.........
                    ..........
                    ......#...
                    .#........
                    .........#
                    ..........
                    .......#..
                    #...#.....
                    """;

        var image = Image.CreateFromString(input, expandFactor);
        var allDistances = image.Solve();
        var solution = allDistances.Sum();
        
        Assert.Equal(shouldResult, solution);
    }
}