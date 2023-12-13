namespace AdventOfCode2023.DayEleven;

// Note and idea for second one.
// We cannot just add that many rows / columns. So we could create a lookup table to modify the index.
// Like "Everything above 3 + 1 million, Everything above 4 + 1 million" and so on


public class Image
{
    private Image(List<List<char>> baseImage, int expandFactor)
    {
        BaseImage = baseImage;
        ExpandFactor = expandFactor;
        ExpandedImage = Expand();

        var expandParams = GetExpandParameters();
        RowsToExpand = expandParams.rows;
        ColumnsToExpand = expandParams.columns;
    }

    public List<List<char>> BaseImage { get; }
    public List<List<char>> ExpandedImage { get; }

    public List<int> RowsToExpand { get; }
    public List<int> ColumnsToExpand { get; }

    public int ExpandFactor { get; }

    public static Image CreateFromString(string input, int expandFactor = 1)
    {
        var baseImage = input.Split('\n').Select(s => s.Where(c => c is '.' or '#').ToList()).ToList();
        return new Image(baseImage, expandFactor);
    }

    public Coordinate ExpandCoordinate(Coordinate coordinate)
    {
        var newRowIndex = coordinate.Row;

        foreach (var rowIndex in RowsToExpand)
        {
            if (coordinate.Row > rowIndex)
            {
                newRowIndex += ExpandFactor;
            }
        }
        
        
        var newColumnIndex = coordinate.Column;
        
        foreach (var columnIndex in ColumnsToExpand)
        {
            if (coordinate.Column > columnIndex)
            {
                newColumnIndex += ExpandFactor;
            }
        }

        return new Coordinate(newColumnIndex, newRowIndex);
    }

    private IEnumerable<Coordinate> GetAllCoordinates()
    {
        var rowIndex = 0;
        foreach (var row in BaseImage)
        {
            var galaxies = Enumerable.Range(0, row.Count).Where(i => row[i] == '#');
            foreach (var column in galaxies)
            {
                yield return new Coordinate(column, rowIndex);
            }

            rowIndex++;
        }
    }

    private long GetDistance(Coordinate first, Coordinate second)
    {
        var expandedFirst = ExpandCoordinate(first);
        var expandedSecond = ExpandCoordinate(second);
        
        return Math.Abs(expandedSecond.Column - expandedFirst.Column) + Math.Abs(expandedSecond.Row - expandedFirst.Row);
    }

    private IEnumerable<long> GetAllDistances(IEnumerable<Coordinate> coordinates)
    {
        coordinates = coordinates.ToArray();
        return coordinates.SelectMany(
            (_, i) => coordinates.Skip(i + 1),
            GetDistance);
        
    }

    public IEnumerable<long> Solve()
    {
        var coordinates = GetAllCoordinates();
        return GetAllDistances(coordinates);
    }

    private List<List<char>> Expand()
    {
        var expandedImage = BaseImage.Select(list => list.ToList()).ToList();
        
        // expand horizontal
        var rowsToExpand = BaseImage.Where(list => !list.Contains('#')).Select(list => BaseImage.IndexOf(list));
        var emptyRow = Enumerable.Repeat('.', BaseImage.First().Count).ToList();
        foreach (var rowIndex in rowsToExpand.Reverse())
        {
            expandedImage.Insert(rowIndex, emptyRow);
        }
        
        
        //expand vertical
        //var toExpandColumns = expandedImage.First().Where(c => c == '.').ToHashSet();
        var toExpandColumns = Enumerable
            .Range(0, expandedImage.First().Count)
            .Where(i => expandedImage.First()[i] == '.')
            .ToHashSet();

        foreach (var expandedRow in expandedImage)
        {
            var nextIndexSet = Enumerable
                .Range(0, expandedRow.Count)
                .Where(i => expandedRow[i] == '.')
                .ToHashSet();

            toExpandColumns.IntersectWith(nextIndexSet);
        }

        foreach (var row in expandedImage)
        {
            foreach (var columnIndex in toExpandColumns.Reverse())
            {
                row.Insert(columnIndex, '.');
            }
        }

        return expandedImage;
    }
    
    private (List<int> rows, List<int> columns) GetExpandParameters()
    {
        var expandedImage = BaseImage.Select(list => list.ToList()).ToList();
        
        // expand horizontal
        var rowsToExpand = BaseImage.Where(list => !list.Contains('#')).Select(list => BaseImage.IndexOf(list));
        
        
        //expand vertical
        //var toExpandColumns = expandedImage.First().Where(c => c == '.').ToHashSet();
        var toExpandColumns = Enumerable
            .Range(0, expandedImage.First().Count)
            .Where(i => expandedImage.First()[i] == '.')
            .ToHashSet();

        foreach (var expandedRow in expandedImage)
        {
            var nextIndexSet = Enumerable
                .Range(0, expandedRow.Count)
                .Where(i => expandedRow[i] == '.')
                .ToHashSet();

            toExpandColumns.IntersectWith(nextIndexSet);
        }

        var rowIndexes = rowsToExpand.ToList();
        rowIndexes.Sort();
        var columnIndexes = toExpandColumns.ToList();
        columnIndexes.Sort();
        return (rowIndexes, columnIndexes);
    }
}

public record struct Coordinate(long Column, long Row) { }