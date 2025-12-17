namespace Hyperspace_Cheese_Battle;
using System;
using System.Collections.Generic;

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Welcome to Hyperspace Cheese Battle!");

        //Select players
        List<IPlayer> players = HyperSpaceCheeseBattle.SelectPlayers();

        //Player chooses a dice from the DiceBag
        IDice dice = DiceBag.ChooseDice();

        //Create game
        IGame game = new HyperSpaceCheeseBattle(players, dice);

        game.PlayGame();
    }
}
