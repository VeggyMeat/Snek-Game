using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The interface for the head controller
/// </summary>
public interface IHeadController
{
    /// <summary>
    /// The transform of the head controller, which follows the snake's Head every frame
    /// </summary>
    public Transform Transform { get; }

    /// <summary>
    /// The speed at which the snake turns
    /// </summary>
    public double TurningRate { get; set; }

    /// <summary>
    /// The number of frames behind each body part is from the previous one
    /// </summary>
    public int FrameDelay { get; }

    /// <summary>
    /// The head of the snake
    /// </summary>
    public BodyController Head { get; }

    /// <summary>
    /// Whether the snake is currently turning
    /// </summary>
    public bool Turning { get; }

    /// <summary>
    /// The list of all the bodies in the snake
    /// </summary>
    public List<string> CurrentBodies { get; }
    
    /// <summary>
    /// The position of the head of the snake
    /// </summary>
    public Vector2 HeadPos { get; }

    /// <summary>
    /// The position of the tail of the snake (the last object in the linked list)
    /// </summary>
    public Vector2 TailPos { get; }

    /// <summary>
    /// The total number of bodies in the snake
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// The total number of bodies in the snake that are alive (not dead)
    /// </summary>
    public int AliveBodies { get; }

    /// <summary>
    /// The percentage completion of the current level of the snake (0-1)
    /// </summary>
    public float XPPercentage { get; }

    /// <summary>
    /// Sets the gameSetup of the snake
    /// </summary>
    /// <param name="gameSetup">the gameSetup</param>
    public void SetGameSetup(IGameSetup gameSetup);

    /// <summary>
    /// Adds the run to the database, with the information from the snake
    /// </summary>
    /// <param name="name">The player's name for this run</param>
    /// <returns>The list of statistics stored in the database for this run (name, score, time, date)</returns>
    public List<string> FinishRun(string name);

    /// <summary>
    /// Increases the amount of XP the snake has
    /// </summary>
    /// <param name="amount">The amount of XP to add</param>
    public void IncreaseXP(int amount);

    /// <summary>
    /// Adds a new body to the snake, at the tail of the snake
    /// </summary>
    /// <param name="name">The name of the body to be added</param>
    public void AddBody(string name);

    /// <summary>
    /// Rearranges the bodies of the snake to the order given
    /// </summary>
    /// <param name="bodyOrder">The new order of the bodies, the first item being the head, the last being the tail</param>
    public void Rearrange(List<BodyController> bodyOrder);
}
