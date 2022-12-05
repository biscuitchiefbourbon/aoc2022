using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    internal abstract class AdventOfCodeChallenge
    {
        protected List<string[]> _exampleInputs;
        protected string[] _input;

        public AdventOfCodeChallenge(String inputFilename, List<string> exampleInputFilenames) {
            _input = File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + inputFilename);
            _exampleInputs = new List<string[]>();
            foreach(String s in exampleInputFilenames) {
                _exampleInputs.Add(File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + s));
            }
        }

        public string[] GetInput(int example) {
            if( example == 0) {
                return _input;
            } else {
                return _exampleInputs[example - 1];
            }

            
        }

        public abstract object SolvePartOne(int example);

        public abstract object SolvePartTwo(int example);
    }
}
