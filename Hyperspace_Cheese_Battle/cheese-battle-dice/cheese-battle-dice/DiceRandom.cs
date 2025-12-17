using System;

class DiceRandom : IDice
{
    private Random random;

    public DiceRandom()
    {
        random = new Random();
    }

    public int Roll()
    {
        return random.Next(1, 7);
    }
}