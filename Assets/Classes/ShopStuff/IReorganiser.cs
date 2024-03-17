// COMPLETE

/// <summary>
/// The interface for the reorganiser
/// </summary>
public interface IReorganiser
{
    /// <summary>
    /// Sets the game setup
    /// </summary>
    /// <param name="gameSetup">The game setup</param>
    public void SetGameSetup(IGameSetup gameSetup);

    /// <summary>
    /// Whether the reorganiser is active,
    /// Activates or deactivates the reorganiser
    /// </summary>
    public bool Active { get; set; }
}
