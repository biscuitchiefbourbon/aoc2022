using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    class Day3 : AdventOfCodeChallenge
    {
        public Day3(String inputFilename, List<string> exampleInputFilenames) : base(inputFilename, exampleInputFilenames) {

        }

        public override object SolvePartOne(int example) {
            string[] input = GetInput(example);
            int prioritySum = 0;

            foreach (String s in input) {
                List<char> left = new List<char>();
                List<char> right = new List<char>();

                for (int i = 0; i < s.Length; i++) {
                    if (i < s.Length / 2) {
                        // Left
                        left.Add(s[i]);
                    } else {
                        // Right
                        right.Add(s[i]);
                    }
                }

                List<char> matches = left.Where(l => right.Contains(l)).ToList();

                int priority = Char.IsLower(matches[0]) ? (int)matches[0] - 96 : (int)matches[0] - 38;
                prioritySum += priority;
            }

            return $"Day 3, part 1. Priority sum = {prioritySum}";
        }

        public override object SolvePartTwo(int example) {
            string[] input = GetInput(example);
            int prioritySum = 0;

            List<char> rucksack1 = new List<char>();
            List<char> rucksack2 = new List<char>();
            List<char> rucksack3 = new List<char>();

            int rucksackNo = 1;

            foreach (String s in input) {
                for (int i = 0; i < s.Length; i++) {
                    if (rucksackNo == 1) {
                        rucksack1.Add(s[i]);
                    }else if(rucksackNo == 2) {
                        rucksack2.Add(s[i]);
                    } else {
                        rucksack3.Add(s[i]);
                    }
                }
                rucksackNo++;
                if(rucksackNo > 3) {
                    List<char> matches =  rucksack1.Where(r1 => rucksack2.Contains(r1)).Where(r1r2 => rucksack3.Contains(r1r2)).ToList();
                    
                    int priority = Char.IsLower(matches[0]) ? (int)matches[0] - 96 : (int)matches[0] - 38;
                    prioritySum += priority;

                    rucksackNo = 1;
                    rucksack1 = new List<char>();
                    rucksack2 = new List<char>();
                    rucksack3 = new List<char>();
                }
            }


            return $"Day 3, part 2. Priority sum = {prioritySum}";
        }
    }
}
