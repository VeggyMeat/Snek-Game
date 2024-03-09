using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

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

    [SerializeField] private List<string> possibleInitialBodies;

    public List<string> PossibleInitialBodies
    {
        get
        {
            return possibleInitialBodies;
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

    [SerializeField] private bool remove;

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

    public List<string> LevelableBodies
    {
        get
        {
            List<string> l = new List<string>();

            BodyController body = gameSetup.HeadController.Head;
            while (body is not null) 
            { 
                if (body.Levelable)
                {
                    l.Add(body.Name);
                }

                body = body.next;
            }

            return l;
        }
    }

    private IGameSetup gameSetup;

    public void SetGameSetup(IGameSetup gameSetup)
    {
        this.gameSetup = gameSetup;
    }

    public void OnLevelUp()
    {
        gameSetup.ChoiceManager.StartSet(nextState);
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
