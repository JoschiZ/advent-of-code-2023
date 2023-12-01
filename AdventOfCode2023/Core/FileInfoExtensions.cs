namespace AdventOfCode2023.Core;

public static class FileInfoExtensions
{
    public static IEnumerable<string> GetInputStrings(this FileInfo fileInfo)
    {
        var fs = fileInfo.OpenRead();
        using var sr = new StreamReader(fs);

        while (sr.ReadLine() is { } line)
        {
            yield return line;
        }
    }
}