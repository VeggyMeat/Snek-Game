using UnityEngine;

public interface IGroundTriggerManager
{
    /// <summary>
    /// The position the item should judge its distance from
    /// </summary>
    public Vector2 DespawnPosition { get; }

    /// <summary>
    /// Called when the item collides with something
    /// </summary>
    /// <returns>Whether the item should destroy itself or not</returns>
    public bool OnCollision(Collider2D collision)
    {
        return false;
    }

    public void OnDespawn()
    {

    }
}