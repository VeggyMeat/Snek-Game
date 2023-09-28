using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class ChoiceManager : CanvasManager
{
    // this is attatched to the OptionChoices canvas
    // this is the canvas that shows the options for the player to choose from

    private ChoiceState state = ChoiceState.None;
    private List<string> options;

    public ShopManager shopManager;

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
                    headController.AddBody(options[button]);
                    break;

                case ChoiceState.BodyUpgrade:
                    // levels up a body

                    // gets the body
                    BodyController body = headController.head;
                    while (body.Name != options[button])
                    {
                        body = body.next;
                    }

                    // levels up the body
                    body.LevelUp();

                    // if the body is no longer levelable, remove it from the list
                    if (!body.levelable)
                    {
                        shopManager.levelableBodies.Remove(body.Name);
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

        shopManager.AfterLevelUp();
    }

    private void GenerateOptions()
    {
        switch (state)
        {
            case ChoiceState.NewBody:
                options = PickAmount(shopManager.bodies, optionsNum);
                break;
            case ChoiceState.BodyUpgrade:
                options = PickAmount(shopManager.levelableBodies, optionsNum);
                break;
            case ChoiceState.Small_Item:
                options = PickAmount(shopManager.smallItems, optionsNum);
                break;
            case ChoiceState.Powerful_Item:
                options = PickAmount(shopManager.powerfulItems, optionsNum);
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
        shopManager.ResumeTime();
    }

    public override void ShowButtons()
    {
        base.ShowButtons();

        // pauses time
        shopManager.PauseTime();
    }

    private void Update()
    {
        if (state != ChoiceState.None)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // refresh shop options
                GenerateOptions();
                UpdateOptions();
            }
        }
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
