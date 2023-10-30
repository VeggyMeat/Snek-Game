using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannoneerArcher : Archer
{
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/ArcherEnchanter/Cannoneer/CannoneerArcher.json";

        primary = false;

        base.ClassSetup();
    }
}
