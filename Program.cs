using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.src.days;

class Program
{
    static void Main(string[] args)
    {
        var days = new Dictionary<int, IDay>
        {
            { 1, new Day1a() },
            { 2, new Day1b() },
        };

        Console.WriteLine("Which problem do you want to solve?");
        days.ToList().ForEach(pair => Console.WriteLine($"\t{pair.Key}) {pair.Value.Name}"));
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
