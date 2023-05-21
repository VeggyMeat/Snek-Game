using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.IO;
using UnityEngine;

public class Swordsman : Frontline
{
    public float attackRadius = 4f;
    public float AOEEffectTime = 0.5f;

    public string AOEEffectPath;

    internal string jsonPath = "Assets/Resources/Jsons/Classes/Frontline/Swordsman.json";

    private GameObject AOEEffect;

    internal override void Setup()
    {
        // loads in all the variables from the json
        StreamReader reader = new StreamReader(jsonPath);
        string text = reader.ReadToEnd();
        reader.Close();

        JsonUtility.FromJsonOverwrite(text, this);

        // gets the AOEEffect ready to be spawned
        AOEEffect = Resources.Load<GameObject>(AOEEffectPath);

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
