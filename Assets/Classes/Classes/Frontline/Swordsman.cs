using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Swordsman : Frontline
{
    private float attackRadius = 4f;
    private GameObject AOEEffect;
    private float AOEEffectTime = 0.5f;

    internal override void Setup()
    {
        // gets the AOEEffect ready to be spawned
        AOEEffect = Resources.Load<GameObject>("GameObjects/AOEEffect");

        // sets up the variables for the swordman
        attackDelay = 1f;
        scanRadius = 5f;
        damage = 50;
        force = 50000;
        regularAttack = true;

        // sets up starting variables for the body
        defence = 5;
        maxHealth = 200;
        contactDamage = 25;
        contactForce = 3500;
        velocityContribution = 5f;

        base.Setup();

        // sets the body's colour to a yellow
        spriteRenderer.color = new Color(1f, 1f, 0f);
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
            EnemyControllerBasic enemyController = enemyObj.GetComponent<EnemyControllerBasic>();

            // if the enemy is not dead
            if (!enemyController.dead)
            {
                // apply damage to the enemy
                if (enemyController.ChangeHealth(-damage))
                {
                    // enemy has been killed
                    EnemyKilled(enemyObj);
                }
                else
                {
                    // broken 

                    // gets the difference between the attack location and the affected enemy
                    // Vector3 dif = enemyObj.transform.position - position;
                    // dif.Normalize();

                    // applies a force in the direction from the attack position, based on the force parameter
                    // enemyController.selfRigid.AddForce(dif * force);
                }
            }
        }
    }
}
