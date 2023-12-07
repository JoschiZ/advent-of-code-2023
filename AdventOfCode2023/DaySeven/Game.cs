using System.Collections.Specialized;
using System.Xml;

namespace AdventOfCode2023.DaySeven;


public partial class Day7
{
    public class Game
    {
        public SortedList<Hand, Hand> Hands { get; } = new();

        public Game(IEnumerable<string> input)
        {
            foreach (var inputString in input)
            {
                var newHand = Hand.CreateHand(inputString);
                Hands.Add(newHand, newHand);
            }
        }

        public int GetTotalWinnings()
        {

            var rank = 1;
            var winnings = 0;
            foreach (var (_, hand) in Hands)
            {
                winnings += rank * hand.Bet;
                rank++;
            }

            return winnings;
        }
    }
}