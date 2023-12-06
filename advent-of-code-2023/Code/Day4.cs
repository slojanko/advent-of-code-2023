using System.Linq;

internal class Day4 : Solver
{
    public class Card
    {
        public List<int> winning_numbers = new List<int>();
        public List<int> numbers_you_have = new List<int>();

        public int matches = 0;
        public int count = 0;
    }

    public override void SolveEasy()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day4.txt");
        int result = 0;
        List<Card> cards = new List<Card>();

        foreach(var line in input)
        {
            var all_numbers = line.Split(": ")[1];
            var number_groups = all_numbers.Split(" | ");

            Card card = new Card();
            card.winning_numbers = number_groups[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            card.numbers_you_have = number_groups[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

            cards.Add(card);
        }

        foreach(var card in cards) {
            foreach(var number in card.numbers_you_have)
            {
                if (card.winning_numbers.Contains(number))
                {
                    card.matches++;
                }
            }

            result += (int)Math.Round(Math.Pow(2, card.matches - 1));
        }

        PrintEasy(result);
    }

    public override void SolveHard()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day4.txt");
        int result = 0;
        List<Card> cards = new List<Card>();

        foreach (var line in input)
        {
            var all_numbers = line.Split(": ")[1];
            var number_groups = all_numbers.Split(" | ");

            Card card = new Card();
            card.winning_numbers = number_groups[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            card.numbers_you_have = number_groups[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            card.count = 1;

            cards.Add(card);
        }

        foreach (var card in cards)
        {
            foreach (var number in card.numbers_you_have)
            {
                if (card.winning_numbers.Contains(number))
                {
                    card.matches++;
                }
            }
        }

        for (int i = 0; i < cards.Count; i++)
        {
            for (int j = i + 1; j <= Math.Min(cards.Count - 1, i + cards[i].matches); j++)
            {
                cards[j].count += cards[i].count;
            }

            result += cards[i].count;
        }

        PrintHard(result);
    }
}