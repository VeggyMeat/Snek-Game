using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// COMPLETE

/// <summary>
/// Placed on the game object to manage the choices menu for the player
/// /summary>
public class ChoiceManager : CanvasManager, IChoiceManager
{
    /// <summary>
    /// The current state of the choice manager
    /// </summary>
    private ChoiceState state = ChoiceState.None;

    /// <summary>
    /// The options that the player can choose from (that are shown as buttons on screen)
    /// </summary>
    private List<string> options;

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
    /// Sets the state of the choice manager and creates the choices, freezing time
    /// </summary>
    /// <param name="state">The new state set</param>
    public void StartSet(ChoiceState state)
    {
        if (this.state == ChoiceState.None)
        {
            this.state = state;

            GenerateOptions();

            UpdateOptions();

            ShowButtons();
        }
    }

    /// <summary>
    /// Called when a button is clicked
    /// </summary>
    /// <param name="button">The number to indicate which button</param>
    /// <exception cref="Exception">Called if this is called when there is no ChoiceState selected</exception>
    public override void ButtonClicked(int button)
    {
        // skip button is -1
        if (button != -1)
        {
            // if the None button is clicked, do nothing
            if (options[button] == "None")
            {
                return;
            }
        }

        // -1 is the skip button and so ignores the adding stuff
        if (button != -1)
        {
            switch (state)
            {
                case ChoiceState.NewBody:
                    gameSetup.HeadController.AddBody(options[button]);
                    break;

                case ChoiceState.BodyUpgrade:
                    BodyController body = gameSetup.HeadController.Head;
                    // finds the body that matches the name of the button
                    while (body.Name != options[button])
                    {
                        body = body.next;
                    }

                    body.LevelUp();

                    break;

                case ChoiceState.Item:
                    ItemManager.AddItem(options[button]);

                    break;

                case ChoiceState.None:
                    throw new Exception();
            }
        }

        // if the button is skip but the player  has no bodies yet, then the player must choose a body
        if (button != -1 || gameSetup.HeadController.Head != null)
        {
            HideButtons();

            state = ChoiceState.None;

            gameSetup.ShopManager.AfterLevelUp();
        }
    }

    /// <summary>
    /// Generates the options for the player to choose from
    /// </summary>
    /// <exception cref="Exception">Called if this is called when ChoiceState is None</exception>
    private void GenerateOptions()
    {
        switch (state)
        {
            case ChoiceState.NewBody:
                // if there is no head on the snake, this is the first body so use the possible initial bodies
                if (gameSetup.HeadController.Head is null)
                {
                    options = PickAmount(gameSetup.ShopManager.PossibleInitialBodies, optionsNum);
                }

                // if there is a head on the snake, use the normal list of bodies
                else
                {
                    options = PickAmount(gameSetup.ShopManager.Bodies, optionsNum);
                }                
                break;
            case ChoiceState.BodyUpgrade:
                options = PickAmount(gameSetup.ShopManager.LevelableBodies, optionsNum);
                break;
            case ChoiceState.Item:
                options = PickAmount(gameSetup.ShopManager.Items, optionsNum);
                break;
            case ChoiceState.None:
                throw new Exception();
        }
    }

    /// <summary>
    /// Picks a number of items from a list
    /// </summary>
    /// <param name="from">The list to pick from</param>
    /// <param name="number">The number of items to pick</param>
    /// <returns></returns>
    private List<string> PickAmount(List<string> from, int number)
    {
        List<string> fromCopy = new List<string>(from);
        options = new List<string>();

        // picks a random item from the list and adds it to the options list, then removes it from the fromCopy list
        for (int i = 0; i < number; i++)
        {
            int index = UnityEngine.Random.Range(0, fromCopy.Count);
            if (fromCopy.Count != 0)
            {
                options.Add(fromCopy[index]);
                fromCopy.RemoveAt(index);
            }
            else
            {
                options.Add("None");
            }
        }

        return options;
    }

    /// <summary>
    /// Called to hide the canvas
    /// </summary>
    public override void HideButtons()
    {
        base.HideButtons();

        // resumes time
        gameSetup.ShopManager.ResumeTime();
    }

    /// <summary>
    /// Called to show the canvas
    /// </summary>
    public override void ShowButtons()
    {
        base.ShowButtons();

        // pauses time
        gameSetup.ShopManager.PauseTime();
    }

    /// <summary>
    /// Updates the text on the buttons to match the options
    /// </summary>
    private void UpdateOptions()
    {
        for (int i = 0; i < optionsNum; i++) 
        {
            string option = options[i];

            // updates the text for that button
            SetButtonText(i, option);
        }
    }
}
