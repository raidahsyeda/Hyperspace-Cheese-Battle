namespace Hyperspace_Cheese_Battle;
using System.Collections.Generic;
public interface IGame
{
    public static abstract string GetName();
 
    public static abstract List<IPlayer> SelectPlayers();
 
    public List<IPlayer> GetPlayers();
 
    public void DisplayGameState(IPlayer currentPlayer);
 
    public void TakePlayerTurn(IPlayer currentPlayer, ref int playerIndex);
 
    public bool IsGameOver(IPlayer currentPlayer);
 
    public void CongratulatePlayer(IPlayer currentPlayer);
 
    public void PlayGame()
    {
        int playerIndex = -1;
        IPlayer currentPlayer;
        List<IPlayer> players = GetPlayers();
        do
        {
            playerIndex = (playerIndex + 1) % players.Count;
            currentPlayer = players[playerIndex];
 
            DisplayGameState(currentPlayer);
 
            TakePlayerTurn(currentPlayer, ref playerIndex);
 
        } while (!IsGameOver(currentPlayer));
 
        CongratulatePlayer(currentPlayer);
    }
}