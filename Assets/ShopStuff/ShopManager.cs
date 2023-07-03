using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Random = UnityEngine.Random;

public class ShopManager : MonoBehaviour
{
    public HeadController head;
    public ChoiceManager choiceManager;
    public SelectionManager selectionManager;

    public List<string> bodies;
    public List<string> smallItems;
    public List<string> powerfulItems;

    internal ChoiceState nextState = ChoiceState.NewBody;

    private static bool timeActive = true;

    internal List<string> levelableBodies = new List<string>();

    public void Start()
    {
        TriggerManager.BodySpawnTrigger.AddTrigger(OnAddedBody);
    }

    public void OnLevelUp()
    {
        choiceManager.StartSet(nextState);
    }

    private BodyController OnAddedBody(BodyController body)
    {
        // if it can be leveled up, add it to the list
        if (body.levelable)
        {
            levelableBodies.Add(body.classes[0].name);
        }

        return null;
    }

    public void AfterLevelUp()
    {
        selectionManager.ShowButtons();
    }

    public void PauseTime()
    {
        if (timeActive)
        {
            timeActive = false;
            Time.timeScale = 0;
        }
    }

    public void ResumeTime()
    {
        if (!timeActive)
        {
            timeActive = true;
            Time.timeScale = 1;
        }
    }
}
