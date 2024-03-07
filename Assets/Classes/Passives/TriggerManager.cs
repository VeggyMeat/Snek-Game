using UnityEngine;

// COMPLETE

/// <summary>
/// Controls all of the triggers in the game
/// </summary>
public static class TriggerManager
{
    /// <summary>
    /// The trigger for when a body in the snake loses health,
    /// The BodyController refers to the body that lost health,
    /// The int value refers to the amount of health lost
    /// </summary>
    public readonly static BodyTrigger<(BodyController, int)> BodyLostHealthTrigger = new BodyTrigger<(BodyController, int)>();

    /// <summary>
    /// The trigger for when a body in the snake gains health,
    /// The BodyController refers to the body that gained health,
    /// The int value refers to the amount of health gained
    /// </summary>
    public readonly static BodyTrigger<(BodyController, int)> BodyGainedHealthTrigger = new BodyTrigger<(BodyController, int)>();

    /// <summary>
    /// The trigger for when the snake levels up,
    /// The int value refers to the new level of the snake
    /// </summary>
    public readonly static BodyTrigger<int> BodyLevelUpTrigger = new BodyTrigger<int>();

    /// <summary>
    /// The trigger for when a body in the snake dies,
    /// The BodyController refers to the body that died
    /// </summary>
    public readonly static BodyTrigger<BodyController> BodyDeadTrigger = new BodyTrigger<BodyController>();

    /// <summary>
    /// The trigger for when a body is added to the snake,
    /// The BodyController refers to the body that was added
    /// </summary>
    public readonly static BodyTrigger<BodyController> BodySpawnTrigger = new BodyTrigger<BodyController>();

    /// <summary>
    /// The trigger for when a new enemy is spawned,
    /// The EnemyController refers to the enemy that was spawned
    /// </summary>
    public readonly static BodyTrigger<GameObject> EnemySpawnTrigger = new BodyTrigger<GameObject>();

    /// <summary>
    /// The trigger for when an enemy dies,
    /// The GameObject refers to the enemy that died
    /// </summary>
    public readonly static BodyTrigger<GameObject> EnemyDeadTrigger = new BodyTrigger<GameObject>();

    /// <summary>
    /// The trigger for when an enemy loses health,
    /// The EnemyController refers to the enemy that lost health
    /// </summary>
    public readonly static BodyTrigger<EnemyController> EnemyLostHealthTrigger = new BodyTrigger<EnemyController>();

    /// <summary>
    /// The trigger for when a projectile is shot,
    /// The GameObject refers to the projectile that was shot
    /// </summary>
    public readonly static BodyTrigger<GameObject> ProjectileShotTrigger = new BodyTrigger<GameObject>();

    /// <summary>
    /// The trigger for when a body is revived,
    /// The BodyController refers to the body that was revived
    /// </summary>
    public readonly static BodyTrigger<BodyController> BodyRevivedTrigger = new BodyTrigger<BodyController>();

    /// <summary>
    /// The trigger for when the player starts turning,
    /// The int value is redundant
    /// </summary>
    public readonly static BodyTrigger<int> StartTurningTrigger = new BodyTrigger<int>();

    /// <summary>
    /// The trigger for when the player stops turning,
    /// The int value is redundant
    /// </summary>
    public readonly static BodyTrigger<int> StopTurningTrigger = new BodyTrigger<int>();

    /// <summary>
    /// The trigger called before the body objects are reorganized,
    /// The int value is redundant
    /// </summary>
    public readonly static BodyTrigger<int> PreBodyMoveTrigger = new BodyTrigger<int>();

    /// <summary>
    /// The trigger called after the body objects are reorganized,
    /// The int value is redundant
    /// </summary>
    public readonly static BodyTrigger<int> PostBodyMoveTrigger = new BodyTrigger<int>();

    /// <summary>
    /// The trigger for when a projectile hits an enemy,
    /// The GameObject refers to the projectile that hit an enemy
    /// </summary>
    public readonly static BodyTrigger<GameObject> ProjectileHitTrigger = new BodyTrigger<GameObject>();

    /// <summary>
    /// The trigger for when the player pauses time,
    /// The int value is redundant
    /// </summary>
    public readonly static BodyTrigger<int> PauseTimeTrigger = new BodyTrigger<int>();

    /// <summary>
    /// The trigger for when the player resumes time,
    /// The int value is redundant
    /// </summary>
    public readonly static BodyTrigger<int> ResumeTimeTrigger = new BodyTrigger<int>();

    /// <summary>
    /// The trigger for when a body is killed,
    /// The GameObject refers to the body that was killed
    /// </summary>
    public readonly static BodyTrigger<GameObject> BodyKilledTrigger = new BodyTrigger<GameObject>();
}
