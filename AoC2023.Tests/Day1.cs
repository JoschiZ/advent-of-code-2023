using Xunit;

namespace AoC2023.Tests;

public class Day1
{
    [Theory]
    [InlineData("1abc2", 12)]
    [InlineData("pqr3stu8vwx", 38)]
    [InlineData("a1b2c3d4e5f", 15)]
    [InlineData("treb7uchet", 77)]
    public void Should_Extract_Correct_Value(string input, int result)
    {
        var toTestResult = AdventOfCode2023._1.Day1.ParseString(input);
        Assert.Equal(result, toTestResult);
    }

    [Theory]
    [InlineData("two1nine", 29)]
    [InlineData("eightwothree", 83)]
    [InlineData("abcone2threexyz", 13)]
    [InlineData("xtwone3four", 24)]
    [InlineData("4nineeightseven2", 42)]
    [InlineData("zoneight234", 14)]
    [InlineData("7pqrstsixteen", 76)]
    public void Should_Extract_Transformed_String(string input, int result)
    {
        var transformedInput = AdventOfCode2023._1.Day1.TransformString(input);
        var toTestResult = AdventOfCode2023._1.Day1.ParseString(transformedInput);
        Assert.Equal(result, toTestResult);
    }
}