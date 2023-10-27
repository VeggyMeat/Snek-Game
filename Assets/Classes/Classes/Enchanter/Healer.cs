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

        StartHealing();
    }

    private void HealRandomAlly()
    {
        List<BodyController> healable = new List<BodyController>();

        BodyController currentBody = body.snake.head;
        while (currentBody is not null)
        {
            if (currentBody.health < currentBody.MaxHealth)
            {
                if (!currentBody.IsDead)
                {
                    healable.Add(currentBody);
                }
            }

            currentBody = currentBody.next;
        }

        if (healable.Count != 0)
        {
            HealAlly(healable.RandomItem());
        }
    }

    private void HealAlly(BodyController healBody)
    {
        healBody.ChangeHealth(healthIncrease);
    }

    private void StartHealing()
    {
        if (healing)
        {
            return;
        }

        InvokeRepeating(nameof(HealRandomAlly), timeDelay / body.attackSpeedBuff.Value, timeDelay / body.attackSpeedBuff.Value);

        healing = true;
    }

    private void StopHealing()
    {
        if (!healing)
        {
            return;
        }

        CancelInvoke(nameof(HealRandomAlly));

        healing = false;
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        if (jsonData.ContainsKey(nameof(timeDelay)))
        {
            timeDelay = int.Parse(jsonData[nameof(timeDelay)].ToString());

            if (jsonLoaded)
            {
                StopHealing();
                StartHealing();
            }
        }

        jsonData.Setup(ref healthIncrease, nameof(healthIncrease));
    }

    internal override void OnDeath()
    {
        base.OnDeath();

        StopHealing();
    }

    internal override void Revived()
    {
        base.Revived();

        StartHealing();
    }

    internal override void OnAttackSpeedBuffUpdate(float amount, bool multiplicative)
    {
        base.OnAttackSpeedBuffUpdate(amount, multiplicative);

        StopHealing();
        StartHealing();
    }
}
