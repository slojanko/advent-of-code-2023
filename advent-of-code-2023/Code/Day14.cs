internal class Day14 : Solver
{
    public override void SolveEasy()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day14.txt");
        long result = 0;

        List<List<char>> grid = new List<List<char>>();

        ReadInput(input, grid);

        Roll(grid);
        result = GetLoad(grid);

        PrintEasy(result);
    }

    public override void SolveHard()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day14.txt");
        long result = 0;

        List<List<char>> grid = new List<List<char>>();

        ReadInput(input, grid);

        // Total cycles | load
        Dictionary<string, (long, long)> cache = new Dictionary<string, (long, long)>();

        long total_cycles = 0;
        long load = 0;
        string key;
        (long, long) value;

        while (true)
        {
            for(int i = 0; i < 4; i++) {
                Roll(grid);
                Rotate(ref grid);
            }
            load = GetLoad(grid);
            total_cycles++;

            key = string.Concat(grid.Select(v => string.Concat(v)));
            if (cache.TryGetValue(key, out value))
            {
                break;
            }

            cache.Add(key, (total_cycles, load));
        }

        long cycle_loop = total_cycles - value.Item1;
        long fast_cycles = (1000000000 - total_cycles) / cycle_loop;
        total_cycles += fast_cycles * cycle_loop;

        while(total_cycles != 1000000000)
        {
            for(int i = 0; i < 4; i++) { 
                Roll(grid);
                Rotate(ref grid);
            }
            load = GetLoad(grid);
            total_cycles++;
        }

        result = load;

        PrintHard(result);
    }

    public void Roll(List<List<char>> grid)
    {
        for (int y = 0; y < grid.Count; y++)
        {
            for (int x = 0; x < grid[0].Count; x++)
            {
                if (grid[y][x] != 'O')
                {
                    continue;
                }

                int curr_y = y;
                while (curr_y != 0 && grid[curr_y - 1][x] == '.')
                {
                    grid[curr_y][x] = '.';
                    grid[curr_y - 1][x] = 'O';
                    curr_y--;
                }
            }
        }
    }

    public long GetLoad(List<List<char>> grid)
    {
        long result = 0;

        for (int y = 0; y < grid.Count; y++)
        {
            for (int x = 0; x < grid[0].Count; x++)
            {
                if (grid[y][x] != 'O')
                {
                    continue;
                }

                result += grid.Count - y;
            }
        }

        return result;
    }

    public void Rotate(ref List<List<char>> grid)
    {
        List<List<char>> new_grid = new List<List<char>>();

        for(int x = 0; x < grid[0].Count; x++)
        {
            new_grid.Add(grid.Select(l => l[x]).Reverse().ToList());
        }

        grid = new_grid;
    }

    public void PrintGrid(List<List<char>> grid)
    {
        for(int y = 0; y < grid.Count; y++)
        {
            Console.WriteLine(string.Concat(grid[y]));
        }

        Console.WriteLine("");
    }

    public void ReadInput(string[] input, List<List<char>> grid)
    {
        foreach(var line in input)
        {
            grid.Add(line.ToList());
        }
    }
}
