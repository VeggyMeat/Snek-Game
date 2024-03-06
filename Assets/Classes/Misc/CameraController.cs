using UnityEngine;

// COMPLETE

/// <summary>
/// The script that is placed on the camera object to follow the snake
/// </summary>
public class CameraController : MonoBehaviour, ICameraController
{
    private const double followSpeed = 5;

    /// <summary>
    /// The game setup
    /// </summary>
    private IGameSetup gameSetup;

    /// <summary>
    /// The transform (position) of the camera
    /// </summary>
    public Transform Transform => transform;

    /// <summary>
    /// Sets the gameSetup of the snake
    /// </summary>
    /// <param name="gameSetup">the gameSetup</param>
    public void SetGameSetup(IGameSetup gameSetup)
    {
        this.gameSetup = gameSetup;
    }

    // called by unity every frame
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(gameSetup.HeadController.Transform.position.x, gameSetup.HeadController.Transform.position.y, transform.position.z), (float)(followSpeed * Time.deltaTime));

    }
}
