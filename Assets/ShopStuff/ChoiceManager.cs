using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
        // hides the canvas and clears the state
        HideButtons();

        switch (state)
        {
            case ChoiceState.NewBody:
                headController.AddBody(options[button]);
                break;
            case ChoiceState.BodyUpgrade:
                // levels up a body
                // add when level up system added
                break;
            case ChoiceState.Small_Item:
                // adds an item
                // add when items added
                break;
            case ChoiceState.Powerful_Item:
                // adds a powerful item
                break;
            case ChoiceState.None: 
                throw new Exception();
        }

        state = ChoiceState.None;

        shopManager.AfterLevelUp();
    }

    private void GenerateOptions()
    {
        switch (state)
        {
            case ChoiceState.NewBody:
                options = PickAmount(headController.bodies, optionsNum);
                break;
            case ChoiceState.BodyUpgrade:
                options = PickAmount(headController.CurrentBodies, Math.Min(optionsNum, headController.CurrentBodies.Count));
                break;
            case ChoiceState.Small_Item:
                // to make, when items are added
                break;
            case ChoiceState.Powerful_Item: 
                // to make, when items are added
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
            options.Add(things[index]);
            things.RemoveAt(index);
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
