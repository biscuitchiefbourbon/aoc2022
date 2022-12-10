using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    class Day10 : AdventOfCodeChallenge
    {
        private const int MY_ADAPTER_BOOST = 3;

        public Day10(String inputFilename, List<string> exampleInputFilenames) : base(inputFilename, exampleInputFilenames) {

        }

        public override object SolvePartOne(int example) {

            string[] input = GetInput(example);

            int cycle = 0;
            int x = 1, prevX = 1;
            int[] interestCycles = { 20, 60, 100, 140, 180, 220 };
            int interestCycleIndex = 0;
            int result = 0;

            foreach(String s in input) {
                string[] parts = s.Split(new char[] { ' ' });
                if (parts[0] == "noop") {
                    cycle++;
                    prevX = x;
                } else if (parts[0] == "addx") {
                    cycle += 2;
                    prevX = x;
                    x += int.Parse(parts[1]);                    
                }
                if(interestCycleIndex < interestCycles.Length && cycle >= interestCycles[interestCycleIndex]) {
                    //Console.WriteLine($"Signal strenth during {interestCycles[interestCycleIndex]} = {prevX* interestCycles[interestCycleIndex]}");
                    result += prevX * interestCycles[interestCycleIndex];
                    interestCycleIndex++;
                }
                
            }

            return $"Day 10, Part 1, Result = {result}";
        }




        public override object SolvePartTwo(int example) {
            string[] input = GetInput(example);

            int cycle = 0;
            int x = 1, prevX = 1;
            int crtPosition = 0;

            foreach (String s in input) {
                string[] parts = s.Split(new char[] { ' ' });
                if (parts[0] == "noop") {
                    cycle++;
                    prevX = x;
                    updateCrt(prevX, ref crtPosition);
                } else if (parts[0] == "addx") {
                    cycle += 2;
                    prevX = x;
                    x += int.Parse(parts[1]);
                    updateCrt(prevX, ref crtPosition);
                    updateCrt(prevX, ref crtPosition);
                }

                

               
            }

            return $"Day 10, Part 2, Result ^";
        }

        private void updateCrt(int prevX, ref int crtPosition) {
            if (crtPosition >= (prevX - 1) && crtPosition <= (prevX + 1)) {
                Console.Write("#");
            } else {
                Console.Write(".");
            }
            crtPosition++;
            if(crtPosition % 40 == 0) {
                crtPosition = 0;
                Console.WriteLine();
            }
        }

       
    }
}
