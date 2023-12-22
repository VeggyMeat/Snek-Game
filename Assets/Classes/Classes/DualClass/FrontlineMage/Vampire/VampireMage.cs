using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireMage : Mage
{
    private int healSnakeAmount;

    private int healVampireAmount;
    private int damageBodiesAmount;

    private int suckHealthDelay;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/FrontlineMage/Vampire/VampireMage.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        base.Setup();

        StartSuckingLife();
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref healSnakeAmount, "healSnakeAmount");
        jsonData.Setup(ref healVampireAmount, "healVampireAmount");
        jsonData.Setup(ref damageBodiesAmount, "damageBodiesAmount");

        if (jsonData.ContainsKey("suckHealthDelay")) 
        {
            suckHealthDelay = int.Parse(jsonData["suckHealthDelay"].ToString());

            if (body.Level != 1)
            {
                StopSuckingLife();
                StartSuckingLife();
            }
        }
    }

    internal override int OnDamageTaken(int amount)
    {
        // heal the entire body a certain amount of hp
        BodyController healBody = body.snake.head;

        while (healBody is not null)
        {
            // if the body is not the vampire
            if (healBody.Name != "Vampire")
            {
                // heal it
                if (!healBody.IsDead)
                {
                    healBody.ChangeHealth(healSnakeAmount);
                }
            }

            // get the next body in the snake
            healBody = healBody.next;
        }

        return base.OnDamageTaken(amount);
    }

    private void SuckLife()
    {
        if (body.health < body.MaxHealth / 2)
        {
            // if there is a snake after
            if (body.next is not null)
            {
                if (!body.IsDead)
                {
                    // deal damage to it
                    body.next.ChangeHealth(-damageBodiesAmount);

                    // heal the amount of health
                    body.ChangeHealth(healVampireAmount);
                }
            }

            // if there is a body before this one
            if (body.prev is not null)
            {
                if (!body.IsDead)
                {
                    // deal damage to it
                    body.prev.ChangeHealth(-damageBodiesAmount);

                    // heal the amount of health
                    body.ChangeHealth(healVampireAmount);
                }
            }
        }
    }

    private void StartSuckingLife()
    {
        InvokeRepeating(nameof(SuckLife), suckHealthDelay, suckHealthDelay);
    }

    private void StopSuckingLife()
    {
        CancelInvoke(nameof(SuckLife));
    }

    internal override void Revived()
    {
        base.Revived();

        StartSuckingLife();
    }

    internal override void OnDeath()
    {
        base.OnDeath();

        StopSuckingLife();
    }
}
