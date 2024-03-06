using UnityEngine;

// COMPLETE

/// <summary>
/// The interface for a camera controller
/// </summary>
public interface ICameraController
{
    /// <summary>
    /// Sets the gameSetup of the snake
    /// </summary>
    /// <param name="gameSetup">the gameSetup</param>
    public void SetGameSetup(IGameSetup gameSetup);

    /// <summary>
    /// The transform (position) of the camera
    /// </summary>
    public Transform Transform { get; }

}
