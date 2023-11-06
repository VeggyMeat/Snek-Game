using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herbologist : Enchanter, IGroundTriggerManager
{
    private int healingOrbHealAmount;

    private float healingOrbDelay;
    private string healingOrbPath;

    private float healingOrbRadius;

    private float healingOrbDespawnRange;
    private float healingOrbLifeSpan;


    private bool summoning;

    private GameObject healingOrbPrefab;

    public Vector2 DespawnPosition
    {
        get
        {
            return body.snake.head.transform.position;
        }
    }

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Enchanter/Herbologist.json";

        base.ClassSetup();
    }

    public bool OnCollision(Collider2D collision)
    {
        // if its not the player ignore it
        if (collision.gameObject.tag != "Player")
        {
            return false;
        }

        // finds all the bodies that arent at full hp
        List<BodyController> bodies = new List<BodyController>();

        // gets the head of the snake
        BodyController currentBody = body.snake.head;

        // goes through all the bodies in the snake
        while (currentBody is not null)
        {
            // if the body is not at max HP add it to the list
            if (currentBody.MaxHealth > currentBody.health)
            {
                // and if its alive
                if (!currentBody.IsDead)
                {
                    bodies.Add(currentBody);
                }
            }

            // get the next body
            currentBody = currentBody.next;
        }

        if (bodies.Count > 0)
        {
            // gets a random body from the list
            BodyController healBody = bodies.RandomItem();

            // heal that body
            healBody.ChangeHealth(healingOrbHealAmount);
        }

        return true;
    }

    internal override void Setup()
    {
        base.Setup();

        // gets the healingOrb gameObject
        healingOrbPrefab = Resources.Load<GameObject>(healingOrbPath);

        // starts summoning orbs
        StartSummoning();
    }

    private void SummonHealingOrb()
    {
        // gets a random position within the healingOrbRadius of the player's transform
        Vector2 position = Random.insideUnitCircle * healingOrbRadius + (Vector2)body.snake.head.transform.position;

        // creates a new healing orb
        GameObject newHealingOrb = Instantiate(healingOrbPrefab, position, Quaternion.identity);

        // sets up the healing orb
        newHealingOrb.GetComponent<GroundTrigger>().Setup(this, true, healingOrbDespawnRange, healingOrbLifeSpan);
    }

    private void StartSummoning()
    {
        // if already summoning, ignore
        if (summoning)
        {
            return;
        }

        // start summoning and note that it is summoning
        InvokeRepeating(nameof(SummonHealingOrb), healingOrbDelay / body.attackSpeedBuff.Value, healingOrbDelay / body.attackSpeedBuff.Value);

        summoning = true;
    }

    private void StopSummoning()
    {
        // if not summoning, ignore
        if (!summoning) 
        { 
            return; 
        }

        // stop summoning and note that it is not summoning
        CancelInvoke();

        summoning = false;
    }

    internal override void Revived()
    {
        base.Revived();

        // continues summoning orbs
        StartSummoning();
    }

    internal override void OnDeath()
    {
        // stops summoning orbs
        StopSummoning();

        base.OnDeath();
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        // not allowed to change after intial setting
        jsonData.Setup(ref healingOrbPath, nameof(healingOrbPath));

        jsonData.Setup(ref healingOrbDespawnRange, nameof(healingOrbDespawnRange));
        jsonData.Setup(ref healingOrbLifeSpan, nameof(healingOrbLifeSpan));
        jsonData.Setup(ref healingOrbRadius, nameof(healingOrbRadius));
        jsonData.Setup(ref healingOrbHealAmount, nameof(healingOrbHealAmount));

        jsonData.SetupAction(ref healingOrbDelay, nameof(healingOrbDelay), StopSummoning, StartSummoning, jsonLoaded);
    }

    internal override void OnAttackSpeedBuffUpdate(float amount, bool multiplicative)
    {
        base.OnAttackSpeedBuffUpdate(amount, multiplicative);

        // restarts the summoning of orbs
        StopSummoning();
        StartSummoning();
    }
}
