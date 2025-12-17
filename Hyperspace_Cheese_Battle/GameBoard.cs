namespace Hyperspace_Cheese_Battle;

using System;
using System.IO;
using System.Collections.Generic;

public class GameBoard
{
    public const int MAX_SQUARE_LOCATION = 7; //need to make them public to avoid errors in hyperspacecheesebattle.cs
    public const int MIN_SQUARE_LOCATION = 0;

    Square[,] HyperSpaceGrid;

    public GameBoard()
    {
        HyperSpaceGrid = new Square[8, 8];

        string[] gridLines = File.ReadAllLines("hyper-space-grid.csv");

        //Need to invert the y coordinate in relation to the array
        int y = MAX_SQUARE_LOCATION;
        for (int i = MIN_SQUARE_LOCATION; i <= MAX_SQUARE_LOCATION; i++)
        {
            string[] gridData = gridLines[i].Split(',');
            for (int x = MIN_SQUARE_LOCATION; x <= MAX_SQUARE_LOCATION; x++)
            {
                string[] squareData = gridData[x].Split('-');
                Enum.TryParse(squareData[0], out SquareDirection direction);
                Enum.TryParse(squareData[1], out SquareType type);

                HyperSpaceGrid[x, y] = new Square(type, direction, x, y);
            }
            y--;
        }
    }

    //helper
    //returns true if (x,y) inside the board
    private bool IsValidLocation(int locationX, int locationY)
    {
        return locationX >= MIN_SQUARE_LOCATION && locationX <= MAX_SQUARE_LOCATION && locationY >= MIN_SQUARE_LOCATION && locationY <= MAX_SQUARE_LOCATION;
    }


    //Recursion for when a square is occupied
    private Square? GetNewSquare(int locationX, int locationY)
    {
        Square? newSquare = null;
        if (IsValidLocation(locationX, locationY))
        {
            newSquare = HyperSpaceGrid[locationX, locationY];
            if (newSquare.Rocket != null)
            {
                //Increment in the direction of the new square
                switch (newSquare.Direction)
                {
                    case SquareDirection.Up:
                        locationY++;
                        break;
                    case SquareDirection.Down:
                        locationY--;
                        break;
                    case SquareDirection.Left:
                        locationX--;
                        break;
                    case SquareDirection.Right:
                        locationX++;
                        break;
                }
                return GetNewSquare(locationX, locationY);
            }
        }
        return newSquare;
    }

    //move player's rocket by dice roll value
    public void MoveRocket(Player player, int diceRoll)
    {
        
        Square? newSquare = null;
        Square oldSquare = player.Rocket.Square;

        if (oldSquare == null)
        {
            oldSquare = HyperSpaceGrid[0,0];
        }
        int locationX;
        int locationY;
        switch (oldSquare.Direction)
        {
            case SquareDirection.Up:
                locationY = oldSquare.LocationY + diceRoll;
                newSquare = GetNewSquare(oldSquare.LocationX, locationY);
                break;
            case SquareDirection.Down:
                locationY = oldSquare.LocationY - diceRoll;
                newSquare = GetNewSquare(oldSquare.LocationX, locationY);
                break;
            case SquareDirection.Left:
                locationX = oldSquare.LocationX - diceRoll;
                newSquare = GetNewSquare(locationX, oldSquare.LocationY);
                break;
            case SquareDirection.Right:
                locationX = oldSquare.LocationX + diceRoll;
                newSquare = GetNewSquare(locationX, oldSquare.LocationY);
                break;
        }

        //Move the rocket onto the new square if we found one.
        if (newSquare != null)
        {
            //Remove rocket from old square and place in new one
            oldSquare.Rocket = null;
            newSquare.Rocket = player.Rocket;
            player.Rocket.Square = newSquare;
        }
    }

    //checking if a move would stay on board
    public bool IsMoveInBounds(Square square, int diceRoll)
    {
        if (square == null)
        {
            square = HyperSpaceGrid[0,0]; //starting square
        }

        int locationX = square.LocationX;
        int locationY = square.LocationY;

        switch (square.Direction)
        {
            case SquareDirection.Up:
                locationY += diceRoll;
                break;
            case SquareDirection.Down:
                locationY -= diceRoll;
                break;
            case SquareDirection.Left:
                locationX -= diceRoll;
                break;
            case SquareDirection.Right:
                locationX += diceRoll;
                break;
        }

        return IsValidLocation(locationX, locationY);
    }

    //Explosion: moves the target player's rocket to the chosen bottom-row column.
    public void ExplodeRocket(Player targetPlayer, int chosenX)
    {
        if (targetPlayer.Rocket.Square != null)
        {
            targetPlayer.Rocket.Square.Rocket = null;
        }

        Square bottomSquare = HyperSpaceGrid[chosenX, 0];
        bottomSquare.Rocket = targetPlayer.Rocket;
        targetPlayer.Rocket.Square = bottomSquare;
    }

    //Helper used by explosion code to ensure a bottom square is free.
    public bool IsBottomSquareFree(int x)
    {
        Square s = HyperSpaceGrid[x, 0];
        return s.Rocket == null;
    }

    //helper to convert direction to single letter string for display
    private string GetDirectionLetter(SquareDirection direction)
    {
        switch (direction)
        {
            case SquareDirection.Up:
                return "@";
            case SquareDirection.Down:
                return "b";
            case SquareDirection.Left:
                return "<";
            case SquareDirection.Right:
                return "d";
            default:
                return ".";
        }
    }

    //displayboard
    public void DisplayBoard()
    {
        for (int row = MAX_SQUARE_LOCATION; row >= MIN_SQUARE_LOCATION; row--)
        {
            for (int col = MIN_SQUARE_LOCATION; col <= MAX_SQUARE_LOCATION; col++)
            {
                Square s = HyperSpaceGrid[col, row];

                string token = "";

                if (s.Type == SquareType.Win)
                {
                    Console.Write("WIN");
                }
                else if (s.Rocket != null)
                {
                    if (s.Type == SquareType.Cheese)
                    {
                        token = "RC"; //rocket + cheese
                    }
                    else
                    {
                        token = "R"; //rocket only
                    }
                }
                else
                {
                    if (s.Type == SquareType.Cheese)
                    {
                        token = "C"; //cheese only
                    }
                    else
                    {
                        token = GetDirectionLetter(s.Direction);
                    }
                }
                Console.Write(token.PadRight(4)); //keeps everything lined up
            }
            Console.WriteLine();
        }
    }
}
