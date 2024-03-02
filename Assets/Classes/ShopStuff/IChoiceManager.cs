using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChoiceManager
{
    public void SetGameSetup(IGameSetup gameSetup);
    public void StartSet(ChoiceState state);
    public void ButtonClicked(int button);
    public void HideButtons();
    public void ShowButtons();
}
