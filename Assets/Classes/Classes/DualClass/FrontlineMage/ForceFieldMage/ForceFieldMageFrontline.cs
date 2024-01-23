using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFieldMageFrontline : Frontline
{
    private float attackRadius;

    private float AOEEffectTime;
    private bool AOEEffectDecay;
    private Color AOEEffectColour;

    private float attackForce;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/FrontlineMage/ForceFieldMage/ForceFieldMageFrontline.json";

        base.ClassSetup();
    }

    internal override void Attack(Vector3 position)
    {
        // creates the AOE effect
        AOEEffect.CreateCircle(transform.position, AOEEffectTime, AOEEffectDecay, AOEEffectColour, attackRadius);

        // gets all the objects within the range
        Collider2D[] objectsInCircle = Physics2D.OverlapCircleAll(transform.position, attackRadius);

        // gets all of the enemies within the range
        Collider2D[] enemiesInCircle = System.Array.FindAll(objectsInCircle, obj => obj.CompareTag("Enemy"));

        foreach (Collider2D collider in enemiesInCircle)
        {
            EnemyController enemyController = collider.gameObject.GetComponent<EnemyController>();

            if (!enemyController.Dead)
            {
                if (!enemyController.ChangeHealth(-damage))
                {
                    EnemyKilled(enemyController.gameObject);
                }
                else
                {
                    enemyController.selfRigid.AddForce((collider.transform.position - transform.position).normalized * attackForce);
                }
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
        jsonData.Setup(ref AOEEffectTime, nameof(AOEEffectTime));
        jsonData.Setup(ref AOEEffectDecay, nameof(AOEEffectDecay));
        jsonData.Setup(ref AOEEffectColour, nameof(AOEEffectColour));
        jsonData.Setup(ref attackForce, nameof(attackForce));
    }
}
