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
        projectile = Resources.Load<GameObject>("BowManProjectile");
        lifeSpan = 5f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void LaunchProjectile()
    {
        // pick a random angle
        float angle = Random.Range(0, Mathf.PI);

        // create the movement vector
        Vector3 movement = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * velocity;

        // create the projectile
        GameObject projectile = Instantiate(base.projectile, transform.position, Quaternion.identity);

        ProjectileController controller = projectile.GetComponent<ProjectileController>();

        controllerList.Add(controller);

        controller.movement = movement + gameObject.GetComponent<BodyController>().lastMoved;

        controller.transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg + 90);

        controller.lifeSpan = lifeSpan;
    }
}
