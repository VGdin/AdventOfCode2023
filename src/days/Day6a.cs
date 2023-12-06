
using System.Text.RegularExpressions;

namespace AdventOfCode.src.days
{
    public partial class Day6a : IDay
    {
        private string _solution = "No Solution";
        public string Solution
        { 
            get { return _solution; }
            private set { _solution = value; } 
        }

        public string Name 
        { 
            get { return "Day6 - Part 1"; }
        }

        public void Solve()
        {
            try
            {
                string workingDirectory = Environment.CurrentDirectory;
                string[] lines = File.ReadAllLines(workingDirectory + "/input/day6.txt");

                List<int> time = FindNumber()
                    .Matches(lines[0])
                    .Cast<Match>()
                    .Where(m => m.Success)
                    .Select(m => int.Parse(m.Groups[1].Value))
                    .ToList();

                List<int> distance = FindNumber()
                    .Matches(lines[1])
                    .Cast<Match>()
                    .Where(m => m.Success)
                    .Select(m => int.Parse(m.Groups[1].Value))
                    .ToList();

                List<(int Time, int Distance)> timeAndDistance = time
                    .Zip(distance, (t, d) => (t,d))
                    .ToList();

                // Optimal time is always T/2
                Solution = timeAndDistance
                    .Select(td => 
                    {
                        int error_margin = 0;
                        for (int i = 1; i<td.Time; i++) // Skip 0 and Max, always will be zero
                        {
                            int traveled = i * (td.Time-i);
                            if (traveled > td.Distance)
                            {
                                error_margin++;
                            }
                        }

                        return error_margin;
                    })
                    .Aggregate(1, (acc, em) => acc * em)
                    .ToString();
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