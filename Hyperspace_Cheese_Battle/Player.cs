namespace Hyperspace_Cheese_Battle;
using System;

public class Player : IPlayer
{
    public string Name { get; private set; }

    public Rocketship Rocket { get; private set; }

    //six power extra credit
    public int ConsecutiveSixes { get; set; }

    //computer-player behaviour extra credit
    public PlayerKind Kind { get; private set; }

    public Player(string playerName)
    {
        Name = playerName;
        Rocket = new Rocketship();
        ConsecutiveSixes = 0;
        Kind = DetermineKindFromName(playerName);
    }

    public string GetName()
    {
        return Name;
    }

    public override string ToString()
    {
        return Name;
    }

    private static PlayerKind DetermineKindFromName(string playerName)
    {
        if (playerName == "Angry Allen")
        {
            return PlayerKind.AngryAllen;
        }
        else if (playerName == "Speedy Steve")
        {
            return PlayerKind.SpeedySteve;
        }
        else if (playerName == "Clever Trevor")
        {
            return PlayerKind.CleverTrevor;
        }

        return PlayerKind.Human;
    }
}
