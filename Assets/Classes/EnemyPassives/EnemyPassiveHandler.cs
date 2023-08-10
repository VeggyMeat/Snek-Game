using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class EnemyPassiveHandler: MonoBehaviour
{
    public Dictionary<string, int> passiveValues;

    private Dictionary<string, int> defaultValues = new Dictionary<string, int>()
    {
        { "SpeedBuff", 0 },
        { "HealthBuff", 0 },
        { "DamageBuff", 0 },
        { "Invulnerability", 0 },
        { "ExtraLives", 0}
    };

    private EnemyController enemy;

    internal List<EnemyPassive> passives;

    public void Setup(EnemyController enemy)
    {
        passives = new List<EnemyPassive>();

        // creates a copy of the defaultvalues list for the passive values
        passiveValues = new Dictionary<string, int>(defaultValues);

        this.enemy = enemy;
    }

    public void AddPassive(string name, int value)
    {
        passiveValues[name] += value;
    }

    public void AddPassive(string name, int value, float duration)
    {
        AddPassive(name, value);

        // attatches a bomb to remove itself in n seconds, and from the list
        EnemyPassive newPassive = gameObject.AddComponent<EnemyPassive>();
        passives.Add(newPassive);
        newPassive.Setup(this, duration, value, name);

        switch(name)
        {
            case "HealthBuff":
                enemy.health *= (1 + value);
                enemy.MaxHealth *= (1 + value);
                break;
        }
    }

    public void RemovePassive(string name, int value)
    {
        passiveValues[name] -= value;

        switch (name)
        {
            case "HealthBuff":
                enemy.health /= (1 + value);
                enemy.MaxHealth /= (1 + value);
                break;
        }
    }

    public void ClearEffects()
    {
        foreach (EnemyPassive passive in passives)
        {
            passive.OnDestroy();
        }

        passives.Clear();

        passiveValues = new Dictionary<string, int>(defaultValues);
    }

    public bool HasPassive(string name)
    {
        return passiveValues[name] != 0;
    }
}
