using AdventOfCode.src.days;

class Program
{
    static void Main(string[] args)
    {
        var days = new Dictionary<int, IDay>
        {
            { 1, new Day1a() },
            { 2, new Day1b() },
            { 3, new Day2a() },
            { 4, new Day2b() },
            { 5, new Day3a() },
            { 6, new Day3b() },
            { 7, new Day4a() },
            { 8, new Day4b() },
            { 9, new Day5a() },
            { 10, new Day5b() },
            { 11, new Day6a() },
            { 12, new Day6b() },
            { 13, new Day7a() },
            { 14, new Day7b() },
            { 15, new Day8a() },
            { 16, new Day8b() },
            { 22, new Day11a() },
            { 23, new Day11b() },
        };

        Console.WriteLine("Which problem do you want to solve?");
        days.ToList().ForEach(pair => Console.WriteLine($"\t{pair.Key})\t{pair.Value.Name}"));
        if (int.TryParse(Console.ReadLine(), out int dayNumber) && days.ContainsKey(dayNumber))
        {
            IDay day = days[dayNumber];
            Console.WriteLine("Solving: " + day.Name);
            days[dayNumber].Solve();
            Console.WriteLine(days[dayNumber].Solution);
        }
        else
        {
            Console.WriteLine("Invalid selection.");
        }
    }
}
