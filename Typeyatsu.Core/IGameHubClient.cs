namespace Typeyatsu.Core
{
    public interface IGameHubClient
    {
        void OnRivalDisconnected();
        void OnRivalFound(Keyword[] words);
        void OnRivalStateChanged(GameState state);
        void OnRivalGameOver(GameResult result);
    }
}
