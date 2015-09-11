namespace Typeyatsu.Core
{
    public interface IGameHub
    {
        void NotifyState(GameState state);
        GameState GetLastRivalState();
        void NotifyGameOver(GameResult result);
    }
}
