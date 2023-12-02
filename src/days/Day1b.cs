
using System.Text.RegularExpressions;

namespace AdventOfCode.src.days
{
    public partial class Day1b : IDay
    {
        private string _solution = "No Solution";
        public string Solution
        { 
            get { return _solution; }
            private set { _solution = value; } 
        }

        public string Name 
        { 
            get { return "Day1 - Part 2"; }
        }

        private static readonly Dictionary<string, string> spelledDigits = new()
        {
            { "on", "1" },
            { "tw", "2" },
            { "thre", "3" },
            { "four", "4" },
            { "fiv", "5" },
            { "six", "6" },
            { "seven", "7" },
            { "eigh", "8" },
            { "nin", "9" }
        };

        public void Solve()
        {
            try
            {
                string workingDirectory = Environment.CurrentDirectory;

                Solution = File.ReadLines(workingDirectory + "/input/day1.txt")
                    .Select(line => 
                    {
                        string tmp = "";
                        MatchCollection matches = FindText().Matches(line);
                        foreach (Match match in matches.Cast<Match>())
                        {
                            if (match.Value.Length == 1)
                            {
                                tmp += match.Value;
                            }
                            else
                            {
                                tmp += spelledDigits.GetValueOrDefault(match.Value, "");
                            }
                        }
                        return tmp;
                    })
                    .Select(line => FindNonDigits().Replace(line, ""))
                    .Where(line => !string.IsNullOrEmpty(line))
                    .Sum(line => 
                    {
                        string concated = line[0].ToString() + line[^1].ToString();
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
        [GeneratedRegex(@"(?:zero|(?=one)on|(?=two)tw|(?=three)thre|four|(?=five)fiv|six|seven|(?=eight)eigh|(?=nine)nin|\d)")]
        private static partial Regex FindText();
    }
}