namespace AdventOfCode2023.DayEleven;

public static class Day11
{
    public static long SolveFirst()
    {
        var inputFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "DayEleven/First.txt"));
        var fs = inputFile.OpenRead();
        using var sr = new StreamReader(fs);

        var input = sr.ReadToEnd();
        Console.WriteLine(input);
        var image = Image.CreateFromString(input);
        return image.Solve().Sum();
    }
    
    public static long SolveSecond()
    {
        var inputFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "DayEleven/First.txt"));
        var fs = inputFile.OpenRead();
        using var sr = new StreamReader(fs);

        var input = sr.ReadToEnd();
        Console.WriteLine(input);
        var image = Image.CreateFromString(input, 999999);
        return image.Solve().Sum();
    }
}