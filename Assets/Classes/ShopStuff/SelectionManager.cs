using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : CanvasManager
{
    public ShopManager shopManager;
    public Reorganiser reorganiser;

    public override void ButtonClicked(int button)
    {
        HideButtons();

        switch (button)
        {
            case 0:
                // new body
                shopManager.nextState = ChoiceState.NewBody;
                break;
            case 1:
                // level up body
                shopManager.nextState = ChoiceState.BodyUpgrade;
                break;
            case 2:
                // add small item
                shopManager.nextState = ChoiceState.Small_Item;
                break;
            case 3:
                // add powerful item
                shopManager.nextState = ChoiceState.Powerful_Item;
                break;
        }
    }

    public override void HideButtons()
    {
        base.HideButtons();

        reorganiser.Active = false;

        // resumes time
        shopManager.ResumeTime();
    }

    public override void ShowButtons()
    {
        base.ShowButtons();

        reorganiser.Active = true;

        // pauses time
        shopManager.PauseTime();
    }
}
