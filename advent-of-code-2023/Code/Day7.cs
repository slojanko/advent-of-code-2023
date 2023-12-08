internal class Day7 : Solver
{
    public enum HandType : int
    {
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind,
    }

    public class Hand
    {
        public string cards;
        public int bid;
        public HandType handType;
        public bool is_hard;

        public static string strengths = "23456789TJQKA";
        public static string strengths_hard = "J23456789TQKA";
        public static int[] count = new int[13];

        public Hand(string cards, int bid, bool is_hard)
        {
            this.cards = cards;
            this.bid = bid;
            this.is_hard = is_hard;
            Array.Clear(count);

            foreach (var ch in cards) 
            {
                count[is_hard ? strengths_hard.IndexOf(ch) : strengths.IndexOf(ch)]++;
            }

            if (is_hard) { 
                int j_count = count[0];
                count[0] = 0;

                Array.Sort(count, (x, y) => y.CompareTo(x));

                count[0] += j_count;
            } else
            {
                Array.Sort(count, (x, y) => y.CompareTo(x));
            }

            handType = DetermineHandType();
        }

        public HandType DetermineHandType()
        {
            if (count[0]== 5)
            {
                return HandType.FiveOfAKind;
            } else if (count[0] == 4)
            {
                return HandType.FourOfAKind;
            } else if (count[0] == 3 && count[1] == 2)
            {
                return HandType.FullHouse;
            } else if (count[0] == 3)
            {
                return HandType.ThreeOfAKind;
            } else if (count[0] == 2 && count[1] == 2)
            {
                return HandType.TwoPair;
            } else if (count[0] == 2)
            {
                return HandType.OnePair;
            } else
            {
                return HandType.HighCard;
            }
        }

        public int CompareTo(Hand other)
        {
            for(int i = 0; i < cards.Length; i++)
            {
                int selfIndex = is_hard ? strengths_hard.IndexOf(cards[i]) : strengths.IndexOf(cards[i]);
                int otherIndex = is_hard ? strengths_hard.IndexOf(other.cards[i]) : strengths.IndexOf(other.cards[i]);

                if (selfIndex > otherIndex) {
                    return 1;
                } else if (selfIndex < otherIndex)
                {
                    return -1;
                }
            }

            return 0;
        }
    }

    public override void SolveEasy()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day7.txt");
        long result = 0;
        List<Hand> hands = new List<Hand>();

        ReadInput(input, hands, false);

        hands.Sort((x, y) =>
        {
            var basic = x.handType.CompareTo(y.handType);
            if (basic != 0)
            {
                return basic;
            }
            return x.CompareTo(y);
        });

        for(int i = 0; i < hands.Count; i++)
        {
            result += (i + 1) * hands[i].bid;
        }

        PrintEasy(result);
    }

    public override void SolveHard()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day7.txt");
        long result = 0;
        List<Hand> hands = new List<Hand>();

        ReadInput(input, hands, true);

        hands.Sort((x, y) =>
        {
            var basic = x.handType.CompareTo(y.handType);
            if (basic != 0)
            {
                return basic;
            }
            return x.CompareTo(y);
        });

        for (int i = 0; i < hands.Count; i++)
        {
            result += (i + 1) * hands[i].bid;
        }

        PrintHard(result);
    }

    public void ReadInput(string[] input, List<Hand> hands, bool is_hard)
    {
        foreach(var line in input)
        {
            var tokens = line.Split(' ');
            hands.Add(new Hand(tokens[0], int.Parse(tokens[1]), is_hard));
        }
    }
}
