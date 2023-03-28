using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject snakeHead;
    public Transform cameraTransform;
    public double followSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // canera LERP

        cameraTransform.position = Vector3.Lerp(cameraTransform.position, new Vector3(snakeHead.transform.position.x, snakeHead.transform.position.y, -1), (float)(followSpeed * Time.deltaTime));

        // camera TRACK

        //cameraTransform.position = new Vector3(snakeHead.transform.position.x, snakeHead.transform.position.y, -1);
    }
}
