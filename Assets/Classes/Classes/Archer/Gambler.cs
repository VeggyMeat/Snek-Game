using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Gambler : Archer
{
    internal string jsonPath = "Assets/Resources/Jsons/Classes/Archer/Gambler.json";

    public string projectilePath;
    public string projectileJson;

    public float minRadius;
    public float maxRadius;

    public float minDamage;
    public float maxDamage;

    // projectile damage unused

    internal override void Setup()
    {
        // loads in all the variables from the json
        StreamReader reader = new StreamReader(jsonPath);
        string text = reader.ReadToEnd();
        reader.Close();

        JsonUtility.FromJsonOverwrite(text, this);

        // grabs the projectile from resources
        projectile = Resources.Load<GameObject>(projectilePath);

        // calls the archer's setup
        base.Setup();
    }

    // called regularly by archer
    internal override void LaunchProjectile()
    {
        // pick a random angle
        float angle = Random.Range(0, 2 * Mathf.PI);

        // picks a random size
        float radius = Random.Range(minRadius, maxRadius);
        float damage = Map(minRadius, maxRadius, minDamage, maxDamage, radius);

        // create the projectile
        GameObject projectile = Instantiate(base.projectile, transform.position, Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg + 90));

        // grabs the projectile controller
        BouncingProjectileController controller = projectile.GetComponent<BouncingProjectileController>();

        // sets the damage of the projectile
        controller.damage = (int)damage;

        // sets the size of the projectile
        projectile.transform.localScale = new Vector3(radius, radius, 1);

        // sets up the projectile controller
        controller.Setup(projectileJson, this);
    }

    // maps one set of values to another
    public float Map(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

}
