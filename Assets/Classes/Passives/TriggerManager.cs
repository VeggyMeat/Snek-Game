using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Controls all of the triggers in the game
/// </summary>
public static class TriggerManager
{
    /// <summary>
    /// The trigger for when a body in the snake loses health
    /// The BodyController refers to the body that lost health
    /// The int value refers to the amount of health lost
    /// </summary>
    public readonly static BodyTrigger<(BodyController, int)> BodyLostHealthTrigger = new BodyTrigger<(BodyController, int)>();

    /// <summary>
    /// The trigger for when a body in the snake gains health
    /// The BodyController refers to the body that gained health
    /// The int value refers to the amount of health gained
    /// </summary>
    public readonly static BodyTrigger<(BodyController, int)> BodyGainedHealthTrigger = new BodyTrigger<(BodyController, int)>();

    /// <summary>
    /// The trigger for when the snake levels up
    /// The int value refers to the new level of the snake
    /// </summary>
    public readonly static BodyTrigger<int> BodyLevelUpTrigger = new BodyTrigger<int>();

    /// <summary>
    /// The trigger for when a body in the snake dies
    /// The BodyController refers to the body that died
    /// </summary>
    public readonly static BodyTrigger<BodyController> BodyDeadTrigger = new BodyTrigger<BodyController>();

    /// <summary>
    /// The trigger for when a body is added to the snake
    /// 
    /// </summary>
    public readonly static BodyTrigger<BodyController> BodySpawnTrigger = new BodyTrigger<BodyController>();

    public readonly static BodyTrigger<GameObject> EnemySpawnTrigger = new BodyTrigger<GameObject>();

    public readonly static BodyTrigger<GameObject> EnemyDeadTrigger = new BodyTrigger<GameObject>();

    public readonly static BodyTrigger<EnemyController> EnemyLostHealthTrigger = new BodyTrigger<EnemyController>();

    public readonly static BodyTrigger<GameObject> ProjectileShotTrigger = new BodyTrigger<GameObject>();

    public readonly static BodyTrigger<BodyController> BodyRevivedTrigger = new BodyTrigger<BodyController>();

    public readonly static BodyTrigger<int> StartTurningTrigger = new BodyTrigger<int>();

    public readonly static BodyTrigger<int> StopTurningTrigger = new BodyTrigger<int>();

    public readonly static BodyTrigger<int> PreBodyMoveTrigger = new BodyTrigger<int>();

    public readonly static BodyTrigger<int> PostBodyMoveTrigger = new BodyTrigger<int>();

    public readonly static BodyTrigger<GameObject> ProjectileHitTrigger = new BodyTrigger<GameObject>();

    public readonly static BodyTrigger<int> PauseTimeTrigger = new BodyTrigger<int>();

    public readonly static BodyTrigger<int> ResumeTimeTrigger = new BodyTrigger<int>();

    public readonly static BodyTrigger<GameObject> BodyKilledTrigger = new BodyTrigger<GameObject>();
}
