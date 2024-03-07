using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// COMPLETE

/// <summary>
/// The spike launcher archer class, a subclass of the archer class
/// </summary>
internal class SpikeLauncherArcher : Archer
{
    /// <summary>
    /// The number of spikes to release
    /// </summary>
    private int spikes;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/FrontlineArcher/SpikeLauncher/SpikeLauncherArcher.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called by the Spike Launcher Frontline class to launch the spikes
    /// </summary>
    internal void LaunchSpikes()
    {
        // gets a random angle
        float angle = Random.Range(0, 2 * Mathf.PI);

        // gets the angle that should be between each spike released
        float dif = (Mathf.PI * 2) / spikes;

        // releases the spikes with the set angle between them
        for (int i = 0; i < spikes; i++)
        {
            Projectile.Shoot(projectile, transform.position, angle + dif * i, projectileVariables.Variables, this, body.DamageMultiplier);
        }
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref spikes, nameof(spikes));
    }
}
