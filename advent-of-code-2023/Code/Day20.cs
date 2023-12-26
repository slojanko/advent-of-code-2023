internal class Day20 : Solver
{
    public enum Pulse
    {
        Low,
        High,
    }

    public class State
    {
        public long button_presses = 0;
        public long low = 0;
        public long high = 0;

        public void Increment(Pulse pulse, int count)
        {
            if (pulse == Pulse.Low)
            {
                low += count;
            } else
            {
                high += count;
            }
        }

        public void Print()
        {
            Console.WriteLine($"Low: {low} High: {high}");
        }
    }

    public class Signal
    {
        public Module source;
        public Module destinaton;
        public Pulse pulse;

        public Signal(Module source, Module destination, Pulse pulse)
        {
            this.source = source;
            this.destinaton = destination;
            this.pulse = pulse;
        }
    }

    public class Module
    {
        public string name;
        public List<Module> outputs;

        public bool for_part_2;
        public List<long> high_pulses;

        public Module(string name)
        {
            this.name = name;
            outputs = new List<Module>();
            for_part_2 = false;
            high_pulses = new List<long>();
        }

        public virtual void AddInput(Module input)
        {
        }

        public virtual void AddOutput(Module output)
        {
            outputs.Add(output);
        }

        public virtual void ProcessPulse(Signal signal, LinkedList<Signal> queue, State state) 
        { 
        }

        public virtual void Print()
        {
            Console.WriteLine($"Name: {name}");
            Console.WriteLine($"Outputs: {string.Join(" ", outputs.Select(x => x.name))}");
        }
    }

    public class FlipFlop : Module
    {
        public bool state;

        public FlipFlop(string name) : base(name)
        {
            state = false;
        }

        public override void ProcessPulse(Signal signal, LinkedList<Signal> queue, State state)
        {
            if (signal.pulse == Pulse.High)
            {
                return;
            }

            this.state = !this.state;

            Pulse new_pulse = this.state ? Pulse.High : Pulse.Low;

            foreach(var output in outputs)
            {
                queue.AddLast(new Signal(this, output, new_pulse));
            }

            state.Increment(new_pulse, outputs.Count);
        }
    }

    public class Conjunction : Module
    {
        public Dictionary<Module, Pulse> inputs;

        public Conjunction(string name) : base(name)
        {
            inputs = new Dictionary<Module, Pulse>();
        }

        public override void AddInput(Module input)
        {
            inputs.Add(input, Pulse.Low);
        }

        public override void ProcessPulse(Signal signal, LinkedList<Signal> queue, State state)
        {
            inputs[signal.source] = signal.pulse;

            Pulse new_pulse = Pulse.Low;

            foreach(var input in inputs)
            {
                if (input.Value == Pulse.Low)
                {
                    new_pulse = Pulse.High;
                    break;
                }
            }

            foreach (var output in outputs)
            {
                queue.AddLast(new Signal(this, output, new_pulse));
            }

            if (for_part_2 && new_pulse == Pulse.High)
            {
                high_pulses.Add(state.button_presses);
            }

            state.Increment(new_pulse, outputs.Count);
        }

        public override void Print()
        {
            base.Print();
            Console.WriteLine($"Inputs: {string.Join(" ", inputs.Select(x => x.Key.name))}");
        }
    }

    public class Broadcast : Module
    {
        public Broadcast(string name) : base(name)
        {
        }

        public override void ProcessPulse(Signal signal, LinkedList<Signal> queue, State state)
        {
            foreach (var output in outputs)
            {
                queue.AddLast(new Signal(this, output, signal.pulse));
            }

            state.Increment(signal.pulse, outputs.Count);
        }
    }

    public override void SolveEasy()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day20.txt");
        long result = 0;

        State state = new State();
        LinkedList<Signal> queue = new LinkedList<Signal>();
        Dictionary<string, Module> modules = new Dictionary<string, Module>();
        ReadInput(input, modules);

        Signal button = new Signal(null, null, Pulse.Low);

        for(int i = 0; i < 1000; i++) {
            // Pressing button
            state.button_presses++;
            state.Increment(Pulse.Low, 1);
            modules["broadcaster"].ProcessPulse(button, queue, state);
            while (queue.Count > 0)
            {
                Signal signal = queue.First();
                queue.RemoveFirst();
                signal.destinaton.ProcessPulse(signal, queue, state);
            }
        }

        result = state.low * state.high;

        PrintEasy(result);
    }

    public override void SolveHard()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day20.txt");
        long result = 1;

        State state = new State();
        LinkedList<Signal> queue = new LinkedList<Signal>();
        Dictionary<string, Module> modules = new Dictionary<string, Module>();
        ReadInput(input, modules);

        Signal button = new Signal(null, null, Pulse.Low);

        // Relatively high loop count
        // Modules that output to conjunction then to rx, need to be high, so we wait for atleast two high pulses
        for (int i = 0; i < 100000; i++)
        {
            // Pressing button
            state.button_presses++;
            state.Increment(Pulse.Low, 1);
            modules["broadcaster"].ProcessPulse(button, queue, state);
            while (queue.Count > 0)
            {
                Signal signal = queue.First();
                queue.RemoveFirst();
                signal.destinaton.ProcessPulse(signal, queue, state);
            }
        }

        foreach(var module in modules)
        {
            if (module.Value.for_part_2)
            {
                result = GetLCM(result, module.Value.high_pulses[^1] - module.Value.high_pulses[^2]);
            }
        }

        PrintHard(result);
    }

    public void ReadInput(string[] input, Dictionary<string, Module> modules)
    {
        string module_with_rx_output = null;

        foreach(var line in input)
        {
            var tokens = line.Split(" -> ");
            var name = ExtractName(tokens[0]);

            if (tokens[0] == "broadcaster")
            {
                modules.Add(name, new Broadcast(name));
            } else if (tokens[0].StartsWith('%'))
            {
                modules.Add(name, new FlipFlop(name));
            } else if (tokens[0].StartsWith('&'))
            {
                modules.Add(name, new Conjunction(name));
            }

            if (tokens[1].Contains("rx"))
            {
                module_with_rx_output = name;
            }
        }

        foreach (var line in input)
        {
            var tokens = line.Split(" -> ");
            var name = ExtractName(tokens[0]);

            Module module = modules[name];

            var outputs = tokens[1].Split(", ");

            foreach(var output in outputs)
            {
                if (!modules.ContainsKey(output))
                {
                    modules.Add(output, new Module(output));
                }

                if (output == module_with_rx_output)
                {
                    module.for_part_2 = true;
                }

                module.AddOutput(modules[output]);
                modules[output].AddInput(module);
            }
        }

        //foreach(var module in modules)
        //{
        //    module.Value.Print();
        //    Console.WriteLine();
        //}
    }
    public long GetLCM(long a, long b)
    {
        return a * b / GetGCD(a, b);
    }

    public long GetGCD(long a, long b)
    {
        while (b > 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

    public string ExtractName(string token)
    {
        if (token.StartsWith('%') || token.StartsWith('&')) {
            return token[1..];
        }

        return token;
    }
}
