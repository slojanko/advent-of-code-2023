using System.Security.Cryptography.X509Certificates;
using System.Transactions;

internal class Day11 : Solver
{
    public class Galaxy
    {
        public int x, y;

        public Galaxy(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public override void SolveEasy()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day11.txt");
        long result = 0;

        List<Galaxy> galaxies = new List<Galaxy>();
        List<int> empty_rows = new List<int>();
        List<int> empty_columns = new List<int>();

        ReadInput(input, galaxies, empty_rows, empty_columns);

        for(int i = 0; i < galaxies.Count; i++)
        {
            for(int j = i + 1; j < galaxies.Count; j++)
            {
                int startx = Math.Min(galaxies[i].x, galaxies[j].x);
                int endx = Math.Max(galaxies[i].x, galaxies[j].x);

                int starty = Math.Min(galaxies[i].y, galaxies[j].y);
                int endy = Math.Max(galaxies[i].y, galaxies[j].y);

                for (int x = startx + 1; x <= endx; x++)
                {
                    result += empty_columns.Contains(x) ? 2 : 1;
                }

                for (int y = starty + 1; y <= endy; y++)
                {
                    result += empty_rows.Contains(y) ? 2 : 1;
                }
            }
        }

        PrintEasy(result);
    }

    public override void SolveHard()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day11.txt");
        long result = 0;

        List<Galaxy> galaxies = new List<Galaxy>();
        List<int> empty_rows = new List<int>();
        List<int> empty_columns = new List<int>();

        ReadInput(input, galaxies, empty_rows, empty_columns);

        for (int i = 0; i < galaxies.Count; i++)
        {
            for (int j = i + 1; j < galaxies.Count; j++)
            {
                int startx = Math.Min(galaxies[i].x, galaxies[j].x);
                int endx = Math.Max(galaxies[i].x, galaxies[j].x);

                int starty = Math.Min(galaxies[i].y, galaxies[j].y);
                int endy = Math.Max(galaxies[i].y, galaxies[j].y);

                for (int x = startx + 1; x <= endx; x++)
                {
                    result += empty_columns.Contains(x) ? 1000000 : 1;
                }

                for (int y = starty + 1; y <= endy; y++)
                {
                    result += empty_rows.Contains(y) ? 1000000 : 1;
                }
            }
        }

        PrintHard(result);
    }

    public void ReadInput(string[] input, List<Galaxy> galaxies, List<int> empty_rows, List<int> empty_colums)
    {
        for(int y = 0; y < input.Length; y++)
        {
            bool empty = true;
            for(int x = 0; x < input[y].Length; x++)
            {
                if (input[y][x] != '.')
                {
                    galaxies.Add(new Galaxy(x, y));
                    empty = false;
                } 
            }

            if (empty)
            {
                empty_rows.Add(y);
            }

            empty = true;
            for (int x = 0; x < input[y].Length; x++)
            {
                if (input[x][y] != '.')
                {
                    empty = false;
                }
            }

            if (empty)
            {
                empty_colums.Add(y);
            }
        }
    }
}
