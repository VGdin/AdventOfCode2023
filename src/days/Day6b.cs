
using System.Text.RegularExpressions;

namespace AdventOfCode.src.days
{
    public partial class Day6b : IDay
    {
        private string _solution = "No Solution";
        public string Solution
        { 
            get { return _solution; }
            private set { _solution = value; } 
        }

        public string Name 
        { 
            get { return "Day6 - Part 2"; }
        }

        public void Solve()
        {
            try
            {
                string workingDirectory = Environment.CurrentDirectory;
                string[] lines = File.ReadAllLines(workingDirectory + "/input/day6.txt");

                List<string> times = FindNumber()
                    .Matches(lines[0])
                    .Cast<Match>()
                    .Where(m => m.Success)
                    .Select(m => m.Groups[1].Value)
                    .ToList();

                List<string> distances = FindNumber()
                    .Matches(lines[1])
                    .Cast<Match>()
                    .Where(m => m.Success)
                    .Select(m => m.Groups[1].Value)
                    .ToList();

                long time = long.Parse(string.Concat(times));
                long distance = long.Parse(string.Concat(distances));

                int error_margin = 0;
                for (long i = 1; i<time; i++) // Skip 0 and Max, always will be zero
                {
                    long traveled = i * (time-i);
                    if (traveled > distance)
                    {
                        error_margin++;
                    }
                }

                Solution = error_margin.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        [GeneratedRegex(@"(\d+)")]
        private static partial Regex FindNumber();
    }
}