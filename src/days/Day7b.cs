
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Dynamic;

namespace AdventOfCode.src.days
{
    public partial class Day7b : IDay
    {
        private string _solution = "No Solution";
        public string Solution
        { 
            get { return _solution; }
            private set { _solution = value; } 
        }

        public string Name 
        { 
            get { return "Day7 - Part 2"; }
        }

        public enum HandType
        {
            FiveOfAKind,
            FourOfAKind,
            FullHouse,
            ThreeOfAKind,
            TwoPair,
            OnePair,
            HighCard
        }

        public static int GetValueFromChar(char c) => c switch
        {
            'A' => 14,
            'K' => 13,
            'Q' => 12,
            'T' => 10,
            'J' => 1, // Joker == 1
            _   => c-'0'
        };

        public static int GetHandTypeRank(HandType handType) => handType switch
        {
            HandType.FiveOfAKind  => 6,
            HandType.FourOfAKind  => 5,
            HandType.FullHouse    => 4,
            HandType.ThreeOfAKind => 3,
            HandType.TwoPair      => 2,
            HandType.OnePair      => 1,
            HandType.HighCard     => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(handType), $"Not expected handtype value: {handType}"),
        };

        private static bool HasAtLeastGivenCards(string input, int no_cards)
        {
            var charCounts = input
                .GroupBy(c => c)
                .Select(group => new { Char = group.Key, Count = group.Count() })
                .ToList();

            return charCounts.Any(x => x.Count >= no_cards);
        }

        private static bool HasFullHouse(string input)
        {
            var charCounts = input
                .GroupBy(c => c)
                .Select(group => new { Char = group.Key, Count = group.Count() })
                .ToList();


            return charCounts.Any(c => c.Count == 3) && charCounts.Any(c => c.Count == 2);
        }
                  
        private static bool HasTwoPairs(string input)
        {
            var charCounts = input
                .GroupBy(c => c)
                .Select(group => new { Char = group.Key, Count = group.Count() })
                .Where(x => x.Count >= 2)
                .ToList();

        return charCounts.Count == 2;
        }
                  
        public static HandType GetHandType(string cards) => cards switch
        {
            var x when HasAtLeastGivenCards(x, 5) => HandType.FiveOfAKind,
            var x when HasAtLeastGivenCards(x, 4) => HandType.FourOfAKind,
            var x when HasFullHouse(x)            => HandType.FullHouse,
            var x when HasAtLeastGivenCards(x, 3) => HandType.ThreeOfAKind,
            var x when HasTwoPairs(x)             => HandType.TwoPair,
            var x when HasAtLeastGivenCards(x, 2) => HandType.OnePair,
            _                                     => HandType.HighCard
        };
        public static HandType ImproveHand(HandType handType) => handType switch
        {
            HandType.FiveOfAKind  => HandType.FiveOfAKind, // Cant be improved
            HandType.FourOfAKind  => HandType.FiveOfAKind,
            HandType.FullHouse    => HandType.FourOfAKind,
            HandType.ThreeOfAKind => HandType.FourOfAKind,
            HandType.TwoPair      => HandType.FullHouse,
            HandType.OnePair      => HandType.ThreeOfAKind,
            HandType.HighCard     => HandType.OnePair,
            _ => throw new ArgumentOutOfRangeException(nameof(handType), $"Not expected handtype value: {handType}"),
        };

        public static HandType GetHandTypeWithUpgrade(string cards)
        {
            int numberOfJokers = cards.Count(c => c == 'J');
            HandType handType = GetHandType(cards.Replace("J", ""));
            for (int i = 0; i < numberOfJokers; i++)
            {
                handType = ImproveHand(handType);
            }
            return handType;
        }

        private static string OrderByOccurences(string s)
        {
            return new string (s
                .GroupBy(c => c)
                .OrderByDescending(g => g.Count())
                .ThenBy(g => g.Key)
                .SelectMany(g => g)
                .ToArray());
        }

        public struct Hand(int bid, string cards)
        {
            private readonly string cards = cards;
            private readonly List<char> _cards = [.. cards];

            public HandType HandType { get; private set; } = GetHandTypeWithUpgrade(cards);
            public int Rank { get; private set; } = GetHandTypeRank(GetHandTypeWithUpgrade(cards));
            public int Bid { get; private set; } = bid;

            public readonly string OrderedRepresentation { get { return OrderByOccurences(cards);} }
            public readonly string Representation { get { return cards;} }
            public readonly ReadOnlyCollection<char> Cards => _cards.AsReadOnly();
        }

        public Comparison<Hand> sortHand = (x, y) => 
        {
            int type_check = x.Rank - y.Rank;

            if (type_check != 0)
            {
                return type_check;
            }

            for (int i = 0; i<5; i++)
            {
                int card_check = GetValueFromChar(x.Cards[i]) - GetValueFromChar(y.Cards[i]);
                if (card_check != 0)
                {
                    return card_check;
                }
            }
            return 0;
        };

        public void Solve()
        {
            try
            {
                string workingDirectory = Environment.CurrentDirectory;
                string[] lines = File.ReadAllLines(workingDirectory + "/input/day7.txt");

                List<Hand> hands = lines
                    .Select( line =>
                    {
                        Match match = FindHandAndBid().Match(line);

                        return new Hand(
                            int.Parse(match.Groups[2].Value),
                            match.Groups[1].Value
                        );
                    })
                    .ToList();

                hands.Sort(sortHand);

                Solution = hands
                    .Select((hand, index) =>
                    {
                        Console.WriteLine(index + ") " + hand.Representation + "-" + hand.OrderedRepresentation + " = " + hand.HandType);
                        return hand.Bid * (index + 1);
                    })
                    .Sum()
                    .ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        [GeneratedRegex(@"^([A-Za-z0-9]+) (\d+)")]
        private static partial Regex FindHandAndBid();
    }
}