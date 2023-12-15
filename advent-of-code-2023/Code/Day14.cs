internal class Day14 : Solver
{

    public override void SolveEasy()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day14.txt");
        long result = 0;

        List<List<char>> grid = new List<List<char>>();

        ReadInput(input, grid);

        for(int y = 0; y < grid.Count; y++)
        {
            for(int x = 0; x < grid[0].Count; x++)
            {
                if (grid[y][x] != 'O')
                {
                    continue;
                }

                int curr_y = y;
                while(true)
                {
                    if (curr_y == 0 || grid[curr_y - 1][x] != '.')
                    {
                        result += grid.Count - curr_y;
                        break;
                    } else
                    {
                        grid[curr_y][x] = '.';
                        grid[curr_y - 1][x] = 'O';
                        curr_y--;
                    }
                }
            }
        }

        PrintEasy(result);
    }

    public override void SolveHard()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day14.txt");
        long result = 0;

        PrintHard(result);
    }

    public void ReadInput(string[] input, List<List<char>> grid)
    {
        foreach(var line in input)
        {
            grid.Add(line.ToList());
        }
    }
}
