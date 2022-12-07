using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    class Day7 : AdventOfCodeChallenge
    {

        public Day7(String inputFilename, List<string> exampleInputFilenames) : base(inputFilename, exampleInputFilenames) {

        }

        public override object SolvePartOne(int example) {
            string[] input = GetInput(example);

            Node topNode = new Node(null, 0, "/");
            ProcessInput(input, topNode);

            return $"Day 7, Part 1, answer = {topNode.GetPart1Answer()}";
        }

       

        public override object SolvePartTwo(int example) {
            string[] input = GetInput(example);

            Node topNode = new Node(null, 0, "/");
            ProcessInput(input, topNode);

            return $"Day 7, Part 2, answer = {topNode.GetPart2Answer()}";
        }

        private static void ProcessInput(string[] input, Node topNode) {
            Node currentNode = topNode;
            foreach (String s in input) {
                if (s.StartsWith("$ cd /")) {
                    currentNode = topNode;
                } else if (s.StartsWith("$ cd ..")) {
                    currentNode = currentNode.Parent;
                } else if (s.StartsWith("$ cd ")) {
                    string directoryName = s.Split(new char[] { ' ' })[2];
                    currentNode = currentNode.GetChild(directoryName);
                } else if (!s.StartsWith("$")) {
                    // This is one of the ls results
                    string[] parts = s.Split(new char[] { ' ' });
                    int.TryParse(parts[0], out int size);
                    string name = parts[1];
                    if (!currentNode.ContainsChild(name)) {
                        currentNode.AddChild(new Node(currentNode, size, name));
                    }
                }
            }
        }


        private class Node
        {
            public Node Parent { get; set; }
            public Dictionary<string, Node> ChildrenByName { get; set; }
            public long Size { get; set; }
            public String Name { get; set; }
            public bool IsDirectory {
                get {
                    return ChildrenByName != null;
                }
            }

            public Node(Node parent, long size, String name) {
                Parent = parent;
                Name = name;
                Size = size;
                Node p = this.Parent;
                while (p != null) {
                    p.Size += Size;
                    p = p.Parent;
                }
            }

            public void AddChild(Node child) {
                if (ChildrenByName == null) ChildrenByName = new Dictionary<string, Node>();
                ChildrenByName.Add(child.Name, child);

            }

            public Node GetChild(string name) {
                return ChildrenByName[name];
            }

            public bool ContainsChild(string name) {
                return ChildrenByName == null ? false : ChildrenByName.ContainsKey(name);
            }

            public override string ToString() {
                return $"Name: {Name} Size:{Size} Parent:{(Parent == null ? "null" : Parent.Name)} Children:{(ChildrenByName == null ? "null" : ChildrenByName.Count.ToString())}";
            }

            public long GetPart1Answer() {
                long result = 0;

                Stack<Node> stack = new Stack<Node>();
                stack.Push(this);
                while (stack.Count > 0) {
                    Node n = stack.Pop();
                    if (n.IsDirectory) {
                        if(n.Size <= 100000) {
                            result += n.Size;
                        }
                        foreach(Node c in n.ChildrenByName.Values) {
                            stack.Push(c);
                        }
                    }

                }
                return result;

            }

            public long GetPart2Answer() {
               
                Stack<Node> stack = new Stack<Node>();
                stack.Push(this);

                long spaceRequired = 30000000 - (70000000 - this.Size);
                long smallest = long.MaxValue;

                while (stack.Count > 0) {
                    Node n = stack.Pop();
                    if (n.IsDirectory) {
                        if(n.Size >= spaceRequired && n.Size < smallest) {
                            smallest = n.Size;
                        }

                        foreach (Node c in n.ChildrenByName.Values) {
                            stack.Push(c);
                        }
                    }


                }
                return smallest;

            }
        }

    }
}
