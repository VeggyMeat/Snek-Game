using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWardEnchanter : Enchanter
{
    private int maxHealthIncrease;
    private float reviveHealthPercentage;


    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/MageEnchanter/DeathWard/DeathWardEnchanter.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        base.Setup();

        // adds the triggers
        TriggerManager.EnemyDeadTrigger.AddTrigger(OnEnemyDeath);
        TriggerManager.BodyDeadTrigger.AddTrigger(OnBodyDeath);
    }

    private GameObject OnEnemyDeath(GameObject enemy)
    {
        Debug.Log("enemy died");

        body.healthBuff.AddBuff(maxHealthIncrease, false, 0);

        return enemy;
    }

    private GameObject OnBodyDeath(GameObject bodyObject)
    {
        // if it is this object, ignore
        if (bodyObject == body.gameObject)
        {
            return bodyObject;
        }

        Debug.Log("body died");

        BodyController bodyController = bodyObject.GetComponent<BodyController>();

        // gets the HP that will be moved to the body
        int health = (int)(bodyController.MaxHealth * reviveHealthPercentage);

        // revives the body that died
        bodyController.Revived();

        // sets its hp back to 0
        bodyController.health = 0;

        // heals it to the right level
        bodyController.ChangeHealth(health);

        if (health < body.MaxHealth)
        {
            // reduces the maxHP of the death ward
            body.healthBuff.AddBuff(-health, false, 0);
        }
        else
        {
            // kills the body
            body.ChangeHealth(-health);
        }

        return bodyObject;
    }

    internal override void OnDeath()
    {
        base.OnDeath();

        TriggerManager.EnemyDeadTrigger.RemoveTrigger(OnEnemyDeath);
        TriggerManager.BodyDeadTrigger.RemoveTrigger(OnBodyDeath);

        // destroys the body permananetly
        body.DestroySelf();
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref maxHealthIncrease, nameof(maxHealthIncrease));
        jsonData.Setup(ref reviveHealthPercentage, nameof(reviveHealthPercentage));
    }
}
