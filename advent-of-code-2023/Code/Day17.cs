using System.Linq;
using static Day16;

internal class Day17 : Solver
{
    public enum Direction
    {
        None = 0,
        Up = 1,
        Left = 2,
        Down = 3,
        Right = 4
    }

    public class BlockVisit
    {
        public Block block;
        public Direction direction;
        public int steps_since_turn;
        public int total_steps;

        public BlockVisit(Block block, Direction direction, int steps_since_turn, int total_steps)
        {
            this.block = block;
            this.direction = direction;
            this.steps_since_turn = steps_since_turn;
            this.total_steps = total_steps;
        }
    }

    public class Block
    {
        public int x;
        public int y;
        public int cost;
        public List<int> finished; // Encodes steps_since_turn and direction

        public Block(int x, int y, int cost)
        {
            this.x = x;
            this.y = y;
            this.cost = cost;
            this.finished = new List<int>();
        }
    }

    public override void SolveEasy()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day17.txt");
        long result = 0;

        Block[,] grid = null;
        ReadInput(input, ref grid);

        List<BlockVisit> queue = new List<BlockVisit>();
        queue.Add(new BlockVisit(grid[0, 0], Direction.Right, 0, 0));
        queue.Add(new BlockVisit(grid[0, 0], Direction.Down, 0, 0));

        while (true)
        {
            BlockVisit visit = queue[0];

            if (visit.block.x == grid.GetLength(1) - 1 && visit.block.y == grid.GetLength(0) - 1)
            {
                break;
            }

            queue.RemoveAt(0);
            visit.block.finished.Add((int)visit.direction + 10 * visit.steps_since_turn);

            // Move in all directions, except back
            if (visit.direction != Direction.Down)
            {
                TryMoveEasy(grid, queue, visit, Direction.Up);
            }
            if (visit.direction != Direction.Right)
            {
                TryMoveEasy(grid, queue, visit, Direction.Left);
            }
            if (visit.direction != Direction.Up)
            {
                TryMoveEasy(grid, queue, visit, Direction.Down);
            }
            if (visit.direction != Direction.Left)
            {
                TryMoveEasy(grid, queue, visit, Direction.Right);
            }
        }

        result = queue[0].total_steps;

        PrintEasy(result);
    }

    public override void SolveHard()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day17.txt");
        long result = 0;

        Block[,] grid = null;
        ReadInput(input, ref grid);

        List<BlockVisit> queue = new List<BlockVisit>();
        queue.Add(new BlockVisit(grid[0, 0], Direction.Right, 0, 0));
        queue.Add(new BlockVisit(grid[0, 0], Direction.Down, 0, 0));

        while (true)
        {
            BlockVisit visit = queue[0];

            if (visit.block.x == grid.GetLength(1) - 1 && visit.block.y == grid.GetLength(0) - 1)
            {
                break;
            }

            queue.RemoveAt(0);
            visit.block.finished.Add((int)visit.direction + 10 * visit.steps_since_turn);

            // Move in all directions, except back
            if (visit.direction != Direction.Down)
            {
                TryMoveHard(grid, queue, visit, Direction.Up);
            }
            if (visit.direction != Direction.Right)
            {
                TryMoveHard(grid, queue, visit, Direction.Left);
            }
            if (visit.direction != Direction.Up)
            {
                TryMoveHard(grid, queue, visit, Direction.Down);
            }
            if (visit.direction != Direction.Left)
            {
                TryMoveHard(grid, queue, visit, Direction.Right);
            }
        }

        result = queue[0].total_steps;

        PrintHard(result);
    }

    public void TryMoveEasy(Block[,] grid, List<BlockVisit> queue, BlockVisit source, Direction direction)
    {
        int new_steps_since_turn = (direction == source.direction) ? (source.steps_since_turn + 1) : 1;
        if (new_steps_since_turn == 4)
        {
            return;
        }

        int new_x = source.block.x + (direction == Direction.Right ? 1 : (direction == Direction.Left ? -1 : 0));
        int new_y = source.block.y + (direction == Direction.Down ? 1 : (direction == Direction.Up ? -1 : 0));
        if (new_x < 0 || new_x >= grid.GetLength(1) || new_y < 0 || new_y >= grid.GetLength(0))
        {
            return;
        }

        Block new_block = grid[new_y, new_x];
        if (new_block.finished.Contains((int)direction + 10 * new_steps_since_turn))
        {
            return;
        }

        int new_total_steps = source.total_steps + new_block.cost;

        for (int i = 0; i < queue.Count; i++)
        {
            if (new_block == queue[i].block && queue[i].direction == direction && queue[i].steps_since_turn == new_steps_since_turn)
            {
                return;
            }
        }

        AddSorted(queue, new BlockVisit(new_block, direction, new_steps_since_turn, new_total_steps));
    }

    public void TryMoveHard(Block[,] grid, List<BlockVisit> queue, BlockVisit source, Direction direction)
    {
        int new_steps_since_turn = (direction == source.direction) ? (source.steps_since_turn + 1) : 1;
        if ((direction != source.direction && source.steps_since_turn < 4) || (direction == source.direction && source.steps_since_turn >= 10))
        {
            return;
        }

        int new_x = source.block.x + (direction == Direction.Right ? 1 : (direction == Direction.Left ? -1 : 0));
        int new_y = source.block.y + (direction == Direction.Down ? 1 : (direction == Direction.Up ? -1 : 0));
        if (new_x < 0 || new_x >= grid.GetLength(1) || new_y < 0 || new_y >= grid.GetLength(0))
        {
            return;
        }

        Block new_block = grid[new_y, new_x];
        if (new_block.finished.Contains((int)direction + 10 * new_steps_since_turn))
        {
            return;
        }

        int new_total_steps = source.total_steps + new_block.cost;

        for (int i = 0; i < queue.Count; i++)
        {
            if (new_block == queue[i].block && queue[i].direction == direction && queue[i].steps_since_turn == new_steps_since_turn)
            {
                return;
            }
        }

        AddSorted(queue, new BlockVisit(new_block, direction, new_steps_since_turn, new_total_steps));
    }

    public void ReAddSorted(List<BlockVisit> queue, int index)
    {
        BlockVisit visit = queue[index];
        queue.RemoveAt(index);
        AddSorted(queue, visit);
    }

    public void AddSorted(List<BlockVisit> queue, BlockVisit visit)
    {
        for (int i = 0; i < queue.Count; i++)
        {
            if (visit.total_steps < queue[i].total_steps)
            {
                queue.Insert(i, visit);
                return;
            }
        }

        queue.Add(visit);
    }

    public void ReadInput(string[] input, ref Block[,] grid)
    {
        grid = new Block[input.Length, input[0].Length];

        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[0].Length; x++)
            {
                grid[y, x] = new Block(x, y, input[y][x] - '0');
            }
        }
    }

    public void PrintGrid(Block[,] grid)
    {
        for (int y = 0; y < grid.GetLength(0); y++)
        {
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                Console.Write(grid[y, x].cost);
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }
}
