using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pyromaniac : Enchanter
{
    private int burnDamage;
    private int burnStacks;
    private int burnDelay;

    private readonly Dictionary<int, List<EnemyController>> enemies = new Dictionary<int, List<EnemyController>>();

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Enchanter/Pyromaniac.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        base.Setup();
    }

    private void BurnBodies()
    {

    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);
    }
}
