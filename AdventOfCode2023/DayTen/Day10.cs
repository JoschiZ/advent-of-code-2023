using AdventOfCode2023.Core;

namespace AdventOfCode2023.DayTen;

public static class Day10
{
    public static int SolveFirst()
    {
        var inputFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "DayTen/First.txt"));
        var fs = inputFile.OpenRead();
        using var sr = new StreamReader(fs);

        var maze = Maze.CreateFromString(sr.ReadToEnd());
        var cycle = maze.NavigateFullCircle().ToArray();
        return cycle.Length / 2;
    }
}