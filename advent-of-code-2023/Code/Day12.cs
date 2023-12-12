internal class Day12 : Solver
{
    public enum MatchType
    {
        None,
        Spring,
    }

    public class Row
    {
        public string springs;
        public List<int> numbers;
        public char[] temp;

        public Row(string springs, List<int> numbers)
        {
            this.springs = springs;
            this.numbers = numbers;
            this.temp = new char[this.springs.Length];
            Array.Fill(temp, '.');
        }
    }

    public override void SolveEasy()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day12.txt");
        long result = 0;

        List<Row> rows = new List<Row>();

        ReadInput(input, rows);

        foreach (var row in rows)
        {
            result += SolveRecursive(row, 0, 0);
        }

        PrintEasy(result);
    }

    public override void SolveHard()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day12.txt");
        long result = 0;

        PrintHard(result);
    }

    public long SolveRecursive(Row row, int sprId, int numId)
    {
        int next_damaged = sprId == -1 ? sprId : GetNextDamaged(row, sprId);

        if (numId == row.numbers.Count && next_damaged == -1)
        {
            Console.WriteLine(new string(row.temp) + " +");
            return 1;
        }

        if (sprId == -1)
        {
            Console.WriteLine(new string(row.temp) + " -");
            return 0;
        }

        long result = 0;

        for (int i = sprId; i <= next_damaged; i++)
        {
            MatchType type = GetMatchType(row, i, numId);
            if (type == MatchType.None)
            {
                continue;
            }

            row.temp[i] = (char)((int)'0' + row.numbers[numId]);
            Console.WriteLine(new string(row.temp));

            int next_potential = GetNextPotential(row, i + row.numbers[numId]);

            if (next_potential != - 1 && next_potential == i + row.numbers[numId])
            {
                if (row.springs[next_potential] == '#')
                {
                    row.temp[i] = '.';
                    continue;
                } else if (row.springs[next_potential] == '?')
                {
                    next_potential = GetNextPotential(row, i + 1 + row.numbers[numId]);
                }
            }

            result += SolveRecursive(row, next_potential, numId + 1);

            row.temp[i] = '.';
        }

        return result;
    }

    public MatchType GetMatchType(Row row, int sprId, int numId)
    {
        int end = sprId + row.numbers[numId];

        for (int i = sprId; i < end; i++)
        {
            if (row.springs[i] == '.')
            {
                return MatchType.None;
            }
        }

        return MatchType.Spring;
    }

    public int GetNextDamaged(Row row, int sprId)
    {
        for(int i = sprId; i < row.springs.Length; i++)
        {
            if (row.springs[i] == '#')
            {
                return i;
            }
        }

        return -1;
    }

    public int GetNextPotential(Row row, int sprId)
    { 
        for (int i = sprId; i < row.springs.Length; i++)
        {
            if (row.springs[i] == '#' || row.springs[i] == '?')
            {
                return i;
            }
        }

        return -1;
    }

    public void ReadInput(string[] input, List<Row> rows)
    {
        foreach (var line in input)
        {
            string[] split = line.Split(' ');
            string springs = split[0];
            List<int> numbers = split[1].Split(',').Select(int.Parse).ToList();
            rows.Add(new Row(springs, numbers));
        }
    }
}
