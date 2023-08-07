using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldmanFrontline : Frontline
{
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/FrontlineEnchanter/Shieldman/ShieldmanFrontline.json";

        primary = false;

        base.ClassSetup();
    }
}
