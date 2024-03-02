using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShopManager
{
    public List<string> Bodies { get; }
    public void RemoveBody(string body);
    public List<string> Items { get; }
    public void RemoveItem(string item);

    public bool Remove { get; }

    public ChoiceState NextState { get; set; }
    public List<string> LevelableBodies { get; }
    public void RemoveLevelableBody(string body);

    public void SetGameSetup(IGameSetup gameSetup);
    public void OnLevelUp();
    public void AfterLevelUp();

    public void PauseTime();
    public void ResumeTime();
}
