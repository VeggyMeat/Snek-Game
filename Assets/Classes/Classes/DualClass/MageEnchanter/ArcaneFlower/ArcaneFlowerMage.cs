using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class ArcaneFlowerMage : Mage
{
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/MageEnchanter/ArcaneFlower/ArcaneFlowerMage.json";

        primary = false;

        base.ClassSetup();
    }
}
