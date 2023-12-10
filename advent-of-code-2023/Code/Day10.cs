using System.Transactions;

internal class Day10 : Solver
{
    public enum Direction : int
    {
        Invalid,
        Right,
        Up,
        Left,
        Down
    }

    public class Pipe
    {
        public char angle;
        public (Direction, Direction) forwards, backwards;
        public bool in_loop;

        public Pipe(char angle)
        {
            this.angle = angle;
            CalculateConnections();
        }

        public void CalculateConnections()
        {
            switch (angle)
            {
                case '|':
                    forwards = (Direction.Up, Direction.Up);
                    backwards = (Direction.Down, Direction.Down);
                    break;
                case '-':
                    forwards = (Direction.Right, Direction.Right);
                    backwards = (Direction.Left, Direction.Left);
                    break;
                case 'L':
                    forwards = (Direction.Down, Direction.Right);
                    backwards = (Direction.Left, Direction.Up);
                    break;
                case 'J':
                    forwards = (Direction.Down, Direction.Left);
                    backwards = (Direction.Right, Direction.Up);
                    break;
                case '7':
                    forwards = (Direction.Right, Direction.Down);
                    backwards = (Direction.Up, Direction.Left);
                    break;
                case 'F':
                    forwards = (Direction.Left, Direction.Down);
                    backwards = (Direction.Up, Direction.Right);
                    break;
            }
        }

        public Direction Travel(Direction source)
        {
            if (forwards.Item1 == source)
            {
                return forwards.Item2;
            }

            if (backwards.Item1 == source)
            {
                return backwards.Item2;
            }

            return Direction.Invalid;
        }
    }

    public override void SolveEasy()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day10.txt");
        long result = 0;
        Pipe[,] grid = null;
        (int, int) start = (0, 0);

        ReadInput(input, ref grid, ref start);

        (int, int) current = start;
        Direction direction = grid[current.Item1, current.Item2].forwards.Item1;

        do
        {
            Direction dir = grid[current.Item1, current.Item2].Travel(direction);
            var diff = DirectionToMove(dir);
            direction = dir;

            current.Item1 += diff.Item1;
            current.Item2 += diff.Item2;

            result++;
        } while (current != start);

        result = result / 2;

        PrintEasy(result);
    }

    public override void SolveHard()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day10.txt");
        long result = 0;
        Pipe[,] grid = null;
        (int, int) start = (0, 0);

        ReadInput(input, ref grid, ref start);

        for(int x = 0; x < grid.GetLength(0); x++)
        {
            for(int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y].in_loop)
                {
                    continue;
                }

                int crossings = 0;
                char enter_angle = ' ';

                for(int i = x + 1; i < grid.GetLength(0); i++)
                {
                    // Check only pipes in main loop
                    if (!grid[i, y].in_loop)
                    {
                        continue;
                    }

                    // Count number of valid intersections, if odd then we're inside
                    if (grid[i, y].angle == '-')
                    {
                        continue;
                    } else if (grid[i, y].angle == '|')
                    {
                        crossings++;
                    } else if (enter_angle == ' ')
                    {
                        if (grid[i, y].angle == 'L')
                        {
                            enter_angle = 'L';
                        } else if (grid[i, y].angle == 'F')
                        {
                            enter_angle = 'F';
                        }
                    } else
                    {
                        if (enter_angle == 'L' && grid[i, y].angle == '7')
                        {
                            crossings++;
                        }
                        else if (enter_angle == 'F' && grid[i, y].angle == 'J')
                        {
                            crossings++;
                        }

                        if (grid[i, y].angle == 'J' || grid[i, y].angle == '7')
                        {
                            enter_angle = ' ';
                        }
                    }
                }

                if (crossings % 2 == 1)
                {
                    Console.WriteLine($"{x} {y} crosses {crossings}");
                    result++;
                }
            }
        }

        PrintHard(result);
    }

    public void ReadInput(string[] input, ref Pipe[,] grid, ref (int, int) start)
    {
        grid = new Pipe[input[0].Length, input.Length];
        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[y].Length; x++)
            {
                grid[x, y] = new Pipe(input[y][x]);

                if (input[y][x] == 'S')
                {
                    start = (x, y);
                }
            }
        }

        grid[start.Item1, start.Item2] = GetMissingPipe(grid, start);

        // Mark all pipes in loop 
        (int, int) current = start;
        Direction direction = grid[start.Item1, start.Item2].forwards.Item1;
        do
        {
            grid[current.Item1, current.Item2].in_loop = true;
            Direction dir = grid[current.Item1, current.Item2].Travel(direction);
            var diff = DirectionToMove(dir);
            direction = dir;

            current.Item1 += diff.Item1;
            current.Item2 += diff.Item2;
        } while (current != start);
    }

    public (int, int) DirectionToMove(Direction direction)
    {
        switch (direction)
        {
            case Direction.Right: return (1, 0);
            case Direction.Up: return (0, -1);
            case Direction.Left: return (-1, 0);
            case Direction.Down: return (0, 1);
        }

        return (0, 0);
    }

    public Direction Flip(Direction direction)
    {
        switch (direction)
        {
            case Direction.Right: return Direction.Left;
            case Direction.Up: return Direction.Down;
            case Direction.Left: return Direction.Right;
            case Direction.Down: return Direction.Up;
        }

        return Direction.Invalid;
    }

    // Doesnt work if S is on edge or corner, but not important for inputs
    public Pipe GetMissingPipe(Pipe[,] grid, (int, int) pos)
    {
        string pipes = "|-FJL7";

        Pipe left = grid[pos.Item1 - 1, pos.Item2];
        Pipe right = grid[pos.Item1 + 1, pos.Item2];
        Pipe up = grid[pos.Item1, pos.Item2 - 1];
        Pipe down = grid[pos.Item1, pos.Item2 + 1];

        (Pipe, Direction)[] local_pipes = { (left, Direction.Left), (right, Direction.Right), (up, Direction.Up), (down, Direction.Down) };

        foreach (var angle in pipes)
        {
            Pipe temp = new Pipe(angle);
            int valid_counts = 0;

            foreach (var local_pipe in local_pipes)
            { 
                //Console.WriteLine($"{temp.first.Item2} {local_pipe.Item2}");
                if (temp.forwards.Item2 == local_pipe.Item2 && local_pipe.Item1.Travel(local_pipe.Item2) != Direction.Invalid)
                {
                    valid_counts++;
                }
                
                if (temp.backwards.Item2 == local_pipe.Item2 && local_pipe.Item1.Travel(local_pipe.Item2) != Direction.Invalid)
                {
                    valid_counts++;
                }
            }

            //Console.WriteLine(valid_counts);
            if (valid_counts == 2)
            {
                return temp;
            }
        }

        return null;
    }
}
