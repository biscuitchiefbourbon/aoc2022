using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    class Day6 : AdventOfCodeChallenge
    {
        
        public Day6(String inputFilename, List<string> exampleInputFilenames) : base(inputFilename, exampleInputFilenames) {

        }

        public override object SolvePartOne(int example) {
            string[] input = GetInput(example);            

            foreach(String s in input) {
                return $"Day 6, Part 1, Marker location: {markerLocation(s, 4)}";
            }
           
            return $"";
        }

        public override object SolvePartTwo(int example) {
            string[] input = GetInput(example);

            foreach (String s in input) {
                return $"Day 6, Part 2, Marker location: {markerLocation(s, 14)}";
            }

            return $"";
        }

        private int markerLocation(String message, int numChars) {
            for (int i = numChars-1; i < message.Length; i++) {
                if (message.Substring(i - numChars+1, numChars).Distinct().Count() == numChars) {
                    return i + 1;
                }
            }
            return -1;
        }

      
        
    }
}
