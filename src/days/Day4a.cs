
using System.Text.RegularExpressions;

namespace AdventOfCode.src.days
{
    public partial class Day4a : IDay
    {
        private string _solution = "No Solution";
        public string Solution
        { 
            get { return _solution; }
            private set { _solution = value; } 
        }

        public string Name 
        { 
            get { return "Day4 - Part 1"; }
        }

        private static int NumbersToPoints(int num)
        {
            return num <= 0 ? 0 : Enumerable
                .Repeat(2, num - 1)
                .Aggregate(1, (a, b) => a * b);
        }

        public void Solve()
        {
            try
            {
                string workingDirectory = Environment.CurrentDirectory;
                Solution = File.ReadLines(workingDirectory + "/input/day4.txt")
                    .Select(s => 
                        {
                            var slaks = s.Split(':');
                            var numberSets = slaks[1].Split('|');
                            return (WinningNumbers: numberSets[0], YouNumbers: numberSets[1]);
                        }) 
                    .Select(ss =>
                    {
                        var winningNumbersList =  FindNumber().Matches(ss.WinningNumbers)
                                    .Cast<Match>()
                                    .Select(m => int.Parse(m.Groups[1].Value))
                                    .ToList();

                        var youNumbersList =  FindNumber().Matches(ss.YouNumbers)
                                    .Cast<Match>()
                                    .Select(m => int.Parse(m.Groups[1].Value))
                                    .ToList();

                        return (WinningNumbers: winningNumbersList, YouNumbers: youNumbersList);
                    })
                    .Sum(ls =>
                    {
                        var matchingNumbers = ls.WinningNumbers.Count(wn => ls.YouNumbers.Contains(wn));
                        var points = NumbersToPoints(matchingNumbers);
                        Console.WriteLine(matchingNumbers +"->"+ points);
                        return points;
                    })
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