using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static AdventOfCode2022.Day9;

namespace AdventOfCode2022
{
    class Day9 : AdventOfCodeChallenge
    {

        public Day9(String inputFilename, List<string> exampleInputFilenames) : base(inputFilename, exampleInputFilenames) {

        }

        public enum TailRelativeToHead
        {
            None,
            Overlap,
            TopLeft,
            Top,
            TopRight,
            Right,
            BottomRight,
            Bottom,
            BottomLeft,
            Left
        }


        Dictionary<Tuple<TailRelativeToHead, char>, Tuple<int, int, TailRelativeToHead, char>> actions = new Dictionary<Tuple<TailRelativeToHead, char>, Tuple<int, int, TailRelativeToHead, char>> {
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Overlap, 'U'), new Tuple<int,int,TailRelativeToHead, char>(0,0,TailRelativeToHead.Bottom, 'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.TopLeft, 'U'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.Left,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Top, 'U'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.Overlap,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.TopRight, 'U'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.Right, 'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Right, 'U'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.BottomRight, 'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.BottomRight, 'U'), new Tuple<int,int,TailRelativeToHead, char>(-1,-1, TailRelativeToHead.Bottom, '7') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Bottom, 'U'), new Tuple<int,int,TailRelativeToHead, char>(0,-1, TailRelativeToHead.Bottom, 'U') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.BottomLeft, 'U'), new Tuple<int,int,TailRelativeToHead, char>(1,-1, TailRelativeToHead.Bottom, '9') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Left, 'U'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.BottomLeft, 'N') },

            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Overlap, 'D'), new Tuple<int,int,TailRelativeToHead, char>(0,0,TailRelativeToHead.Top,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.TopLeft, 'D'), new Tuple<int,int,TailRelativeToHead, char>(1,1, TailRelativeToHead.Top,'3') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Top, 'D'), new Tuple<int,int,TailRelativeToHead, char>(0,1, TailRelativeToHead.Top,'D') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.TopRight, 'D'), new Tuple<int,int,TailRelativeToHead, char>(-1,1, TailRelativeToHead.Top,'1') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Right, 'D'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.TopRight,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.BottomRight, 'D'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.Right,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Bottom, 'D'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.Overlap,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.BottomLeft, 'D'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.Left,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Left, 'D'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.TopLeft,'N') },

            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Overlap, 'L'), new Tuple<int,int,TailRelativeToHead, char>(0,0,TailRelativeToHead.Right,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.TopLeft, 'L'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.Top,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Top, 'L'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.TopRight,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.TopRight, 'L'), new Tuple<int,int,TailRelativeToHead, char>(-1,1, TailRelativeToHead.Right,'1') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Right, 'L'), new Tuple<int,int,TailRelativeToHead, char>(-1,0, TailRelativeToHead.Right,'L') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.BottomRight, 'L'), new Tuple<int,int,TailRelativeToHead, char>(-1,-1, TailRelativeToHead.Right,'7') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Bottom, 'L'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.BottomRight,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.BottomLeft, 'L'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.Bottom,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Left, 'L'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.Overlap,'N') },

            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Overlap, 'R'), new Tuple<int,int,TailRelativeToHead, char>(0,0,TailRelativeToHead.Left,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.TopLeft, 'R'), new Tuple<int,int,TailRelativeToHead, char>(1,1, TailRelativeToHead.Left,'3') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Top, 'R'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.TopLeft,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.TopRight, 'R'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.Top,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Right, 'R'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.Overlap,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.BottomRight, 'R'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.Bottom,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Bottom, 'R'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.BottomLeft,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.BottomLeft, 'R'), new Tuple<int,int,TailRelativeToHead, char>(1,-1, TailRelativeToHead.Left,'9') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Left, 'R'), new Tuple<int,int,TailRelativeToHead, char>(1,0, TailRelativeToHead.Left,'R') },

            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.None, 'U'), new Tuple<int,int,TailRelativeToHead, char>(0,-1,TailRelativeToHead.None,'U') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.None, 'D'), new Tuple<int,int,TailRelativeToHead, char>(0,1, TailRelativeToHead.None,'D') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.None, 'L'), new Tuple<int,int,TailRelativeToHead, char>(-1,0, TailRelativeToHead.None,'L') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.None, 'R'), new Tuple<int,int,TailRelativeToHead, char>(1,0, TailRelativeToHead.None,'R') },

            // 7 = Up left!
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Overlap, '7'), new Tuple<int,int,TailRelativeToHead, char>(0,0,TailRelativeToHead.BottomRight,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.TopLeft, '7'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.Overlap,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Top, '7'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.Right,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.TopRight, '7'), new Tuple<int,int,TailRelativeToHead, char>(-1,0, TailRelativeToHead.Right,'L') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Right, '7'), new Tuple<int,int,TailRelativeToHead, char>(-1,-1, TailRelativeToHead.Right, '7') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.BottomRight, '7'), new Tuple<int,int,TailRelativeToHead, char>(-1,-1, TailRelativeToHead.BottomRight,'7') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Bottom, '7'), new Tuple<int,int,TailRelativeToHead, char>(-1,-1, TailRelativeToHead.Bottom,'7') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.BottomLeft, '7'), new Tuple<int,int,TailRelativeToHead, char>(0,-1, TailRelativeToHead.Bottom,'U') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Left, '7'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.Bottom,'N') },

            // 9 = Up right!
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Overlap, '9'), new Tuple<int,int,TailRelativeToHead, char>(0,0,TailRelativeToHead.BottomLeft,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.TopLeft, '9'), new Tuple<int,int,TailRelativeToHead, char>(1,0, TailRelativeToHead.Left,'R') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Top, '9'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.Left,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.TopRight, '9'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.Overlap,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Right, '9'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.Bottom, 'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.BottomRight, '9'), new Tuple<int,int,TailRelativeToHead, char>(0,-1, TailRelativeToHead.Bottom,'U') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Bottom, '9'), new Tuple<int,int,TailRelativeToHead, char>(1,-1, TailRelativeToHead.Bottom,'9') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.BottomLeft, '9'), new Tuple<int,int,TailRelativeToHead, char>(1,-1, TailRelativeToHead.BottomLeft,'9') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Left, '9'), new Tuple<int,int,TailRelativeToHead, char>(1,-1, TailRelativeToHead.Left,'9') },

            // 1 = Down left!
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Overlap, '1'), new Tuple<int,int,TailRelativeToHead, char>(0,0,TailRelativeToHead.TopRight,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.TopLeft, '1'), new Tuple<int,int,TailRelativeToHead, char>(0,1, TailRelativeToHead.Top,'D') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Top, '1'), new Tuple<int,int,TailRelativeToHead, char>(-1,1, TailRelativeToHead.Top,'1') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.TopRight, '1'), new Tuple<int,int,TailRelativeToHead, char>(-1,1, TailRelativeToHead.TopRight,'1') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Right, '1'), new Tuple<int,int,TailRelativeToHead, char>(-1,1, TailRelativeToHead.Right, '1') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.BottomRight, '1'), new Tuple<int,int,TailRelativeToHead, char>(-1,0, TailRelativeToHead.Right,'L') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Bottom, '1'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.Right,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.BottomLeft, '1'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.Overlap,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Left, '1'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.Top,'N') },

            // 3 = Down right!
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Overlap, '3'), new Tuple<int,int,TailRelativeToHead, char>(0,0,TailRelativeToHead.TopLeft,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.TopLeft, '3'), new Tuple<int,int,TailRelativeToHead, char>(1,1, TailRelativeToHead.TopLeft,'3') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Top, '3'), new Tuple<int,int,TailRelativeToHead, char>(1,1, TailRelativeToHead.Top,'3') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.TopRight, '3'), new Tuple<int,int,TailRelativeToHead, char>(0,1, TailRelativeToHead.Top,'D') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Right, '3'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.Top, 'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.BottomRight, '3'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.Overlap,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Bottom, '3'), new Tuple<int,int,TailRelativeToHead, char>(0,0, TailRelativeToHead.Left,'N') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.BottomLeft, '3'), new Tuple<int,int,TailRelativeToHead, char>(1,0, TailRelativeToHead.Left,'R') },
            { new Tuple<TailRelativeToHead,char>(TailRelativeToHead.Left, '3'), new Tuple<int,int,TailRelativeToHead, char>(1,1, TailRelativeToHead.Left,'3') },

        };

        public override object SolvePartOne(int example) {
            string[] input = GetInput(example);

            List<Tuple<int, int>> tailVisits = new List<Tuple<int, int>>();
            int tailX = 0, tailY = 0;
            TailRelativeToHead tailRelativeToHead = TailRelativeToHead.Overlap;

            tailVisits.Add(new Tuple<int, int>(tailX, tailY));

            foreach(String s in input) {
                char command = s[0];
                int distance = int.Parse(s.Split(new char[] { ' ' })[1]);

                while(distance > 0) {
                    Tuple<int, int, TailRelativeToHead,char> adustment = actions[new Tuple<TailRelativeToHead, char>(tailRelativeToHead, command)];
                    tailX += adustment.Item1;
                    tailY += adustment.Item2;
                    tailRelativeToHead = adustment.Item3;

                    Tuple<int,int> newTailCoordinates = new Tuple<int, int>(tailX, tailY);
                    if(!tailVisits.Contains(newTailCoordinates)) tailVisits.Add(newTailCoordinates);

                    distance--;

                }
                
            }

            return $"Day 9 Part 1, Tail visited {tailVisits.Count} locations";
        }




        public override object SolvePartTwo(int example) {
            string[] input = GetInput(example);


            List<Tuple<int, int>> tailVisits = new List<Tuple<int, int>>();
            List<Tuple<int, int, TailRelativeToHead>> current = new List<Tuple<int, int, TailRelativeToHead>>();
            for(int i= 0; i < 10; i++) {
                current.Add(new Tuple<int, int, TailRelativeToHead>(0, 0, i==0? TailRelativeToHead.None : TailRelativeToHead.Overlap));
            }

            tailVisits.Add(new Tuple<int, int>(0, 0));

            foreach (String s in input) {
                char parsedCommand = s[0];
                int distance = int.Parse(s.Split(new char[] { ' ' })[1]);

                while (distance > 0) {
                    char command;
                    char previousCommand='N';
                    for(int i = 0; i < 10; i++) {
                        if (i == 0) {
                            command = parsedCommand;
                        } else {
                            command = previousCommand;
                        }
                        if (command != 'N') {
                            Tuple<int, int, TailRelativeToHead, char> adustment = actions[new Tuple<TailRelativeToHead, char>(current[i].Item3, command)];
                            current[i] = new Tuple<int, int, TailRelativeToHead>(current[i].Item1 + adustment.Item1, current[i].Item2 + adustment.Item2, adustment.Item3);
                            previousCommand = adustment.Item4;
                        }

                    }

                    Tuple<int, int> newTailCoordinates = new Tuple<int, int>(current[9].Item1, current[9].Item2);
                    if (!tailVisits.Contains(newTailCoordinates)){
                        tailVisits.Add(newTailCoordinates);
                    }

                   
                    distance--;

                }

            }

            return $"Day 9 Part 2, Tail visited {tailVisits.Count} locations";

        }

      

       

    }
}
