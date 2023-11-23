using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinceFrontline : Frontline
{
    private float attackRadius;
    private float AOEEffectTime;
    private bool AOEEffectDecay;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/FrontlineEnchanter/Prince/PrinceFrontline.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        base.Setup();
    }

    internal override void Attack(Vector3 position)
    {
        // spawns in the AOEEffect
        AOEEffect.CreateCircle(position, AOEEffectTime, AOEEffectDecay, Color.red, attackRadius);

        // gets all the objects within the range
        Collider2D[] objectsInCircle = Physics2D.OverlapCircleAll(position, attackRadius);

        // gets all of the enemies within the range
        Collider2D[] enemiesInCircle = System.Array.FindAll(objectsInCircle, obj => obj.CompareTag("Enemy"));

        foreach (Collider2D enemy in enemiesInCircle)
        {
            // gets the game object and the script
            GameObject enemyObj = enemy.gameObject;
            EnemyController enemyController = enemyObj.GetComponent<EnemyController>();

            // if the enemy is not dead
            if (!enemyController.Dead)
            {
                // apply damage to the enemy
                if (!enemyController.ChangeHealth(-(int)(damage * body.DamageMultiplier)))
                {
                    // enemy has been killed
                    EnemyKilled(enemyObj);
                }
                else
                {
                    // add a knockback thing
                }
            }
        }
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref attackRadius, "attackRadius");
        jsonData.Setup(ref AOEEffectTime, "AOEEffectTime");
        jsonData.Setup(ref AOEEffectDecay, "AOEEffectDecay");
    }
}
