using System.Runtime.ExceptionServices;

internal class Day6 : Solver
{
    public override void SolveEasy()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day6.txt");
        long result = 1;
        List<long> times = new List<long>();
        List<long> distances = new List<long>();

        ReadInputEasy(input, times, distances);

        for(int i = 0; i < times.Count; i++)
        {
            var res = FindZeroes(times[i], distances[i]);

            res.Item1 = Math.Ceiling(res.Item1);
            res.Item2 = Math.Floor(res.Item2);

            result *= (long)(res.Item2 - res.Item1 + 1);
        }

        PrintEasy(result);
    }

    public override void SolveHard()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day6.txt");
        long result = 1;
        List<long> times = new List<long>();
        List<long> distances = new List<long>();

        ReadInputHard(input, times, distances);

        var res = FindZeroes(times[0], distances[0]);

        res.Item1 = Math.Ceiling(res.Item1);
        res.Item2 = Math.Floor(res.Item2);

        result *= (long)(res.Item2 - res.Item1 + 1);

        PrintHard(result);
    }

    public void ReadInputEasy(string[] input, List<long> times, List<long> distances)
    {
        times.AddRange(input[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse));
        distances.AddRange(input[1].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse));
    }

    public void ReadInputHard(string[] input, List<long> times, List<long> distances)
    {
        times.Add(long.Parse(input[0].Split(':')[1].Replace(" ", "")));
        distances.Add(long.Parse(input[1].Split(':')[1].Replace(" ", "")));
    }

    public (double, double) FindZeroes(long time, long distance)
    {
        double a = -1;
        double b = time;
        double c = -distance;

        double D = b * b - 4 * a * c;

        double first = (-b + Math.Sqrt(D)) / (2 * a);
        double second = (-b - Math.Sqrt(D)) / (2 * a);

        return (first, second);
    }

}