using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeLauncherFrontline : Frontline
{
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/FrontlineArcher/SpikeLauncher/SpikeLauncherFrontline.json";

        primary = false;

        base.ClassSetup();
    }

    internal override int OnDamageTaken(int amount)
    {
        // launches spikes
        ((SpikeLauncherArcher)body.classes[0]).LaunchSpikes();

        return base.OnDamageTaken(amount);
    }
}
