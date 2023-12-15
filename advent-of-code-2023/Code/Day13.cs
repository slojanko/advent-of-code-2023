using System.Data;
using System.Data.Common;
using static Day13;

internal class Day13 : Solver
{
    public class Grid
    {
        public List<string> rows;
        public List<string> columns;

        public List<string> reversed_rows;
        public List<string> reversed_columns;

        public int RowLength;
        public int ColumnLength;

        public int OrigAnswer;

        public Grid()
        {
            rows = new List<string>();
            columns = new List<string>();

            reversed_rows = new List<string>();
            reversed_columns = new List<string>();
        }

        public void AddRow(string str)
        {
            RowLength = str.Length;
            rows.Add(str);

            //Console.WriteLine(str);

            char[] arr = str.ToCharArray();
            Array.Reverse(arr);
            reversed_rows.Add(new string(arr));
        }

        public void AddColumn(string str)
        {
            ColumnLength = str.Length;
            columns.Add(str);

            //Console.WriteLine(str);

            char[] arr = str.ToCharArray();
            Array.Reverse(arr);
            reversed_columns.Add(new string(arr));
        }

        public void FlipValue(int x, int y)
        {
            rows[y] = FlipValue(rows[y], x);
            reversed_rows[y] = FlipValue(reversed_rows[y], RowLength - x - 1);

            columns[x] = FlipValue(columns[x], y);
            reversed_columns[x] = FlipValue(reversed_columns[x], ColumnLength - y - 1);
        }

        public string FlipValue(string str, int pos)
        {
            return str.Substring(0, pos) + (str[pos] == '#' ? '.' : '#') + str.Substring(pos + 1);
        }

        public void PrintAll()
        {
            foreach(string row in rows)
            {
                Console.WriteLine(row);
            }
            Console.WriteLine("");
            foreach (string column in columns)
            {
                Console.WriteLine(column);
            }
            Console.WriteLine("");
            foreach (string row in reversed_rows)
            {
                Console.WriteLine(row);
            }
            Console.WriteLine("");
            foreach (string column in reversed_columns)
            {
                Console.WriteLine(column);
            }
        }
    }

    public override void SolveEasy()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day13.txt");
        long result = 0;

        List<Grid> grids = new List<Grid>();

        ReadInput(input, grids);

        for(int i = 0; i < grids.Count; i++)
        {
            Grid grid = grids[i];

            result += FindMirror(grid);
        }

        PrintEasy(result);
    }

    public override void SolveHard()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day13.txt");
        long result = 0;

        List<Grid> grids = new List<Grid>();

        ReadInput(input, grids);

        for (int i = 0; i < grids.Count; i++)
        {
            Grid grid = grids[i];

            int avoid = FindMirror(grid);

            bool found = false;

            for(int x = 0; x < grid.RowLength; x++)
            {
                for (int y = 0; y < grid.ColumnLength; y++)
                {
                    grid.FlipValue(x, y);

                    int temp = FindMirror(grid, avoid);

                    grid.FlipValue(x, y);

                    if (temp > 0)
                    {
                        result += temp;
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    break;
                }
            }
        }

        PrintHard(result);
    }

    public int FindMirror(Grid grid, int avoid = -100)
    {
        for (int mirror = 0; mirror < grid.RowLength - 1; mirror++)
        {
            int temp = TestMirror(grid.rows, grid.reversed_rows, mirror);
            if (temp > 0 && temp != avoid)
            {
                return temp;
            }
        }

        for (int mirror = 0; mirror < grid.ColumnLength - 1; mirror++)
        {
            int temp = TestMirror(grid.columns, grid.reversed_columns, mirror);
            if (temp > 0 && temp != (avoid / 100))
            {
                return 100 * temp;
            }
        }

        return 0;
    }

    public int TestMirror(List<string> normal, List<string> reversed, int mirror) 
    {
        int total_length = normal[0].Length;

        int left_end = mirror;
        int right_start = mirror + 1;
        int length = Math.Min(left_end + 1, total_length - right_start);

        for (int j = 0; j < normal.Count; j++)
        {
            if (normal[j].Substring(left_end + 1 - length, length) != reversed[j].Substring(total_length - right_start - length, length))
            {
                return 0;
            }
        }

        return left_end + 1;
    }

    public void ReadInput(string[] input, List<Grid> grids)
    {
        int from = 0;
        for(int i = 0; i < input.Length; i++)
        {
            if (string.IsNullOrEmpty(input[i]) || i == input.Length - 1)
            {
                int take = i - from;

                // Take one more if we're at end, since now it's included
                if (i == input.Length - 1)
                {
                    take++;
                }

                Grid grid = new Grid();

                // Take only rows with grid
                for (int j = from; j < from + take; j++)
                {
                    grid.AddRow(input[j]);
                }

                // Take only columns from grid
                for (int j = 0; j < input[from].Length; j++)
                {
                    string column = String.Concat(input.Skip(from).Take(take).Select(x => x[j]));
                    grid.AddColumn(column);
                }

                grids.Add(grid);

                from = i + 1;
            }
        }
    }
}
