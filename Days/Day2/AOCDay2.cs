using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Days.Day2
{
    public struct GameRound
    {
        public int roundIndex;
        public int redCubes;
        public int greenCubes;
        public int blueCubes;
        public bool ValidRound(int maxRed, int maxGreen, int maxBlue)
        {
            var IsValid = (redCubes <= maxRed && greenCubes <= maxGreen && blueCubes <= maxBlue);
            return IsValid;
        }

        public override string ToString()
        {
            return $"[Round {roundIndex}]: Red {redCubes}, Green {greenCubes}, Blue {blueCubes}";
        }
    }
    public class Game
    {
        public int gameIndex = -1;
        List<GameRound> rounds = new List<GameRound>();

        public int minRedCubes;
        public int minGreenCubes;
        public int minBlueCubes;
        public bool ValidGame(int maxRed, int maxGreen, int maxBlue)
        {
            Console.WriteLine($"{this}");
            for (int i = 0; i < rounds.Count; i++)
            {
                if (!rounds[i].ValidRound(maxRed, maxGreen, maxBlue))
                {
                    return false;
                }
            }
            return true;
        }

        public void AddGameRound(GameRound round)
        {
            if (round.redCubes > minRedCubes) { minRedCubes = round.redCubes; }
            if (round.greenCubes > minGreenCubes) { minGreenCubes = round.greenCubes; }
            if (round.blueCubes > minBlueCubes) { minBlueCubes = round.blueCubes; }

            rounds.Add(round);
        }

        public int GetGamePower()
        {
            int power = minRedCubes * minGreenCubes * minBlueCubes;
            return power;
        }
        public override string ToString()
        {
            return $"[Game {gameIndex}]: Has {rounds.Count} rounds! GamePower is {GetGamePower()}";
        }
    }
    public class AOCDay2
    {
        string[] input;
        List<Game> games = new List<Game>();
        public AOCDay2(string[] inputText)
        {
            input = inputText;
            games = new List<Game>(inputText.Length);

            for (int i = 0; i < input.Length; i++)
            {
                var one = input[i].Split(":");
                var gameIndex = int.Parse(one[0].Trim().Split(" ")[1]);
                var rounds = one[1].Split(";");

                Game newGame = new Game();
                newGame.gameIndex = gameIndex;
                for (int r = 0; r < rounds.Length; r++)
                {
                    var singleRound = rounds[r].Trim().Split(",");
                    GameRound round = new GameRound();
                    round.roundIndex = r;

                    for (int sr = 0; sr < singleRound.Length; sr++)
                    {
                        var colors = singleRound[sr].Trim().Split(" ");
                        var count = int.Parse(colors[0]);
                        var color = colors[1];

                        switch (color)
                        {
                            case "red":
                                round.redCubes = count;
                                break;
                            case "green":
                                round.greenCubes = count;
                                break;
                            case "blue":
                                round.blueCubes = count;
                                break;
                        }
                    }
                    newGame.AddGameRound(round);
                }
                games.Add(newGame);
            }
        }

        public void Run(int maxRed, int maxGreen, int maxBlue)
        {
            Console.WriteLine($"%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
            int combinedIDs = 0;
            int combinedGamePower = 0;
            foreach (var game in games)
            {
                if (game.ValidGame(maxRed, maxGreen, maxBlue))
                {
                    combinedIDs += game.gameIndex;
                }

                combinedGamePower += game.GetGamePower();
            }
            Console.WriteLine($"Combined IDs {combinedIDs}");
            Console.WriteLine($"Combined Game Power {combinedGamePower}");
        }
    }
}
