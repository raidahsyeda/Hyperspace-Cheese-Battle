namespace Hyperspace_Cheese_Battle;
using System;
public enum SquareDirection
{
    Up,
    Down,
    Left,
    Right
}

public enum SquareType
{
    Regular,
    Cheese,
    Win
}

public class Square
{
    public SquareType Type { get; private set; }
    public SquareDirection Direction { get; private set; }

    public int LocationX { get; private set; }
    public int LocationY { get; private set; }

    public Rocketship Rocket { get; set; }

    //new square with direction, type and coordinates
    public Square(SquareType type, SquareDirection direction, int x, int y)
    {
        Type = type;
        Direction = direction;
        LocationX = x;
        LocationY = y;
        Rocket = null;
    }

    public override string ToString()
    {
        return $"({LocationX},{LocationY})";
    }
}
