using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("EditorTests")]

// COMPLETE

/// <summary>
/// Handles the management of the shop
/// </summary>
public class ShopManager : MonoBehaviour, IShopManager
{
    /// <summary>
    /// The list of bodies that the player still can add to their snake
    /// </summary>
    [SerializeField] private List<string> bodies;

    /// <summary>
    /// The list of bodies that the player still can add to their snake
    /// </summary>
    public List<string> Bodies
    {
        get
        {
            return bodies;
        }
    }

    /// <summary>
    /// The list of bodies that the player can add to their snake at the start of the game
    /// </summary>
    [SerializeField] private List<string> possibleInitialBodies;

    /// <summary>
    /// The list of bodies that the player can add to their snake at the start of the game
    /// </summary>
    public List<string> PossibleInitialBodies
    {
        get
        {
            return possibleInitialBodies;
        }
    }

    /// <summary>
    /// Removes a body from the list of bodies
    /// </summary>
    /// <param name="body">The body to remove</param>
    public void RemoveBody(string body)
    {
        bodies.Remove(body);
    }

    /// <summary>
    /// The list of items that the player still can add to their snake
    /// </summary>
    [SerializeField] private List<string> items;

    /// <summary>
    /// The list of items that the player still can add to their snake
    /// </summary>
    public List<string> Items
    {
        get
        {
            return items;
        }
    }

    /// <summary>
    /// Removes an item from the list of items
    /// </summary>
    /// <param name="item">The item to remove</param>
    public void RemoveItem(string item)
    {
        items.Remove(item);
    }

    /// <summary>
    /// Whether the shop manager should remove the item or body after it is chosen by the player,
    /// (Allows or does not allow duplicate items or bodies)
    /// </summary>
    [SerializeField] private bool remove;

    /// <summary>
    /// Whether the shop manager should remove the item or body after it is chosen by the player,
    /// (Allows or does not allow duplicate items or bodies)
    /// </summary>
    public bool Remove
    {
        get
        {
            return remove;
        }
    }

    /// <summary>
    /// The next state for the next level up
    /// </summary>
    private ChoiceState nextState = ChoiceState.NewBody;

    /// <summary>
    /// The next state for the next level up
    /// </summary>
    public ChoiceState NextState
    {
        get
        {
            return nextState;
        }
        set
        {
            nextState = value;
        }
    }

    /// <summary>
    /// Whether the time is currently active or not
    /// </summary>
    private static bool timeActive = true;

    /// <summary>
    /// The list of bodies that can be leveled up in the snake
    /// </summary>
    public List<string> LevelableBodies
    {
        get
        {
            List<string> l = new List<string>();

            BodyController body = gameSetup.HeadController.Head;
            while (body != null) 
            { 
                if (body.Levelable)
                {
                    l.Add(body.Name);
                }

                body = body.next;
            }

            return l;
        }
    }

    /// <summary>
    /// The game setup
    /// </summary>
    private IGameSetup gameSetup;

    /// <summary>
    /// Sets the game setup
    /// </summary>
    /// <param name="gameSetup">The game setup</param>
    public void SetGameSetup(IGameSetup gameSetup)
    {
        this.gameSetup = gameSetup;
    }

    /// <summary>
    /// Called when the player levels up
    /// </summary>
    public void OnLevelUp()
    {
        gameSetup.ChoiceManager.StartSet(nextState);
    }

    /// <summary>
    /// Called after the player levels up
    /// </summary>
    public void AfterLevelUp()
    {
        gameSetup.SelectionManager.ShowButtons();
    }

    /// <summary>
    /// Pauses the game time
    /// </summary>
    public void PauseTime()
    {
        if (timeActive)
        {
            timeActive = false;
            Time.timeScale = 0;

            TriggerManager.PauseTimeTrigger.CallTrigger(0);
        }
    }

    /// <summary>
    /// Resumes the game time
    /// </summary>
    public void ResumeTime()
    {
        if (!timeActive)
        {
            timeActive = true;
            Time.timeScale = 1;

            TriggerManager.ResumeTimeTrigger.CallTrigger(0);
        }
    }
}
