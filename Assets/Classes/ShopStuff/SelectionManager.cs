using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : CanvasManager, ISelectionManager
{
    private IGameSetup gameSetup;

    public void SetGameSetup(IGameSetup gameSetup)
    {
        this.gameSetup = gameSetup;
    }

    /// <summary>
    /// Called when the user clicks on a button (only called by unity button)
    /// </summary>
    /// <param name="button">The number corresponding to which button was clicked</param>
    public override void ButtonClicked(int button)
    {
        HideButtons();

        switch (button)
        {
            case 0:
                // new body

                // if no more bodies can be added, ignore the button click
                if (gameSetup.HeadController.Bodies == 8)
                {
                    break;
                }

                gameSetup.ShopManager.NextState = ChoiceState.NewBody;
                break;
            case 1:
                // level up body
                gameSetup.ShopManager.NextState = ChoiceState.BodyUpgrade;
                break;
            case 2:
                // add item

                // if no more items can be added, ignore the button click
                if (ItemManager.items.Count == 8)
                {
                    break;
                }

                gameSetup.ShopManager.NextState = ChoiceState.Item;
                break;
        }
    }

    public override void HideButtons()
    {
        base.HideButtons();

        gameSetup.Reorganiser.Active = false;

        // resumes time
        gameSetup.ShopManager.ResumeTime();
    }

    public override void ShowButtons()
    {
        base.ShowButtons();

        gameSetup.Reorganiser.Active = true;

        // pauses time
        gameSetup.ShopManager.PauseTime();
    }
}
