using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyWithAGunArcher : Archer
{
    internal override void ClassSetup()
    {
        Debug.Log("archer added");

        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/ArcherEnchanter/FairyWithAGun/FairyWithAGunArcher.json";

        primary = false;

        base.ClassSetup();
    }
}
