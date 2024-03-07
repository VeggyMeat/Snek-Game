using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

internal class SpikeLauncherArcher : Archer
{
    private int spikes;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/FrontlineArcher/SpikeLauncher/SpikeLauncherArcher.json";

        base.ClassSetup();
    }

    internal void LaunchSpikes()
    {
        float add = Random.Range(0, 2 * Mathf.PI);
        float dif = (Mathf.PI * 2) / spikes;
        for (int i = 0; i < spikes; i++)
        {
            Projectile.Shoot(projectile, transform.position, add + dif * i, projectileVariables.Variables, this, body.DamageMultiplier);
        }
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref spikes, nameof(spikes));
    }
}
