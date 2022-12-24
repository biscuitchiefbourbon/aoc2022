using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    class Day15 : AdventOfCodeChallenge
    {


        public Day15(String inputFilename, List<string> exampleInputFilenames) : base(inputFilename, exampleInputFilenames) {


        }

        public override object SolvePartOne(int example) {
            String[] input = GetInput(example);

            Dictionary<int, Point> sensorsByIndex = new Dictionary<int, Point>();

            Dictionary<int, Point> beaconsByIndex = new Dictionary<int, Point>();

            Dictionary<int, int> distancesByIndex = new Dictionary<int, int>();

            Dictionary<int, Point[]> beaconRangeByIndex = new Dictionary<int, Point[]>();

            int minX = int.MaxValue, maxX = 0, minY = int.MaxValue, maxY = 0;
            int checkY = example == 1 ? 10 : 2000000;

            List<Point> impossiblePointsForBeacon = new List<Point>();

            for (int i = 0; i < input.Length; i++) {
                string[] parts = input[i].Split(new char[] { ' ' });

                int sensorX = int.Parse(parts[2].Replace("x=", "").Replace(",", ""));
                if (sensorX < minX) minX = sensorX;
                if (sensorX > maxX) maxX = sensorX;
                int sensorY = int.Parse(parts[3].Replace("y=", "").Replace(":", ""));
                if (sensorY < minY) minY = sensorY;
                if (sensorY > maxY) maxY = sensorY;

                int beaconX = int.Parse(parts[8].Replace("x=", "").Replace(",", ""));
                if (beaconX < minX) minX = beaconX;
                if (beaconX > maxX) maxX = beaconX;
                int beaconY = int.Parse(parts[9].Replace("y=", ""));
                if (beaconY < minY) minY = beaconY;
                if (beaconY > maxY) maxY = beaconY;


                sensorsByIndex.Add(i, new Point(sensorX, sensorY));
                beaconsByIndex.Add(i, new Point(beaconX, beaconY));


                int distance = Math.Abs(sensorX - beaconX) + Math.Abs(sensorY - beaconY);
                distancesByIndex.Add(i, distance);

                Point[] beaconRange = new Point[] { new Point(sensorX - distance, sensorY), new Point(sensorX, sensorY + distance), new Point(sensorX + distance, sensorY), new Point(sensorX, sensorY - distance) };
                beaconRangeByIndex.Add(i, beaconRange);



                for (int x = sensorX - distance; x <= sensorX + distance; x++) {
                    int distanceFromCheck = Math.Abs(x - beaconX) + Math.Abs(checkY - beaconY);
                    if (distanceFromCheck <= distance) {
                        Point nonBeacon = new Point(x, checkY);
                        if (!impossiblePointsForBeacon.Contains(nonBeacon))
                            impossiblePointsForBeacon.Add(new Point(x, checkY));
                    }
                }

                Console.WriteLine($"Sensor {sensorX},{sensorY} Closest beacon {beaconX},{beaconY} distance {distance}");


            }


            List<Tuple<int, int>> impossibleBeaconCoords = new List<Tuple<int, int>>();

            //for (int x = -50; x < 50; x++) {
            //    foreach (KeyValuePair<int, Point[]> beaconRange in beaconRangeByIndex) {
            //        if (checkInside(beaconRange.Value, 4, new Point(x, checkY))) {
            //            Console.WriteLine($"x={x} inside");
            //        } else {
            //            Console.WriteLine($"x={x} outside");
            //        }
            //    }
            //}






            return $"";
        }

        public override object SolvePartTwo(int example) {

            String[] input = GetInput(example);



            return $"";


        }


        struct Point
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Point(int x, int y) {
                X = x;
                Y = y;
            }

            public override string ToString() {
                return $"({X},{Y})";
            }

            public override bool Equals(object obj) {
                Point other = (Point)obj;
                return this.X == other.X && this.Y == other.Y;
            }
        }

        struct Line
        {
            public Point Start { get; set; }
            public Point End { get; set; }

            public Line(Point start, Point end) {
                Start = start;
                End = end;
            }


        }

        private static bool onLine(Line l1, Point p) {
            // Check whether p is on the line or not
            if (p.X <= Math.Max(l1.Start.X, l1.End.X)
                && p.X <= Math.Min(l1.Start.X, l1.End.X)
                && (p.Y <= Math.Max(l1.Start.Y, l1.End.Y)
                    && p.Y <= Math.Min(l1.Start.Y, l1.End.Y)))
                return true;

            return false;
        }
        int direction(Point a, Point b, Point c) {
            int val = (b.Y - a.Y) * (c.X - b.X)
                      - (b.X - a.X) * (c.Y - b.Y);

            if (val == 0)

                // Colinear
                return 0;

            else if (val < 0)

                // Anti-clockwise direction
                return 2;

            // Clockwise direction
            return 1;
        }

        bool isIntersect(Line l1, Line l2) {
            // Four direction for two lines and points of other line
            int dir1 = direction(l1.Start, l1.End, l2.Start);
            int dir2 = direction(l1.Start, l1.End, l2.End);
            int dir3 = direction(l2.Start, l2.End, l1.Start);
            int dir4 = direction(l2.Start, l2.End, l1.End);

            // When intersecting
            if (dir1 != dir2 && dir3 != dir4)
                return true;

            // When p2 of line2 are on the line1
            if (dir1 == 0 && onLine(l1, l2.Start))
                return true;

            // When p1 of line2 are on the line1
            if (dir2 == 0 && onLine(l1, l2.End))
                return true;

            // When p2 of line1 are on the line2
            if (dir3 == 0 && onLine(l2, l1.Start))
                return true;

            // When p1 of line1 are on the line2
            if (dir4 == 0 && onLine(l2, l1.End))
                return true;

            return false;
        }


        bool checkInside(Point[] poly, int n, Point p) {

            // When polygon has less than 3 edge, it is not polygon
            if (n < 3)
                return false;

            // Create a point at infinity, y is same as point p
            Line exline = new Line(p, new Point(int.MaxValue, p.Y));

            int count = 0;
            int i = 0;
            do {

                // Forming a line from two consecutive points of
                // poly
                Line side = new Line(poly[i], poly[(i + 1) % n]);

                if (isIntersect(side, exline)) {

                    // If side is intersects exline
                    if (direction(side.Start, p, side.End) == 0)
                        return onLine(side, p);
                    count++;
                }
                i = (i + 1) % n;
            } while (i != 0);

            // When count is odd
            //return  count & 1;
            return count % 2 != 0;
        }




    }
}
