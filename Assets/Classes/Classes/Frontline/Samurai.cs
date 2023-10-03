using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class Samurai : Frontline
{
    // todo: make the samurai slash show on the screen with some visual effect

    private float critChance;
    private float critMultiplier;

    private float attackLength;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Frontline/Samurai.json";

        base.ClassSetup();
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

        // TODO draws a line showing the attack
        // temp solution (only works in scene view)
        Debug.DrawLine(point1, point2, Color.white, 0.5f, false);

        foreach (RaycastHit2D hit in objectsInLine)
        {
            // gets the hit object's gameObject
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.tag == "Enemy")
            {
                // hit an enemy
                EnemyController enemyController = hitObject.GetComponent<EnemyController>();
                if (!enemyController.Dead)
                {
                    if (!enemyController.ChangeHealth(-(int)(hitDamage * body.DamageMultiplier)))
                    {
                        // enemy has been killed
                        EnemyKilled(hitObject);
                    }
                }
            }
        }
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref critChance, nameof(critChance));
        jsonData.Setup(ref critMultiplier, nameof(critMultiplier));
        jsonData.Setup(ref attackLength, nameof(attackLength));
    }
}
