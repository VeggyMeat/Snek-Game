using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class DeathWardEnchanter : Enchanter
{
    private int maxHealthIncrease;
    private float reviveHealthPercentage;

    private int selfDamage = 0;

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
        body.healthBuff.AddBuff(maxHealthIncrease, false, 0);

        return enemy;
    }

    private BodyController OnBodyDeath(BodyController bodyController)
    {
        // if it is this object, ignore
        if (bodyController == body)
        {
            return bodyController;
        }

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
            selfDamage = health;

            // kills self soon after
            Invoke(nameof(DamageSelf), 0.05f);
        }

        return bodyController;
    }

    private void DamageSelf()
    {
        // kills the body
        body.ChangeHealth(-selfDamage);
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
