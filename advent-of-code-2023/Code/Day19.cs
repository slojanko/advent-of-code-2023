internal class Day19 : Solver
{
    public enum Compare
    {
        None,
        Less,
        Greater,
    }

    public class Rule
    {
        public char category;
        public Compare compare;
        public int value;
        public string workflow;

        public void Print()
        {
            if (category == '\0')
            {
                Console.WriteLine($"\t{workflow}");
            } else
            {
                Console.WriteLine($"\t{category} {compare} {value} {workflow}");
            }
        }
    }

    public class Workflow
    {
        public string name;
        public List<Rule> rules;

        public Workflow()
        {
            rules = new List<Rule>();
        }

        public void Print()
        {
            Console.WriteLine(name);
            foreach (var item in rules)
            {
                item.Print();
            }
        }

        public string GetNext(Part part, Rule rule)
        {
            if (rule.compare == Compare.None)
            {
                return rule.workflow;
            }

            int part_value = part.GetValue(rule.category);

            switch (rule.compare)
            {
                case Compare.Less:
                    if (part_value < rule.value)
                    {
                        return rule.workflow;
                    }
                    break;
                case Compare.Greater:
                    if (part_value > rule.value)
                    {
                        return rule.workflow;
                    }
                    break;
            }

            return null;
        }

        public string GetNext(Part part)
        {
            foreach(var rule in rules)
            {
                var workflow = GetNext(part, rule);
                if (workflow != null)
                {
                    return workflow;
                }
            }

            return string.Empty;
        }
    }

    public class Part
    {
        public int x;
        public int m;
        public int a;
        public int s;

        public void Print()
        {
            Console.WriteLine($"x:{x} m:{m} a:{a} s:{s}");
        }

        public int GetValue(char category)
        {
            switch (category)
            {
                case 'x': return x;
                case 'm': return m;
                case 'a': return a;
                case 's': return s;
            }

            return 0;
        }
    }

    public class PartRange
    {
        public (long, long) x;
        public (long, long) m;
        public (long, long) a;
        public (long, long) s;

        public PartRange((long, long) x, (long, long) m, (long, long) a, (long, long) s)
        {
            this.x = x;
            this.m = m;
            this.a = a;
            this.s = s;
        }

        public (long, long) GetValue(char category)
        {
            switch (category)
            {
                case 'x': return x;
                case 'm': return m;
                case 'a': return a;
                case 's': return s;
            }

            return (0, 0);
        }

        public void SetValue(char category, (long, long) value)
        {
            switch (category)
            {
                case 'x': x = value; break;
                case 'm': m = value; break;
                case 'a': a = value; break;
                case 's': s = value; break;
            }
        }

        public long GetCombinations()
        {
            return (x.Item2 - x.Item1 + 1) * (m.Item2 - m.Item1 + 1) * (a.Item2 - a.Item1 + 1) * (s.Item2 - s.Item1 + 1);
        }

        public PartRange Clone()
        {
            return new PartRange(x, m, a, s);
        }

        public void Print()
        {
            Console.WriteLine($"x:{x.Item1} {x.Item2} m:{m.Item1} {m.Item2} a:{a.Item1} {a.Item2} s:{s.Item1} {s.Item2}");
        }
    }

    public override void SolveEasy()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day19.txt");
        long result = 0;

        Dictionary<string, Workflow> workflows = new Dictionary<string, Workflow>();
        List<Part> parts = new List<Part>();

        ReadInput(input, workflows, parts);

        foreach(var part in parts)
        {
            Workflow workflow = workflows["in"];

            while(true)
            {
                string next = workflow.GetNext(part);
                if (next == "R")
                {
                    break;
                }

                if (next == "A")
                {
                    result += part.x + part.m + part.a + part.s;
                    break;
                }

                workflow = workflows[next];
            }
        }

        PrintEasy(result);
    }

    public override void SolveHard()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day19.txt");
        long result = 0;

        Dictionary<string, Workflow> workflows = new Dictionary<string, Workflow>();
        List<Part> parts = new List<Part>();

        LinkedList<(string, PartRange)> queue = new LinkedList<(string, PartRange)>();
        queue.AddLast(("in", new PartRange((1, 4000), (1, 4000), (1, 4000), (1, 4000))));

        ReadInput(input, workflows, parts);

        while(queue.Count > 0)
        {
            var range = queue.First();
            queue.RemoveFirst();

            if (range.Item1 == "R")
            {
                continue;
            }

            if (range.Item1 == "A")
            {
                result += range.Item2.GetCombinations();
                continue;
            }

            Workflow workflow = workflows[range.Item1];
            bool full_range = false;

            foreach (var rule in workflow.rules)
            {
                if (rule.compare == Compare.None)
                {
                    range.Item1 = rule.workflow;
                    queue.AddLast(range);
                    break;
                }

                var range_value = range.Item2.GetValue(rule.category);

                switch (rule.compare)
                {
                    case Compare.Less:
                        if (range_value.Item1 < rule.value)
                        {
                            PartRange clone = range.Item2.Clone();
                            if (range_value.Item2 >= rule.value)
                            {
                                clone.SetValue(rule.category, (range_value.Item1, rule.value - 1));
                                range.Item2.SetValue(rule.category, (rule.value, range_value.Item2));
                            } else
                            {
                                full_range = true;
                            }
                            queue.AddLast((rule.workflow, clone));
                        }
                        break;
                    case Compare.Greater:
                        if (range_value.Item2 > rule.value)
                        {
                            PartRange clone = range.Item2.Clone();
                            if (range_value.Item1 <= rule.value)
                            {
                                clone.SetValue(rule.category, (rule.value + 1, range_value.Item2));
                                range.Item2.SetValue(rule.category, (range_value.Item1, rule.value));
                            }
                            else
                            {
                                full_range = true;
                            }
                            queue.AddLast((rule.workflow, clone));
                        }
                        break;
                }

                if (full_range)
                {
                    break;
                }
            }
        }

        PrintHard(result);
    }

    public void ReadInput(string[] input, Dictionary<string, Workflow> workflows, List<Part> parts) 
    {
        int start_of_parts = 0;

        for(int i = 0; i < input.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(input[i]))
            {
                start_of_parts = i + 1;
                break;
            }

            Workflow workflow = new Workflow();

            string[] tokens = input[i].Split('{');
            workflow.name = tokens[0]; 
            // C# 8.0 feature https://www.c-sharpcorner.com/article/working-with-ranges-and-indices-in-c-sharp-8-0/ 
            tokens = tokens[1][0..^1].Split(',');

            foreach(var token in tokens)
            {
                if (token.Contains(':'))
                {
                    var rule_tokens = token.Split(':'); // [a<2006, qkq]
                    Rule rule = new Rule();
                    rule.category = rule_tokens[0][0];
                    rule.compare = rule_tokens[0].Contains("<") ? Compare.Less : Compare.Greater;
                    rule.value = int.Parse(rule_tokens[0][2..]);
                    rule.workflow = rule_tokens[1];

                    workflow.rules.Add(rule);
                } else
                {
                    Rule rule = new Rule();
                    rule.compare = Compare.None;
                    rule.workflow = token;
                    workflow.rules.Add(rule);
                }
            }

            workflows.Add(workflow.name, workflow);
            //workflow.Print();
        }

        for(int i = start_of_parts; i < input.Length; i++)
        {
            Part part = new Part();
            string[] tokens = input[i][1..^1].Split(',');

            foreach(var token in tokens)
            {
                var part_tokens = token.Split('=');
                switch (part_tokens[0][0])
                {
                    case 'x': part.x = int.Parse(part_tokens[1]); break;
                    case 'm': part.m = int.Parse(part_tokens[1]); break;
                    case 'a': part.a = int.Parse(part_tokens[1]); break;
                    case 's': part.s = int.Parse(part_tokens[1]); break;
                }
            }

            parts.Add(part);
            //part.Print();
        }
    }
}
