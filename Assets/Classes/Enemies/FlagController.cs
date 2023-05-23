using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FlagController : MonoBehaviour
{
    private int range;
    private int speedBuff;

    public void Setup(float timeDelay, int range, int speedBuff, int lifeSpan)
    {
        this.range = range;
        this.speedBuff = speedBuff;

        // sets the circle to the size of the range as radius
        transform.localScale = new Vector3(range, range, 1);

        // starts adding a repeating call to add effects to nearby enemies
        InvokeRepeating(nameof(AddEffects), timeDelay, timeDelay);

        // kills itself after lifeSpan seconds
        Invoke(nameof(OnDestroy), lifeSpan);

        Debug.Log(lifeSpan);
    }

    void AddEffects()
    {
        // gets all the objects within the range
        Collider2D[] objectsInCircle = Physics2D.OverlapCircleAll(transform.position, range);

        // gets all of the enemies within the range
        Collider2D[] enemiesInCircle = System.Array.FindAll(objectsInCircle, obj => obj.CompareTag("Enemy"));

        foreach (Collider2D enemy in enemiesInCircle)
        {
            Debug.Log("added effects");

            // gets the enemy controller
            EnemyController enemyController = enemy.GetComponent<EnemyController>();

            // if the enemy is still alive gives it a buff
            if (!enemyController.dead)
            {
                enemyController.passiveHandler.AddPassive("SpeedBuff", speedBuff);
            }
        }
    }

    // when the flag is destroyed, stop repeating the effect
    public void OnDestroy()
    {
        CancelInvoke(nameof(AddEffects));

        Destroy(gameObject);
    }
}
