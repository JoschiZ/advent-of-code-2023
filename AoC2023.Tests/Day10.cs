using AdventOfCode2023.DayTen;
using Xunit.Abstractions;

namespace AoC2023.Tests;

public class Day10
{
    private ITestOutputHelper _outputHelper;

    public Day10(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }


    [Theory]
    [InlineData("""
                -L|F7
                7S-7|
                L|7||
                -L-J|
                L|-JF
                """)]
    [InlineData("""
                .....
                .S-7.
                .|.|.
                .L-J.
                .....
                """)]
    [InlineData("""
                ..F7.
                .FJ|.
                SJ.L7
                |F--J
                LJ...
                """)]

    public void Should_Navigate_Maze(string input)
    {

        var maze = Maze.CreateFromString(input);
        _outputHelper.WriteLine(maze.CurrentPosition.ToString());
        maze.MoveNext();
        _outputHelper.WriteLine(maze.CurrentPosition.ToString());
        
        while (maze.CurrentPosition.Character != 'S')
        {
            maze.MoveNext();
            _outputHelper.WriteLine(maze.CurrentPosition.ToString());
        }
    }

    [Theory]
    [InlineData("""
                -L|F7
                7S-7|
                L|7||
                -L-J|
                L|-JF
                """, 4)]
    [InlineData("""
                ..F7.
                .FJ|.
                SJ.L7
                |F--J
                LJ...
                """, 8)]
    public void Should_Solve_First(string input, int result)
    {
        var maze = Maze.CreateFromString(input);
        var cycle = maze.NavigateFullCircle().ToArray();
        Assert.Equal(result, cycle.Length / 2);
    }
}