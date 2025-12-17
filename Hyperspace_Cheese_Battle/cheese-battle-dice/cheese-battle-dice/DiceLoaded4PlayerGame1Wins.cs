using System;

class DiceLoaded4PlayerGame1Wins : IDice
{
    private int[] rollValues;
    private int rollIndex;

    public DiceLoaded4PlayerGame1Wins()
    {
        rollValues = new int[]{
            2,2,3,4,3,2,2,6,5,5,6,6,5,5
        };
        rollIndex = 0;
    }

    public int Roll()
    {
        return rollValues[rollIndex++];
    }
}