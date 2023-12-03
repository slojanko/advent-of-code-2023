internal abstract class Solver
{
    public abstract void SolveEasy();

    public abstract void SolveHard();

    public void PrintEasy<T>(T result)
    {
        Console.WriteLine($"{this.GetType().Name} Easy solution: {result}");
    }

    public void PrintHard<T>(T result)
    {
        Console.WriteLine($"{this.GetType().Name} Hard solution: {result}");
    }
}
