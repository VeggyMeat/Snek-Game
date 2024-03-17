using System.Collections.Generic;

/// <summary>
/// The interface for the shop manager
/// </summary>
public interface IShopManager
{
    /// <summary>
    /// The list of bodies that the player still can add to their snake
    /// </summary>
    public List<string> Bodies { get; }

    /// <summary>
    /// The list of bodies that the player can add to their snake at the start of the game
    /// </summary>
    public List<string> PossibleInitialBodies { get; }

    /// <summary>
    /// Removes a body from the list of bodies
    /// </summary>
    /// <param name="body">The body to remove</param>
    public void RemoveBody(string body);

    /// <summary>
    /// The list of items that the player still can add to their snake
    /// </summary>
    public List<string> Items { get; }

    /// <summary>
    /// Removes an item from the list of items
    /// </summary>
    /// <param name="item">The item to remove</param>
    public void RemoveItem(string item);

    /// <summary>
    /// Whether the shop manager should remove the item or body after it is chosen by the player,
    /// (Allows or does not allow duplicate items or bodies)
    /// </summary>
    public bool Remove { get; }

    /// <summary>
    /// The next state for the next level up
    /// </summary>
    public ChoiceState NextState { get; set; }

    /// <summary>
    /// The list of bodies that can be leveled up in the snake
    /// </summary>
    public List<string> LevelableBodies { get; }

    /// <summary>
    /// Sets the game setup
    /// </summary>
    /// <param name="gameSetup">The game setup</param>
    public void SetGameSetup(IGameSetup gameSetup);

    /// <summary>
    /// Called when the player levels up
    /// </summary>
    public void OnLevelUp();

    /// <summary>
    /// Called after the player levels up
    /// </summary>
    public void AfterLevelUp();

    /// <summary>
    /// Pauses the game time
    /// </summary>
    public void PauseTime();

    /// <summary>
    /// Resumes the game time
    /// </summary>
    public void ResumeTime();
}
