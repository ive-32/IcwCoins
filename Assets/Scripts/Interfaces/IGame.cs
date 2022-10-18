
public interface IGame
{
    enum EnumGameState { FirstStart, Paused, GameOver, GameWin, InProgress };
    EnumGameState gameState {get; set;}
    void StartNewGame();
    
    IWalker GetPlayer();

}
