using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFieldMageFrontline : Frontline
{
    private float attackRadius;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/FrontlineMage/ForceFieldMage/ForceFieldMageFrontline.json";

        base.ClassSetup();
    }

    internal override void Attack(Vector3 position)
    {
        // gets all the objects within the range
        Collider2D[] objectsInCircle = Physics2D.OverlapCircleAll(transform.position, attackRadius);

        // gets all of the enemies within the range
        Collider2D[] enemiesInCircle = System.Array.FindAll(objectsInCircle, obj => obj.CompareTag("Enemy"));

        foreach (Collider2D collider in enemiesInCircle)
        {
            EnemyController enemy = collider.gameObject.GetComponent<EnemyController>();

            if (!enemy.ChangeHealth(-damage))
            {
                EnemyKilled(enemy.gameObject);
            }
        }
    }

    internal override void EnemyKilled(GameObject enemy)
    {
        ((ForceFieldMageMage)body.classes[1]).BuffBody();

        base.EnemyKilled(enemy);
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref attackRadius, nameof(attackRadius));
    }
}
