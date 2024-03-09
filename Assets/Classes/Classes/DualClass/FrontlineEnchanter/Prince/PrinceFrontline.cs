using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The prince frontline class, a subclass of the frontline class
/// </summary>
internal class PrinceFrontline : Frontline
{
    /// <summary>
    /// The radius around the attacked spot to deal damage to enemies
    /// </summary>
    private float attackRadius;

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
    /// The force applied to enemies when hit, away from the body
    /// </summary>
    private float attackForce;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/FrontlineEnchanter/Prince/PrinceFrontline.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called by the body after it has been set up
    /// </summary>
    internal override void Setup()
    {
        base.Setup();
    }

    /// <summary>
    /// Called regularly by Frontline based on timeDelay
    /// </summary>
    /// <param name="position">The position which should be attacked</param>
    internal override void Attack(Vector3 position)
    {
        // spawns in the AOEEffect
        AOEEffect.CreateCircle(position, AOEEffectTime, AOEEffectDecay, AOEEffectColour, attackRadius);

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
                    // applies a knockback force to the enemy
                    enemyController.selfRigid.AddForce((enemyObj.transform.position - transform.position).normalized * attackForce);
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

        jsonData.Setup(ref attackRadius, nameof(attackRadius));
        jsonData.Setup(ref AOEEffectTime, nameof(AOEEffectTime));
        jsonData.Setup(ref AOEEffectDecay, nameof(AOEEffectDecay));
        jsonData.Setup(ref AOEEffectColour, nameof(AOEEffectColour));
        jsonData.Setup(ref attackForce, nameof(attackForce));
    }
}
