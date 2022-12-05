using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    class Day5 : AdventOfCodeChallenge
    {
        
        public Day5(String inputFilename, List<string> exampleInputFilenames) : base(inputFilename, exampleInputFilenames) {

        }

        public override object SolvePartOne(int example) {
            string[] input = GetInput(example);

            int numStacks = (input[0].Length + 1) / 4;
            List<Stack<char>> stacks = new List<Stack<char>>();
            for(int i = 0; i < numStacks; i++) {
                stacks.Add(new Stack<char>());
            }

            foreach(String s in input) {
                if (s.StartsWith("move")) {
                    string[] commandParts = s.Split(new char[] { ' ' });
                    int numCrates = int.Parse(commandParts[1]);
                    int sourceStack = int.Parse(commandParts[3]);
                    int destStack = int.Parse(commandParts[5]);

                    for(int i = 0; i < numCrates; i++) {
                        stacks[destStack - 1].Push(stacks[sourceStack - 1].Pop());
                    }
                } else if (s.StartsWith(" 1")) {
                    //End of the crates, reverse the stacks
                    for (int i = 0; i < stacks.Count; i++) {
                        stacks[i] = new Stack<char>(stacks[i].ToList());
                    }

                } else if(s.Length == 0) {

                } else {
                    for(int i = 0; i < numStacks; i++) {
                        char crate = s[4 * i + 1];
                        if(crate!= ' ') {
                            stacks[i].Push(crate);
                        }
                    }
                }
            }

            String answer = "";
            for(int i = 0; i < stacks.Count; i++) {
                answer += stacks[i].Pop();
            }


            return $"Day 5, Part 1: {answer}";
        }

        public override object SolvePartTwo(int example) {
            string[] input = GetInput(example);

            int numStacks = (input[0].Length + 1) / 4;
            List<Stack<char>> stacks = new List<Stack<char>>();
            for (int i = 0; i < numStacks; i++) {
                stacks.Add(new Stack<char>());
            }

            foreach (String s in input) {
                if (s.StartsWith("move")) {
                    string[] commandParts = s.Split(new char[] { ' ' });
                    int numCrates = int.Parse(commandParts[1]);
                    int sourceStack = int.Parse(commandParts[3]);
                    int destStack = int.Parse(commandParts[5]);

                    Stack<char> temp = new Stack<char>();
                    for (int i = 0; i < numCrates; i++) {
                        temp.Push(stacks[sourceStack - 1].Pop());
                    }
                    while (temp.Count > 0) {
                        stacks[destStack - 1].Push(temp.Pop());
                    }
                } else if (s.StartsWith(" 1")) {
                    //End of the crates, reverse the stacks
                    for (int i = 0; i < stacks.Count; i++) {
                        stacks[i] = new Stack<char>(stacks[i].ToList());
                    }

                } else if (s.Length == 0) {

                } else {
                    for (int i = 0; i < numStacks; i++) {
                        char crate = s[4 * i + 1];
                        if (crate != ' ') {
                            stacks[i].Push(crate);
                        }
                    }
                }
            }

            String answer = "";
            for (int i = 0; i < stacks.Count; i++) {
                answer += stacks[i].Pop();
            }


            return $"Day 5, Part 2: {answer}";

        }

        
    }
}
