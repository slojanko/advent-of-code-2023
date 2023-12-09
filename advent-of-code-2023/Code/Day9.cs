internal class Day9 : Solver
{
    public override void SolveEasy()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day9.txt");
        long result = 0;
        List<List<long>> numbers = new List<List<long>>();

        ReadInput(input, numbers);

        foreach(var line in numbers)
        {
            //Console.WriteLine(GetNext(line));
            result += GetNext(line);
        }

        PrintEasy(result);
    }

    public override void SolveHard()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day9.txt");
        long result = 0;
        List<List<long>> numbers = new List<List<long>>();

        ReadInput(input, numbers);

        foreach (var line in numbers)
        {
            //Console.WriteLine(GetNext(line));
            result += GetPrevious(line);
        }

        PrintHard(result);
    }

    public long GetNext(List<long> numbers)
    {
        if (numbers.All(x => x == 0))
        {
            return 0;
        }

        var diff = numbers.Skip(1).Select((x, i) => { 
            return x - numbers[i]; 
        }).ToList();

        return numbers[^1] + GetNext(diff);
    }

    public long GetPrevious(List<long> numbers)
    {
        if (numbers.All(x => x == 0))
        {
            return 0;
        }

        var diff = numbers.Skip(1).Select((x, i) => {
            return x - numbers[i];
        }).ToList();

        return numbers[0] - GetPrevious(diff);
    }

    public void ReadInput(string[] input, List<List<long>> numbers)
    {
        foreach(var line in input)
        {
            numbers.Add(line.Split(' ').Select(long.Parse).ToList());
        }
    }
}
