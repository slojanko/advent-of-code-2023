internal class Day5 : Solver
{
    public class Map
    {
        public List<Range> ranges = new List<Range>();

        public long MapValue(long value)
        {
            foreach(var range in ranges)
            {
                if (value >= range.source && value < range.source + range.length)
                {
                    return range.destination + value - range.source;
                }
            }

            return value;
        }

        public long MapValueReverse(long value)
        {
            foreach (var range in ranges)
            {
                if (value >= range.destination && value < range.destination + range.length)
                {
                    return range.source + value - range.destination;
                }
            }

            return value;
        }
    }

    public class Range
    {
        public long destination, source, length;
    }

    public override void SolveEasy()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day5.txt");
        long result = long.MaxValue;
        List<long> seeds = new List<long>();
        List<Map> maps = new List<Map>();

        ReadInput(input, seeds, maps);

        foreach(var seed in seeds)
        {
            result = Math.Min(result, MapSeed(seed, maps));
        }

        PrintEasy(result);
    }

    public override void SolveHard()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day5.txt");
        long result = long.MaxValue;
        List<long> seeds = new List<long>();
        List<Map> maps = new List<Map>();

        ReadInput(input, seeds, maps);

        // Fast version
        List<long> still_needed = [.. seeds];
        List<long> mapped_ranges = new List<long>();

        // Go through each map
        for (int j = 0; j < maps.Count; j++)
        {
            var map = maps[j];

            // Go through each range in map
            for (int k = 0; k < map.ranges.Count; k++) {
                var range = map.ranges[k];

                // For current range go through all not yet found pairs for this map
                for(int i = still_needed.Count; i > 0; i-=2)
                {
                    // end is actually start + length - 1, since 1 is used for start itself
                    long start = still_needed[0];
                    long length = still_needed[1];
                    long end = start + length - 1;

                    still_needed.RemoveRange(0, 2);

                    // Find split for range and input (seeds)
                    long left_split = Math.Max(start, range.source);
                    long right_split = Math.Min(end, range.source + range.length - 1);

                    // Check if either split actually split the input
                    if (left_split <= right_split)
                    {
                        mapped_ranges.Add(range.destination + left_split - range.source);
                        mapped_ranges.Add(right_split - left_split + 1);

                        if (left_split > start)
                        {
                            still_needed.Add(start);
                            still_needed.Add(left_split - start);
                        }

                        if (right_split < end)
                        {
                            still_needed.Add(right_split + 1);
                            still_needed.Add(end - right_split);
                        }
                    } else
                    {
                        still_needed.Add(start);
                        still_needed.Add(length);
                    }
                }

            }

            still_needed.AddRange(mapped_ranges);
            mapped_ranges.Clear();
        }

        for(int i = 0; i < still_needed.Count; i+=2)
        {
            result = Math.Min(result, still_needed[i]);
        }

        //// Slow version
        //result = 0;
        //while (true)
        //{
        //    long seed = MapReverse(result, maps);
        //    if (IsValidHard(seed, seeds))
        //    {
        //        break;
        //    }

        //    result++;
        //}

        PrintHard(result);
    }

    public void ReadInput(string[] input, List<long> seeds, List<Map> maps)
    {
        seeds.AddRange(input[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList());

        bool reading_ranges = false;
        for (int i = 1; i < input.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(input[i]))
            {
                reading_ranges = false;
            }

            if (reading_ranges)
            {
                List<long> values = input[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
                Range range = new Range();
                range.destination = values[0];
                range.source = values[1];
                range.length = values[2];
                maps[^1].ranges.Add(range);
            }
            
            if (input[i].Contains(':'))
            {
                reading_ranges = true;
                maps.Add(new Map());
            }
        }

        foreach (var map in maps)
        {
            map.ranges.Sort((x, y) => x.source.CompareTo(y.source));
        }
    }

    public long MapSeed(long value, List<Map> maps)
    {
        long result = value;
        foreach(var map in maps)
        {
            result = map.MapValue(result);
        }

        return result;
    }

    public bool IsValidHard(long value, List<long> seeds)
    {
        for(int i = 0; i < seeds.Count; i+=2)
        {
            if (value >= seeds[i] && value < seeds[i] + seeds[i + 1])
            {
                return true;
            }
        }

        return false;
    }

    public long MapReverse(long value, List<Map> maps)
    {
        long result = value;
        for (int i = maps.Count - 1; i >= 0; i--)
        {
            result = maps[i].MapValueReverse(result);
        }

        return result;
    }
}