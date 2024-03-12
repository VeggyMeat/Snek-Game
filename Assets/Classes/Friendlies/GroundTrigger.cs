using UnityEngine;

// COMPLETE

/// <summary>
/// The class that lies on any ground triggers (like Herbologists' plants)
/// </summary>
public class GroundTrigger : MonoBehaviour
{
    /// <summary>
    /// The parent of the object
    /// </summary>
    private IGroundTriggerManager parent;

    /// <summary>
    /// If the object should despawn when out of range
    /// </summary>
    private bool despawn;

    /// <summary>
    /// The range the object should despawn at
    /// </summary>
    private float despawnRange;

    /// <summary>
    /// How long the object should live for
    /// </summary>
    private float lifeSpan;

    // Called by Unity on the first frame after the object is created
    private void Start()
    {
        // kill the object after a certain time
        Destroy(gameObject, lifeSpan);
    }

    // Called by Unity every frame
    private void Update()
    {
        // if the object is set to despawn
        if (despawn)
        {
            // if the object is out of range
            if (Vector2.Distance(transform.position, parent.DespawnPosition) > despawnRange)
            {
                // tells the parent that the object is despawning
                parent.OnDespawn();

                // destroy the object
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Called by the parent to set up the object
    /// </summary>
    /// <param name="parent">The parent who created the object</param>
    /// <param name="despawn">If the object should despawn when out of range</param>
    /// <param name="despawnRange">The range the object should despawn at</param>
    /// <param name="lifeSpan">How long the object should live for</param>
    internal void Setup(IGroundTriggerManager parent, bool despawn, float despawnRange, float lifeSpan)
    {
        this.parent = parent;
        this.despawn = despawn;
        this.despawnRange = despawnRange;
        this.lifeSpan = lifeSpan;
    }

    // Called by Unity when the object collides with something
    private void OnTriggerEnter2D(Collider2D obj)
    {
        // tells the parent its colliding with something
        bool response = parent.OnCollision(obj);

        // if the parent wants it to be destroyed then destroy the object
        if (response)
        {
            Destroy(gameObject);
        }
    }
}
