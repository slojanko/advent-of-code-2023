﻿internal class Day1 : Solver
{
    public override void SolveEasy()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day1.txt");
        int result = 0;

        foreach(string line in input)
        {
            result += FindDigitsEasy(line);
        }

        PrintEasy(result);
    }

    public override void SolveHard()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day1.txt");
        int result = 0;

        foreach (string line in input)
        {
            result += FindDigitsHard(line);
        }

        PrintHard(result);
    }

    private int FindDigitsEasy(string line)
    {
        int? value = null;
        int lastDigit = 0;

        foreach (var ch in line)
        {
            if (char.IsDigit(ch))
            {
                lastDigit = ch - '0';
                if (!value.HasValue)
                {
                    value = lastDigit;
                }
            }
        }

        value = value * 10 + lastDigit;
        return value.Value;
    }

    private int FindDigitsHard(string line)
    {
        List<string> spelled = new() { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        int? firstSpelledDigitIndex = null;
        int? lastSpelledDigitIndex = null;
        int? firstDigit = null;
        int? lastDigit = null;

        for (int i = 0; i < spelled.Count; i++)
        {
            int index = line.IndexOf(spelled[i]);
            if (index >= 0)
            {
                if (!firstSpelledDigitIndex.HasValue || firstSpelledDigitIndex.Value > index)
                {
                    firstSpelledDigitIndex = index;
                    firstDigit = (i % 9) + 1;
                }
            }

            index = line.LastIndexOf(spelled[i]);
            if (index >= 0)
            {
                if (!lastSpelledDigitIndex.HasValue || lastSpelledDigitIndex.Value < index)
                {
                    lastSpelledDigitIndex = index;
                    lastDigit = (i % 9) + 1;
                }
            }
        }

        return firstDigit.Value * 10 + lastDigit.Value;
    }
}