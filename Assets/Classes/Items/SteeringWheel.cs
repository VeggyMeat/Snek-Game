using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringWheel : Item
{
    private float turningRateMultiplier;

    internal override void Setup()
    {
        jsonPath = "Assets/Resources/Jsons/Items/SteeringWheel.json";

        base.Setup();
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

            Debug.Log(ItemManager.headController.turningRate);
        }
    }
}
