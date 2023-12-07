namespace AoC2023.Tests;

public class Day7
{
    [Theory]
    [InlineData("AAAAA", AdventOfCode2023.DaySeven.Day7.HandType.FiveOfAKind)]
    [InlineData("AAAA2", AdventOfCode2023.DaySeven.Day7.HandType.FourOfAKind)]
    [InlineData("AAA22", AdventOfCode2023.DaySeven.Day7.HandType.FullHouse)]
    [InlineData("AAA23", AdventOfCode2023.DaySeven.Day7.HandType.ThreeOfAKind)]
    [InlineData("AA223", AdventOfCode2023.DaySeven.Day7.HandType.TwoPairs)]
    [InlineData("AA234", AdventOfCode2023.DaySeven.Day7.HandType.OnePair)]
    [InlineData("23456", AdventOfCode2023.DaySeven.Day7.HandType.HighCard)]
    public void Should_Determine_HandType(string cards, AdventOfCode2023.DaySeven.Day7.HandType handType)
    {
        var hand = new AdventOfCode2023.DaySeven.Day7.Hand(cards);
        Assert.Equal(handType, hand.HandType);
    }

    [Theory]
    [InlineData("AAAAA", "23456", true)]
    [InlineData("AAAAA", "22222", true)]
    [InlineData("22334", "2AA33", false)]
    [InlineData("JJJJJ", "22222", true)]
    public void Should_Determine_Correct_Ordering(string hand, string otherHand, bool isHandHigher)
    {
        var hand1 = new AdventOfCode2023.DaySeven.Day7.Hand(hand);
        var hand2 = new AdventOfCode2023.DaySeven.Day7.Hand(otherHand);
        var result = hand1.CompareTo(hand2);
        Assert.Equal(isHandHigher, result > 0);
    }

    [Fact]
    public void Should_Solve_First()
    {
        var input = """
                    32T3K 765
                    T55J5 684
                    KK677 28
                    KTJJT 220
                    QQQJA 483
                    """;
        var game = new AdventOfCode2023.DaySeven.Day7.Game(input.Split("\n"));
        var totalWinnings = game.GetTotalWinnings();
        Assert.Equal(6440, totalWinnings);
    }
}