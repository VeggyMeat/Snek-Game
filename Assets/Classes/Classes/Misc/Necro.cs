using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEditor.Progress;

public class Necro : Class
{
    private int maxSummoned;

    private string zombiePath;
    private string zombieJson;

    // currently unused (mostly) should be replaced or utilised in the future
    internal List<NecromancerZombieController> summonedZombies;

    private JsonVariable necroZombieVariables;

    private GameObject zombie;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Misc/Necro.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        body.classNames.Add("Misc");

        // sets up a list of the controlled zombies
        summonedZombies = new List<NecromancerZombieController>();

        // adds this to the enemy death trigger list
        TriggerManager.EnemyDeadTrigger.AddTrigger(EnemyKilledTrigger);

        // gets the zombie asset ready
        zombie = Resources.Load<GameObject>(zombiePath);

        necroZombieVariables = new JsonVariable(zombieJson);

        base.Setup();
    }

    internal GameObject EnemyKilledTrigger(GameObject enemy)
    {
        if (body.IsDead)
        {
            return enemy;
        }

        // if there is capacity for another zombie spawn it
        if (summonedZombies.Count < maxSummoned)
        {
            SummonZombie(enemy.transform);
        }

        return enemy;
    }

    // spawns a new friendly zombie at a certain position
    private void SummonZombie(Transform position)
    {
        // spawns a zombie at the position
        GameObject summonedZombie = Instantiate(zombie, new Vector3(position.position.x, position.position.y, 0f), Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
        
        // sets up the zombie
        NecromancerZombieController controller = summonedZombie.GetComponent<NecromancerZombieController>();
        controller.Setup(necroZombieVariables.Variables, this, body.DamageMultiplier);

        // adds the zombie to the list of controlled zombies
        summonedZombies.Add(controller);
    }

    internal void ZombieDeath(GameObject zombie)
    {
        summonedZombies.Remove(zombie.GetComponent<NecromancerZombieController>());
    }

    // called when the body is revived from the dead
    internal override void Revived()
    {
        base.Revived();

        // adds this back to the list for enemy death triggers
        TriggerManager.EnemyDeadTrigger.AddTrigger(EnemyKilledTrigger);
    }

    // called when the body dies
    internal override void OnDeath()
    {
        base.OnDeath();

        // removes this from the list of enemy death triggers
        TriggerManager.EnemyDeadTrigger.RemoveTrigger(EnemyKilledTrigger);
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref maxSummoned, "maxSummoned");
        jsonData.Setup(ref zombieJson, "zombieJson");
        if (jsonData.ContainsKey("zombiePath"))
        {
            zombiePath = (string)jsonData["zombiePath"];

            if (jsonLoaded)
            {
                zombie = Resources.Load<GameObject>(zombiePath);
            }
        }
    }

    internal override void LevelUp()
    {        
        base.LevelUp();

        // levels up the necro zombies
        if (!jsonLoaded)
        {
            necroZombieVariables.IncreaseIndex();
        }
    }
}
