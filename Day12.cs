using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    class Day12 : AdventOfCodeChallenge
    {


        public Day12(String inputFilename, List<string> exampleInputFilenames) : base(inputFilename, exampleInputFilenames) {

        }

        public override object SolvePartOne(int example) {
            String[] input = GetInput(example);

            int minSteps = int.MaxValue;
            char[,] map = new char[input[0].Length, input.Length];
            int startX = 0, startY = 0, destX = 0, destY = 0;
            int[,] minStepsToPoint = new int[input[0].Length, input.Length];


            for (int y = 0; y < input.Length; y++) {
                for (int x = 0; x < input[y].Length; x++) {
                    map[x, y] = input[y][x];
                    if (map[x, y] == 'S') {
                        startX = x;
                        startY = y;
                        map[x, y] = 'a';
                    } else if (map[x, y] == 'E') {
                        destX = x;
                        destY = y;
                        map[x, y] = 'z';
                    }
                    minStepsToPoint[x, y] = int.MaxValue;
                }
            }

            Stack<Node> nodes = new Stack<Node>();
            nodes.Push(new Node(startX, startY));

            while (nodes.Count > 0) {
                Node n = nodes.Pop();
                // Right
                if (n.CurrentX < map.GetUpperBound(0) && // Not already at the right 
                  (map[n.CurrentX + 1, n.CurrentY] < (map[n.CurrentX, n.CurrentY] + 2)) &&
                  (n.Steps + 1) < minStepsToPoint[n.CurrentX + 1, n.CurrentY]) { // We havent already been here in less steps
                    Node rightNode = (Node)n.Clone();
                    rightNode.AddStep(n.CurrentX + 1, n.CurrentY, ref minStepsToPoint);
                    minSteps = checkForDestination(minSteps, destX, destY, nodes, rightNode);
                }
                // Down
                if (n.CurrentY < map.GetUpperBound(1) && // Not already at the bottom 
                  (map[n.CurrentX, n.CurrentY + 1] < (map[n.CurrentX, n.CurrentY] + 2)) &&
                  (n.Steps + 1) < minStepsToPoint[n.CurrentX, n.CurrentY + 1]) { // We havent already been here in less steps
                    Node downNode = (Node)n.Clone();
                    downNode.AddStep(n.CurrentX, n.CurrentY + 1, ref minStepsToPoint);
                    minSteps = checkForDestination(minSteps, destX, destY, nodes, downNode);
                }
                // Left 
                if (n.CurrentX > 0 && // Not already at the left 
                   (map[n.CurrentX - 1, n.CurrentY] < (map[n.CurrentX, n.CurrentY] + 2)) &&
                  (n.Steps + 1) < minStepsToPoint[n.CurrentX - 1, n.CurrentY]) { // We havent already been here in less steps
                    Node leftNode = (Node)n.Clone();
                    leftNode.AddStep(n.CurrentX - 1, n.CurrentY, ref minStepsToPoint);
                    minSteps = checkForDestination(minSteps, destX, destY, nodes, leftNode);
                }

                // Up
                if (n.CurrentY > 0 && // Not already at the top 
                  (map[n.CurrentX, n.CurrentY - 1] < (map[n.CurrentX, n.CurrentY] + 2)) &&
                  (n.Steps + 1) < minStepsToPoint[n.CurrentX, n.CurrentY - 1]) { // We havent already been here in less steps
                    Node upNode = (Node)n.Clone();
                    upNode.AddStep(n.CurrentX, n.CurrentY - 1, ref minStepsToPoint);
                    minSteps = checkForDestination(minSteps, destX, destY, nodes, upNode);
                }


            }


            return $"Day 12 Part 1: Minimum steps ={minSteps}";
        }

        private static int checkForDestination(int minSteps, int destX, int destY, Stack<Node> nodes, Node newNode) {
            if (newNode.CurrentX == destX && newNode.CurrentY == destY) {
                if (newNode.Steps < minSteps) {
                    minSteps = newNode.Steps;
                }
            } else {
                nodes.Push(newNode);
            }

            return minSteps;
        }

        public override object SolvePartTwo(int example) {

            String[] input = GetInput(example);

            int minSteps = int.MaxValue;
            char[,] map = new char[input[0].Length, input.Length];
            int destX = 0, destY = 0;

            List<Tuple<int, int>> startLocations = new List<Tuple<int, int>>();

            int[,] minStepsToPoint = new int[input[0].Length, input.Length];


            for (int y = 0; y < input.Length; y++) {
                for (int x = 0; x < input[y].Length; x++) {
                    map[x, y] = input[y][x];
                    if (map[x, y] == 'S') {
                        startLocations.Add(new Tuple<int, int>(x, y));
                        map[x, y] = 'a';
                    } else if (map[x, y] == 'E') {
                        destX = x;
                        destY = y;
                        map[x, y] = 'z';
                    } else if (map[x, y] == 'a') {
                        startLocations.Add(new Tuple<int, int>(x, y));
                    }
                    minStepsToPoint[x, y] = int.MaxValue;
                }
            }


            foreach (Tuple<int, int> startLocation in startLocations) {
                Stack<Node> nodes = new Stack<Node>();
                nodes.Push(new Node(startLocation.Item1, startLocation.Item2));
                for (int y = 0; y < input.Length; y++) {
                    for (int x = 0; x < input[y].Length; x++) {
                        minStepsToPoint[x, y] = int.MaxValue;
                    }
                }

                while (nodes.Count > 0) {
                    Node n = nodes.Pop();
                    // Right
                    if (n.CurrentX < map.GetUpperBound(0) && // Not already at the right 
                      (map[n.CurrentX + 1, n.CurrentY] < (map[n.CurrentX, n.CurrentY] + 2)) &&
                      (n.Steps + 1) < minStepsToPoint[n.CurrentX + 1, n.CurrentY]) { // We havent already been here in less steps
                        Node rightNode = (Node)n.Clone();
                        rightNode.AddStep(n.CurrentX + 1, n.CurrentY, ref minStepsToPoint);
                        minSteps = checkForDestination(minSteps, destX, destY, nodes, rightNode);
                    }
                    // Down
                    if (n.CurrentY < map.GetUpperBound(1) && // Not already at the bottom 
                      (map[n.CurrentX, n.CurrentY + 1] < (map[n.CurrentX, n.CurrentY] + 2)) &&
                      (n.Steps + 1) < minStepsToPoint[n.CurrentX, n.CurrentY + 1]) { // We havent already been here in less steps
                        Node downNode = (Node)n.Clone();
                        downNode.AddStep(n.CurrentX, n.CurrentY + 1, ref minStepsToPoint);
                        minSteps = checkForDestination(minSteps, destX, destY, nodes, downNode);
                    }
                    // Left 
                    if (n.CurrentX > 0 && // Not already at the left 
                       (map[n.CurrentX - 1, n.CurrentY] < (map[n.CurrentX, n.CurrentY] + 2)) &&
                      (n.Steps + 1) < minStepsToPoint[n.CurrentX - 1, n.CurrentY]) { // We havent already been here in less steps
                        Node leftNode = (Node)n.Clone();
                        leftNode.AddStep(n.CurrentX - 1, n.CurrentY, ref minStepsToPoint);
                        minSteps = checkForDestination(minSteps, destX, destY, nodes, leftNode);
                    }

                    // Up
                    if (n.CurrentY > 0 && // Not already at the top 
                      (map[n.CurrentX, n.CurrentY - 1] < (map[n.CurrentX, n.CurrentY] + 2)) &&
                      (n.Steps + 1) < minStepsToPoint[n.CurrentX, n.CurrentY - 1]) { // We havent already been here in less steps
                        Node upNode = (Node)n.Clone();
                        upNode.AddStep(n.CurrentX, n.CurrentY - 1, ref minStepsToPoint);
                        minSteps = checkForDestination(minSteps, destX, destY, nodes, upNode);
                    }


                }

            }



            return $"Day 12 Part 2: Minimum steps ={minSteps}";


        }

        private class Node : ICloneable
        {

            public int CurrentX { get; set; }
            public int CurrentY { get; set; }
            public int Steps { get; set; }


            public Node(int startX, int startY) {
                CurrentX = startX;
                CurrentY = startY;
                Steps = 0;
            }

            public void AddStep(int x, int y, ref int[,] minStepsToPoint) {
                Steps++;

                CurrentX = x;
                CurrentY = y;
                minStepsToPoint[x, y] = Steps;
            }

            public object Clone() {
                return new Node(CurrentX, CurrentY) { Steps = this.Steps };
            }
        }



    }
}
