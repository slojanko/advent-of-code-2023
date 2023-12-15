internal class Day15 : Solver
{
    public class Box
    {
        public List<(string, int)> lenses;

        public Box()
        {
            lenses = new List<(string, int)>();
        }

        public void Remove(string label)
        {
            for (int i = 0; i < lenses.Count; i++)
            {
                if (lenses[i].Item1 == label)
                {
                    lenses.RemoveAt(i);
                    break;
                }
            }
        }

        public void Replace(string label, int focal)
        {
            for (int i = 0; i < lenses.Count; i++)
            {
                if (lenses[i].Item1 == label)
                {
                    lenses[i] = (label, focal);
                    return;
                }
            }

            lenses.Add((label, focal));
        }
    }

    public override void SolveEasy()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day15.txt");
        long result = 0;

        List<string> steps = new List<string>();

        ReadInput(input, steps);

        foreach(var step in steps)
        {
            result += GetHash(step, 0);
        }

        PrintEasy(result);
    }

    public override void SolveHard()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day15.txt");
        long result = 0;

        List<string> steps = new List<string>();
        Box[] boxes = new Box[256];
        for(int i = 0; i < boxes.Length; i++)
        {
            boxes[i] = new Box();
        }

        ReadInput(input, steps);

        foreach (var step in steps)
        {
            if (step.Contains('-'))
            {
                var boxIndex = GetHash(step.Substring(0, step.Length - 1), 0);

                //Console.WriteLine($"Hash of {step.Substring(0, step.Length - 1)} is {boxIndex}");

                boxes[boxIndex].Remove(step.Substring(0, step.Length - 1));
            }

            if (step.Contains('='))
            {
                var boxIndex = GetHash(step.Substring(0, step.Length - 2), 0);

                //Console.WriteLine($"Hash of {step.Substring(0, step.Length - 2)} is {boxIndex}");

                int focal = step[step.Length - 1] - '0';
                boxes[boxIndex].Replace(step.Substring(0, step.Length - 2), focal);
            }

            //Console.WriteLine($"After {step}");

            //for (int i = 0; i < boxes.Length; i++)
            //{
            //    if (boxes[i].lenses.Count > 0)
            //        Console.WriteLine($"Box {i}: {string.Join(" | ", boxes[i].lenses)}");
            //}

            //Console.WriteLine();
        }

        for(int i = 0; i < boxes.Length; i++)
        {
            for(int j = 0; j < boxes[i].lenses.Count; j++)
            {
                //Console.WriteLine($"{i + 1} {j + 1} {boxes[i].lenses[j].Item2} {(i + 1) * (j + 1) * boxes[i].lenses[j].Item2}");
                result += (i + 1) * (j + 1) * boxes[i].lenses[j].Item2;
            }
        }

        PrintHard(result);
    }

    public long GetHash(string str, long current)
    {
        long result = current;

        foreach(var ch in str)
        {
            result += (long)ch;
            result *= 17;
            result = result % 256;
        }

        return result;
    }

    public void ReadInput(string[] input, List<string> steps)
    {
        steps.AddRange(input[0].Split(','));
    }
}
