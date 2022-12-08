using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    class Day8 : AdventOfCodeChallenge
    {

        public Day8(String inputFilename, List<string> exampleInputFilenames) : base(inputFilename, exampleInputFilenames) {

        }

        public override object SolvePartOne(int example) {
            string[] input = GetInput(example);

            Tree[,] trees = new Tree[input[0].Length, input.Length];
            for(int y = 0; y < input.Length; y++) {
                for(int x = 0; x < input[y].Length; x++) {
                    trees[x, y] = new Tree(x,y,int.Parse(input[y][x].ToString()));
                }
            }

            int visible = 0;
            for(int x = 0; x <= trees.GetUpperBound(0); x++) {
                for(int y = 0; y <= trees.GetUpperBound(1); y++) {
                    if (trees[x, y].CalculateVisibility(trees)){
                        visible++;
                    }
                }
            }
            
          
            return $"Day 8, Part 1: {visible} trees visible";
        }



        public override object SolvePartTwo(int example) {

            string[] input = GetInput(example);

            Tree[,] trees = new Tree[input[0].Length, input.Length];
            for (int y = 0; y < input.Length; y++) {
                for (int x = 0; x < input[y].Length; x++) {
                    trees[x, y] = new Tree(x, y, int.Parse(input[y][x].ToString()));
                }
            }

            int bestScenicScore = 0;
            for (int x = 0; x <= trees.GetUpperBound(0); x++) {
                for (int y = 0; y <= trees.GetUpperBound(1); y++) {
                    trees[x, y].CalculateVisibility(trees);
                    if (trees[x,y].ScenicScore > bestScenicScore) {
                        bestScenicScore = trees[x,y].ScenicScore;
                    }   
                }
            }


            return $"Day 8, Part 2: Best scenic score: {bestScenicScore}";
        }

        private class Tree {
            [Flags]
            public enum VisibilityFlags {
                None = 0,
                Top = 1,
                Bottom = 2,
                Left = 4,
                Right = 8
            }

            public VisibilityFlags Visibility { get; set; }

            public int Height { get; set; }

            public int X { get; set; }

            public int Y { get; set; }

            public int ScenicScore { get; set; }

            public Tree(int x, int y, int height) {
                Visibility = VisibilityFlags.None;
                Height = height;    
                X = x;
                Y = y;
            }

            public bool CalculateVisibility(Tree[,] trees) {
                if(X==2 && Y == 3) {
                    Debugger.Break();
                }

                bool left = true;
                int leftViewingDistance = 0;
                int check = X;
                while (check > 0) {
                    leftViewingDistance++;
                    if (trees[--check, Y].Height >= Height) {
                        left = false;
                        break;
                    }
                }
                if (left) Visibility |= Tree.VisibilityFlags.Left;

                bool right = true;
                int rightViewingDistance = 0;
                check = X;
                while (check < trees.GetUpperBound(0)) {
                    rightViewingDistance++;
                    if (trees[++check, Y].Height >= Height) {
                        right = false;
                        break;
                    }
                }
                if (right) Visibility |= Tree.VisibilityFlags.Right;

                bool top = true;
                int topViewingDistance = 0;
                check = Y;
                while (check > 0) {
                    topViewingDistance++;
                    if (trees[X, --check].Height >= Height) {
                        top = false;
                        break;
                    }
                }
                if (top) Visibility |= Tree.VisibilityFlags.Top;

                bool bottom = true;
                int bottomViewingDistance = 0;
                check = Y;
                while (check < trees.GetUpperBound(1)) {
                    bottomViewingDistance++;
                    if (trees[X, ++check].Height >= Height) {
                        bottom = false;
                        break;
                    }
                }
                if (bottom) Visibility |= Tree.VisibilityFlags.Bottom;

                ScenicScore = leftViewingDistance * rightViewingDistance * topViewingDistance * bottomViewingDistance;

                return Visibility > 0;
            }

          

        }


    }
}
