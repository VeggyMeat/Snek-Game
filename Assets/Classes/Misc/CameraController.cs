using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject snakeHead;
    public static Transform cameraTransform;
    public double followSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        // canera LERP

        transform.position = Vector3.Lerp(cameraTransform.position, new Vector3(snakeHead.transform.position.x, snakeHead.transform.position.y, -1), (float)(followSpeed * Time.deltaTime));

        // camera TRACK

        //cameraTransform.position = new Vector3(snakeHead.transform.position.x, snakeHead.transform.position.y, -1);
    }
}
