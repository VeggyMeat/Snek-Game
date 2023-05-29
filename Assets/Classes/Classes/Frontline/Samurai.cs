using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class Samurai : Frontline
{
    // todo: make the samurai slash show on the screen with some visual effect

    public float critChance;
    public float critMultiplier;

    public float attackLength;

    internal string jsonPath = "Assets/Resources/Jsons/Classes/Frontline/Samurai.json";

    // Start is called before the first frame update
    internal override void Setup()
    {
        // sets up the json data into the class
        JsonSetup(jsonPath);

        base.Setup();
    }

    internal override void Attack(Vector3 position)
    {
        bool critHit = Random.Range(0, 1) < critChance;
        int hitDamage;

        if (critHit)
        {
            hitDamage = (int)(critMultiplier * damage);
        }
        else
        {
            hitDamage = damage;
        }

        // gets a random direction for the slice
        float angle = Random.Range(0, Mathf.PI * 2);

        // gets the two positions of the slice
        Vector2 point1 = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * attackLength + (Vector2)position;
        Vector2 point2 = 2 * (Vector2)position - point1;

        // gets all the objects within the range
        RaycastHit2D[] objectsInLine = Physics2D.LinecastAll(point1, point2);

        // draws a line showing the attack
        Debug.DrawLine(point1, point2, Color.white, 0.5f, false);

        foreach (RaycastHit2D hit in objectsInLine)
        {
            // gets the hit object's gameObject
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.tag == "Enemy")
            {
                // hit an enemy
                EnemyController enemyController = hitObject.GetComponent<EnemyController>();
                if (!enemyController.dead)
                {
                    if (!enemyController.ChangeHealth(-hitDamage))
                    {
                        // enemy has been killed
                        EnemyKilled(hitObject);
                    }
                }
            }
        }
    }
}
