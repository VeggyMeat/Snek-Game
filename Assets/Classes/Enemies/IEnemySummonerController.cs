using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemySummonerController
{
    public void SetGameSetup(IGameSetup gameSetup);
    public int EnemiesDead { get; }
    public void EnemyDied();
    public void EnemyDespawned(EnemyController enemyController);
}
