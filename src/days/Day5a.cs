
using System.Text.RegularExpressions;

namespace AdventOfCode.src.days
{
    public partial class Day5a : IDay
    {
        private string _solution = "No Solution";
        public string Solution
        { 
            get { return _solution; }
            private set { _solution = value; } 
        }

        public string Name 
        { 
            get { return "Day5 - Part 1"; }
        }

        private static readonly char[] separator = [' ', ':'];

        public void Solve()
        {
            try
            {

                string workingDirectory = Environment.CurrentDirectory;
                string[] lines = File.ReadAllLines(workingDirectory + "/input/day5.txt");

                List<long> seeds = []; 
                List<List<(long End, long Start, long Range, long Diff)>> maps = [];
                foreach (var line in lines)
                {
                    if (line.StartsWith("seeds:"))
                    {
                        seeds = line.Split(separator, StringSplitOptions.RemoveEmptyEntries)
                                    .Skip(1)
                                    .Select(s => long.Parse(s))
                                    .ToList();
                    }
                    else if (line.EndsWith("map:"))
                    {
                        List<(long, long, long,long)> tmp = [];
                        maps.Add(tmp);
                    }
                    else if (!string.IsNullOrWhiteSpace(line))
                    {
                        var m = FindRange().Match(line);
                        if (!m.Success)
                        {
                            continue;
                        }

                        var mapping = (
                            End: long.Parse(m.Groups[1].Value),
                            Start: long.Parse(m.Groups[2].Value),
                            Range: long.Parse(m.Groups[3].Value),
                            Diff: long.Parse(m.Groups[1].Value) - long.Parse(m.Groups[2].Value));

                        maps.Last().Add(mapping);
                    }
                }

                Solution = seeds
                    .Select(s =>
                    {
                        foreach (var map in maps)
                        {
                            var applicaple_map = map.Find(m => s >= m.Start && s < m.Start + m.Range);
                            if (applicaple_map != (0,0,0,0))
                            {
                                s += applicaple_map.Diff;
                            }
                        }
                        return s;
                    })
                    .Min()
                    .ToString();

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        [GeneratedRegex(@"(\d+)\s+(\d+)\s+(\d+)")]
        private static partial Regex FindRange();
    }
}