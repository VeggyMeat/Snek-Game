using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ExplosiveTippedArrows : Item
{
    private float radius;
    private int damage;

    internal override void Setup()
    {
        jsonPath = "Assets/Resources/Jsons/Items/ExplosiveTippedArrows.json";

        base.Setup();

        TriggerManager.ProjectileHitTrigger.AddTrigger(OnHit);
    }

    private GameObject OnHit(GameObject projectileObject)
    {
        // gets all the objects within the range
        Collider2D[] objectsInCircle = Physics2D.OverlapCircleAll(projectileObject.transform.position, radius);

        // gets all of the enemies within the range
        Collider2D[] enemiesInCircle = System.Array.FindAll(objectsInCircle, obj => obj.CompareTag("Enemy"));

        foreach (Collider2D collider in enemiesInCircle)
        {
            EnemyController enemyController = collider.GetComponent<EnemyController>();

            if (!enemyController.Dead)
            {
                if (!enemyController.ChangeHealth(-damage))
                {
                    ItemManager.headController.IncreaseXP(enemyController.XPDrop);
                }
            }
        }

        return projectileObject;
    }

    protected override void JsonSetup()
    {
        base.JsonSetup();

        jsonVariables.Setup(ref radius, nameof(radius));
        jsonVariables.Setup(ref damage, nameof(damage));
    }
}
