using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    class Day11 : AdventOfCodeChallenge
    {


        public Day11(String inputFilename, List<string> exampleInputFilenames) : base(inputFilename, exampleInputFilenames) {

        }

        public override object SolvePartOne(int example) {
            String[] input = GetInput(example);

            long monkeyBusiness = calculateMonkeyBusiness(input, 20, true);

            return $"Day 11, Part 1. Monkey business = {monkeyBusiness}";
        }

        public override object SolvePartTwo(int example) {

            String[] input = GetInput(example);


         
            long monkeyBusiness = calculateMonkeyBusiness(input, 10000, false );

            return $"Day 11, Part 2. Monkey business = {monkeyBusiness}";


        }

        private static long calculateMonkeyBusiness(string[] input, int rounds, bool divideBy3) {
            List<Monkey> monkeys = new List<Monkey>();

            Monkey m = new Monkey();
            Monkey.CommonModulus = 1;
            foreach (String s in input) {
                string[] parts = s.Split(new char[] { ' ' });
                if (s.StartsWith("Monkey")) {
                    m = new Monkey();
                } else if (s.Contains("Starting items")) {
                    for (int i = 4; i < parts.Length; i++) {
                        m.Items.Enqueue(ulong.Parse(parts[i].Replace(",", "")));
                    }
                } else if (s.Contains("Operation")) {
                    if (parts[6] == "*" && parts[7] == "old") {
                        m.Operation = Monkey.OperationList.OldTimesOld;
                    } else if (parts[6] == "*") {
                        m.Operation = Monkey.OperationList.OldTimesNumber;
                        m.OperatorNumber = ulong.Parse(parts[7]);
                    } else {
                        m.Operation = Monkey.OperationList.OldPlusNumber;
                        m.OperatorNumber = ulong.Parse(parts[7]);
                    }
                } else if (s.Contains("Test")) {
                    m.TestDivisor = ulong.Parse(parts[5]);
                    Monkey.CommonModulus *= m.TestDivisor;
                } else if (s.Contains("true")) {
                    m.TrueMonkey = int.Parse(parts[9]);
                } else if (s.Contains("false")) {
                    m.FalseMonkey = int.Parse(parts[9]);
                    monkeys.Add(m);
                }

            }

            for (int i = 0; i < rounds; i++) {
                
                for (int j = 0; j < monkeys.Count; j++) {
                    m = monkeys[j];
                    while (m.Items.Count > 0) {
                        ulong item = m.NextItem();
                        switch (m.Operation) {
                            case Monkey.OperationList.OldPlusNumber:
                                item += m.OperatorNumber;
                                break;
                            case Monkey.OperationList.OldTimesNumber:
                                item *= m.OperatorNumber;
                                break;
                            case Monkey.OperationList.OldTimesOld:
                                item *= item;
                                break;
                        }
                        if (divideBy3) {
                            item /= 3;
                        }

                        item %= Monkey.CommonModulus;


                        monkeys[item % m.TestDivisor == 0 ? m.TrueMonkey : m.FalseMonkey].Items.Enqueue(item);
                    }
                }
            }

            monkeys.Sort();
            long monkeyBusiness = monkeys[monkeys.Count - 1].ItemsInspected * monkeys[monkeys.Count - 2].ItemsInspected;
            return monkeyBusiness;
        }

        private class Monkey : IComparable<Monkey> {

            public enum OperationList {
                OldTimesOld,
                OldTimesNumber,
                OldPlusNumber
            }

            public ulong OperatorNumber { get; set; }

            public Queue<ulong> Items { get; set; }


            public ulong TestDivisor { get; set; }

            public int TrueMonkey { get; set; }

            public int FalseMonkey { get; set; }

            public OperationList Operation { get; set; }

            public long ItemsInspected { get; set; }

            static public ulong CommonModulus { get; set; }

            public Monkey() {
                Items = new Queue<ulong>();
                ItemsInspected = 0;
            }

            public ulong NextItem() {
               
                ItemsInspected++;
                return Items.Dequeue();

            }

            public int CompareTo(Monkey other) {
                return ItemsInspected.CompareTo(other.ItemsInspected); 
            }
        }
     
    }
}
