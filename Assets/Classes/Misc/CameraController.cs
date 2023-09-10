using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private GameObject snakeHead;
    internal static Transform cameraTransform;
    [SerializeField] private double followSpeed = 5;

    void Start()
    {
        cameraTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        // camera LERP

        transform.position = Vector3.Lerp(cameraTransform.position, new Vector3(snakeHead.transform.position.x, snakeHead.transform.position.y, cameraTransform.position.z), (float)(followSpeed * Time.deltaTime));

    }
}
