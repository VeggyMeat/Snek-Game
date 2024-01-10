using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class TriggerManager
{
    public readonly static BodyTrigger<(BodyController, int)> BodyLostHealthTrigger = new BodyTrigger<(BodyController, int)>();

    public readonly static BodyTrigger<(BodyController, int)> BodyGainedHealthTrigger = new BodyTrigger<(BodyController, int)>();

    public readonly static BodyTrigger<int> BodyLevelUpTrigger = new BodyTrigger<int>();

    public readonly static BodyTrigger<BodyController> BodyDeadTrigger = new BodyTrigger<BodyController>();

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
}
