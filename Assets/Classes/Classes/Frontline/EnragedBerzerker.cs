using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnragedBerzerker : Frontline
{
    private float attackRadius;
    private float AOEEffectTime;

    private string AOEEffectPath;

    private GameObject AOEEffect;

    private float attackSpeedBuff;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Frontline/EnragedBerzerker.json";

        base.ClassSetup();
    }

    internal override int OnDamageTaken(int amount)
    {
        // adds the buff to the body
        body.attackSpeedBuff.AddBuff(attackSpeedBuff, false, null);

        // returns the base effect on the orginal damage
        return base.OnDamageTaken(amount);
    }

    internal override void Setup()
    {
        base.Setup();
    }

    internal override void Attack(Vector3 position)
    {
        // spawns in the AOEEffect
        GameObject AOEEffectInstance = Instantiate(AOEEffect, position, Quaternion.identity);
        AOEEffectInstance.GetComponent<AOEEffectController>().Setup(AOEEffectTime, attackRadius);

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
                    // todo: add a knockback thing
                }
            }
        }
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref attackRadius, nameof(attackRadius));
        jsonData.Setup(ref AOEEffectTime, nameof(AOEEffectTime));
        jsonData.Setup(ref attackSpeedBuff, nameof(attackSpeedBuff));
        jsonData.SetupAction(ref AOEEffectPath, nameof(AOEEffectPath), null, () => AOEEffect = Resources.Load<GameObject>(AOEEffectPath), true);
    }
}
