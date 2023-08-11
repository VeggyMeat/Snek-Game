using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireFrontline : Frontline
{
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/FrontlineMage/Vampire/VampireFrontline.json";

        primary = false;

        base.ClassSetup();
    }
}
