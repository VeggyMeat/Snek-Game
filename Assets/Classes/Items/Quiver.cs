using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiver : Item
{
    private float attackSpeedModifier;

    private int archerKillsLevelUp;
    private int archerKills = 0;

    internal override void Setup(IGameSetup gameSetup)
    {
        jsonPath = "Assets/Resources/Jsons/Items/Quiver.json";

        base.Setup(gameSetup);

        TriggerManager.BodyKilledTrigger.AddTrigger(OnBodyKilled);

        // when a new body is added calls the AddArcherBuff function on it
        TriggerManager.BodySpawnTrigger.AddTrigger(AddArcherBuff);

        // buffs all the current archers
        BuffArchers();
    }

    // adds the buff to a body controller if its an archer
    private BodyController AddArcherBuff(BodyController body)
    {
        // if its an archer
        if (body.classNames.Contains(nameof(Archer)))
        {
            // add the buff
            body.attackSpeedBuff.AddBuff(attackSpeedModifier, true, null);
        }

        return body;
    }

    // goes through and buffs each archer
    private void BuffArchers()
    {
        // gets the head of the snake
        BodyController body = gameSetup.HeadController.Head;

        while (body is not null)
        {
            // tries to buff the body if its an archer
            AddArcherBuff(body);

            // gets the next body in the snake
            body = body.next;
        }
    }

    private GameObject OnBodyKilled(GameObject body)
    {
        // gets the bodyController from the body
        BodyController bodyClass = body.GetComponent<BodyController>();

        if (bodyClass.classNames.Contains(nameof(Archer)))
        {
            // its an archer, so add to the archer kills
            archerKills++;
        }

        // if the archer kills is greater than the level up amount
        if (archerKills >= archerKillsLevelUp && Levelable)
        {
            // level up
            LevelUp();
        }

        return body;
    }

    protected override void JsonSetup()
    {
        base.JsonSetup();

        jsonVariables.Setup(ref attackSpeedModifier, nameof(attackSpeedModifier));
        jsonVariables.Setup(ref archerKillsLevelUp, nameof(archerKillsLevelUp));
    }

    protected override void LevelUp()
    {
        if (jsonLoaded)
        {
            // resets the archer kills
            archerKills -= archerKillsLevelUp;
        }

        base.LevelUp();
    }
}
