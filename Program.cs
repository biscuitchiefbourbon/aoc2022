using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    class Program
    {
        static void Main(string[] args) {
            Day1 day1 = new Day1("Inputs\\Day1_Input.txt", new List<String> { "Inputs\\Day1_Example.txt" });
            Console.WriteLine(day1.SolvePartOne(0));
            Console.WriteLine(day1.SolvePartTwo(0));

            Day2 day2 = new Day2("Inputs\\Day2_Input.txt", new List<String> { "Inputs\\Day2_Example.txt" });
            Console.WriteLine(day2.SolvePartOne(0));
            Console.WriteLine(day2.SolvePartTwo(0));

            Day3 day3 = new Day3("Inputs\\Day3_Input.txt", new List<String> { "Inputs\\Day3_Example.txt" });
            Console.WriteLine(day3.SolvePartOne(0));
            Console.WriteLine(day3.SolvePartTwo(0));

            Day4 day4 = new Day4("Inputs\\Day4_Input.txt", new List<String> { "Inputs\\Day4_Input.txt" });
            Console.WriteLine(day4.SolvePartOne(0));
            Console.WriteLine(day4.SolvePartTwo(0));

            Day5 day5 = new Day5("Inputs\\Day5_Input.txt", new List<String> { "Inputs\\Day5_Example.txt" });
            Console.WriteLine(day5.SolvePartOne(0));
            Console.WriteLine(day5.SolvePartTwo(0));

            Day6 day6 = new Day6("Inputs\\Day6_Input.txt", new List<String> { "Inputs\\Day6_Example.txt" });
            Console.WriteLine(day6.SolvePartOne(0));
            Console.WriteLine(day6.SolvePartTwo(0));

            Day7 day7 = new Day7("Inputs\\Day7_Input.txt", new List<String> { "Inputs\\Day7_Example.txt" });
            Console.WriteLine($"{day7.SolvePartOne(1)} (Example)");
            Console.WriteLine($"{day7.SolvePartTwo(1)} (Example)");
            Console.WriteLine($"{day7.SolvePartOne(0)} (Actual)");
            Console.WriteLine($"{day7.SolvePartTwo(0)} (Actual)");

            //Day8 day8 = new Day8("Inputs\\Day8_Input.txt", new List<String> { "Inputs\\Day8_Example.txt" });
            //Console.WriteLine(day8.SolvePartOne(0));
            //Console.WriteLine(day8.SolvePartTwo(0));

            //Day9 day9 = new Day9("Inputs\\Day9_Input.txt", new List<String> { "Inputs\\Day9_Example.txt" });
            //Console.WriteLine(day9.SolvePartOne(0));
            //Console.WriteLine(day9.SolvePartTwo(0));

            //Day10 day10 = new Day10("Inputs\\Day10_Input.txt", new List<String> { "Inputs\\Day10_Example1.txt" });
            //Console.WriteLine(day10.SolvePartOne(0));
            //Console.WriteLine(day10.SolvePartTwo(0));

            //Day11 day11 = new Day11("Inputs\\Day11_Input.txt", new List<String> { "Inputs\\Day11_Example.txt" });
            //Console.WriteLine(day11.SolvePartOne(0));
            //Console.WriteLine(day11.SolvePartTwo(0));

            //Day12 Day12 = new Day12("Inputs\\Day12_Input.txt", new List<String> { "Inputs\\Day12_Example.txt" });
            //Console.WriteLine(Day12.SolvePartOne(0));
            //Console.WriteLine(Day12.SolvePartTwo(0));

            //Day13 Day13 = new Day13("Inputs\\Day13_Input.txt", new List<String> { "Inputs\\Day13_Example.txt" });
            //Console.WriteLine(Day13.SolvePartOne(0));
            //Console.WriteLine(Day13.SolvePartTwo(0));

            //Day14 Day14 = new Day14("Inputs\\Day14_Input.txt", new List<String> { "Inputs\\Day14_Example.txt" });
            //Console.WriteLine(Day14.SolvePartOne(0));
            //Console.WriteLine(Day14.SolvePartTwo(0));

            //Day15 Day15 = new Day15("Inputs\\Day15_Input.txt", new List<String> { "Inputs\\Day15_Example.txt" });
            //Console.WriteLine(Day15.SolvePartOne(0));
            //Console.WriteLine(Day15.SolvePartTwo(0));

            //Day16 Day16 = new Day16("Inputs\\Day16_Input.txt", new List<String> { "Inputs\\Day16_Example.txt" });
            //Console.WriteLine(Day16.SolvePartOne(0));
            //Console.WriteLine(Day16.SolvePartTwo(0));

            //Day17 Day17 = new Day17("Inputs\\Day17_Input.txt", new List<String> { "Inputs\\Day17_Example.txt" });
            //Console.WriteLine(Day17.SolvePartOne(0));
            //Console.WriteLine(Day17.SolvePartTwo(0));

            //Day18 Day18 = new Day18("Inputs\\Day18_Input.txt", new List<String> { "Inputs\\Day18_Example.txt" });
            //Console.WriteLine(Day18.SolvePartOne(0));
            //Console.WriteLine(Day18.SolvePartTwo(0));

            //Day19 Day19 = new Day19("Inputs\\Day19_Input.txt", new List<String> { "Inputs\\Day19_Example.txt"});
            //Console.WriteLine(Day19.SolvePartOne(0));
            //Console.WriteLine(Day19.SolvePartTwo(2));


            //Day20 Day20 = new Day20("Inputs\\Day20_Input.txt", new List<String> { "Inputs\\Day20_Example.txt" });
            //Console.WriteLine(Day20.SolvePartOne(0));
            //Console.WriteLine(Day20.SolvePartTwo(0));

            //Day21 Day21 = new Day21("Inputs\\Day21_Input.txt", new List<String> { "Inputs\\Day21_Example.txt" });
            //Console.WriteLine(Day21.SolvePartOne(0));
            //Console.WriteLine(Day21.SolvePartTwo(0));

            //Day22 Day22 = new Day22("Inputs\\Day22_Input.txt", new List<String> { "Inputs\\Day22_Example.txt" });
            //Console.WriteLine(Day22.SolvePartOne(0));
            //Console.WriteLine(Day22.SolvePartTwo(0));

            //Day23 Day23 = new Day23("Inputs\\Day23_Input.txt", new List<String> { "Inputs\\Day23_Example.txt" });
            //Console.WriteLine(Day23.SolvePartOne(0));
            //Console.WriteLine(Day23.SolvePartTwo(0));

            //Day24 Day24 = new Day24("Inputs\\Day24_Input.txt", new List<String> { "Inputs\\Day24_Example.txt" });
            //Console.WriteLine(Day24.SolvePartOne(0));
            //Console.WriteLine(Day24.SolvePartTwo(0));

            //Day25 Day25 = new Day25("Inputs\\Day25_Input.txt", new List<String> { "Inputs\\Day25_Example.txt" });
            //Console.WriteLine(Day25.SolvePartOne(0));
            //Console.WriteLine(Day25.SolvePartTwo(1));


            Console.ReadLine();
        }
    }
}
