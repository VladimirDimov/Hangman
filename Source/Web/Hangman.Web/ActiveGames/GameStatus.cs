using System;

namespace Hangman.Web.ActiveGames
{
    public enum GameStatus
    {
        Active = 0,
        WaitingForOpponent = 1,
        HasWinner = 2,
        Closed = 3
    }
}
