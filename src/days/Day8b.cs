
using System.Security.AccessControl;
using System.Text.RegularExpressions;

namespace AdventOfCode.src.days
{
    public partial class Day8b : IDay
    {
        private string _solution = "No Solution";
        public string Solution
        { 
            get { return _solution; }
            private set { _solution = value; } 
        }

        public string Name 
        { 
            get { return "Day8 - Part 2"; }
        }

        public enum Instruction
        {
            Left,
            Right
        }

        private static (bool, int, int) FindLoop(List<string> strings, int startIndex)
        {

            if (startIndex >= strings.Count)
            {
                return (false, -1, 0); // Base case: no loop found
            }

            for (int len = 1; len <= (strings.Count - startIndex) / 2; len++)
            {
                if (IsRepeatingSequence(strings, startIndex, len))
                {
                    return (true, startIndex, len);
                }
            }

            // Recursive call to check from the next index
            return FindLoop(strings, startIndex + 1);
        }

        private static bool IsRepeatingSequence(List<string> strings, int startIndex, int len)
        {
            for (int i = startIndex; i + len < strings.Count; i++)
            {
                if (strings[i] != strings[i + len])
                {
                    return false;
                }
            }
            return true;
        }

        private static int FindSyncTime(List<List<string>> cycles)
        {
            var cycleLengths = cycles
                .Select(cycle => cycle.Count)
                .ToList();

            var offsets = cycles
                .Select(sublist => sublist.FindIndex(str => str.EndsWith('Z')))
                .ToList();
            int lcm = cycleLengths.Aggregate(1, (currentLcm, length) => Lcm(currentLcm, length));
            int maxOffset = offsets.Max();

            Console.WriteLine("Cycle Lenghts");
            cycleLengths.ForEach(cl => Console.WriteLine(cl));
            Console.WriteLine("Offsets");
            offsets.ForEach(os => Console.WriteLine(os));
            Console.WriteLine("Lcm: " + lcm + " Max Offset: " + maxOffset);


            int syncTime = maxOffset;
            while (syncTime % lcm != 0 || syncTime < maxOffset)
            {
                syncTime += lcm;
            }

            return syncTime;
        }

        private static int Gcd(int a, int b)
        {
            return b == 0 ? a : Gcd(b, a % b);
        }

        private static int Lcm(int a, int b)
        {
            return a / Gcd(a, b) * b;
        }

        public static bool AllGhostsOnZ(string[] positions)
        {
            return positions.All(p => p.EndsWith('Z'));
        }

        public void Solve()
        {
            try
            {
                string workingDirectory = Environment.CurrentDirectory;
                string[] lines = File.ReadAllLines(workingDirectory + "/input/day8.txt");

                List<Instruction> instructions = lines[0]
                    .Select(c => c == 'L' ? Instruction.Left : (c == 'R' ? Instruction.Right : (Instruction?)null))
                    .Where(i => i != null)
                    .Cast<Instruction>()
                    .ToList();

                var mapping = new Dictionary<(string, Instruction), string>();
                lines
                    .Select(line => FindMaps().Match(line))
                    .Where(m => m.Success)
                    .Select(m => (Origin: m.Groups[1].Value, Left: m.Groups[2].Value, Right: m.Groups[3].Value))
                    .ToList()
                    .ForEach(t => 
                    {
                        mapping.Add((t.Origin, Instruction.Left), t.Left);
                        mapping.Add((t.Origin, Instruction.Right), t.Right);
                    });

                string[] ghostPos = lines
                    .Select(line => FindMaps().Match(line))
                    .Where(m => m.Success)
                    .Select(m => m.Groups[1].Value)
                    .Where(s => s.EndsWith('A'))
                    .ToArray();

                List<string>[] ghostHistory = ghostPos
                    .Select(p => new List<string>())
                    .ToArray();

                (bool Found, int StartIndex, int Length)[] ghostLoops = ghostPos
                    .Select(p => (false, -1, 0))
                    .ToArray();                

                int steps = 0;
                while (!AllGhostsOnZ(ghostPos))
                {
                    // Every so often, check if a cycle has appeared
                    if (steps % 50000 == 0)
                    {
                        Console.WriteLine(steps + " - " + ghostLoops.Count(l => l.Found) + " Loop(s) found yet.");
                        for (int i = 0; i < ghostPos.Length; i++)
                        {
                            if(ghostLoops[i].Found)
                            {
                                continue;
                            }

                            ghostLoops[i] = FindLoop(ghostHistory[i], 0);
                            (bool hasLoop, int startIndex, int length) = ghostLoops[i];
                            Console.WriteLine(i + ") " + ghostPos[i] + " = " + hasLoop + " " + length + " starting at step " + startIndex );
                        }
                    }

                    // If all of the cycles are found, calc time until sync and break
                    if (ghostLoops.All(l => l.Found))
                    {
                        Console.WriteLine("All loops found!");
                        List<List<string>> onlyCyclesWithStartFixed = ghostHistory
                            .Select((h, index) =>
                            {
                                var onlyCycle = h.GetRange(ghostLoops[index].StartIndex, ghostLoops[index].Length);
                                int indexInCycle = (steps - ghostLoops[index].StartIndex) % ghostLoops[index].Length;

                                // Re-arrange cycle to match current step with index in cycle
                                var tmp = onlyCycle.GetRange(0, indexInCycle);
                                onlyCycle.RemoveRange(0, indexInCycle);
                                onlyCycle.AddRange(tmp);

                                Console.WriteLine(onlyCycle.Count);
                                return onlyCycle;
                            })
                            .ToList();

                        int timeToSync = FindSyncTime(onlyCyclesWithStartFixed);
                        Console.WriteLine("Time to sync: " + timeToSync);
                        Console.WriteLine("Time when done: " + (timeToSync + steps));
                        steps += timeToSync;
                        break;
                    }

                    // Update the position, and add to history
                    Instruction currentInstruction = instructions[steps % instructions.Count];
                    for (int i = 0; i < ghostPos.Length; i++)
                    {
                        ghostHistory[i].Add(ghostPos[i]);
                        ghostPos[i] = mapping.GetValueOrDefault((ghostPos[i], currentInstruction),"");
                    }

                    steps++;
                }


                Solution = steps.ToString();
            }

            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
        [GeneratedRegex(@"(\w+)\s*=\s*\((\w+),\s*(\w+)\)")]
        private static partial Regex FindMaps();
        [GeneratedRegex(@"^(A\w+)\s")]
        private static partial Regex FindGhostStarts();
    }
}