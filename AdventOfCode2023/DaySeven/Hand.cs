using System.Diagnostics;
using System.Xml;

namespace AdventOfCode2023.DaySeven;

public partial class Day7
{
    [DebuggerDisplay($"{nameof(_cardString)} = {{{nameof(_cardString)}}}")]
    public class Hand: IEquatable<Hand>, IComparable<Hand>
    {
        public Hand(string cards, bool ignoreJokers, int bet = 0)
        {
            _cardString = cards;
            Bet = bet;
            var cardCounts = new Dictionary<char, int>();
            int? jokerOverride = ignoreJokers ? 10 : null;
            
            foreach (var card in cards)
            {
                cardCounts.TryAdd(card, 0);
                cardCounts[card] += 1;
                Cards.Add(new Card(card, jokerOverride));       
            }

            HandType = GetHandType(cardCounts, ignoreJokers);
            if (_cardString.Contains('J'))
            {
                var sorted = _cardString.Select(c => (int)c).OrderDescending().Select(i => (char)i).ToList();
                Console.WriteLine($"{string.Join("", sorted)} -> {HandType}");
            }
        }

        private readonly string _cardString;

        private static HandType GetHandType(Dictionary<char, int> cards, bool ignoreJokers)
        {

            if (ignoreJokers || !cards.TryGetValue('J', out var jokerCount))
            {
                // Without any jokers we can use the old logic
                var rawCounts = cards.Values.OrderDescending().ToList();
                return SimpleMatcher(rawCounts);
            }

            cards['J'] = 0;
            var counts = cards.Values.OrderDescending().ToList();
            counts[0] += jokerCount;

            return SimpleMatcher(counts);

            

            HandType SimpleMatcher(List<int> cardCounts)
            {
                 return cardCounts switch
                {
                    [5, ..] => HandType.FiveOfAKind,
                    [4, ..] => HandType.FourOfAKind,
                    [3, 2, ..] => HandType.FullHouse,
                    [3, ..] => HandType.ThreeOfAKind,
                    [2, 2, ..] => HandType.TwoPairs,
                    [2, ..] => HandType.OnePair,
                    [1, 1, 1, 1, 1, ..] => HandType.HighCard,
                    _ => throw new ArgumentOutOfRangeException(nameof(cards))
                };
            }
        }

        public static Hand CreateHand(string input, bool ignoreJokers)
        {
            // Example Input 32T3K 765
            var splited = input.Split(" ");
            return new Hand(splited[0], ignoreJokers, int.Parse(splited[1]));
        }
        
        public HandType HandType { get; }

        public List<Card> Cards { get; } = new(5);
        
        public int Bet { get; }

        public bool Equals(Hand? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Cards.Equals(other.Cards);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Hand)obj);
        }

        public override int GetHashCode()
        {
            return Cards.GetHashCode();
        }

        public int CompareTo(Hand? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            if (HandType != other.HandType)
            {
                return HandType.CompareTo(other.HandType);
            }

            for (var i = 0; i < 5; i++)
            {
                var card = Cards[i];
                var otherCard = other.Cards[i];
                var cardComparison = card.CompareTo(otherCard);
                if (cardComparison != 0)
                {
                    return cardComparison;
                }
            }

            return 0;
        }
    }
    
    
    
    public enum HandType
    {
        HighCard,
        OnePair,
        TwoPairs,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind
    }
}