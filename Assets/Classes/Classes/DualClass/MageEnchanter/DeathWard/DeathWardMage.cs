using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class DeathWardMage : Mage
{
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/MageEnchanter/DeathWard/DeathWardMage.json";

        primary = false;

        base.ClassSetup();
    }
}
