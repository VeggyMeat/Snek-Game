using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Random = UnityEngine.Random;

[assembly: InternalsVisibleTo("EditorTests")]

public class ShopManager : MonoBehaviour, IShopManager
{
    [SerializeField] private List<string> bodies;

    public List<string> Bodies
    {
        get
        {
            return bodies;
        }
    }

    public void RemoveBody(string body)
    {
        bodies.Remove(body);
    }

    [SerializeField] private List<string> items;

    public List<string> Items
    {
        get
        {
            return items;
        }
    }

    public void RemoveItem(string item)
    {
        items.Remove(item);
    }

    private bool remove;

    public bool Remove
    {
        get
        {
            return remove;
        }
    }

    private ChoiceState nextState = ChoiceState.NewBody;

    public ChoiceState NextState
    {
        get
        {
            return nextState;
        }
        set
        {
            nextState = value;
        }
    }

    private static bool timeActive = true;

    private List<string> levelableBodies = new List<string>();

    public List<string> LevelableBodies
    {
        get
        {
            return levelableBodies;
        }
    }

    public void RemoveLevelableBody(string body)
    {
        levelableBodies.Remove(body);
    }

    private IGameSetup gameSetup;

    public void SetGameSetup(IGameSetup gameSetup)
    {
        this.gameSetup = gameSetup;
    }

    private void Start()
    {
        TriggerManager.BodySpawnTrigger.AddTrigger(OnAddedBody);
    }

    public void OnLevelUp()
    {
        gameSetup.ChoiceManager.StartSet(nextState);
    }

    private BodyController OnAddedBody(BodyController body)
    {
        // if it can be leveled up, add it to the list
        if (body.levelable)
        {
            levelableBodies.Add(body.Name);
        }

        return null;
    }

    public void AfterLevelUp()
    {
        gameSetup.SelectionManager.ShowButtons();
    }

    public void PauseTime()
    {
        if (timeActive)
        {
            timeActive = false;
            Time.timeScale = 0;

            TriggerManager.PauseTimeTrigger.CallTrigger(0);
        }
    }

    public void ResumeTime()
    {
        if (!timeActive)
        {
            timeActive = true;
            Time.timeScale = 1;

            TriggerManager.ResumeTimeTrigger.CallTrigger(0);
        }
    }
}
