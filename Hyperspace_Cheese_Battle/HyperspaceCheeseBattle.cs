namespace Hyperspace_Cheese_Battle;

using System;
using System.Collections.Generic;

public class HyperSpaceCheeseBattle : IGame
{
    private List<IPlayer> players;
    private IDice gameDice;
    private GameBoard gameBoard;
    private bool gameOver;
    private static Random random = new Random();

    private bool enableEnhancements; //enabling extra enhancements based on dice

    public HyperSpaceCheeseBattle(List<IPlayer> playersForGame, IDice diceForGame)
    {
        players = playersForGame;
        gameDice = diceForGame;
        enableEnhancements = (gameDice.GetType().Name == "DicePicker"); //extra enhancements should only be enabled when using dice picker
        gameBoard = new GameBoard();
        gameOver = false;
    }

    public static string GetName()
    {
        return "Hyperspace Cheese Battle";
    }

    public static List<IPlayer> SelectPlayers()
    {
        List<IPlayer> selected = new List<IPlayer>();

        int count = ReadPlayerCount();

        for (int i = 0; i < count; i++)
        {
            Console.Write($"Enter name for Player {i + 1}: ");
            string name = Console.ReadLine();
            selected.Add(new Player(name));
        }
        return selected;
    }

    private static int ReadPlayerCount()
    {
        while (true)
        {
            Console.Write("Enter the number of players (2-4) : ");
            string input = Console.ReadLine();
            int value;

            if (int.TryParse(input, out value))
            {
                if (value >= 2 && value <= 4)
                {
                    return value;
                }
            }

            Console.WriteLine("Invalid value. Please try again.");
        }
    }

    public List<IPlayer> GetPlayers()
    {
        return players;
    }

    public void DisplayGameState(IPlayer currentPlayer)
    {
        //Display board before each turn (extra credit)
        if (enableEnhancements)
        {
            gameBoard.DisplayBoard();
        }
        Console.WriteLine($"It is {currentPlayer.GetName()}'s turn.");
    }

    public bool IsGameOver(IPlayer currentPlayer)
    {
        return gameOver;
    }

    public void CongratulatePlayer(IPlayer currentPlayer)
    {
        Console.WriteLine($"Congratulations {currentPlayer.GetName()}! You have won the game.");
    }

    //helpers for TakePlayerTurn()
    private void AdjustPlayerIndexForExtraTurn(ref int playerIndex)
    {
        int count = players.Count;
        playerIndex = (playerIndex - 1 + count) % count;
    }

    private int ChooseBottomRowSquare(Player player)
    {
        while (true)
        {
            Console.Write($"{player.GetName()} choose an unoccupied square on the bottom row; 0-7 : ");
            string input = Console.ReadLine();
            int value;

            if (int.TryParse(input, out value))
            {
                if (value >= GameBoard.MIN_SQUARE_LOCATION && value <= GameBoard.MAX_SQUARE_LOCATION && gameBoard.IsBottomSquareFree(value))
                {
                    return value;
                }
            }

            Console.WriteLine("Invalid value. Please try again.");
        }
    }

    private Player ChooseVictimFromInput(Player currentPlayer)
    {
        while (true)
        {
            Console.Write("Choose a player to explode ");

            for (int i = 0; i < players.Count; i++)
            {
                Console.Write($"{i + 1}-{players[i].GetName()}");
                if (i < players.Count - 1)
                {
                    Console.Write("; ");
                }
            }

            Console.Write(" : ");

            string input = Console.ReadLine();
            int value;

            if (int.TryParse(input, out value))
            {
                if (value >= 1 && value <= players.Count && players[value - 1] != currentPlayer)
                {
                    return players[value - 1] as Player;
                }
            }

            Console.WriteLine("Invalid value. Please try again.");
        }
    }

    private Player PickRandomVictim(Player currentPlayer)
    {
        List<Player> candidates = new List<Player>();

        for (int i = 0; i < players.Count; i++)
        {
            Player p = players[i] as Player;
            if (p != null && p != currentPlayer)
            {
                candidates.Add(p);
            }
        }

        int index = random.Next(candidates.Count);
        return candidates[index];
    }

    private Player FindFurthestAheadPlayer()
    {
        Player best = null;
        int bestScore = -1;

        for (int i = 0; i < players.Count; i++)
        {
            Player p = players[i] as Player;
            if (p == null)
            {
                continue;
            }

            if (p.Rocket.Square == null)
            {
                continue;
            }

            int score = p.Rocket.Square.LocationX + p.Rocket.Square.LocationY;

            if (score > bestScore)
            {
                bestScore = score;
                best = p;
            }
        }

        return best;
    }

    public void TakePlayerTurn(IPlayer currentPlayer, ref int playerIndex)
    {
        Player player = currentPlayer as Player;
        if (player == null)
        {
            return;
        }

        Console.WriteLine($"{currentPlayer.GetName()}, press Enter to roll the dice...");
        Console.ReadLine();

        int roll = gameDice.Roll();

        //Six Power
        if (enableEnhancements)
        {
            if (roll == 6)
            {
                player.ConsecutiveSixes++;
            }
            else
            {
                player.ConsecutiveSixes = 0;
            }

            //Three sixes in a row = explode
            if (player.ConsecutiveSixes >= 3)
            {
                Console.WriteLine($"{currentPlayer.GetName()} rolls a {roll}. That is three sixes in a row! The rocket engine explodes.");

                int colSelf = ChooseBottomRowSquare(player);
                gameBoard.ExplodeRocket(player, colSelf);

                player.ConsecutiveSixes = 0;
                return;
            }
        }

        Square currentSquare = player.Rocket.Square;

        //Check bounds
        bool canMove = gameBoard.IsMoveInBounds(currentSquare, roll);

        if (!canMove)
        {
            Console.WriteLine($"{currentPlayer.GetName()} rolls a {roll}. The rocket is unable to move because this would take it off the board.");
            return;
        }

        //Move rocket
        gameBoard.MoveRocket(player, roll);

        Square newSquare = player.Rocket.Square;

        Console.WriteLine($"{currentPlayer.GetName()} rolls a {roll}. The rocket moves to square {newSquare}.");

        if (newSquare.Type == SquareType.Win)
        {
            gameOver = true;
            return;
        }

        bool onCheese = (newSquare.Type == SquareType.Cheese);

        if (!onCheese)
        {
            if (enableEnhancements && roll == 6)
            {
                Console.WriteLine($"{currentPlayer.GetName()} gains an extra throw from Six Power!");
                AdjustPlayerIndexForExtraTurn(ref playerIndex);
            }
            return;
        }

        //Cheese power
        player.ConsecutiveSixes = 0;

        Console.WriteLine($"{currentPlayer.GetName()} has landed on a Cheese Power Square.");
        Console.WriteLine($"Does {currentPlayer.GetName()} want to roll again or explode the engines of another rocket?");

        string choice = "";

        //Computer players
        if (enableEnhancements)
        {
            if (player.Kind == PlayerKind.SpeedySteve)
            {
                Console.WriteLine($"{player.GetName()} chooses to throw again.");
                choice = "t";
            }
            else if (player.Kind == PlayerKind.AngryAllen)
            {
                Console.WriteLine($"{player.GetName()} chooses to explode another rocket.");
                choice = "e";
            }
            else if (player.Kind == PlayerKind.CleverTrevor)
            {
                Player leader = FindFurthestAheadPlayer();
                if (leader == player)
                {
                    Console.WriteLine($"{player.GetName()} chooses to throw again.");
                    choice = "t";
                }
                else
                {
                    Console.WriteLine($"{player.GetName()} chooses to explode {leader.GetName()}.");
                    choice = "e";
                }
            }
        }
        else
        {
            //Human player
            while (choice != "t" && choice != "e")
            {
                Console.Write("Enter t (throw) or e (explode) : ");
                choice = Console.ReadLine();

                if (choice != "t" && choice != "e")
                {
                    Console.WriteLine("Invalid value. Please try again.");
                }
            }
        }

        //Apply cheese choice
        if (choice == "t")
        {
            AdjustPlayerIndexForExtraTurn(ref playerIndex);
        }
        else
        {
            Player victim;

            if (player.Kind == PlayerKind.AngryAllen)
            {
                victim = PickRandomVictim(player);
            }
            else if (player.Kind == PlayerKind.CleverTrevor)
            {
                victim = FindFurthestAheadPlayer();
            }
            else
            {
                victim = ChooseVictimFromInput(player);
            }

            int col = ChooseBottomRowSquare(victim);
            gameBoard.ExplodeRocket(victim, col);
        }
    }
}
