using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowMan : Archer
{
    List<ProjectileController> controllerList = new List<ProjectileController>();

    public void Setup()
    {
        timeDelay = 1f;
        velocity = 10f;
        projectile = Resources.Load<GameObject>("Projectile1");
        lifeSpan = 5f;
        projectileDamage = 25;
    }

    void Update()
    {

    }

    public override void LaunchProjectile()
    {
        // pick a random angle
        float angle = Random.Range(0, 2 * Mathf.PI);

        // create the movement vector
        Vector2 movement = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * velocity;

        // create the projectile
        GameObject projectile = Instantiate(base.projectile, transform.position, Quaternion.identity);

        // gets the controller of the projectile and adds it to the list
        ProjectileController controller = projectile.GetComponent<ProjectileController>();
        controllerList.Add(controller);
        
        // gives the projectile its properties
        controller.movement = movement + gameObject.GetComponent<BodyController>().lastMoved;
        controller.transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg + 90);
        controller.lifeSpan = lifeSpan;
        controller.damage = projectileDamage;
    }
}
