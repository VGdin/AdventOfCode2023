
using System.Text.RegularExpressions;

namespace AdventOfCode.src.days
{
    public partial class Day3a : IDay
    {
        private string _solution = "No Solution";
        public string Solution
        { 
            get { return _solution; }
            private set { _solution = value; } 
        }

        public string Name 
        { 
            get { return "Day3 - Part 1"; }
        }

        private static List<(int Number, int StartX, int EndX, int Y)> FindNumbersAndIndices(string line, int y)
        {
            return FindNumber()
                .Matches(line)
                .Cast<Match>()
                .Select(match => (
                    Number: int.Parse(match.Value), 
                    StartX: match.Index, 
                    EndX: match.Index + match.Length - 1, 
                    Y: y
                ))
                .ToList();
        }

        private static List<(char Character, int X, int Y)> FindParts(string line, int y)
        {
            return FindParts()
                .Matches(line)
                .Cast<Match>()
                .Select(match => (
                    Character: match.Value[0], 
                    X: match.Index, 
                    Y: y
                ))
                .ToList();
        }

        private static bool IsSpanningCellAdjacentToSingleCell(int startX1, int endX1, int y1, int x2, int y2)
        {
            if(Math.Abs(y1 - y2) > 1)
            {
                return false;
            } 

            for(int x = startX1; x <= endX1; x++)
            {
                if (Math.Abs(x - x2) <= 1) return true;
            }

            return false;
        } 

        public void Solve()
        {
            try
            {
                string workingDirectory = Environment.CurrentDirectory;
                string[] lines = File.ReadAllLines(workingDirectory + "/input/day3.txt");

                List<(int Number, int StartX, int EndX, int Y)> numbers = [];
                List<(char Character, int X, int Y)> parts = [];
                for(int y = 0; y < lines.Length; y++)
                {
                    numbers.AddRange(FindNumbersAndIndices(lines[y], y));
                    parts.AddRange(FindParts(lines[y], y));
                }
                
                Solution = numbers
                    .Where(n => parts.Any(p => IsSpanningCellAdjacentToSingleCell(n.StartX, n.EndX, n.Y, p.X, p.Y)))
                    .Sum(n => n.Number)
                    .ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        [GeneratedRegex(@"\d+")]
        private static partial Regex FindNumber();
        [GeneratedRegex(@"[^0-9.\s]")]
        private static partial Regex FindParts();
    }
}