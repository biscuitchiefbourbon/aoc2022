using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    class Day1 : AdventOfCodeChallenge
    {
        public Day1(String inputFilename, List<string> exampleInputFilenames): base(inputFilename, exampleInputFilenames) {

        }

        public override object SolvePartOne(int example) {
            string[] input = GetInput(example);

            List<List<long>> caloriesByElf = new List<List<long>>();
            List<long> calorieTotalsByElf = new List<long>();

            Calculate(input, caloriesByElf, calorieTotalsByElf);

            return $"Day 1, part 1. Maximum calories carried: {calorieTotalsByElf.Max()}";
        }

       

        public override object SolvePartTwo(int example) {
            string[] input = GetInput(example);

            List<List<long>> caloriesByElf = new List<List<long>>();
            List<long> calorieTotalsByElf = new List<long>();

            Calculate(input, caloriesByElf, calorieTotalsByElf);
            
            long total = 0;

            for(int i = 0; i < 3; i++) {
                long max = calorieTotalsByElf.Max();
                total += max;
                calorieTotalsByElf.Remove(max);
            }


            return $"Day 1, part 2. Top 3 calories carried: {total}";
        }

        private static void Calculate(string[] input, List<List<long>> caloriesByElf, List<long> calorieTotalsByElf) {
            List<long> calorieList = new List<long>();
            foreach (String s in input) {
                if (s.Length == 0) {
                    caloriesByElf.Add(calorieList);
                    calorieTotalsByElf.Add(calorieList.Sum());
                    calorieList = new List<long>();
                } else {
                    calorieList.Add(long.Parse(s));
                }
            }
            if (calorieList.Count > 0) {
                caloriesByElf.Add(calorieList);
                calorieTotalsByElf.Add(calorieList.Sum());
            }
        }
    }
}
