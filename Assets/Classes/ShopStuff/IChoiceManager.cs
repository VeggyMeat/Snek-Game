// COMPLETE

/// <summary>
/// The interface for the choice manager
/// </summary>
public interface IChoiceManager
{
    /// <summary>
    /// Sets the game setup
    /// </summary>
    /// <param name="gameSetup">The game setup</param>
    public void SetGameSetup(IGameSetup gameSetup);

    /// <summary>
    /// Sets the state of the choice manager and creates the choices, freezing time
    /// </summary>
    /// <param name="state">The new state set</param>
    public void StartSet(ChoiceState state);

    /// <summary>
    /// Called when a button is clicked
    /// </summary>
    /// <param name="button">The number to indicate which button</param>
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
