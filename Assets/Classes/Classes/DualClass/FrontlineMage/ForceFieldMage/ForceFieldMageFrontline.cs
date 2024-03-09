using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The force field mage frontline class, a subclass of the frontline class
/// </summary>
internal class ForceFieldMageFrontline : Frontline
{
    /// <summary>
    /// The radius of the attack centred at the body
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
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/FrontlineMage/ForceFieldMage/ForceFieldMageFrontline.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called regularly by Frontline based on timeDelay
    /// </summary>
    /// <param name="position">The position which should be attacked</param>
    internal override void Attack(Vector3 position)
    {
        // creates the AOE effect
        AOEEffect.CreateCircle(transform.position, AOEEffectTime, AOEEffectDecay, AOEEffectColour, attackRadius);

        // gets all the objects within the range
        Collider2D[] objectsInCircle = Physics2D.OverlapCircleAll(transform.position, attackRadius);

        // gets all of the enemies within the range
        Collider2D[] enemiesInCircle = System.Array.FindAll(objectsInCircle, obj => obj.CompareTag("Enemy"));

        foreach (Collider2D collider in enemiesInCircle)
        {
            // grab the enemy controller
            EnemyController enemyController = collider.gameObject.GetComponent<EnemyController>();

            // if its not dead
            if (!enemyController.Dead)
            {
                if (!enemyController.ChangeHealth(-damage))
                {
                    // if the enemy dies note that its been killed
                    EnemyKilled(enemyController.gameObject);
                }
                else
                {
                    // aplies a knockback force to the enemy
                    enemyController.selfRigid.AddForce((collider.transform.position - transform.position).normalized * attackForce);
                }
            }
        }
    }

    /// <summary>
    /// Called when an enemy is killed
    /// </summary>
    /// <param name="enemy">The enemy's GameObject</param>
    internal override void EnemyKilled(GameObject enemy)
    {
        ((ForceFieldMageMage)body.classes[1]).BuffBody();

        base.EnemyKilled(enemy);
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
