
using System.Text.RegularExpressions;

namespace AdventOfCode.src.days
{
    public partial class Day2a : IDay
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

        static bool ExceedsLimit(string input, string color, int limit)
        {
            return Regex.Matches(input, $@"(\d+)\s+{color}")
                    .Cast<Match>()
                    .Any(m => int.Parse(m.Groups[1].Value) > limit);
        }

        public void Solve()
        {

            try
            {
                string workingDirectory = Environment.CurrentDirectory;
                Solution = File.ReadLines(workingDirectory + "/input/day2.txt")
                    .Where(s => 
                        !ExceedsLimit(s, "red", 12) && 
                        !ExceedsLimit(s, "green", 13) &&
                        !ExceedsLimit(s, "blue", 14))
                    .Select(s => FindGameNumber().Match(s))
                    .Where(m => m.Success)
                    .Sum(m => int.Parse(m.Groups[1].Value))
                    .ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        [GeneratedRegex(@"Game\s+(\d+)")]
        private static partial Regex FindGameNumber();
    }
}