using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour, ICameraController
{
    [SerializeField] private double followSpeed = 5;

    private IGameSetup gameSetup;

    public Transform Transform => transform;

    public void SetGameSetup(IGameSetup gameSetup)
    {
        this.gameSetup = gameSetup;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(gameSetup.HeadController.Transform.position.x, gameSetup.HeadController.Transform.position.y, transform.position.z), (float)(followSpeed * Time.deltaTime));

    }
}
