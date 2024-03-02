using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectionManager
{
    public void SetGameSetup(IGameSetup gameSetup);
    public void ButtonClicked(int button);
    public void HideButtons();
    public void ShowButtons();
}
