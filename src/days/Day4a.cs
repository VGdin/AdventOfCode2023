
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
            get { return "Day4 - Part a"; }
        }

        public void Solve()
        {
            try
            {
                string workingDirectory = Environment.CurrentDirectory;
                string[] lines = File.ReadAllLines(workingDirectory + "/input/day3.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}