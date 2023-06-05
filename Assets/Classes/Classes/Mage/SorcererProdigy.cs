using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SorcererProdigy : Mage
{
    public int damage;
    public float beamThickness;

    internal string jsonPath = "Assets/Resources/Jsons/Classes/Mage/SorcererProdigy.json";

    internal override void Setup()
    {
        // sets up the json data into the class
        JsonSetup(jsonPath);

        base.Setup();
    }

    internal override void Attack()
    {
        // gets a random angle
        float angle = Random.Range(0, Mathf.PI * 2);
        Vector2 angleVector = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        // gets objects hit by the 3 rays
        List<RaycastHit2D> objectsHit = Physics2D.RaycastAll((Vector2)transform.position + Vector2.Perpendicular(angleVector) * beamThickness / 2, angleVector).ToList();
        objectsHit.AddRange(Physics2D.RaycastAll((Vector2)transform.position + Vector2.Perpendicular(angleVector) * -beamThickness / 2, angleVector));
        objectsHit.AddRange(Physics2D.RaycastAll((Vector2)transform.position, angleVector));

        foreach (RaycastHit2D hit in objectsHit)
        {
            // grabs the gameobject that was hit
            GameObject objectHit = hit.collider.gameObject;

            // filters out the enemies
            if (objectHit.tag == "Enemy")
            {
                EnemyController enemy = objectHit.GetComponent<EnemyController>();

                // if the enemy is alive, deal damage to it
                if (!enemy.dead)
                {
                    // if the enemy dies trigger it
                    if (!enemy.ChangeHealth(-damage))
                    {
                        EnemyKilled(objectHit);
                    }
                }
            }
        }
    }
}
