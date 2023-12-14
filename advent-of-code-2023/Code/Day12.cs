internal class Day12 : Solver
{
    public class Row
    {
        public Int128 damagedSprings;
        public Int128 allSprings;

        public int springsCount;
        public List<int> numbers;
        public long tries;

        public Dictionary<(int, int), long> cache;

        public Row(string springs, List<int> numbers)
        {
            this.numbers = numbers;
            this.springsCount = springs.Length;
            this.cache = new Dictionary<(int, int), long>();
            
            for (int i = 0; i < springs.Length; i++)
            {
                if (springs[i] == '#')
                {
                    damagedSprings |= (Int128)1 << i;
                    allSprings |= (Int128)1 << i;
                } else if (springs[i] == '?')
                {
                    allSprings |= (Int128)1 << i;
                }
            }
        }

        public void AddCache((int, int) key, long value)
        {
            cache.Add(key, value);
        }
    }

    public override void SolveEasy()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day12.txt");
        long result = 0;

        List<Row> rows = new List<Row>();

        ReadInput(input, rows, false);

        foreach (var row in rows)
        {
            long temp = SolveRecursive(row, 0, 0, 0);

            result += temp;

            row.cache.Clear();
        }

        PrintEasy(result);
    }

    public override void SolveHard()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day12.txt");
        long result = 0;

        List<Row> rows = new List<Row>();

        ReadInput(input, rows, true);

        foreach (var row in rows)
        {
            long temp = SolveRecursive(row, 0, 0, 0);

            result += temp;

            row.cache.Clear();
        }

        PrintHard(result);
    }

    public long SolveRecursive(Row row, int startSpring, int startNum, Int128 current)
    {
        if (row.cache.TryGetValue((startSpring, startNum), out long v))
        {
            return v;
        }

        // Check if everything up to now was correct 
        Int128 valid_damagedSpring = row.damagedSprings & (((Int128)1 << startSpring) - 1);
        if ((((valid_damagedSpring ^ current) & valid_damagedSpring) > 0) || ((row.allSprings & current) != current))
        {
            return 0;
        }

        // If we ran out of numbers, we will always return
        if (startNum == row.numbers.Count)
        {
            if (((row.damagedSprings & current) == row.damagedSprings) && ((row.allSprings & current) == current))
            {
                return 1;
            }

            return 0;
        }

        if (startSpring >= row.springsCount)
        {
            return 0;
        }

        long result = 0;

        result += SolveRecursive(row, startSpring + 1, startNum, current);

        Int128 alternative = current | ((((Int128)1 << row.numbers[startNum]) - 1) << startSpring);

        result += SolveRecursive(row, startSpring + row.numbers[startNum] + 1, startNum + 1, alternative);

        row.AddCache((startSpring, startNum), result);
        return result;
    }

    public void ReadInput(string[] input, List<Row> rows, bool is_hard)
    {
        foreach (var line in input)
        {
            string[] split = line.Split(' ');
            string springs = split[0];
            List<int> numbers = split[1].Split(',').Select(int.Parse).ToList();

            if (is_hard) {
                string orig_springs = springs;
                List<int> orig_numbers = new List<int>(numbers);

                for(int i = 0; i < 4; i++)
                {
                    numbers.AddRange(orig_numbers);
                    springs = springs + (i < 4 ? "?" : "") + orig_springs;
                }
            }

            rows.Add(new Row(springs, numbers));
        }
    }
}
