internal class Day8 : Solver
{
    public class Node
    {
        public string name;
        public bool is_start, is_end;
        public int source, left, right;

        public Node(string name, bool is_start, bool is_end, int source, int left, int right)
        {
            this.name = name;
            this.is_start = is_start;
            this.is_end = is_end;
            this.source = source;
            this.left = left;
            this.right = right;
        }
    }

    public class Ghost
    {
        public Dictionary<Node, List<int>> end_visits_local_steps = new Dictionary<Node, List<int>>();
        public List<int> end_visits_global_steps = new List<int>();

        public Ghost(int start, string path, Dictionary<int, Node> nodes)
        {
            int step_counter = 0;
            int path_index = 0;
            int node_id = start;

            while (true)
            {
                if (nodes[node_id].is_end)
                {
                    end_visits_local_steps.TryAdd(nodes[node_id], new List<int>());
                    List<int> visits = end_visits_local_steps[nodes[node_id]];

                    bool should_stop = false;
                    // If we already visited node before, but now the step index is multiple of some previous visit, break immediately 
                    foreach (var end_visits_global_step in end_visits_global_steps)
                    {
                        if (step_counter % end_visits_global_step == 0)
                        {
                            should_stop = true;
                        }
                    }

                    if (should_stop)
                    {
                        break;
                    }

                    visits.Add(path_index);
                    end_visits_global_steps.Add(step_counter);
                }

                step_counter++;

                node_id = path[path_index] == 'R' ? nodes[node_id].right : nodes[node_id].left;

                path_index = path_index == path.Length - 1 ? 0 : path_index + 1;
            }

            Console.WriteLine(string.Join(' ', end_visits_global_steps));
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

            result++;

            node_id = path[path_index] == 'R' ? nodes[node_id].right : nodes[node_id].left;

            path_index = path_index == path.Length - 1 ? 0 : path_index + 1;
        }

        PrintEasy(result);
    }

    public override void SolveHard()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day8.txt");
        long result = 1;
        Dictionary<int, Node> nodes = new Dictionary<int, Node>();
        string path = "";
        List<Ghost> ghosts = new List<Ghost>();

        ReadInput(input, nodes, ref path, true);

        foreach (var value in nodes.Values)
        {
            if (value.is_start)
            {
                ghosts.Add(new Ghost(value.source, path, nodes));
            }
        }

        result = GetGhostsLCM(ghosts, 1, 0);

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

            nodes.Add(node_id, new Node(node, is_hard ? node.EndsWith('A') : node == "AAA", is_hard ? node.EndsWith("Z") : node == "ZZZ", node_id, left_id, right_id));
        }
    }

    public long GetGhostsLCM(List<Ghost> ghosts, long current_lcm, int index)
    {
        if (index == ghosts.Count)
        {
            return current_lcm;
        }

        long new_lcm = long.MaxValue;

        foreach (int steps in ghosts[index].end_visits_global_steps)
        {
            long local_lcm = GetGhostsLCM(ghosts, GetLCM(current_lcm, steps), index + 1);

            new_lcm = Math.Min(new_lcm, local_lcm);
        }

        return new_lcm;
    }

    // Copied from https://stackoverflow.com/questions/147515/least-common-multiple-for-3-or-more-numbers
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
