using System;

class DiceBag
{

    public static IDice ChooseDice()
    {
        IDice dice;
        Console.WriteLine("These are the dice in the bag; 1 - Standard 6 sides; 2 - You choose the number; 3 - Loaded: 2 Player Game (Player 2 wins); 4 - Loaded: 4 Player Game (Player 1 wins)");
        Console.Write("Select the number of the dice you would like to play with? ");
        string diceChoice = Console.ReadLine();

        switch (diceChoice)
        {
            case "4":
                dice = new DiceLoaded4PlayerGame1Wins();
                break;
            case "3":
                dice = new DiceLoaded2PlayerGame2Wins();
                break;
            case "2":
                dice = new DicePicker();
                break;
            case "1":
            default:
                dice = new DiceRandom();
                break;
        }

        return dice;
    }
}