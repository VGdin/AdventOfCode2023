
using System.Text.RegularExpressions;

namespace AdventOfCode.src.days
{
    public partial class Day5b : IDay
    {
        private string _solution = "No Solution";
        public string Solution
        { 
            get { return _solution; }
            private set { _solution = value; } 
        }

        public string Name 
        { 
            get { return "Day5 - Part 2"; }
        }

        public class Map
        {
            public long DestBegin { get; set; }
            public long DestEnd { get; set; }
            public long SourceBegin { get; set; }
            public long SourceEnd { get; set; }
            public long Range { get; set; }
            public long Diff { get; set; }
        }

        public class Seed
        {
            public long Start { get; set; }
            public long End { get; set; }
            public long Range{ get; set; }
        }


        private List<Seed> MapToNext(Seed seed, List<Map> maps)
        {
            List<Seed> result = [];

            foreach(Map map in maps)
            {
                if (map.SourceBegin > seed.End || map.SourceEnd < seed.Start)
                {
                    continue;
                }

                long overlapStart = Math.Max(seed.Start, map.SourceBegin);
                long overlapEnd = Math.Min(seed.End, map.SourceEnd);

                result.Add(new Seed { 
                    Start = overlapStart + map.Diff, 
                    End = overlapEnd + map.Diff, 
                    Range = overlapEnd - overlapStart 
                });

                if (seed.End > map.SourceEnd)
                {
                    result.AddRange(MapToNext(new Seed { 
                        Start = map.SourceEnd + 1, 
                        End = seed.End,
                        Range = map.SourceEnd + 1 - seed.End 
                    }, maps));
                }

                if (seed.Start < map.SourceBegin)
                {
                    result.AddRange(MapToNext(new Seed { 
                        Start = seed.Start, 
                        End = map.SourceBegin - 1,
                        Range = map.SourceBegin - 1 - seed.Start 
                    }, maps));
                }
            }

            if (result.Count == 0)
            {
                result.Add(seed);
            }

            return result;
        }

        public void Solve()
        {
            try
            {
                string workingDirectory = Environment.CurrentDirectory;
                string[] lines = File.ReadAllLines(workingDirectory + "/input/day5.txt");

                List<Seed> seeds = FindSeeds().Matches(lines[0])
                    .Cast<Match>()
                    .Select(m => new Seed()
                    {
                        Start = long.Parse(m.Groups[1].Value),
                        End = long.Parse(m.Groups[1].Value) + long.Parse(m.Groups[2].Value),
                        Range = long.Parse(m.Groups[2].Value)
                    })
                    .ToList();

                List<List<Map>> maps = [];
                foreach (var line in lines)
                {
                    if (line.StartsWith("seeds:"))
                    {
                        continue;
                    }
                    else if (line.EndsWith("map:"))
                    {
                        List<Map> tmp = [];
                        maps.Add(tmp);
                    }
                    else if (!string.IsNullOrWhiteSpace(line))
                    {
                        var m = FindRange().Match(line);
                        if (!m.Success)
                        {
                            continue;
                        }

                        maps.Last().Add(new Map()
                        {
                            DestBegin = long.Parse(m.Groups[1].Value),
                            DestEnd = long.Parse(m.Groups[1].Value) + long.Parse(m.Groups[3].Value),
                            SourceBegin = long.Parse(m.Groups[2].Value),
                            SourceEnd = long.Parse(m.Groups[2].Value) + long.Parse(m.Groups[3].Value),
                            Range = long.Parse(m.Groups[3].Value),
                            Diff = long.Parse(m.Groups[1].Value) - long.Parse(m.Groups[2].Value)
                        });
                    }
                }

                foreach(var map in maps)
                {
                    seeds = seeds.SelectMany(seed => MapToNext(seed, map)).ToList();
                }

                Solution = seeds.Min(s => s.Start).ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        [GeneratedRegex(@"(\d+)\s+(\d+)")]
        private static partial Regex FindSeeds();
        [GeneratedRegex(@"(\d+)\s+(\d+)\s+(\d+)")]
        private static partial Regex FindRange();
    }
}