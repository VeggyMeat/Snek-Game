using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringWheel : Item
{
    private float turningRateMultiplier;

    private float timeTurning = 0;
    private float timeTurningLevelUp;

    private DateTime startedTurning;

    internal override void Setup()
    {
        jsonPath = "Assets/Resources/Jsons/Items/SteeringWheel.json";

        startedTurning = DateTime.Now;

        base.Setup();

        TriggerManager.StopTurningTrigger.AddTrigger(OnTurnStop);
        TriggerManager.StartTurningTrigger.AddTrigger(OnTurnStart);
    }

    protected override void JsonSetup()
    {
        base.JsonSetup();

        if (jsonVariables.ContainsKey("turningRateMultiplier"))
        {
            // if this is a level upgrade, rather than a load
            if (level > 1)
            {
                // remove the current multiplier from the snake
                ItemManager.headController.turningRate *= 1 / turningRateMultiplier;
            }

            // load the new multiplier in
            turningRateMultiplier = float.Parse(jsonVariables["turningRateMultiplier"].ToString());

            // add the new multiplier to the snake
            ItemManager.headController.turningRate *= turningRateMultiplier;
        }

        jsonVariables.Setup(ref timeTurningLevelUp, nameof(timeTurningLevelUp));
    }

    protected override void LevelUp()
    {
        if (jsonLoaded)
        {
            timeTurning -= timeTurningLevelUp;
        }

        base.LevelUp();
    }

    private int OnTurnStart(int value = 0)
    {
        startedTurning = DateTime.Now;

        return value;
    }

    private int OnTurnStop(int value = 0)
    {
        timeTurning += (float)TimeManager.GetElapsedTimeSince(startedTurning).TotalSeconds;

        if (timeTurning >= timeTurningLevelUp && Levelable)
        {
            LevelUp();
        }

        return value;
    }
}
