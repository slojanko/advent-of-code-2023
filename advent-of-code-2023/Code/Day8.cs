internal class Day8 : Solver
{
    public class Node
    {
        public bool is_start, is_end;
        public int source, left, right;

        public Node(bool is_start, bool is_end, int source, int left, int right)
        {
            this.is_start = is_start;
            this.is_end = is_end;
            this.source = source;
            this.left = left;
            this.right = right;
        }
    }

    public override void SolveEasy()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day8.txt");
        long result = 0;
        Dictionary<int, Node> nodes = new Dictionary<int, Node>();
        string path = "";
        int path_index = 0;
        int node_id = 0;

        ReadInput(input, nodes, ref path, false);

        foreach (var value in nodes.Values)
        {
            if (value.is_start)
            {
                node_id = value.source;
                break;
            }
        }

        while (true)
        {

            if (nodes[node_id].is_end)
            {
                break;
            }

            if (path[path_index] == 'R')
            {
                node_id = nodes[node_id].right;
            } else
            {
                node_id = nodes[node_id].left;
            }

            result++;

            if (path_index == path.Length - 1)
            {
                path_index = 0;
            } else
            {
                path_index++;
            }
        }

        PrintEasy(result);
    }

    // Theres only one **Z node for each ghost, idk if this is problem with input or not
    // If there were multiple **Z nodes, we'd just get LCM for all combinations.
    // If we had 3 ghosts and two of them had two **Z while third had just one, 
    // we'd have to do 2*2*1 (4) different LCMs and get minimum from them.
    // Also the logic for breaking out of loop would change, you'd only break
    // if you came back to a previously visited **Z while path_index matched previous visit.
    public override void SolveHard()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day8.txt");
        long result = 1;
        Dictionary<int, Node> nodes = new Dictionary<int, Node>();
        string path = "";
        int path_index = 0;
        int node_id = 0;

        ReadInput(input, nodes, ref path, true);

        foreach (var value in nodes.Values)
        {
            if (value.is_start)
            {
                long local_result = 0;
                path_index = 0;
                node_id = value.source;

                Console.WriteLine("New ghost");

                while (true)
                {
                    if (nodes[node_id].is_end)
                    {
                        Console.WriteLine(node_id);
                        break;
                    }

                    if (path[path_index] == 'R')
                    {
                        node_id = nodes[node_id].right;
                    }
                    else
                    {
                        node_id = nodes[node_id].left;
                    }

                    local_result++;

                    if (path_index == path.Length - 1)
                    {
                        path_index = 0;
                    }
                    else
                    {
                        path_index++;
                    }
                }

                result = GetLCM(result, local_result);
            }
        }

        PrintHard(result);
    }

    public void ReadInput(string[] input, Dictionary<int, Node> nodes, ref string path, bool is_hard)
    {
        path = input[0];
        Dictionary<string, int> node_to_id = new Dictionary<string, int>();

        for(int i = 2; i < input.Length; i++)
        {
            string node = input[i].Substring(0, 3);
            string left = input[i].Substring(7, 3);
            string right = input[i].Substring(12, 3);

            node_to_id.TryAdd(node, node_to_id.Count);
            node_to_id.TryAdd(left, node_to_id.Count);
            node_to_id.TryAdd(right, node_to_id.Count);

            int node_id = node_to_id.GetValueOrDefault(node);
            int left_id = node_to_id.GetValueOrDefault(left);
            int right_id = node_to_id.GetValueOrDefault(right);

            nodes.Add(node_id, new Node(is_hard ? node.EndsWith('A') : node == "AAA", is_hard ? node.EndsWith("Z") : node == "ZZZ", node_id, left_id, right_id));
        }
    }

    // Copied from https://stackoverflow.com/questions/147515/least-common-multiple-for-3-or-more-numbers
    public long GetLCM(List<long> nums)
    {
        long result = nums[0];
        for (int i = 1; i < nums.Count; i++)
        {
            result = GetLCM(result, nums[i]);
        }

        return result;
    }

    public long GetLCM(long a, long b)
    {
        return a * b / GetGCD(a, b);
    }

    public long GetGCD(long a, long b)
    {
        while(b > 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }
}
