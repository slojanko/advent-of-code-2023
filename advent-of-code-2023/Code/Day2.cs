internal class Day2 : Solver
{
    public class Game
    {
        public int ID, red, green, blue;
    }

    public override void SolveEasy()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day2.txt");
        int result = 0;

        foreach (string line in input)
        {
            Game game = ParseGame(line);
            if (game.red <= 12 && game.green <= 13 && game.blue <= 14)
            {
                result += game.ID;
            }
        }

        PrintEasy(result);
    }

    public override void SolveHard()
    {
        string[] input = File.ReadAllLines(".\\Inputs\\day2.txt");
        int result = 0;

        foreach (string line in input)
        {
            Game game = ParseGame(line);
            result += game.red * game.green * game.blue;
        }

        PrintHard(result);
    }

    public Game ParseGame(string line)
    {
        Game game = new Game();

        string[] line_split = line.Split(": ");
        game.ID = int.Parse(line_split[0].Split(' ')[1]);

        foreach(var set in line_split[1].Split("; "))
        {
            foreach(var cubes in set.Split(", "))
            {
                if (cubes.EndsWith("red"))
                {
                    game.red = Math.Max(game.red, int.Parse(cubes.Split(' ')[0]));
                } else if (cubes.EndsWith("green"))
                {
                    game.green = Math.Max(game.green, int.Parse(cubes.Split(' ')[0]));
                } else if (cubes.EndsWith("blue"))
                {
                    game.blue = Math.Max(game.blue, int.Parse(cubes.Split(' ')[0]));
                }
            }
        }

        return game;
    }
}