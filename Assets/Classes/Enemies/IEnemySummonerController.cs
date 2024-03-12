// COMPLETE

/// <summary>
/// The interface for the enemy summoner controller
/// </summary>
public interface IEnemySummonerController
{
    /// <summary>
    /// Sets the gameSetup of the snake
    /// </summary>
    /// <param name="gameSetup"></param>
    public void SetGameSetup(IGameSetup gameSetup);

    /// <summary>
    /// The number of enemies that have died
    /// </summary>
    public int EnemiesDead { get; }

    /// <summary>
    /// Called when an enemy dies, to increase the number of enemies dead
    /// </summary>
    public void EnemyDied();

    /// <summary>
    /// Called by enemies when the enemy despawns
    /// </summary>
    /// <param name="enemyController">The enemy that despawned</param>
    public void EnemyDespawned(EnemyController enemyController);
}
