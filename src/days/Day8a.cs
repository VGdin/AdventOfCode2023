
using System.Security.AccessControl;
using System.Text.RegularExpressions;

namespace AdventOfCode.src.days
{
    public partial class Day8a : IDay
    {
        private string _solution = "No Solution";
        public string Solution
        { 
            get { return _solution; }
            private set { _solution = value; } 
        }

        public string Name 
        { 
            get { return "Day8 - Part 1"; }
        }

        public enum Instruction
        {
            Left,
            Right
        }

        public void Solve()
        {
            try
            {
                string workingDirectory = Environment.CurrentDirectory;
                string[] lines = File.ReadAllLines(workingDirectory + "/input/day8.txt");

                List<Instruction> instructions = lines[0]
                    .Select(c => c == 'L' ? Instruction.Left : (c == 'R' ? Instruction.Right : (Instruction?)null))
                    .Where(i => i != null)
                    .Cast<Instruction>()
                    .ToList();

                var mapping = new Dictionary<(string, Instruction), string>();
                lines
                    .Select(line => FindMaps().Match(line))
                    .Where(m => m.Success)
                    .Select(m => (Origin: m.Groups[1].Value, Left: m.Groups[2].Value, Right: m.Groups[3].Value))
                    .ToList()
                    .ForEach(t => 
                    {
                        mapping.Add((t.Origin, Instruction.Left), t.Left);
                        mapping.Add((t.Origin, Instruction.Right), t.Right);
                    });

                string current = "AAA";
                int steps = 0;
                while (current != "ZZZ")
                {
                    Instruction currentInstruction = instructions[steps % instructions.Count];
                    current = mapping.GetValueOrDefault((current, currentInstruction),"");
                    steps++;
                }

                Solution = steps.ToString();
            }

            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        [GeneratedRegex(@"(\w+)\s*=\s*\((\w+),\s*(\w+)\)")]
        private static partial Regex FindMaps();
    }
}