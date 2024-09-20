using System;
using System.Collections.Generic;

namespace TCOM2010.TicTacToe
{
    internal class Program
    {
        internal static void Main()
        {
            var game = new Game();

            game.Start();
        }
    }

    internal class Player
    {
        internal string? Name { get; set; }

        internal int Wins { get; set; }

        internal Player(string? name)
        {
            Name = name;
        }

        internal void PrintWins()
        {
            Console.WriteLine($"{Name} won {Wins} rounds(s)!");
        }
    }

    internal class Game
    {
        internal char[,]? GameBoard;
        internal List<Player> players;
        internal int turn;
        private bool isPlaying;
        private static Random rng = new Random();
        private int ties;

        internal void Start()
        {
            // Game set up
            Console.WriteLine("Welcome to Tic-Tac-Toe. Good luck!");
            Console.WriteLine();

            Console.Write("Enter a name for Player 1: ");
            var playerOneName = Console.ReadLine();
            var playerOne = new Player(playerOneName);

            Console.Write("Enter a name for Player 2: ");
            var playerTwoName = Console.ReadLine();
            var playerTwo = new Player(playerTwoName);

            players = new List<Player> { playerOne, playerTwo };
            SetUpGame();
            isPlaying = true;

            // Main game loop
            while (isPlaying)
            {
                GameLoop();
            }

            // Print game results after the game ends
            Console.WriteLine("Game over!");
            playerOne.PrintWins();
            playerTwo.PrintWins();
            Console.WriteLine($"The round ended in a tie {ties} time(s).");

            string overallWinnerMessage;

            if (playerOne.Wins > playerTwo.Wins)
            {
                overallWinnerMessage = $"{playerOne.Name} wins the game!";
            }
            else if (playerTwo.Wins > playerOne.Wins)
            {
                overallWinnerMessage = $"{playerTwo.Name} wins the game!";
            }
            else
            {
                overallWinnerMessage = "The game is a tie!";
            }

            Console.WriteLine(overallWinnerMessage);
        }

        internal void SetUpGame()
        {
            GameBoard = new char[3, 3];
            turn = 0;

            for (int x = 0; x < GameBoard.GetLength(0); x++)
            {
                for (int y = 0; y < GameBoard.GetLength(1); y++)
                {
                    GameBoard[x, y] = ' ';
                }
            }

            // Shuffle the turn order by shuffling the player list
            var n = players.Count;

            for (var i = n - 1; i > 0; i--)
            {
                var j = rng.Next(i + 1); // Pick a random index from 0 to i
                var temp = players[i];

                players[i] = players[j];
                players[j] = temp;
            }

            Console.WriteLine("New game!");
        }

        internal void PrintGameBoard()
        {
            Console.WriteLine();

            for (var y = GameBoard.GetLength(1) - 1; y >= 0; y--)
            {
                Console.Write($"{y} |");

                for (var x = 0; x < GameBoard.GetLength(0); x++)
                {
                    if (x >= GameBoard.GetLength(0) - 1)
                    {
                        Console.WriteLine($" {GameBoard[x, y]} ");
                    }
                    else
                    {
                        Console.Write($" {GameBoard[x, y]} ");
                    }

                    if (x < GameBoard.GetLength(0) - 1)
                    {
                        Console.Write("|");
                    }
                }

                if (y > 0)
                {
                    Console.WriteLine("——————————————");
                }
            }

            Console.WriteLine("——————————————");
            Console.WriteLine("  | 0 | 1 | 2");
            Console.WriteLine();
        }

        internal void GameLoop()
        {
            PrintGameBoard();
            var moveIsValid = false;
            char marker = turn == 0 ? 'X' : 'O';

            while (!moveIsValid)
            {
                Console.Write($"{players[turn].Name}'s Turn! ({marker}) Enter a coordinate pair in the form x,y: ");

                var coord = Console.ReadLine();

                if (!coord.Contains(","))
                {
                    Console.WriteLine("Your input seems to be formatted incorrectly. Enter a coordinate pair in the form x,y (including the comma).");
                    continue;
                }

                var splitInput = coord.Split(",");
                var xInput = splitInput[0];
                var yInput = splitInput[1];
                int x;
                int y;

                if (!int.TryParse(xInput, out x) || !int.TryParse(yInput, out y))
                {
                    Console.WriteLine("Your input seems to be formatted incorrectly. Enter a coordinate pair in the form x,y (where x and y are numbers).");
                    continue;
                }

                if (x < 0 || y < 0 || x > 2 || y > 2)
                {
                    Console.WriteLine("Your input is out of bounds. Enter a coordinate pair in the form x,y (where x and y are between 0 and 2).");
                    continue;
                }

                if (GameBoard[x, y] != ' ')
                {
                    Console.WriteLine("That square is already marked. Enter a coordinate pair in the form of x,y (and mind the existing marks on the grid).");
                    continue;
                }

                GameBoard[x, y] = marker;
                moveIsValid = true;
            }

            if (CheckWinner())
            {
                Console.WriteLine("Would you like to continue playing? Enter 'y' to continue, or anything else to quit.");

                var input = Console.ReadLine();

                if (input.ToLower() == "y")
                {
                    SetUpGame();
                }
                else
                {
                    isPlaying = false;
                }

                return;
            }
            else
            {
                if (++turn > 1)
                {
                    turn = 0;
                }
            }
        }

        internal bool CheckWinner()
        {
            var currentPlayer = players[turn];
            var otherPlayer = players[turn == 0 ? 1 : 0];
            var marker = turn == 0 ? "X" : "0";

            // Check rows
            for (var x = 0; x < GameBoard.GetLength(0); x++)
            {
                var firstChar = GameBoard[x, 0];

                if (firstChar != ' ')
                {
                    var allSame = true;

                    for (var y = 1; y < GameBoard.GetLength(1); y++)
                    {
                        if (GameBoard[x, y] != firstChar)
                        {
                            allSame = false;
                            break;
                        }
                    }

                    if (allSame)
                    {
                        currentPlayer.Wins++;
                        PrintGameBoard();
                        Console.WriteLine($"{currentPlayer.Name} ({marker}) wins the round!");
                        Console.WriteLine($"{currentPlayer.Name}: {currentPlayer.Wins}-{otherPlayer.Wins}-{ties}");
                        Console.WriteLine($"{otherPlayer.Name}: {otherPlayer.Wins}-{currentPlayer.Wins}-{ties}");
                        return true;
                    }
                }
            }

            // Check columns
            for (var y = 0; y < GameBoard.GetLength(1); y++)
            {
                var firstChar = GameBoard[0, y];

                if (firstChar != ' ')
                {
                    var allSame = true;

                    for (var x = 1; x < GameBoard.GetLength(0); x++)
                    {
                        if (GameBoard[x, y] != firstChar)
                        {
                            allSame = false;
                            break;
                        }
                    }

                    if (allSame)
                    {
                        currentPlayer.Wins++;
                        PrintGameBoard();
                        Console.WriteLine($"{currentPlayer.Name} ({marker}) wins the round!");
                        Console.WriteLine($"{currentPlayer.Name}: {currentPlayer.Wins}-{otherPlayer.Wins}-{ties}");
                        Console.WriteLine($"{otherPlayer.Name}: {otherPlayer.Wins}-{currentPlayer.Wins}-{ties}");
                        return true;
                    }
                }
            }

            // Check diagonals (if applicable)
            if (GameBoard.GetLength(0) == GameBoard.GetLength(1))
            {
                var firstChar = GameBoard[0, 0];

                if (firstChar != ' ')
                {
                    var allSame = true;

                    for (var i = 1; i < GameBoard.GetLength(0); i++)
                    {
                        if (GameBoard[i, i] != firstChar)
                        {
                            allSame = false;
                            break;
                        }
                    }

                    if (allSame)
                    {
                        currentPlayer.Wins++;
                        PrintGameBoard();
                        Console.WriteLine($"{currentPlayer.Name} ({marker}) wins the round!");
                        Console.WriteLine($"{currentPlayer.Name}: {currentPlayer.Wins}-{otherPlayer.Wins}-{ties}");
                        Console.WriteLine($"{otherPlayer.Name}: {otherPlayer.Wins}-{currentPlayer.Wins}-{ties}");
                        return true;
                    }
                }

                // Check the other direction, now
                firstChar = GameBoard[0, GameBoard.GetLength(1) - 1];

                if (firstChar != ' ')
                {
                    var allSame = true;

                    for (var i = 1; i < GameBoard.GetLength(0); i++)
                    {
                        if (GameBoard[i, GameBoard.GetLength(1) - 1 - i] != firstChar)
                        {
                            allSame = false;
                            break;
                        }
                    }

                    if (allSame)
                    {
                        currentPlayer.Wins++;
                        PrintGameBoard();
                        Console.WriteLine($"{currentPlayer.Name} ({marker}) wins the round!");
                        Console.WriteLine($"{currentPlayer.Name}: {currentPlayer.Wins}-{otherPlayer.Wins}-{ties}");
                        Console.WriteLine($"{otherPlayer.Name}: {otherPlayer.Wins}-{currentPlayer.Wins}-{ties}");
                        return true;
                    }
                }
            }

            var allFilled = true;

            // Now check for draws (no winners, but full board)
            foreach (var x in GameBoard)
            {
                if (x == ' ')
                {
                    allFilled = false;
                }
            }

            if (allFilled)
            {
                ties++;
                PrintGameBoard();
                Console.WriteLine("The round ends in a tie.");
                Console.WriteLine($"{currentPlayer.Name}: {currentPlayer.Wins}-{otherPlayer.Wins}-{ties}");
                Console.WriteLine($"{otherPlayer.Name}: {otherPlayer.Wins}-{currentPlayer.Wins}-{ties}");
                return true;
            }

            // Otherwise, the game just hasn't ended
            return false;
        }
    }
}