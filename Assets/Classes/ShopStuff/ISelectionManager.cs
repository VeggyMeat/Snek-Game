// COMPLETE

/// <summary>
/// The interface for the selection manager
/// </summary>
public interface ISelectionManager
{
    /// <summary>
    /// Sets the game setup
    /// </summary>
    /// <param name="gameSetup">The game setup</param>
    public void SetGameSetup(IGameSetup gameSetup);

    /// <summary>
    /// Called when the user clicks on a button (only called by unity button)
    /// </summary>
    /// <param name="button">The number corresponding to which button was clicked</param>
    public void ButtonClicked(int button);

    /// <summary>
    /// Called to hide the canvas
    /// </summary>
    public void HideButtons();

    /// <summary>
    /// Called to show the canvas
    /// </summary>
    public void ShowButtons();
}
