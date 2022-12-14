using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    class Day13 : AdventOfCodeChallenge {


        public Day13(String inputFilename, List<string> exampleInputFilenames) : base(inputFilename, exampleInputFilenames) {

        }

        public override object SolvePartOne(int example) {
            String[] input = GetInput(example);



            List<ListType> leftLists = new List<ListType>();
            List<ListType> rightLists = new List<ListType>();
            bool isLeft = true;

            foreach (String s in input) {
                if (s.Length > 0) {
                    ListType topLevelList = new ListType(null, s);
                    ListType currentList = topLevelList;
                    String workingNum = "";


                    for (int i = 1; i < s.Length - 1; i++) {
                        if (s[i] == '[') {
                            ListType newSubList = new ListType(currentList, null);
                            currentList.Items.Add(newSubList);
                            currentList = newSubList;
                        } else if (s[i] == ']') {
                            if (workingNum.Length > 0) {
                                currentList.Items.Add(new IntegerType(currentList, int.Parse(workingNum)));
                                workingNum = "";
                            }
                            currentList = currentList.Parent;
                        } else if (Char.IsDigit(s[i])) {
                            workingNum += s[i];
                        } else if (s[i] == ',') {
                            if (workingNum.Length > 0) {
                                currentList.Items.Add(new IntegerType(currentList, int.Parse(workingNum)));
                                workingNum = "";
                            }
                        }
                    }
                    if (workingNum.Length > 0) {
                        currentList.Items.Add(new IntegerType(currentList, int.Parse(workingNum)));
                        workingNum = "";
                    }

                    if (isLeft) {
                        leftLists.Add(topLevelList);
                        isLeft = false;
                    } else {
                        rightLists.Add(topLevelList);
                        isLeft = true;
                    }
                }

            }

            int indecesSum = 0;

            for (int i = 0; i < leftLists.Count; i++) {
                //leftLists[i].MatchTypes(rightLists[i]);
                int rightOrder = leftLists[i].CompareTo(rightLists[i]);
                if (rightOrder==1) {
                    indecesSum += (i + 1);
                }
            }


            return $"Day 13 Part 1, IndecesSum = {indecesSum}";
        }

        public override object SolvePartTwo(int example) {

            String[] input = GetInput(example);
            String[] inputWithDividers = new string[input.Length + 2];
            inputWithDividers[0] = "[[2]]";
            inputWithDividers[1] = "[[6]]";
            Array.Copy(input, 0, inputWithDividers, 2, input.Length);


            List<ListType> all = new List<ListType>();



            foreach (String s in inputWithDividers) {
                if (s.Length > 0) {
                    ListType topLevelList = new ListType(null, s);
                    ListType currentList = topLevelList;
                    String workingNum = "";


                    for (int i = 1; i < s.Length - 1; i++) {
                        if (s[i] == '[') {
                            ListType newSubList = new ListType(currentList, null);
                            currentList.Items.Add(newSubList);
                            currentList = newSubList;
                        } else if (s[i] == ']') {
                            if (workingNum.Length > 0) {
                                currentList.Items.Add(new IntegerType(currentList, int.Parse(workingNum)));
                                workingNum = "";
                            }
                            currentList = currentList.Parent;
                        } else if (Char.IsDigit(s[i])) {
                            workingNum += s[i];
                        } else if (s[i] == ',') {
                            if (workingNum.Length > 0) {
                                currentList.Items.Add(new IntegerType(currentList, int.Parse(workingNum)));
                                workingNum = "";
                            }
                        }
                    }
                    if (workingNum.Length > 0) {
                        currentList.Items.Add(new IntegerType(currentList, int.Parse(workingNum)));
                        workingNum = "";
                    }

                    all.Add(topLevelList);
                }

            }

            all.Sort((a, b) => b.CompareTo(a));

            List<String> sorted = new List<string>();
            foreach(ListType lt in all) {
                sorted.Add(lt.Original);
            }

            int answer = (sorted.IndexOf(inputWithDividers[0])+1) * (sorted.IndexOf(inputWithDividers[1])+1);

            return $"Day 14, Part 2. Decoder key = {answer}";


        }



        class ListType : TypeBase
        {
        
            public List<TypeBase> Items { get; set; }

            public String Original { get; set; }


            public ListType(ListType parent, String original) : base(parent) {
                Items = new List<TypeBase>();
                Parent = parent;
                Original = original;
            }

           
            public void MatchTypes(ListType other) {
                for (int i = 0; i < Items.Count; i++) {
                    if (i < other.Items.Count) {
                        if (this.Items[i] is ListType && other.Items[i] is IntegerType) {
                            ListType lt = new ListType(this, null);
                            lt.Items.Add(other.Items[i]);
                            other.Items[i] = lt;
                            ((ListType)this.Items[i]).MatchTypes((ListType)other.Items[i]);
                        } else if (this.Items[i] is IntegerType && other.Items[i] is ListType) {
                            ListType lt = new ListType(this, null);
                            lt.Items.Add(this.Items[i]);
                            this.Items[i] = lt;
                            ((ListType)this.Items[i]).MatchTypes((ListType)other.Items[i]);
                        }else if(this.Items[i] is ListType && other.Items[i] is ListType) {
                            ((ListType)this.Items[i]).MatchTypes((ListType)other.Items[i]);
                        }
                    }
                        
                }
            }

            public object Clone() {
                ListType clone = new ListType(this.Parent, this.Original);
                for(int i = 0; i < Items.Count; i++) {
                    if (Items[i] is IntegerType) {
                        clone.Items.Add(new IntegerType(clone, ((IntegerType)Items[i]).Value));
                    }else if (Items[i] is ListType) {
                        clone.Items.Add((ListType)((ListType)Items[i]).Clone());
                    }
                }
                return clone;
            }

            public override int CompareTo(object obj) {
               
                ListType leftClone = (ListType)this.Clone();
                ListType rightClone = (ListType)((ListType)obj).Clone();
                if(Parent == null) {
                    leftClone.MatchTypes(rightClone);
                }
               


                int i = 0;
                for (i = 0; i < leftClone.Items.Count; i++) {
                    if (i < rightClone.Items.Count) {

                       int subResult  = leftClone.Items[i].CompareTo(rightClone.Items[i]);
                        if (subResult!=0) {
                            return subResult;
                        }

                    } else {
                        // Right side ran out of items
                        return -1;
                    }
                }
                if (i < rightClone.Items.Count) {
                    // Left side ran out of items,  in right order 
                    return 1;
                }
                return 0;
            }

            public override string ToString() {
                return Original;
            }
        }

        class IntegerType : TypeBase
        {
            public int Value { get; set; }

            public IntegerType(ListType parent, int value) : base(parent) {
                Value = value;
            }

            public override string ToString() {
                return Value.ToString();
            }

          

            public override int CompareTo(object obj) {
                IntegerType rightIntegerType = (IntegerType)obj;
                if (this.Value < rightIntegerType.Value) {
                    // Correct order
                    return 1;
                } else if (this.Value > rightIntegerType.Value) {
                    // Incorrect order
                    return -1;
                } else {
                    // Equal means check next, return null;
                    return 0;
                }
            }
        }

        abstract class TypeBase : IComparable
        {
            public ListType Parent { get; set; }


            public TypeBase(ListType parent) {
                Parent = parent;
            }

            public abstract int CompareTo(object obj);

        

        }


    }
}
