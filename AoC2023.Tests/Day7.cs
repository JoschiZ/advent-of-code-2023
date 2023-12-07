namespace AoC2023.Tests;
using static AdventOfCode2023.DaySeven.Day7;

public class Day7
{
    [Theory]
    [InlineData("AAAAA", HandType.FiveOfAKind)]
    [InlineData("AAAA2", HandType.FourOfAKind)]
    [InlineData("AAA22", HandType.FullHouse)]
    [InlineData("AAA23", HandType.ThreeOfAKind)]
    [InlineData("AA223", HandType.TwoPairs)]
    [InlineData("AA234", HandType.OnePair)]
    [InlineData("23456", HandType.HighCard)]
    public void Should_Determine_HandType(string cards, HandType handType)
    {
        var hand = new Hand(cards, true);
        Assert.Equal(handType, hand.HandType);
    }

    [Theory]
    [InlineData("AAAAA", "23456", true)]
    [InlineData("AAAAA", "22222", true)]
    [InlineData("22334", "2AA33", false)]
    [InlineData("JJJJJ", "22222", true)]
    public void Should_Determine_Correct_Ordering(string hand, string otherHand, bool isHandHigher)
    {
        var hand1 = new Hand(hand, true);
        var hand2 = new Hand(otherHand, true);
        var result = hand1.CompareTo(hand2);
        Assert.Equal(isHandHigher, result > 0);
    }
    
    [Theory]
    [InlineData("AAAAJ", HandType.FiveOfAKind)]
    [InlineData("AAAJ2", HandType.FourOfAKind)]
    [InlineData("AAA22", HandType.FullHouse)]
    [InlineData("AAA23", HandType.ThreeOfAKind)]
    [InlineData("AAJJJ", HandType.FiveOfAKind)]
    [InlineData("AAJJ4", HandType.FourOfAKind)]
    [InlineData("23456", HandType.HighCard)]
    [InlineData("J3456", HandType.OnePair)]
    [InlineData("32T3K", HandType.OnePair)]
    [InlineData("T55J5", HandType.FourOfAKind)]
    [InlineData("KK677", HandType.TwoPairs)]
    [InlineData("KTJJT", HandType.FourOfAKind)]
    [InlineData("QQQJA", HandType.FourOfAKind)]
    [InlineData("QQJ88", HandType.FullHouse)]
    public void Should_Determine_HandType_With_Jokers(string cards, HandType handType)
    {
        var hand = new Hand(cards, false);
        Assert.Equal(handType, hand.HandType);
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
        var game = new Game(input.Split("\n"), true);
        var totalWinnings = game.GetTotalWinnings();
        Assert.Equal(6440, totalWinnings);
    }
    
    [Fact]
    public void Should_Solve_Second()
    {
        var input = """
                    32T3K 765
                    T55J5 684
                    KK677 28
                    KTJJT 220
                    QQQJA 483
                    """;
        var game = new Game(input.Split("\n"), false);
        var totalWinnings = game.GetTotalWinnings();
        Assert.Equal(5905, totalWinnings);
    }
}