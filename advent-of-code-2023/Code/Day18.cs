// https://en.wikipedia.org/wiki/Shoelace_formula#Trapezoid_formula_2
internal class Day18 : Solver
{
    public enum Direction
    {
        Up = 1,
        Left = 2,
        Down = 3,
        Right = 4
    }

    public class Point
    {
        public long x;
        public long y;

        public Point(long x, long y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public override void SolveEasy()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day18.txt");
        long result = 0;

        List<Point> plan = new List<Point>();

        ReadInputEasy(input, ref plan);

        result = GetArea(plan);

        PrintEasy(result);
    }

    public override void SolveHard()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day18.txt");
        long result = 0;

        List<Point> plan = new List<Point>();

        ReadInputHard(input, ref plan);

        result = GetArea(plan);

        PrintHard(result);
    }

    // Nice when the solution is some random algorithm -.-
    // https://en.wikipedia.org/wiki/Pick%27s_theorem
    // https://en.wikipedia.org/wiki/Shoelace_formula
    public long GetArea(List<Point> plan)
    {
        long perimeter = 0;
        long area = 0;

        for(int i = 0; i < plan.Count; i++)
        {
            int ni = (i + 1) % plan.Count;
            perimeter += Math.Abs(plan[i].x - plan[ni].x + plan[i].y - plan[ni].y);
            area += (plan[i].y + plan[ni].y) * (plan[ni].x - plan[i].x);
        }

        area = Math.Abs(area) / 2;
        long inside = area - perimeter / 2 + 1;

        return perimeter + inside;
    }

    public void AddInstruction(Direction direction, int distance, ref long x, ref long y)
    {
        switch(direction)
        {
            case Direction.Up: y += distance; break;
            case Direction.Left: x -= distance; break;
            case Direction.Down: y -= distance; break;
            case Direction.Right: x += distance; break;
        }
    }
    
    public Direction CharToDirection(char ch)
    {
        switch(ch)
        {
            case 'U': return Direction.Up;
            case 'L': return Direction.Left;
            case 'D': return Direction.Down;
            case 'R': return Direction.Right;
        }

        return Direction.Up;
    }

    public Direction IntToDirection(int v)
    {
        switch (v)
        {
            case 0: return Direction.Right;
            case 1: return Direction.Down;
            case 2: return Direction.Left;
            case 3: return Direction.Up;
        }

        return Direction.Up;
    }

    public void ReadInputEasy(string[] input, ref List<Point> plan)
    {
        long x = 0;
        long y = 0;

        plan.Add(new Point(0, 0));
        foreach (var line in input)
        {
            string[] split = line.Split(' ');
            AddInstruction(CharToDirection(split[0][0]), int.Parse(split[1]), ref x, ref y);
            plan.Add(new Point(x, y));
        }
    }

    public void ReadInputHard(string[] input, ref List<Point> plan)
    {
        long x = 0;
        long y = 0;

        plan.Add(new Point(0, 0));
        foreach (var line in input)
        {
            string[] split = line.Split(' ');
            // Console.WriteLine($"{IntToDirection(split[2][7] - '0')} {int.Parse(split[2].Substring(2, 5), System.Globalization.NumberStyles.HexNumber)}");
            AddInstruction(IntToDirection(split[2][7] - '0'), int.Parse(split[2].Substring(2, 5), System.Globalization.NumberStyles.HexNumber), ref x, ref y);
            plan.Add(new Point(x, y));
        }
    }
}
