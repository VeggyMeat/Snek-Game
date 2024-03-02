using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDeathScreen
{
    public void OnDeath();

    public void NameTyped();

    public void OnMainMenu();

    public void SetGameSetup(IGameSetup gameSetup);
}
