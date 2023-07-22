using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class Projectile 
{
    public static ProjectileController Shoot(GameObject projectile, Vector3 position, float angle, Dictionary<string, object> jsonVariables, Class controller, float damageMultiplier)
    {
        // create the projectile
        GameObject proj = Object.Instantiate(projectile, position, Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg));

        // grabs the projectile controller
        ProjectileController projController = proj.GetComponent<ProjectileController>();

        // sets up the projectile controller
        projController.Setup(jsonVariables, controller, damageMultiplier);

        return projController;
    }
}
