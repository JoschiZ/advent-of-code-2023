using Xunit;

namespace AoC2023.Tests;

public class Day2
{
    [Theory]
    [InlineData("Game 15: 19 blue, 1 green; 1 red, 5 blue; 3 green, 8 blue; 1 red, 13 blue, 3 green", 15, 1, 3, 19)]
    public void Should_Parse_Correctly(string input, int id, int red, int green, int blue)
    {
        var game = AdventOfCode2023.DayTwo.Day2.Game.CreateMaximumGame(input);
        Assert.Equal(id, game.Id);
        Assert.Equal(red, game.MaxRed);
        Assert.Equal(green, game.MaxGreen);
        Assert.Equal(blue, game.MaxBlue);
    }

    [Theory]
    [InlineData("Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green", true)]
    public void Should_Validate_Game(string input, bool isValid)
    {
        var game = AdventOfCode2023.DayTwo.Day2.Game.CreateMaximumGame(input);
        Assert.Equal(isValid, game.IsValid());
    }
}