
using System.Text.RegularExpressions;

namespace AdventOfCode.src.days
{
    public partial class Day11b : IDay
    {
        private string _solution = "No Solution";
        public string Solution
        { 
            get { return _solution; }
            private set { _solution = value; } 
        }

        public string Name 
        { 
            get { return "Day11 - Part 2"; }
        }

        private struct Galaxy(int id, long x, long y)
        {
            public int id = id;
            public long x = x;
            public long y = y;
        }

        private static long DistanceBetweenGalaxies(List<Galaxy> galaxies)
        {
            long sum = 0;

            for (int i = 0; i < galaxies.Count - 1; i++)
            {
                for (int j = i + 1; j < galaxies.Count; j++)
                {
                    sum += Math.Abs(galaxies[i].x - galaxies[j].x)
                        + Math.Abs(galaxies[i].y - galaxies[j].y);
                }
            }

            return sum;
        }

        private static List<Galaxy> GetGalaxies(string [] universe)
        {
            int years = 1000000;
            int x_size = universe[0].Length;
            int y_size = universe.Length;

            int galaxy_id = 1;
            List<Galaxy> galaxies = [];

            long y_expansion = 0;
            for(int y = 0; y < y_size; y++)
            {

                if (!universe[y].Contains('.'))
                {
                    y_expansion += years - 1;
                }

                long x_expansion = 0;
                for(int x = 0; x < x_size; x++)
                {
                    if (universe[y][x] == '@')
                    {
                        x_expansion += years - 1;
                    }

                    if (universe[y][x] == '#')
                    {
                        galaxies.Add(new Galaxy(galaxy_id++, x + x_expansion, y + y_expansion));
                    }
                }
            }

            return galaxies;
        }

        private static void PrintUniverse(string[] universe)
        {
            foreach(string row in universe)
            {
                Console.WriteLine(row);
            }
        }

        private static string[] ExpandUniverse(string[] start)
        {
            List<string> expandedUnivers = [];
            int x_size = start[0].Length;
            int y_size = start.Length;

            List<int> rowsToExpand = [];
            List<int> colsToExpand = [];

            // Find cols to expand
            for(int x = 0; x < x_size; x++)
            {
                bool containsUniverse = false;
                for(int y = 0; y < y_size; y++)
                {
                    containsUniverse |= start[y][x] == '#';
                }

                if (!containsUniverse)
                {
                    colsToExpand.Add(x);
                }
            }

            // Find rows to expand
            for(int y = 0; y < y_size; y++)
            {
                if (!start[y].Contains('#'))
                {
                    rowsToExpand.Add(y);
                }
            }

            // Expand cols
            for(int y = 0; y < y_size; y++)
            {
                System.Text.StringBuilder row = new(start[y]);
                for(int i = 0; i < colsToExpand.Count; i++)
                {
                    row[colsToExpand[i]] = '@';
                }
                expandedUnivers.Add(row.ToString());
            }

            // Expand rows
            for(int i = 0; i < rowsToExpand.Count; i++)
            {
                expandedUnivers[rowsToExpand[i]] = new string('@', x_size);
            }

            return [.. expandedUnivers];
        }

        public void Solve()
        {
            try
            {
                string workingDirectory = Environment.CurrentDirectory;
                string[] lines = File.ReadAllLines(workingDirectory + "/input/day11.txt");

                Console.WriteLine("Before expansion: ");
                PrintUniverse(lines);

                var expanded = ExpandUniverse(lines);
                Console.WriteLine("After expansion: ");
                PrintUniverse(expanded);

                List<Galaxy> galaxies = GetGalaxies(expanded);
                galaxies.ForEach(g => Console.WriteLine(g.id + ")" + "\ty: " + g.y + "\tx: " + g.x));

                Solution = DistanceBetweenGalaxies(galaxies).ToString();
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