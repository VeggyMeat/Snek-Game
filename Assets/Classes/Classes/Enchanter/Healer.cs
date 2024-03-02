using System.Collections.Generic;
using UnityEngine;

public class Healer : Enchanter
{
    private int timeDelay;

    private int healthIncrease;

    private bool healing = false;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Enchanter/Healer.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        base.Setup();

        // starts healing bodies
        StartHealing();
    }

    private void HealRandomAlly()
    {
        // creates a list of healable bodies
        List<BodyController> healable = new List<BodyController>();

        // goes through each body in the snake and checks if its healable
        BodyController currentBody = body.snake.Head;
        while (currentBody is not null)
        {
            if (currentBody.health < currentBody.MaxHealth)
            {
                if (!currentBody.IsDead)
                {
                    healable.Add(currentBody);
                }
            }

            // gets the next body
            currentBody = currentBody.next;
        }

        // if something is healable, heal a random body that is healable
        if (healable.Count != 0)
        {
            HealAlly(healable.RandomItem());
        }
    }

    private void HealAlly(BodyController healBody)
    {
        // increase the body's health by healthIncrease
        healBody.ChangeHealth(healthIncrease);
    }

    private void StartHealing()
    {
        // if already healing, ignore
        if (healing)
        {
            return;
        }

        // regularly calls HealRandomAlly
        InvokeRepeating(nameof(HealRandomAlly), timeDelay / body.attackSpeedBuff.Value, timeDelay / body.attackSpeedBuff.Value);

        healing = true;
    }

    private void StopHealing()
    {
        // if not healing already, ignore
        if (!healing)
        {
            return;
        }

        // stops regularly calling HealRandomAlly
        CancelInvoke(nameof(HealRandomAlly));

        healing = false;
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.SetupAction(ref timeDelay, nameof(timeDelay), UnbuffAllBodies, BuffAllBodies, jsonLoaded);
        jsonData.Setup(ref healthIncrease, nameof(healthIncrease));
    }

    internal override void OnDeath()
    {
        base.OnDeath();

        // stops regularly healing bodies
        StopHealing();
    }

    internal override void Revived()
    {
        base.Revived();

        // continues regularly healing bodies
        StartHealing();
    }

    internal override void OnAttackSpeedBuffUpdate(float amount, bool multiplicative)
    {
        base.OnAttackSpeedBuffUpdate(amount, multiplicative);

        // restarts the healing process
        StopHealing();
        StartHealing();
    }
}
