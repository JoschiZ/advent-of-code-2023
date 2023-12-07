using System.Xml;
using AdventOfCode2023.Core;

namespace AdventOfCode2023.DaySeven;

public abstract partial class Day7
{
    public static int SolveFirst()
    {
        var inputFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "DaySeven/First.txt"));
        var game = new Game(inputFile.GetInputStrings(), true);
        return game.GetTotalWinnings();
    }
    
    public static int SolveSecond()
    {
        var inputFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "DaySeven/First.txt"));
        var game = new Game(inputFile.GetInputStrings(), false);
        return game.GetTotalWinnings();
    }
}