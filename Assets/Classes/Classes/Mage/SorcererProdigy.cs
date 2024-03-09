using System;
using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The sorcerer prodigy class, a subclass of the mage class
/// </summary>
internal class SorcererProdigy : Mage
{
    /// <summary>
    /// The damage done by the attack on the enemies
    /// </summary>
    private int damage;

    /// <summary>
    /// The thickness of the beam fired by the attack
    /// </summary>
    private float beamThickness;

    /// <summary>
    /// The length of the beam fired by the attack
    /// </summary>
    private float beamLength;

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
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Mage/SorcererProdigy.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called regularly by the mage based on timeDelay
    /// </summary>
    protected override void Attack()
    {
        // gets a random angle
        float angle = UnityEngine.Random.Range(0, Mathf.PI * 2) * Mathf.Rad2Deg;

        // Get the direction of the beam based on the angle
        Vector2 beamDirection = Quaternion.Euler(0, 0, angle) * transform.right;

        // gets the position of the centre of the AOE effect
        Vector2 AOEPosition = new Vector2(transform.position.x + beamLength / 2 * (float)Math.Cos(angle * Mathf.Deg2Rad), transform.position.y + beamLength / 2 * (float)Math.Sin(angle * Mathf.Deg2Rad));

        // creates the AOE effect
        AOEEffect.CreateRectangle(AOEPosition, AOEEffectTime, AOEEffectDecay, AOEEffectColour, angle, beamLength, beamThickness);

        // Cast a box-shaped ray in the specified direction
        RaycastHit2D[] objectsHit = Physics2D.BoxCastAll(transform.position, new Vector2(beamThickness, 0.1f), angle, beamDirection, beamLength);

        // for each object hit
        foreach (RaycastHit2D hit in objectsHit)
        {
            // grabs the gameobject that was hit
            GameObject objectHit = hit.collider.gameObject;

            // grabs only the enemies
            if (objectHit.tag == "Enemy")
            {
                EnemyController enemy = objectHit.GetComponent<EnemyController>();

                // if the enemy is alive, deal damage to it
                if (!enemy.Dead)
                {
                    // if the enemy dies trigger it
                    if (!enemy.ChangeHealth(-(int)(damage * body.DamageMultiplier)))
                    {
                        EnemyKilled(objectHit);
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

        jsonData.Setup(ref damage, nameof(damage));
        jsonData.Setup(ref beamThickness, nameof(beamThickness));
        jsonData.Setup(ref beamLength, nameof(beamLength));
        jsonData.Setup(ref AOEEffectTime, nameof(AOEEffectTime));
        jsonData.Setup(ref AOEEffectDecay, nameof(AOEEffectDecay));
        jsonData.Setup(ref AOEEffectColour, nameof(AOEEffectColour));
    }
}
