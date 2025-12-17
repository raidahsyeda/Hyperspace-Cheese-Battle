using System;

class DicePicker : IDice
{
    private int rollIndex;
    const string INVALID_VALUE = "Invalid Value";

    public int Roll()
    {
        int rollValue = 0;
        while (true)
        {
            Console.Write("Choose a number between 1 and 6: ");
            try
            {
                rollValue = int.Parse(Console.ReadLine());
                if ((rollValue > 6) || (rollValue < 1))
                {
                    Console.WriteLine(INVALID_VALUE);
                }
                else
                {
                    break;
                }
            }
            catch
            {
                Console.WriteLine(INVALID_VALUE);
            }
        }
        return rollValue;
    }
}