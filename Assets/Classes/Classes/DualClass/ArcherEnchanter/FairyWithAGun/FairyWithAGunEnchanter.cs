using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class FairyWithAGunEnchanter : Enchanter
{
    private string gunJson;
    private List<Dictionary<string, object>> gunData;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/ArcherEnchanter/FairyWithAGun/FairyWithAGunEnchanter.json";

        base.ClassSetup();
    }

    internal void LoadGunData()
    {
        // loads in the text from the gunJson file
        StreamReader reader = new StreamReader(gunJson);
        string text = reader.ReadToEnd();
        reader.Close();

        // deserializes the json into a list of dictionaries containing the variables' contents for each level
        gunData = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(text);
    }

    protected override void AddBuff(GameObject player)
    {
        // gets the BodyController
        BodyController body = player.GetComponent<BodyController>();

        // gives the body a gun
        Gun gun = player.AddComponent<Gun>();

        float damageModifier = 1f;

        // if the body is an archer
        if (body.classNames.Contains("Archer"))
        {
            damageModifier = 2f;
        }

        // set up the gun
        gun.Setup(body.classes[0], damageModifier, gunData);

        // matches the gun to the current level
        for (int i = 1; i < body.Level; i++)
        {
            gun.UpgradeGun();
        }
    }

    protected override void RemoveBuff(GameObject thing)
    {
        Gun gun;

        // if it doesnt have a gun, raise an error
        if (!thing.TryGetComponent(out gun))
        {
            throw new System.Exception();
        }

        // otherwise destroy the gun
        Destroy(gun);
    }

    internal override void LevelUp()
    {
        base.LevelUp();

        // if its not level one, upgrade the bullets
        if (body.Level != 1)
        {
            // starts at the head
            BodyController gunBody = body.snake.head;

            while (gunBody is not null)
            {
                Gun gun;

                // if it doesnt have a gun, raise an error
                if (!gunBody.TryGetComponent(out gun))
                {
                    throw new System.Exception();
                }

                // upgrade the gun
                gun.UpgradeGun();

                // get the next body in the snake
                gunBody = gunBody.next;
            }
        }
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        if (jsonData.ContainsKey("gunJson"))
        {
            // load the gunJson location in
            gunJson = jsonData["gunJson"].ToString();

            // load the data from the json in
            LoadGunData();
        }

        base.InternalJsonSetup(jsonData);
    }
}
