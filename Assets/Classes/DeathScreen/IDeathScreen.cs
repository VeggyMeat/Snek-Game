// COMPLETE

/// <summary>
/// The interface for the death screen
/// </summary>
public interface IDeathScreen
{
    /// <summary>
    /// Called when the player dies
    /// </summary>
    public void OnDeath();

    /// <summary>
    /// Called when the player has typed their name and presses enter (only called by unity button)
    /// </summary>
    public void NameTyped();

    /// <summary>
    /// Called when the player presses the main menu button (only called by unity button)
    /// </summary>
    public void OnMainMenu();

    /// <summary>
    /// Sets the gameSetup of the snake
    /// </summary>
    /// <param name="gameSetup"></param>
    public void SetGameSetup(IGameSetup gameSetup);
}
