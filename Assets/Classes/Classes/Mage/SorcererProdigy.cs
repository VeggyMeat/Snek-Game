using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SorcererProdigy : Mage
{
    // todo: make it so this cant be buffed (oh god why)

    private int damage;
    private float beamThickness;
    private float beamLength;

    private float AOEEffectTime;
    private bool AOEEffectDecay;
    private Color AOEEffectColour;


    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Mage/SorcererProdigy.json";

        base.ClassSetup();
    }

    internal override void Attack()
    {
        // gets a random angle
        float angle = UnityEngine.Random.Range(0, Mathf.PI * 2) * Mathf.Rad2Deg;

        // Get the direction of the beam based on the angle
        Vector2 beamDirection = Quaternion.Euler(0, 0, angle) * transform.right;

        Vector2 AOEPosition = new Vector2(transform.position.x + beamLength / 2 * (float)Math.Cos(angle * Mathf.Deg2Rad), transform.position.y + beamLength / 2 * (float)Math.Sin(angle * Mathf.Deg2Rad));

        AOEEffect.CreateRectangle(AOEPosition, AOEEffectTime, AOEEffectDecay, AOEEffectColour, angle, beamLength, beamThickness);

        // Cast a box-shaped ray in the specified direction
        RaycastHit2D[] objectsHit = Physics2D.BoxCastAll(transform.position, new Vector2(beamThickness, 0.1f), angle, beamDirection, beamLength);

        Debug.Log(objectsHit.Length);

        foreach (RaycastHit2D hit in objectsHit)
        {
            // grabs the gameobject that was hit
            GameObject objectHit = hit.collider.gameObject;

            // filters out the enemies
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
