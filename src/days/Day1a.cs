
using System.Text.RegularExpressions;

namespace AdventOfCode.src.days
{
    public partial class Day1a : IDay
    {
        private string _solution = "No Solution";
        public string Solution
        { 
            get { return _solution; }
            private set { _solution = value; } 
        }

        public string Name 
        { 
            get { return "Day1 - Part 1"; }
        }

        public void Solve()
        {
            try
            {
                string workingDirectory = Environment.CurrentDirectory;
                Solution = File.ReadLines(workingDirectory + "/input/day1.txt")
                    .Select(line => FindNonDigits().Replace(line, ""))
                    .Where(line => !string.IsNullOrEmpty(line))
                    .Sum(line => 
                    {
                        string concated = line[0].ToString() + line[line.Length -1].ToString();
                        return int.Parse(concated);
                    })
                    .ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        [GeneratedRegex(@"\D")]
        private static partial Regex FindNonDigits();
    }
}