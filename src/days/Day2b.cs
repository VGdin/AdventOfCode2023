
using System.Text.RegularExpressions;

namespace AdventOfCode.src.days
{
    public partial class Day2b : IDay
    {
        private string _solution = "No Solution";
        public string Solution
        { 
            get { return _solution; }
            private set { _solution = value; } 
        }

        public string Name 
        { 
            get { return "Day2 - Part 2"; }
        }

        private static int FindLargestNumberForColor(string input, string color)
        {
            return Regex.Matches(input, $@"(\d+)\s+{color}")
                        .Cast<Match>()
                        .Select(m => int.Parse(m.Groups[1].Value))
                        .DefaultIfEmpty(0)
                        .Max();
        }

        public void Solve()
        {
            try
            {
                string workingDirectory = Environment.CurrentDirectory;
                Solution = File.ReadLines(workingDirectory + "/input/day2.txt")
                    .Select(s => (
                            Red: FindLargestNumberForColor(s, "red"),
                            Green: FindLargestNumberForColor(s, "green"),
                            Blue: FindLargestNumberForColor(s, "blue")
                        )) 
                    .Sum(g => g.Red * g.Green * g.Blue)
                    .ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}