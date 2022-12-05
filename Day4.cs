using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    class Day4 : AdventOfCodeChallenge
    {
        public Day4(String inputFilename, List<string> exampleInputFilenames) : base(inputFilename, exampleInputFilenames) {

        }

        public override object SolvePartOne(int example) {
            string[] input = GetInput(example);

            int numContainingOthers = 0;
            
            foreach(String s in input) {
                string[] elves = s.Split(new char[] { ',' });
                string[] elf1 = elves[0].Split(new char[] { '-' });
                string[] elf2 = elves[1].Split(new char[] { '-' });

                int elf1Min = int.Parse(elf1[0]);
                int elf1Max = int.Parse(elf1[1]);
                int elf2Min = int.Parse(elf2[0]);
                int elf2Max = int.Parse(elf2[1]);

                if(elf1Min >= elf2Min && elf1Max <= elf2Max) {
                    // 2 contains 1
                    numContainingOthers++;
                }else if( elf2Min >= elf1Min && elf2Max <= elf1Max) {
                    // 1 contains 2
                    numContainingOthers++;
                }
            }

            return $"Day 4 Part 1, Answer = {numContainingOthers}";
        }

        public override object SolvePartTwo(int example) {
            string[] input = GetInput(example);

            int numOverlaps = 0;

            foreach (String s in input) {
                string[] elves = s.Split(new char[] { ',' });
                string[] elf1 = elves[0].Split(new char[] { '-' });
                string[] elf2 = elves[1].Split(new char[] { '-' });

                int elf1Min = int.Parse(elf1[0]);
                int elf1Max = int.Parse(elf1[1]);
                int elf2Min = int.Parse(elf2[0]);
                int elf2Max = int.Parse(elf2[1]);

                if (elf1Min >= elf2Min && elf1Max <= elf2Max) {
                    // 2 contains 1
                    numOverlaps++;
                } else if (elf2Min >= elf1Min && elf2Max <= elf1Max) {
                    // 1 contains 2
                    numOverlaps++;
                }else if( elf2Min <= elf1Max && elf2Min >= elf1Min) {
                    // 2 overlaps the right of 1
                    numOverlaps++;
                }else if(elf2Max >= elf1Min && elf2Max <= elf1Max) {
                    // 2 overlaps the left of 1
                    numOverlaps++;
                }
            }

            return $"Day 4 Part 2, Answer = {numOverlaps}";
        }

      
    }
}
