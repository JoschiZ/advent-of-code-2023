using System.Collections.Frozen;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using AdventOfCode2023.Core;

namespace AdventOfCode2023.DayEight;
using Node = (string left, string right);

public partial class Day8
{
    [GeneratedRegex(@"[A-Z|1-9]{3}")]
    private static partial Regex MatchNodes();
    // Note before starting First: I thought about using a directed graph, but I think just building and traversing a simple dict
    // is the quicker solution. Let's see how hard that bites us on the second question

    private static Stopwatch _stopwatch = new Stopwatch();

    public static int SolveFirst()
    {
        var inputFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "DayEight/First.txt"));
        var (directions, nodes) = ParseInput(inputFile.GetInputStrings());
        var steps = TraverseTree(directions, nodes);
        return steps;
    }
    
    public static int SolveSecond()
    {
        _stopwatch.Start();
        var inputFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "DayEight/First.txt"));
        var (directions, nodes) = ParseInput(inputFile.GetInputStrings());
        var steps = TraverseTreeSimultaniously(directions, nodes);
        return steps;
    }
    
    
    private static int TraverseTree(string directions, IReadOnlyDictionary<string, Node> nodes)
    {
        var currentNode = "AAA";
        var stepCounter = 0;
        
        while (currentNode != "ZZZ")
        {
            foreach (var direction in directions)
            {
                stepCounter++;
                currentNode = direction switch
                {
                    'R' => nodes[currentNode].right,
                    'L' => nodes[currentNode].left,
                    _ => throw new ArgumentOutOfRangeException()
                };

                if (currentNode == "ZZZ")
                {
                    break;
                }
            }
        }

        return stepCounter;
    }
    
    private static int TraverseTreeSimultaniously(string directions, IReadOnlyDictionary<string, Node> nodes)
    {
        var currentNodes = nodes.Keys.Where(s => s.EndsWith('A')).ToArray();
        var stepCounter = 0;
        
        while (currentNodes.Any(s => !s.EndsWith('Z')))
        {
            foreach (var direction in directions)
            {
                stepCounter++;

                currentNodes = currentNodes.AsParallel().Select(currentNode =>
                {
                    return direction switch
                    {
                        'R' => nodes[currentNode].right,
                        'L' => nodes[currentNode].left,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                }).ToArray();

                if (currentNodes.All(s => s.EndsWith('Z')))
                {
                    break;
                }
            }
            Console.WriteLine("RetracingPath");
            Console.WriteLine("Current Steps: " + stepCounter);
            Console.WriteLine(_stopwatch.Elapsed);
        }

        return stepCounter;
    }

    private static (string directions, FrozenDictionary<string, Node> nodeDict) ParseInput(IEnumerable<string> input)
    {
        const string directions = "LRRRLRRLRRLRLRRLRRRLLRRLLRRLRRRLRLRRLLRRLRRLRLRRRLRRRLRLRLRLRRRLRRLRRRLRLRRLLLRLRLLRLRRRLRLRRRLRRRLLLRRLRLRRLRRRLLRRLRRLRRLRRRLRRLRRLRRLRLRRLRLRLRLRLRLRRRLRRLRLLLRRRLRLRRRLRRRLLRRLRRRLRRLRRRLRRRLRLRRRLRRLRLLRRLLRLRRLRLRLLRRLLRRLLRRLRRLRRRLRLRRLRLRRRLRRRLLRLRRLLLLRRRLLRLLLRRLRRRLRRRLRRRLRLRRRLRRRLRRRLRLRRRR";
        FrozenDictionary<string, Node> outDict;
        

        outDict = BuildNodeDictionary(input);
        return (directions, outDict);
    }
    
    
    [Pure]
    private static FrozenDictionary<string, Node> BuildNodeDictionary(IEnumerable<string> input)
    {
        return input.Where(s => !string.IsNullOrWhiteSpace(s)).Select(ParseEntryLine).ToFrozenDictionary();
    }
    
    [Pure]
    private static KeyValuePair<string, Node> ParseEntryLine(string input)
    {
        var nodeCodes = MatchNodes().Matches(input).Select(match => match.ToString()).ToArray();
        return new KeyValuePair<string, Node>(nodeCodes[0], (nodeCodes[1], nodeCodes[2]));
    }
}