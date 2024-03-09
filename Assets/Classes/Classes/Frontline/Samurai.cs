using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The Samurai class, a subclass of the frontline class
/// </summary>
internal class Samurai : Frontline
{
    /// <summary>
    /// The chance of the attack being a critical hit
    /// </summary>
    private float critChance;

    /// <summary>
    /// The multiplier for the damage when the attack is a critical hit
    /// </summary>
    private float critMultiplier;


    /// <summary>
    /// The length of the attack
    /// </summary>
    private float attackLength;


    /// <summary>
    /// The time the AOEEffect should stay on screen for
    /// </summary>
    private float AOEEffectTime;

    /// <summary>
    /// Whether the AOEEffect should decay in colour over time (true) or not (false)
    /// </summary>
    private bool AOEEffectDecay;

    /// <summary>
    /// The initial colour of the AOEEffect
    /// </summary>
    private Color AOEEffectColour;

    /// <summary>
    /// The width of the AOEEffect
    /// </summary>
    private float AOEEffectWidth;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Frontline/Samurai.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called regularly by Frontline based on timeDelay
    /// </summary>
    /// <param name="position">The position which should be attacked</param>
    internal override void Attack(Vector3 position)
    {
        // gets a random number between 0 and 1, if its less than the crit chance, its a crit
        bool critHit = Random.Range(0f, 1f) <= critChance;
        int hitDamage;

        // if its a crit, set the damage higher, otherwise default damage
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

        AOEEffect.CreateRectangle(position, AOEEffectTime, AOEEffectDecay, AOEEffectColour, angle * Mathf.Rad2Deg, attackLength, AOEEffectWidth);

        foreach (RaycastHit2D hit in objectsInLine)
        {
            // gets the hit object's gameObject
            GameObject hitObject = hit.collider.gameObject;

            // if its an enemy
            if (hitObject.tag == "Enemy")
            {
                EnemyController enemyController = hitObject.GetComponent<EnemyController>();

                // if the enemy is not dead
                if (!enemyController.Dead)
                {
                    // deals damage to the enemy
                    if (!enemyController.ChangeHealth(-(int)(hitDamage * body.DamageMultiplier)))
                    {
                        // enemy has been killed
                        EnemyKilled(hitObject);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref critChance, nameof(critChance));
        jsonData.Setup(ref critMultiplier, nameof(critMultiplier));
        jsonData.Setup(ref attackLength, nameof(attackLength));
        jsonData.Setup(ref AOEEffectTime, nameof(AOEEffectTime));
        jsonData.Setup(ref AOEEffectDecay, nameof(AOEEffectDecay));
        jsonData.Setup(ref AOEEffectColour, nameof(AOEEffectColour));
        jsonData.Setup(ref AOEEffectWidth, nameof(AOEEffectWidth));
    }
}
