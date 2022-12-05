using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    class Day2 : AdventOfCodeChallenge
    {
        public Day2(String inputFilename, List<string> exampleInputFilenames) : base(inputFilename, exampleInputFilenames) {

        }

        public override object SolvePartOne(int example) {
            string[] input = GetInput(example);

            long score = 0;

            foreach (String s in input) {
                switch (s[0]) {
                    case 'A': // Rock
                        switch (s[2]) {
                            case 'X':// Rock
                                score += 1 + 3;
                                break;
                            case 'Y':// Paper
                                score += 2 + 6;
                                break;
                            case 'Z':// Scissors
                                score += 3;
                                break;
                        }
                        break;
                    case 'B': // Paper
                        switch (s[2]) {
                            case 'X':// Rock
                                score += 1;
                                break;
                            case 'Y':// Paper
                                score += 2 + 3;
                                break;
                            case 'Z':// Scissors
                                score += 3 + 6;
                                break;
                        }
                        break;
                    case 'C': // Scissors
                        switch (s[2]) {
                            case 'X':// Rock
                                score += 1 + 6; 
                                break;
                            case 'Y':// Paper
                                score += 2;
                                break;
                            case 'Z':// Scissors
                                score += 3 + 3;
                                break;
                        }
                        break;
                }
            }

            return $"Day 2, Part 1, Score = {score}";
        }

        public override object SolvePartTwo(int example) {
            string[] input = GetInput(example);

            long score = 0;
    

            foreach (String s in input) {
                switch (s) {
                    case "A X":
                        // Opponent rock, Lose by playing scissors
                        score += 3; break;
                    case "A Y":
                        // Opponent rock, Draw by playing rock
                        score += 1 + 3;break;
                    case "A Z":
                        // Opponent rock, win by playing paper
                        score += 2 + 6; break;
                    case "B X":
                        // Opponent paper, Lose by playing rock
                        score += 1; break;
                    case "B Y":
                        // Opponent paper, Draw by playing paper
                        score += 2 + 3; break;
                    case "B Z":
                        // Opponent paper, win by playing scissors
                        score += 3 + 6; break;
                    case "C X":
                        // Opponent scissors, Lose by playing paper
                        score += 2; break;
                    case "C Y":
                        // Opponent scissors, Draw by playing scissors
                        score += 3 + 3; break;
                    case "C Z":
                        // Opponent scissors, win by playing rock
                        score += 1 + 6; break;


                }


             
            }

            return $"Day 2, Part 1, Score = {score}";
        }
    }
}
