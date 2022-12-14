using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    class Day14 : AdventOfCodeChallenge
    {


        public Day14(String inputFilename, List<string> exampleInputFilenames) : base(inputFilename, exampleInputFilenames) {


        }

        public override object SolvePartOne(int example) {
            String[] input = GetInput(example);
            int minX = int.MaxValue, maxX = 0, minY = int.MaxValue, maxY = 0;

            List<List<Tuple<int, int>>> rockLines = new List<List<Tuple<int, int>>>();
            foreach (String s in input) {
                List<Tuple<int, int>> rockLine = new List<Tuple<int, int>>();
                string[] coords = s.Replace("-> ", "").Split(new char[] { ' ' });
                foreach (String coord in coords) {
                    String[] coordParts = coord.Split(new char[] { ',' });
                    int x = int.Parse(coordParts[0]);
                    int y = int.Parse(coordParts[1]);

                    if (x < minX) minX = x;
                    if (x > maxX) maxX = x;
                    if (y < minY) minY = y;
                    if (y > maxY) maxY = y;

                    rockLine.Add(new Tuple<int, int>(x, y));
                }
                rockLines.Add(rockLine);
            }

            char[,] map = new char[maxX - minX + 1, maxY + 1];
            foreach (List<Tuple<int, int>> rockLine in rockLines) {
                for (int i = 0; i < rockLine.Count - 1; i++) {
                    if (rockLine[i].Item1 == rockLine[i + 1].Item1) {
                        // Vertical line
                        if (rockLine[i + 1].Item2 > rockLine[i].Item2) {
                            for (int y = rockLine[i].Item2; y <= rockLine[i + 1].Item2; y++) {
                                map[rockLine[i].Item1 - minX, y] = '#';
                            }
                        } else {
                            for (int y = rockLine[i + 1].Item2; y <= rockLine[i].Item2; y++) {
                                map[rockLine[i].Item1 - minX, y] = '#';
                            }
                        }

                    } else {
                        // Horizontal line
                        if (rockLine[i + 1].Item1 > rockLine[i].Item1) {
                            for (int x = rockLine[i].Item1; x <= rockLine[i + 1].Item1; x++) {
                                map[x - minX, rockLine[i].Item2] = '#';
                            }
                        } else {
                            for (int x = rockLine[i + 1].Item1; x <= rockLine[i].Item1; x++) {
                                map[x - minX, rockLine[i].Item2] = '#';
                            }
                        }

                    }
                }
            }

            printMap(map);

           
            bool rest = true;
            int sandReleased = 0;

            while (true) {
                rest = true;
                int sandX = 500 - minX, sandY = 0;
                sandReleased++;
                while (sandY < map.GetUpperBound(1) && rest) {
                    if (map[sandX, sandY + 1] == '\0') {
                        // Nothing here
                        sandY++;
                    } else if(sandX == 0) {
                        // We can't go down left, as it take us into left abyss
                        break;
                    }else if (map[sandX - 1, sandY + 1] == '\0') {
                        // Nothing here (down left)
                        sandY++;
                        sandX--;
                    }else if(sandX + 1 > map.GetUpperBound(0)) {
                        // We can't go down right, as it takes us into right abyss
                        break;

                    }  else if (map[sandX + 1, sandY + 1] == '\0') {
                        // Nothing here (down right)
                        sandY++;
                        sandX++;
                    } else {
                        // Can't go anyhere else
                        map[sandX, sandY] = 'o';
                        rest = false;
                    }


                }
                //printMap(map);
                if (rest) break;

            }
            

            printMap(map);
            return $"Day 14, Part 1. Sand released = {sandReleased-1}";
        }

        private static void printMap(char[,] map) {
            for (int y = 0; y <= map.GetUpperBound(1); y++) {
                for (int x = 0; x <= map.GetUpperBound(0); x++) {
                    Console.Write(map[x, y] == '\0' ? "." : map[x,y].ToString());   
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public override object SolvePartTwo(int example) {

            String[] input = GetInput(example);
            int minX = int.MaxValue, maxX = 0, minY = int.MaxValue, maxY = 0;
            int xPadding = 1000  ;

            List<List<Tuple<int, int>>> rockLines = new List<List<Tuple<int, int>>>();
            foreach (String s in input) {
                List<Tuple<int, int>> rockLine = new List<Tuple<int, int>>();
                string[] coords = s.Replace("-> ", "").Split(new char[] { ' ' });
                foreach (String coord in coords) {
                    String[] coordParts = coord.Split(new char[] { ',' });
                    int x = int.Parse(coordParts[0]);
                    int y = int.Parse(coordParts[1]);

                    if (x < minX) minX = x;
                    if (x > maxX) maxX = x;
                    if (y < minY) minY = y;
                    if (y > maxY) maxY = y;

                    rockLine.Add(new Tuple<int, int>(x, y));
                }
                rockLines.Add(rockLine);
            }

            char[,] map = new char[maxX - minX + 1 + xPadding, maxY + 1 + 2];
            foreach (List<Tuple<int, int>> rockLine in rockLines) {
                for (int i = 0; i < rockLine.Count - 1; i++) {
                    if (rockLine[i].Item1 == rockLine[i + 1].Item1) {
                        // Vertical line
                        if (rockLine[i + 1].Item2 > rockLine[i].Item2) {
                            for (int y = rockLine[i].Item2; y <= rockLine[i + 1].Item2; y++) {
                                map[rockLine[i].Item1 - minX + xPadding/2, y] = '#';
                            }
                        } else {
                            for (int y = rockLine[i + 1].Item2; y <= rockLine[i].Item2; y++) {
                                map[rockLine[i].Item1 - minX + xPadding / 2, y] = '#';
                            }
                        }

                    } else {
                        // Horizontal line
                        if (rockLine[i + 1].Item1 > rockLine[i].Item1) {
                            for (int x = rockLine[i].Item1; x <= rockLine[i + 1].Item1; x++) {
                                map[x - minX + xPadding / 2, rockLine[i].Item2] = '#';
                            }
                        } else {
                            for (int x = rockLine[i + 1].Item1; x <= rockLine[i].Item1; x++) {
                                map[x - minX + xPadding / 2, rockLine[i].Item2] = '#';
                            }
                        }

                    }
                }
            }

            for(int x = 0; x <= map.GetUpperBound(0); x++) {
                map[x, maxY + 2] = '#';
            }

            //printMap(map);


            bool rest = true;
            int sandReleased = 0;

            while (true) {
                rest = true;
                int sandX = 500 - minX + xPadding / 2, sandY = 0;
                sandReleased++;
                while (sandY < map.GetUpperBound(1) && rest) {
                    if (map[sandX, sandY + 1] == '\0') {
                        // Nothing here
                        sandY++;
                    } else if (sandX == 0) {
                        // We can't go down left, as it take us into left abyss
                        break;
                    } else if (map[sandX - 1, sandY + 1] == '\0') {
                        // Nothing here (down left)
                        sandY++;
                        sandX--;
                    } else if (sandX + 1 > map.GetUpperBound(0)) {
                        // We can't go down right, as it takes us into right abyss
                        break;

                    } else if (map[sandX + 1, sandY + 1] == '\0') {
                        // Nothing here (down right)
                        sandY++;
                        sandX++;
                    } else {
                        // Can't go anyhere else
                        map[sandX, sandY] = 'o';
                        rest = false;
                        if (sandY == 0) {
                            rest = true;
                            break;
                        }
                    }


                }
                //printMap(map);
                if (rest) break;

            }


           // printMap(map);
            return $"Day 14, Part 2. Sand released = {sandReleased}";

        }








    }
}
