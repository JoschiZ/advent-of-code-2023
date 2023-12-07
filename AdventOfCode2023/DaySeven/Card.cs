namespace AdventOfCode2023.DaySeven;

public partial class Day7
{
    public class Card: IComparable<Card>, IEquatable<Card>
    {
        public Card(char label)
        {
            Label = label;
        }

        public static Dictionary<char, int> CardValues { get; } = new Dictionary<char, int>()
        {
            { 'A', 13 },
            { 'K', 12 },
            { 'Q', 11 },
            { 'J', 10 },
            { 'T', 9 },
            { '9', 8 },
            { '8', 7 },
            { '7', 6 },
            { '6', 5 },
            { '5', 4 },
            { '4', 3 },
            { '3', 2 },
            { '2', 1 },
        }; 
        
        public char Label { get; }
        public int Value => CardValues[Label];
        
        public int CompareTo(Card? other)
        {
            if (other is null)
            {
                return 1;
            }
            
            return Value - other.Value;
        }

        public bool Equals(Card? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Label == other.Label;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Card)obj);
        }

        public override int GetHashCode()
        {
            return Label.GetHashCode();
        }
    }
}