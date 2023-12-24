internal class Day16 : Solver
{
    public enum Direction
    {
        None = 0,
        Up = 1,
        Left = 2,
        Down = 4,
        Right = 8
    }

    public class Cell
    {
        public char type;
        public Direction existing;

        public Cell(char type)
        {
            this.type = type;
            existing = Direction.None;
        }

        public (Direction, Direction) Process(Direction direction)
        {
            switch(type)
            {
                case '.':
                    return (direction, Direction.None);
                case '|':
                    return (direction == Direction.Right || direction == Direction.Left) ? (Direction.Up, Direction.Down) : (direction, Direction.None);
                case '-':
                    return (direction == Direction.Up || direction == Direction.Down) ? (Direction.Left, Direction.Right) : (direction, Direction.None);
                case '/':
                    {
                        switch(direction)
                        {
                            case Direction.Up: return (Direction.Right, Direction.None);
                            case Direction.Left: return (Direction.Down, Direction.None);
                            case Direction.Down: return (Direction.Left, Direction.None);
                            case Direction.Right: return (Direction.Up, Direction.None);
                        }
                        break;
                    }
                case '\\':
                    {
                        switch (direction)
                        {
                            case Direction.Up: return (Direction.Left, Direction.None);
                            case Direction.Left: return (Direction.Up, Direction.None);
                            case Direction.Down: return (Direction.Right, Direction.None);
                            case Direction.Right: return (Direction.Down, Direction.None);
                        }
                        break;
                    }
            }

            return (Direction.None, Direction.None);
        }
    }

    public override void SolveEasy()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day16.txt");
        long result = 0;

        Cell[,] cells = null;
        ReadInput(input, ref cells);

        Queue<(int, int, Direction)> queue = new Queue<(int, int, Direction)>();
        queue.Enqueue((0, 0, Direction.Right));

        while(queue.Count > 0)
        {
            (int, int, Direction) beam = queue.Dequeue();
            ProcessBeam(cells, beam, queue);
        }

        result = CountEnergizedCells(cells);

        PrintEasy(result);
    }

    public override void SolveHard()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day16.txt");
        long result = 0;

        Cell[,] cells = null;
        ReadInput(input, ref cells);

        Queue<(int, int, Direction)> queue = new Queue<(int, int, Direction)>();

        for(int y = 0; y < cells.GetLength(0); y++)
        {
            // From Left
            queue.Enqueue((y, 0, Direction.Right));
            while (queue.Count > 0)
            {
                (int, int, Direction) beam = queue.Dequeue();
                ProcessBeam(cells, beam, queue);
            }
            result = Math.Max(result, CountEnergizedCells(cells));
            ClearEnergizedCells(cells);

            // From Right
            queue.Enqueue((y, cells.GetLength(1) - 1, Direction.Left));
            while (queue.Count > 0)
            {
                (int, int, Direction) beam = queue.Dequeue();
                ProcessBeam(cells, beam, queue);
            }
            result = Math.Max(result, CountEnergizedCells(cells));
            ClearEnergizedCells(cells);
        }

        for (int x = 0; x < cells.GetLength(1); x++)
        {
            // From Up
            queue.Enqueue((0, x, Direction.Down));
            while (queue.Count > 0)
            {
                (int, int, Direction) beam = queue.Dequeue();
                ProcessBeam(cells, beam, queue);
            }
            result = Math.Max(result, CountEnergizedCells(cells));
            ClearEnergizedCells(cells);

            // From Down
            queue.Enqueue((cells.GetLength(0) - 1, x, Direction.Up));
            while (queue.Count > 0)
            {
                (int, int, Direction) beam = queue.Dequeue();
                ProcessBeam(cells, beam, queue);
            }
            result = Math.Max(result, CountEnergizedCells(cells));
            ClearEnergizedCells(cells);
        }

        PrintHard(result);
    }

    public void ProcessBeam(Cell[,] cells, (int, int, Direction) beam, Queue<(int, int, Direction)> queue)
    {
        if (beam.Item1 < 0 || beam.Item1 >= cells.GetLength(0) || beam.Item2 < 0 || beam.Item2 >= cells.GetLength(1))
        {
            return;
        }

        Cell cell = cells[beam.Item1, beam.Item2];

        if (cell.existing.HasFlag(beam.Item3))
        {
            return;
        }

        (Direction, Direction) new_dir = cell.Process(beam.Item3);

        cell.existing |= beam.Item3;

        if (new_dir.Item1 != Direction.None)
        {
            (int, int) new_pos = GetNewPosition(beam.Item1, beam.Item2, new_dir.Item1);
            queue.Enqueue((new_pos.Item1, new_pos.Item2, new_dir.Item1));
        }

        if (new_dir.Item2 != Direction.None)
        {
            (int, int) new_pos = GetNewPosition(beam.Item1, beam.Item2, new_dir.Item2);
            queue.Enqueue((new_pos.Item1, new_pos.Item2, new_dir.Item2));
        }
    }

    public long CountEnergizedCells(Cell[,] cells)
    {
        long result = 0;

        for (int y = 0; y < cells.GetLength(0); y++)
        {
            for (int x = 0; x < cells.GetLength(1); x++)
            {
                if (cells[y, x].existing != Direction.None)
                {
                    result++;
                }
            }
        }

        return result;
    }

    public void ClearEnergizedCells(Cell[,] cells)
    {
        for (int y = 0; y < cells.GetLength(0); y++)
        {
            for (int x = 0; x < cells.GetLength(1); x++)
            {
                cells[y, x].existing = Direction.None;
            }
        }
    }

    public (int, int) GetNewPosition(int y, int x, Direction direction)
    {
        switch (direction)
        {
            case Direction.Up: return (y - 1, x);
            case Direction.Left: return (y, x - 1);
            case Direction.Down: return (y + 1, x);
            case Direction.Right: return (y, x + 1);
        }

        return (0, 0);
    }

    public void PrintCells(Cell[,] cells)
    {
        for (int y = 0; y < cells.GetLength(0); y++)
        {
            for (int x = 0; x < cells.GetLength(1); x++)
            {
                Console.Write(cells[y, x].type);
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    public void ReadInput(string[] input, ref Cell[,] cells)
    {
        cells = new Cell[input.Length, input[0].Length];

        for(int y = 0; y < input.Length; y++)
        {
            for(int x = 0; x < input[0].Length; x++)
            {
                cells[y, x] = new Cell(input[y][x]);
            }
        }
    }
}
