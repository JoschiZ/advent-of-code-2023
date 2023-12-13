using AdventOfCode2023.Core;

namespace AoC2023.Tests;
using static AdventOfCode2023.DayNine.Day9;


public class Day9
{
    [Fact]
    public void Should_Build_Differences()
    {
        List<int> input = [0, 3, 6, 9, 12, 15];
        var differences = GetAllDifferences(input);
        List<List<int>> shouldResult = [[0, 3, 6, 9, 12, 15], [3, 3, 3, 3, 3], [0,0,0,0]];
        Assert.Equivalent(shouldResult, differences, strict: true);
    }

    [Theory]
    [InlineData("0   3   6   9  12  15", 18)]
    [InlineData("1   3   6  10  15  21", 28)]
    [InlineData("10  13  16  21  30  45", 68)]
    public void Should_Regenerate_Numbers(string input, int result)
    {
        var numbers = MatchNumbers().ExtractNumbers(input);
        var differences = GetAllDifferences(numbers.ToList());
        var regeneratedNumber = RegenerateAllNumbers(differences);
        Assert.Equal(result, regeneratedNumber);
    }
    
    [Theory]
    [InlineData("0   3   6   9  12  15", -3)]
    [InlineData("1   3   6  10  15  21", 0)]
    [InlineData("10  13  16  21  30  45", 5)]
    public void Should_Regenerate_First_Numbers(string input, int result)
    {
        var numbers = MatchNumbers().ExtractNumbers(input);
        var differences = GetAllDifferences(numbers.ToList());
        var regeneratedNumber = RegenerateAllFirstNumbers(differences);
        Assert.Equal(result, regeneratedNumber);
    }
}