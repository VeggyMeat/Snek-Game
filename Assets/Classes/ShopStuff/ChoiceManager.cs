using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class ChoiceManager : CanvasManager, IChoiceManager
{
    // this is attatched to the OptionChoices canvas
    // this is the canvas that shows the options for the player to choose from

    private ChoiceState state = ChoiceState.None;
    private List<string> options;

    private IGameSetup gameSetup;

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

    public override void ButtonClicked(int button)
    {
        if (button != -1)
        {
            if (options[button] == "None")
            {
                return;
            }
        }

        // hides the canvas and clears the state
        HideButtons();

        // -1 is the skip button and so ignores the adding stuff
        if (button != -1)
        {
            switch (state)
            {
                case ChoiceState.NewBody:
                    gameSetup.HeadController.AddBody(options[button]);
                    break;

                case ChoiceState.BodyUpgrade:
                    // levels up a body

                    // gets the body
                    BodyController body = gameSetup.HeadController.Head;
                    while (body.Name != options[button])
                    {
                        body = body.next;
                    }

                    // levels up the body
                    body.LevelUp();

                    // if the body is no longer levelable, remove it from the list
                    if (!body.levelable)
                    {
                        gameSetup.ShopManager.RemoveLevelableBody(body.Name);
                    }
                    break;

                case ChoiceState.Small_Item:
                    // adds an item
                    ItemManager.AddItem(options[button]);
                    break;

                case ChoiceState.Powerful_Item:
                    // adds a powerful item
                    ItemManager.AddItem(options[button]);
                    break;

                case ChoiceState.None:
                    throw new Exception();
            }
        }

        state = ChoiceState.None;

        gameSetup.ShopManager.AfterLevelUp();
    }

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
            case ChoiceState.Powerful_Item:
                options = PickAmount(gameSetup.ShopManager.Items, optionsNum);
                break;
            case ChoiceState.None:
                throw new Exception();
        }
    }

    private List<string> PickAmount(List<string> from, int number)
    {
        List<string> things = new List<string>(from);
        options = new List<string>();

        for (int i = 0; i < number; i++)
        {
            int index = UnityEngine.Random.Range(0, things.Count);
            if (things.Count != 0)
            {
                options.Add(things[index]);
                things.RemoveAt(index);
            }
            else
            {
                options.Add("None");
            }
        }

        return options;
    }

    public override void HideButtons()
    {
        base.HideButtons();

        // resumes time
        gameSetup.ShopManager.ResumeTime();
    }

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
