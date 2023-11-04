using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class TriggerManager
{
    public static BodyTrigger<(BodyController, int)> BodyLostHealthTrigger = new BodyTrigger<(BodyController, int)>();

    public static BodyTrigger<(BodyController, int)> BodyGainedHealthTrigger = new BodyTrigger<(BodyController, int)>();

    public static BodyTrigger<int> BodyLevelUpTrigger = new BodyTrigger<int>();

    public static BodyTrigger<BodyController> BodyDeadTrigger = new BodyTrigger<BodyController>();

    public static BodyTrigger<BodyController> BodySpawnTrigger = new BodyTrigger<BodyController>();

    public static BodyTrigger<GameObject> EnemySpawnTrigger = new BodyTrigger<GameObject>();

    public static BodyTrigger<GameObject> EnemyDeadTrigger = new BodyTrigger<GameObject>();

    public static BodyTrigger<EnemyController> EnemyLostHealthTrigger = new BodyTrigger<EnemyController>();

    public static BodyTrigger<GameObject> ProjectileShotTrigger = new BodyTrigger<GameObject>();

    public static BodyTrigger<BodyController> BodyRevivedTrigger = new BodyTrigger<BodyController>();

    public static BodyTrigger<int> StartTurningTrigger = new BodyTrigger<int>();

    public static BodyTrigger<int> StopTurningTrigger = new BodyTrigger<int>();

    public static BodyTrigger<int> PreBodyMoveTrigger = new BodyTrigger<int>();

    public static BodyTrigger<int> PostBodyMoveTrigger = new BodyTrigger<int>();

    public static BodyTrigger<GameObject> ProjectileHitTrigger = new BodyTrigger<GameObject>();
}
