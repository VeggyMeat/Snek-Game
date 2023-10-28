using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneFlowerEnchanter : Enchanter
{
    private float damageModifier;
    private float healingModifier;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/MageEnchanter/ArcaneFlower/ArcaneFlowerEnchanter.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        base.Setup();

        body.snake.healingModifier *= healingModifier;
    }

    protected override void AddBuff(GameObject thing)
    {
        BodyController bodyBuff = thing.GetComponent<BodyController>();

        bodyBuff.damageBuff.AddBuff(damageModifier, true, null);

        Debug.Log($"Buffed {bodyBuff.name} with damageModifier {damageModifier} to a new value of {bodyBuff.damageBuff.Value}");
    }

    protected override void RemoveBuff(GameObject thing)
    {
        BodyController bodyBuff = thing.GetComponent<BodyController>();

        bodyBuff.damageBuff.AddBuff(1 / damageModifier, true, null);

        Debug.Log($"Removed Buff {bodyBuff.name} with damageModifier {1 / damageModifier} to a new value of {bodyBuff.damageBuff.Value}");
    }

    internal override void OnDeath()
    {
        base.OnDeath();

        body.snake.healingModifier /= healingModifier;
    }

    internal override void LevelUp()
    {
        base.LevelUp();

        body.snake.healingModifier *= healingModifier;
    }


    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        if (jsonData.ContainsKey(nameof(damageModifier))) 
        {
            if (jsonLoaded)
            {
                UnbuffAllBodies();
            }

            damageModifier = float.Parse(jsonData[nameof(damageModifier)].ToString());

            if (jsonLoaded)
            {
                BuffAllBodies();
            }
        }

        if (jsonData.ContainsKey(nameof(healingModifier)))
        {
            if (jsonLoaded)
            {
                UnbuffAllBodies();
            }

            healingModifier = float.Parse(jsonData[nameof(healingModifier)].ToString());

            if (jsonLoaded)
            {
                BuffAllBodies();
            }
        }
    }
}
