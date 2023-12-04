
using System.Text.RegularExpressions;

namespace AdventOfCode.src.days
{
    public partial class Day4b : IDay
    {
        private string _solution = "No Solution";
        public string Solution
        { 
            get { return _solution; }
            private set { _solution = value; } 
        }

        public string Name 
        { 
            get { return "Day4 - Part 2"; }
        }
        private static List<int> ModifyList(List<int> numbers, List<int> copies, int index)
        {
            if (index >= copies.Count)
            {
                return copies;
            }

            int incrementCount = numbers[index-1];
            var updatedCopies = copies
                .Select((n, i) => (i > index &&
                                   i <= index + incrementCount && 
                                   i < numbers.Count) ? 
                                   n + copies[index] : n)
                .ToList();

            return ModifyList(numbers, updatedCopies, index + 1);
        }

        public void Solve()
        {
            try
            {
                string workingDirectory = Environment.CurrentDirectory;
                var lists = File.ReadLines(workingDirectory + "/input/day4.txt")
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
                    });

                
                var winningNumbers = lists 
                    .Select(ls =>
                    {
                        return ls.WinningNumbers.Count(wn => ls.YouNumbers.Contains(wn));
                    })
                    .ToList();
                
                List<int> initialCopies = Enumerable
                    .Repeat(1, winningNumbers.Count + 1)
                    .ToList();
                var finalCopies = ModifyList(winningNumbers, initialCopies, 1);

                finalCopies.RemoveAt(0);
                Solution = finalCopies.Sum().ToString();

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