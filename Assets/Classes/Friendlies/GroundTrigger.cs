using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTrigger : MonoBehaviour
{
    private IGroundTriggerManager parent;
    private bool despawn;
    private float despawnRange;

    private float lifeSpan;

    private void Start()
    {
        // kill the object after a certain time
        Destroy(gameObject, lifeSpan);
    }

    private void Update()
    {
        // if the object is set to despawn
        if (despawn)
        {
            // if the object is out of range
            if (Vector2.Distance(transform.position, parent.DespawnPosition) > despawnRange)
            {
                Debug.Log("Despawn");

                // tells the parent that the object is despawning
                parent.OnDespawn();

                // destroy the object
                Destroy(gameObject);
            }
        }
    }

    internal void Setup(IGroundTriggerManager parent, bool despawn, float despawnRange, float lifeSpan)
    {
        this.parent = parent;
        this.despawn = despawn;
        this.despawnRange = despawnRange;
        this.lifeSpan = lifeSpan;
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        // tells the parent its colliding with something
        bool response = parent.OnCollision(obj);

        // if the parent wants it dead then destroy the object
        if (response)
        {
            Destroy(gameObject);
        }
    }
}
