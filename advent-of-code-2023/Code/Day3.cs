using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Day3 : Solver
{
    public class Gear
    {
        public int x, y, count, value;
    }

    public override void SolveEasy()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day3.txt");
        int result = 0;

        for(int i = 0; i < input.Length; i++)
        {
            string line = input[i];

            int value = 0;
            int width = 0;
            bool is_valid = false;

            for (int j = 0; j < line.Length; j++)
            {
                if (char.IsDigit(line[j]))
                {
                    value = value * 10 + line[j] - '0';
                    width++;
                    is_valid = true;
                } else if (is_valid)
                {
                    if (IsSymbolAroundArea(input, j - width, i, width))
                    {
                        result += value;
                    }

                    value = 0;
                    width = 0;
                    is_valid = false;
                }
            }

            if (is_valid && IsSymbolAroundArea(input, line.Length - width, i, width))
            {
                result += value;
            }
        }

        PrintEasy(result);
    }

    public override void SolveHard()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day3.txt");
        List<Gear> gears = new List<Gear>();
        int result = 0;

        for (int i = 0; i < input.Length; i++)
        {
            string line = input[i];

            int value = 0;
            int width = 0;
            bool is_valid = false;

            for (int j = 0; j < line.Length; j++)
            {
                if (char.IsDigit(line[j]))
                {
                    value = value * 10 + line[j] - '0';
                    width++;
                    is_valid = true;
                }
                else if (is_valid)
                {
                    TryContributeToGear(input, gears, j - width, i, width, value);

                    value = 0;
                    width = 0;
                    is_valid = false;
                }
            }

            if (is_valid)
            {
                TryContributeToGear(input, gears, line.Length - width, i, width, value);
            }
        }

        foreach(var gear in gears)
        {
            if (gear.count == 2)
            {
                result += gear.value;
            }
        }

        PrintHard(result);
    }

    public bool IsSymbolAroundArea(string[] input, int x, int y, int width)
    {
        for(int i = Math.Max(0, y - 1); i <= Math.Min(input.Length - 1, y + 1); i++)
        {
            for (int j = Math.Max(0, x - 1); j <= Math.Min(input[0].Length - 1, x + width); j++)
            {
                if (!char.IsDigit(input[i][j]) && input[i][j] != '.')
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void TryContributeToGear(string[] input, List<Gear> gears, int x, int y, int width, int value)
    {
        for (int i = Math.Max(0, y - 1); i <= Math.Min(input.Length - 1, y + 1); i++)
        {
            for (int j = Math.Max(0, x - 1); j <= Math.Min(input[0].Length - 1, x + width); j++)
            {
                if (input[i][j] == '*')
                {
                    int existing = gears.FindIndex(gear => gear.x == j && gear.y == i);
                    if (existing < 0) {
                        Gear gear = new Gear();
                        gear.x = j;
                        gear.y = i;

                        gears.Add(gear);
                        existing = gears.Count - 1;
                    }

                    gears[existing].count++;
                    if (gears[existing].count == 1)
                    {
                        gears[existing].value = value;
                    } else
                    {
                        gears[existing].value *= value;
                    }

                    break;
                }
            }
        }
    }
}