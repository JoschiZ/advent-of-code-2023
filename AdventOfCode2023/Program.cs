using System.Diagnostics;
using AdventOfCode2023._1;
using AdventOfCode2023.DayEight;
using AdventOfCode2023.DayEleven;
using AdventOfCode2023.DayFive;
using AdventOfCode2023.DayFour;
using AdventOfCode2023.DayNine;
using AdventOfCode2023.DaySeven;
using AdventOfCode2023.DaySix;
using AdventOfCode2023.DayTen;
using AdventOfCode2023.DayThree;
using AdventOfCode2023.DayTwelve;
using AdventOfCode2023.DayTwo;

//Console.WriteLine(Day1.SolveDay1First());
//Console.WriteLine(Day1.SolveDay1Second());

//Console.WriteLine(Day2.SolveFirst());
//Console.WriteLine(Day2.SolveSecond());

//Console.WriteLine(Day3.SolveFirst());
//Console.WriteLine(Day3.SolveSecond());

//Console.WriteLine(Day4.SolveFirst());
//Console.WriteLine(Day4.SolveSecond());


//Console.WriteLine("Result: " + Day5.SolveFirst());
//Console.WriteLine("Result: " + Day5Second.Solve());

//Console.WriteLine("Result: " + Day6.SolveFirst());
//Console.WriteLine("Result: " + Day6.SolveSecond());

//Console.WriteLine("Result: " + Day7.SolveFirst());
//Console.WriteLine("Result: " + Day7.SolveSecond());

//Console.WriteLine("Result: " + Day8.SolveFirst());
//Console.WriteLine("Result: " + Day8.SolveSecond());


//Console.WriteLine("Result: " + Day9.SolveFirst());
//Console.WriteLine("Result: " + Day9.SolveSecond());

//Console.WriteLine("Result: " + Day10.SolveFirst());


//Console.WriteLine("Result: " + Day11.SolveFirst());
//Console.WriteLine("Result: " + Day11.SolveSecond());

//Console.WriteLine("Result: " + Day12.SolveFirst());

var input = "#?#???##??#.?#?#?#?#?#???##??#.?#?#?#?#?#???##??#.?#?#?#?#?#???##??#.?#?#?#?#?#???##??#.?#?#?#? 3,3,1,7,3,3,1,7,3,3,1,7,3,3,1,7,3,3,1,7";

var sw = new Stopwatch();
var d12 = new Day12();
sw.Start();
var test = d12.GenerateCombinations(input, 0).ToArray();
Console.WriteLine(sw.Elapsed);
sw.Stop();