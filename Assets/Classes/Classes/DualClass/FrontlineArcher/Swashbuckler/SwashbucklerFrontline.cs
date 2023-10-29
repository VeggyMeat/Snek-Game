using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwashbucklerFrontline : Frontline
{
    private int perFrontlineDamageIncrease;
    private int perFrontlineAreaIncrease;

    private int frontlineNumber = -1;

    private float attackRadius;
    private float AOEEffectTime;

    private string AOEEffectPath;

    private GameObject AOEEffect;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/FrontlineArcher/Swashbuckler/SwashbucklerFrontline.json";

        primary = false;

        base.ClassSetup();
    }
    internal override void Setup()
    {
        base.Setup();

        // gets the AOEEffect ready to be spawned
        AOEEffect = Resources.Load<GameObject>(AOEEffectPath);

        // initial count for the number of frontlines
        BodyController bodyController = body.snake.head;
        while (bodyController is not null)
        {
            if (bodyController.classNames.Contains(nameof(Frontline)))
            {
                frontlineNumber++;
            }

            bodyController = bodyController.next;
        }

        TriggerManager.BodySpawnTrigger.AddTrigger(IncreaseFrontline);
        TriggerManager.BodyDeadTrigger.AddTrigger(DecreaseFrontline);
        TriggerManager.BodyRevivedTrigger.AddTrigger(IncreaseFrontline);
    }

    internal override void Attack(Vector3 position)
    {
        Debug.Log(frontlineNumber);

        float frontlineDamageIncrease = 1 + frontlineNumber * perFrontlineDamageIncrease;
        float frontlineAreaIncrease = 1 + frontlineNumber * perFrontlineAreaIncrease;

        // spawns in the AOEEffect
        GameObject AOEEffectInstance = Instantiate(AOEEffect, position, Quaternion.identity);
        AOEEffectInstance.GetComponent<AOEEffectController>().Setup(AOEEffectTime, attackRadius * frontlineAreaIncrease);

        // gets all the objects within the range
        Collider2D[] objectsInCircle = Physics2D.OverlapCircleAll(position, attackRadius * frontlineAreaIncrease);

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
                if (!enemyController.ChangeHealth(-(int)(damage * body.DamageMultiplier * frontlineDamageIncrease)))
                {
                    // enemy has been killed
                    EnemyKilled(enemyObj);
                }
                else
                {
                    // add a knockback thing
                }
            }
        }
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref perFrontlineDamageIncrease, nameof(perFrontlineDamageIncrease));
        jsonData.Setup(ref perFrontlineAreaIncrease, nameof(perFrontlineAreaIncrease));

        jsonData.Setup(ref attackRadius, nameof(attackRadius));
        jsonData.Setup(ref AOEEffectTime, nameof(AOEEffectTime));

        if (jsonData.ContainsKey(nameof(AOEEffectPath)))
        {
            AOEEffectPath = jsonData[nameof(AOEEffectPath)].ToString();

            if (jsonLoaded)
            {
                // gets the AOEEffect ready to be spawned
                AOEEffect = Resources.Load<GameObject>(AOEEffectPath);
            }
        }
    }

    private BodyController IncreaseFrontline(BodyController bodyCon)
    {
        if (bodyCon.classNames.Contains(nameof(Frontline)))
        {
            frontlineNumber++;
        }

        return bodyCon;
    }

    private GameObject DecreaseFrontline(GameObject bodyCon)
    {
        if (bodyCon.GetComponent<BodyController>().classNames.Contains(nameof(Frontline)))
        {
            frontlineNumber--;
        }

        return bodyCon.gameObject;
    }
}
