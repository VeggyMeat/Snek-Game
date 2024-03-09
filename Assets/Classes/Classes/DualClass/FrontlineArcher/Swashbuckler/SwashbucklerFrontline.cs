using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The swashbuckler frontline class, a subclass of the frontline class
/// </summary>
internal class SwashbucklerFrontline : Frontline
{
    /// <summary>
    /// The increase in percentage of damage per frontline
    /// </summary>
    private int perFrontlineDamageIncrease;

    /// <summary>
    /// The increase in percentage of the attack area per frontline
    /// </summary>
    private int perFrontlineAreaIncrease;

    /// <summary>
    /// The attack radius of the attack, after the buff calculation
    /// </summary>
    private float AttackRadius 
    { 
        get
        {
            return attackRadius * (1 + frontlineNumber * perFrontlineAreaIncrease);
        }
    }

    /// <summary>
    /// The damage the class does, after the buff calculation
    /// </summary>
    private int Damage
    {
        get
        {
            return (int)(damage * (1 + frontlineNumber * perFrontlineDamageIncrease));
        }
    }

    /// <summary>
    /// The number of frontlines in the body
    /// </summary>
    private int frontlineNumber = -1;

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
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/FrontlineArcher/Swashbuckler/SwashbucklerFrontline.json";

        // indicates that this is not the primary class on the body
        primary = false;

        base.ClassSetup();
    }

    /// <summary>
    /// Called by the body after it has been set up
    /// </summary>
    internal override void Setup()
    {
        base.Setup();

        // initial count for the number of frontlines
        // goes through each body and increases the frontline number if the body has a frontline class
        BodyController bodyController = body.snake.Head;
        while (bodyController is not null)
        {
            if (bodyController.classNames.Contains(nameof(Frontline)))
            {
                frontlineNumber++;
            }

            bodyController = bodyController.next;
        }

        // sets up the triggers for the frontline number
        TriggerManager.BodySpawnTrigger.AddTrigger(IncreaseFrontline);
        TriggerManager.BodyDeadTrigger.AddTrigger(DecreaseFrontline);
        TriggerManager.BodyRevivedTrigger.AddTrigger(IncreaseFrontline);
    }

    /// <summary>
    /// Called regularly by Frontline based on timeDelay
    /// </summary>
    /// <param name="position">The position which should be attacked</param>
    internal override void Attack(Vector3 position)
    {
        // spawns in the AOEEffect
        AOEEffect.CreateCircle(position, AOEEffectTime, AOEEffectDecay, AOEEffectColour, AttackRadius);

        // gets all the objects within the range
        Collider2D[] objectsInCircle = Physics2D.OverlapCircleAll(position, AttackRadius);

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
                if (!enemyController.ChangeHealth(-Damage))
                {
                    // enemy has been killed
                    EnemyKilled(enemyObj);
                }
                else
                {
                    // adds knockback to the enemy
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

        jsonData.Setup(ref perFrontlineDamageIncrease, nameof(perFrontlineDamageIncrease));
        jsonData.Setup(ref perFrontlineAreaIncrease, nameof(perFrontlineAreaIncrease));

        jsonData.Setup(ref attackRadius, nameof(attackRadius));
        jsonData.Setup(ref AOEEffectTime, nameof(AOEEffectTime));
        jsonData.Setup(ref AOEEffectDecay, nameof(AOEEffectDecay));
        jsonData.Setup(ref AOEEffectColour, nameof(AOEEffectColour));
        jsonData.Setup(ref attackForce, nameof(attackForce));
    }

    /// <summary>
    /// Increases the frontline number if the body is a frontline
    /// </summary>
    /// <param name="bodyController">The body being checked</param>
    /// <returns>The body being checked</returns>
    private BodyController IncreaseFrontline(BodyController bodyController)
    {
        if (bodyController.classNames.Contains(nameof(Frontline)))
        {
            frontlineNumber++;
        }

        return bodyController;
    }

    /// <summary>
    /// Decreases the frontline number if the body is a frontline
    /// </summary>
    /// <param name="bodyController">The body being checked</param>
    /// <returns>The body being checked</returns>
    private BodyController DecreaseFrontline(BodyController bodyController)
    {
        if (bodyController.classNames.Contains(nameof(Frontline)))
        {
            frontlineNumber--;
        }

        return bodyController;
    }
}
