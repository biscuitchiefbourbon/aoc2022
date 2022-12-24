using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Runtime.Remoting.Lifetime;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    class Day16 : AdventOfCodeChallenge
    {


        public Day16(String inputFilename, List<string> exampleInputFilenames) : base(inputFilename, exampleInputFilenames) {


        }

        public override object SolvePartOne(int example) {
            String[] input = GetInput(example);

            Dictionary<string, Valve> valvesByID;
            List<string> valveIDsWithFlow;
            WorkOutRoutesBetweenValves(input, out valvesByID, out valveIDsWithFlow);

            Stack<Route> routes = new Stack<Route>();
            Route topRoute = new Route();
            topRoute.VisitedValves.Add("AA");
            routes.Push(topRoute);
            int maxPressure = 0;


            while (routes.Count > 0) {
                Route r = routes.Pop();
                foreach (RoutePath path in valvesByID[r.CurrentValveID].PathsToOtherValves) {
                    if (!r.OpenedValves.Contains(path.CurrentValveID)) {
                        // We haven't openend this target valve
                        if (r.RemainingMinutes - path.TravelTimeMinutes > 0) {
                            // We have time to travel here and open the valve
                            Route newRoute = (Route)r.Clone();
                            newRoute.VisitedValves.AddRange(path.VisitedValves.Skip(1));
                            newRoute.RemainingMinutes -= path.TravelTimeMinutes;
                            newRoute.TotalPressureReleased += valvesByID[path.CurrentValveID].FlowRate * (newRoute.RemainingMinutes - 1);
                            newRoute.RemainingMinutes--;
                            newRoute.OpenedValves.Add(path.CurrentValveID);
                            if (valveIDsWithFlow.Intersect(newRoute.VisitedValves).Count() == valveIDsWithFlow.Count()) {
                                // All valve IDs with flow have been visited, this route is done                               
                                if (newRoute.TotalPressureReleased > maxPressure) {
                                    maxPressure = newRoute.TotalPressureReleased;
                                }
                            } else {
                                routes.Push(newRoute);
                            }
                        }
                    }
                    if (r.TotalPressureReleased > maxPressure) {
                        maxPressure = r.TotalPressureReleased;
                    }
                }
            }
            return $"Day 16, part 1. Max pressure = {maxPressure}";
        }

        private static void WorkOutRoutesBetweenValves(string[] input, out Dictionary<string, Valve> valvesByID, out List<string> valveIDsWithFlow) {
            valvesByID = new Dictionary<string, Valve>();
            valveIDsWithFlow = new List<string>();
            foreach (String s in input) {
                string[] parts = s.Split(new char[] { ' ' });

                string valve = parts[1];
                int flowRate = int.Parse(parts[4].Replace("rate=", "").Replace(";", ""));
                List<string> targetValves = new List<string>();
                for (int i = 9; i < parts.Length; i++) {
                    targetValves.Add(parts[i].Replace(",", ""));
                }

                valvesByID.Add(valve, new Valve(valve, flowRate, targetValves));
                if (flowRate > 0) {
                    valveIDsWithFlow.Add(valve);
                }


            }

            // The only valve we care about without flow is AA as we are starting there
            foreach (String targetValveID in valveIDsWithFlow) {

                // Find the quickest route between the two valves with flow
                CalculateFastestRoute(valvesByID, "AA", targetValveID);

            }

            foreach (String valveIDWithFlow in valveIDsWithFlow) {
                foreach (String targetValveID in valveIDsWithFlow) {
                    if (valveIDWithFlow != targetValveID) {
                        // Find the quickest route between the two valves with flow
                        CalculateFastestRoute(valvesByID, valveIDWithFlow, targetValveID);
                    }
                }
            }
        }

        private static void CalculateFastestRoute(Dictionary<string, Valve> valvesByID, string valveIDWithFlow, string targetValveID) {
            Stack<RoutePath> routes = new Stack<RoutePath>();
            RoutePath root = new RoutePath();
            root.VisitedValves.Add(valveIDWithFlow);
            routes.Push(root);
            while (routes.Count > 0) {
                RoutePath r = routes.Pop();
                foreach (String nextValveID in valvesByID[r.CurrentValveID].TargetValveIDs) {
                    if (!r.VisitedValves.Contains(nextValveID)) {
                        RoutePath newRoutePath = (RoutePath)r.Clone();
                        newRoutePath.VisitedValves.Add(nextValveID);
                        if (nextValveID == targetValveID) {
                            valvesByID[valveIDWithFlow].PathsToOtherValves.Add(newRoutePath);
                        } else {
                            routes.Push(newRoutePath);
                        }
                    }
                }
            }
            valvesByID[valveIDWithFlow].FilterPaths();
        }

        public override object SolvePartTwo(int example) {

            String[] input = GetInput(example);

            Dictionary<string, Valve> valvesByID;
            List<string> valveIDsWithFlow;
            WorkOutRoutesBetweenValves(input, out valvesByID, out valveIDsWithFlow);

            Stack<ElephantRoute> routes = new Stack<ElephantRoute>();
            ElephantRoute topRoute = new ElephantRoute();
            topRoute.MyVisitedValves.Add("AA");
            topRoute.ElephantVisitedValves.Add("AA");
            routes.Push(topRoute);
            int maxPressure = 0;


            while (routes.Count > 0) {
                ElephantRoute r = routes.Pop();
                if (r.MyElapsedMinutes == r.ElephantElapsedMinutes) {
                    // If the elapsed minutes are the same, route both me and elephant to each achievable pair
                    foreach (RoutePath myPath in valvesByID[r.MyCurrentValveID].PathsToOtherValves) {
                        foreach (RoutePath elephantPath in valvesByID[r.ElephantCurrentValveID].PathsToOtherValves) {
                            if (myPath.CurrentValveID != elephantPath.CurrentValveID) {
                                if (!r.OpenedValves.Contains(myPath.CurrentValveID) && !r.OpenedValves.Contains(elephantPath.CurrentValveID)) {
                                    // This pairing would result in unopened valve for both me and elephant
                                    if (r.StartingMinutes - r.MyElapsedMinutes - Math.Max(myPath.TravelTimeMinutes, elephantPath.TravelTimeMinutes) > 0) {
                                        // We both have time to get to these locations and open the valves
                                        ElephantRoute newRoute = (ElephantRoute)r.Clone();
                                        newRoute.MyVisitedValves.AddRange(myPath.VisitedValves.Skip(1));
                                        newRoute.ElephantVisitedValves.AddRange(elephantPath.VisitedValves.Skip(1));

                                        newRoute.TotalPressureReleased += valvesByID[myPath.CurrentValveID].FlowRate * (newRoute.StartingMinutes - newRoute.MyElapsedMinutes - myPath.TravelTimeMinutes - 1);
                                        newRoute.TotalPressureReleased += valvesByID[elephantPath.CurrentValveID].FlowRate * (newRoute.StartingMinutes - newRoute.ElephantElapsedMinutes - elephantPath.TravelTimeMinutes - 1);

                                        newRoute.ElephantElapsedMinutes += elephantPath.TravelTimeMinutes+1;
                                        newRoute.MyElapsedMinutes += myPath.TravelTimeMinutes+1;

                                        newRoute.OpenedValves.Add(myPath.CurrentValveID);
                                        newRoute.OpenedValves.Add(elephantPath.CurrentValveID);

                                        if (valveIDsWithFlow.Intersect(newRoute.MyVisitedValves.Union(newRoute.ElephantVisitedValves)).Count() == valveIDsWithFlow.Count()) {
                                            // All valve IDs with flow have been visited, this route is done                               
                                            if (newRoute.TotalPressureReleased > maxPressure) {
                                                maxPressure = newRoute.TotalPressureReleased;
                                                if (maxPressure == 1793) {
                                                    Debugger.Break();
                                                }
                                            }
                                        } else {
                                            routes.Push(newRoute);
                                        }
                                    }
                                } else {
                                    //Console.WriteLine("Option where only one can go to a valve");
                                }
                            }
                        }
                    }
                } else if (r.MyElapsedMinutes < r.ElephantElapsedMinutes) {
                    // We are 'behind' the elephant, so should make another move
                    foreach (RoutePath myPath in valvesByID[r.MyCurrentValveID].PathsToOtherValves) {
                        if (!r.OpenedValves.Contains(myPath.CurrentValveID)) {
                            if (r.StartingMinutes - r.MyElapsedMinutes - myPath.TravelTimeMinutes > 0) {
                                ElephantRoute newRoute = (ElephantRoute)r.Clone();
                                newRoute.MyVisitedValves.AddRange(myPath.VisitedValves.Skip(1));
                                newRoute.TotalPressureReleased += valvesByID[myPath.CurrentValveID].FlowRate * (newRoute.StartingMinutes - newRoute.MyElapsedMinutes - myPath.TravelTimeMinutes - 1);
                                newRoute.MyElapsedMinutes += myPath.TravelTimeMinutes + 1;
                                newRoute.OpenedValves.Add(myPath.CurrentValveID);
                                if (valveIDsWithFlow.Intersect(newRoute.MyVisitedValves.Union(newRoute.ElephantVisitedValves)).Count() == valveIDsWithFlow.Count()) {
                                    // All valve IDs with flow have been visited, this route is done                               
                                    if (newRoute.TotalPressureReleased > maxPressure) {
                                        maxPressure = newRoute.TotalPressureReleased;
                                        if (maxPressure == 1793) {
                                            Debugger.Break();
                                        }
                                    }
                                } else {
                                    routes.Push(newRoute);
                                }
                            }
                        }
                        
                    }

                } else {
                    // We are 'in front' of the elephant, so the elephant should make another move
                    foreach (RoutePath elephantPath in valvesByID[r.ElephantCurrentValveID].PathsToOtherValves) {
                        if (!r.OpenedValves.Contains(elephantPath.CurrentValveID)) {
                            if (r.StartingMinutes - r.MyElapsedMinutes - elephantPath.TravelTimeMinutes > 0) {
                                ElephantRoute newRoute = (ElephantRoute)r.Clone();
                                newRoute.ElephantVisitedValves.AddRange(elephantPath.VisitedValves.Skip(1));
                                newRoute.TotalPressureReleased += valvesByID[elephantPath.CurrentValveID].FlowRate * (newRoute.StartingMinutes - newRoute.ElephantElapsedMinutes - elephantPath.TravelTimeMinutes - 1);
                                newRoute.ElephantElapsedMinutes += elephantPath.TravelTimeMinutes + 1;
                                newRoute.OpenedValves.Add(elephantPath.CurrentValveID);
                                if (valveIDsWithFlow.Intersect(newRoute.MyVisitedValves.Union(newRoute.ElephantVisitedValves)).Count() == valveIDsWithFlow.Count()) {
                                    // All valve IDs with flow have been visited, this route is done                               
                                    if (newRoute.TotalPressureReleased > maxPressure) {
                                        maxPressure = newRoute.TotalPressureReleased;
                                        if(maxPressure == 1793) {
                                            Debugger.Break();
                                        }
                                    }
                                } else {
                                    routes.Push(newRoute);
                                }
                            }
                        }
                    }

                }

                if (r.TotalPressureReleased > maxPressure) {
                    maxPressure = r.TotalPressureReleased;
                }

            }
            return $"Day 16, part 2. Max pressure = {maxPressure}";


        }

        private class Valve
        {
            public String ID { get; set; }
            public int FlowRate { get; set; }
            public List<string> TargetValveIDs { get; set; }

            public List<RoutePath> PathsToOtherValves { get; set; }

            public Valve(string iD, int flowRate, List<string> targetValveIDs) {
                ID = iD;
                FlowRate = flowRate;
                TargetValveIDs = targetValveIDs;
                PathsToOtherValves = new List<RoutePath>();
            }

            public void FilterPaths() {

                Dictionary<string, RoutePath> shortestRouteByTarget = new Dictionary<string, RoutePath>();
                foreach (RoutePath r in PathsToOtherValves) {
                    if (shortestRouteByTarget.ContainsKey(r.CurrentValveID)) {
                        if (r.VisitedValves.Count < shortestRouteByTarget[r.CurrentValveID].VisitedValves.Count) {
                            shortestRouteByTarget[r.CurrentValveID] = r;
                        }
                    } else {
                        shortestRouteByTarget.Add(r.CurrentValveID, r);
                    }
                }
                PathsToOtherValves = new List<RoutePath>(shortestRouteByTarget.Values);
            }
        }

        private class Route : ICloneable
        {
            public List<string> VisitedValves { get; set; }

            public List<string> OpenedValves { get; set; }

            public int TotalPressureReleased { get; set; }

            public int RemainingMinutes { get; set; }


            public String CurrentValveID {
                get {
                    return VisitedValves[VisitedValves.Count - 1];
                }
            }

            public Route() {
                RemainingMinutes = 30;
                VisitedValves = new List<string>();
                TotalPressureReleased = 0;
                OpenedValves = new List<string>();
            }

            public object Clone() {
                return new Route() { VisitedValves = new List<string>(VisitedValves), RemainingMinutes = this.RemainingMinutes, TotalPressureReleased = this.TotalPressureReleased, OpenedValves = new List<string>(this.OpenedValves) };
            }
        }


        private class ElephantRoute : ICloneable
        {
            public List<string> MyVisitedValves { get; set; }

            public List<string> ElephantVisitedValves { get; set; }

            public List<string> OpenedValves { get; set; }

            public int TotalPressureReleased { get; set; }

            public int RemainingElephantMinutes {
                get {
                    return StartingMinutes - ElephantElapsedMinutes;
                }
            }

            public int RemainingMyMinutes {
                get {
                    return StartingMinutes - MyElapsedMinutes;
                }
            }

            public int StartingMinutes { get; set; }

            public int ElephantElapsedMinutes { get; set; }
            public int MyElapsedMinutes { get; set; }

            public String MyCurrentValveID {
                get {
                    return MyVisitedValves[MyVisitedValves.Count - 1];
                }
            }

            public String ElephantCurrentValveID {
                get {
                    return ElephantVisitedValves[ElephantVisitedValves.Count - 1];
                }
            }

            public ElephantRoute() {
                StartingMinutes = 26;
                ElephantElapsedMinutes = 0;
                MyElapsedMinutes = 0;
                MyVisitedValves = new List<string>();
                ElephantVisitedValves = new List<string>();
                TotalPressureReleased = 0;
                OpenedValves = new List<string>();
            }

            public object Clone() {
                return new ElephantRoute() { MyVisitedValves = new List<string>(MyVisitedValves), ElephantVisitedValves = new List<string>(ElephantVisitedValves), StartingMinutes = this.StartingMinutes, ElephantElapsedMinutes = this.ElephantElapsedMinutes, MyElapsedMinutes = this.MyElapsedMinutes, TotalPressureReleased = this.TotalPressureReleased, OpenedValves = new List<string>(this.OpenedValves) };
            }
        }


        private class RoutePath : ICloneable
        {
            public List<string> VisitedValves { get; set; }

            public String CurrentValveID {
                get {
                    return VisitedValves[VisitedValves.Count - 1];
                }
            }

            public string StartValveID {
                get {
                    return VisitedValves[0];
                }
            }

            public int TravelTimeMinutes {
                get {
                    return VisitedValves.Count - 1;
                }
            }

            public RoutePath() {
                VisitedValves = new List<string>();
            }
            public object Clone() {
                return new RoutePath() { VisitedValves = new List<string>(this.VisitedValves) };
            }

            public override string ToString() {
                return $"{StartValveID} -> {CurrentValveID} ({TravelTimeMinutes} Minutes)";
            }
        }


    }
}
