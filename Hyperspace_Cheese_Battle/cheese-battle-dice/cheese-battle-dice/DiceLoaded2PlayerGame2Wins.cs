using System;

class DiceLoaded2PlayerGame2Wins : IDice
{
    private int[] rollValues;
    private int rollIndex;

    public DiceLoaded2PlayerGame2Wins()
    {
        rollValues = new int[]{
            1,6,1,4,1,1,1,3
        };
        rollIndex = 0;
    }

    public int Roll()
    {
        return rollValues[rollIndex++];
    }
}