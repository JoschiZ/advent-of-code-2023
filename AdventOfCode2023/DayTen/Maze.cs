using System.Diagnostics.Contracts;

namespace AdventOfCode2023.DayTen;

public class Maze
{
    private Position _currentPosition;

    public Maze(List<List<char>> mazeMap)
    {
        MazeMap = mazeMap;
        
        for (var i = 0; i < mazeMap.Count; i++)
        {
            var xIndex = mazeMap[i].IndexOf('S');
            if (xIndex != -1)
            {
                CurrentPosition = new Position(xIndex, i, 'S');
                StartPosition = CurrentPosition;
            }
        }
    }

    public static Maze CreateFromString(string input)
    {
        var mazeMap = input.Split(Environment.NewLine).Select(row => row.ToList()).ToList();
        return new Maze(mazeMap);
    }

    public List<List<char>> MazeMap { get; }

    private Position StartPosition { get; }
    private Position LastPosition { get; set; }
    public Position CurrentPosition
    {
        get => _currentPosition;
        private set
        {
            LastPosition = _currentPosition;
            _currentPosition = value;
        }
    }
    [Pure]
    private Position LookNorth()
    {
        var newY = CurrentPosition.Y - 1;
        var newX = CurrentPosition.X;
        if (newY > MazeMap.Count - 1 || newY < 0 || newX < 0 || newX > MazeMap[newY].Count - 1)
        {
            return LastPosition;
        }
        var newValue = MazeMap[newY][newX];
        
        return new Position(newX, newY, newValue);
    }
    [Pure]
    private Position LookSouth()
    {
        var newY = CurrentPosition.Y + 1;
        var newX = CurrentPosition.X;
        if (newY > MazeMap.Count - 1 || newY < 0 || newX < 0 || newX > MazeMap[newY].Count - 1)
        {
            return LastPosition;
        }
        var newValue = MazeMap[newY][newX];
        return new Position(newX, newY, newValue);
    }
    [Pure]
    private Position LookWest()
    {
        var newY = CurrentPosition.Y;
        var newX = CurrentPosition.X - 1;
        if (newY > MazeMap.Count - 1 || newY < 0 || newX < 0 || newX > MazeMap[newY].Count - 1)
        {
            return LastPosition;
        }
        var newValue = MazeMap[newY][newX];
        return new Position(newX, newY, newValue);
    }
    [Pure]
    private Position LookEast()
    {
        var newY = CurrentPosition.Y;
        var newX = CurrentPosition.X + 1;
        if (newY > MazeMap.Count - 1 || newY < 0 || newX < 0 || newX > MazeMap[newY].Count - 1)
        {
            return LastPosition;
        }
        var newValue = MazeMap[newY][newX];
        return new Position(newX, newY, newValue);
    }
    [Pure]
    private Position DecideNext(Position first, Position other)
    {
        return first == LastPosition ? other : first;
    }

    private Position MakeFirstMove()
    {
        if (CurrentPosition != StartPosition)
        {
            throw new Exception("This Move would be invalid!");
        }
        
        var north = LookNorth();
        if (north.Character is '|' or '7' or 'F')
        {
            CurrentPosition = north;
            return CurrentPosition;
        }
        var south = LookSouth();
        if (south.Character is '|' or 'L' or 'J')
        {
            CurrentPosition = south;
            return CurrentPosition;
        }
        var east = LookEast();
        if (east.Character is '-' or 'J' or '7')
        {
            CurrentPosition = east;
            return CurrentPosition;
        }
        
        var west = LookWest();
        if (west.Character is '-' or 'L' or 'F')
        {
            CurrentPosition = west;
            return CurrentPosition;
        }

        throw new Exception("This should not be possible");
    }

    public Position MoveNext()
    {
        if (CurrentPosition == StartPosition)
        {
            return MakeFirstMove();
        }
        
        var nextPosition = CurrentPosition.Character switch
        {
            '|' => DecideNext(LookNorth(), LookSouth()),
            '-' => DecideNext(LookEast(), LookWest()),
            'L' => DecideNext(LookNorth(), LookEast()),
            'J' => DecideNext(LookNorth(), LookWest()),
            '7' => DecideNext(LookSouth(), LookWest()),
            'F' => DecideNext(LookSouth(), LookEast()),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        CurrentPosition = nextPosition;
        return CurrentPosition;
    }

    public IEnumerable<Position> NavigateFullCircle()
    {
        yield return CurrentPosition;
        
        while (true)
        {
            var nextPosition = MoveNext();

            if (nextPosition == StartPosition)
            {
                break;
            }

            yield return nextPosition;
        }
    }
}

public record struct Position(int X, int Y, char Character);