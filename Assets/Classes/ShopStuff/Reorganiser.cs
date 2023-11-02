using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Reorganiser : MonoBehaviour
{
    List<GameObject> bodies = new List<GameObject>();

    [SerializeField] private HeadController headController;
    [SerializeField] private GameObject body;

    [SerializeField] private float seperationValue;
    [SerializeField] private Vector2 startingSpot;

    private Vector2 StartingSpot
    {
        get
        {
            return startingSpot + (Vector2)headController.enemySummonerController.CameraTransform.position;
        }
    }

    [SerializeField] private float mouseDistanceValue;


    private bool active;

    public bool Active
    {
        get
        {
            return active;
        }
        set
        {
            if (value is true)
            {
                Setup();
            }
            else
            {
                Hide();
            }

            active = value;
        }
    }

    // -1 indicates none, 0 before body 0, 1 inbetween bodies 0 and 1, 2 inbetween bodies 1 and 2 etc
    private int bodyGapPosition;

    private void Setup()
    {
        // grabs the head
        BodyController bodyController = headController.head;

        // goes through each body in the snake
        while (bodyController is not null)
        {
            // creates a new body object
            GameObject bodyObject = Instantiate(body);

            // sets its color
            bodyObject.GetComponent<SpriteRenderer>().color = bodyController.Color;

            // adds it to the body list
            bodies.Add(bodyObject);

            bodyController = bodyController.next;
        }
    }

    private void Update()
    {
        if (active)
        {
            CheckMousePos();
            SetBodiesPositions();
        }
    }

    private void CheckMousePos()
    {
        bodyGapPosition = -1;

        // gets the mouse position
        Vector2 mousePos = Input.mousePosition;

        // looks at each space in the bodies places
        float y = StartingSpot.y;
        float x = StartingSpot.x;
        
        for (int i = 0; i <= bodies.Count; i++)
        {
            if (Vector2.Distance(mousePos, new Vector2(x, y)) < bodyGapPosition)
            {
                bodyGapPosition = i;
            }

            // increases the value of the x for the next space
            x += seperationValue;
        }
    }

    private void SetBodiesPositions()
    {
        Vector2 position = StartingSpot;

        // goes through each body
        int i = 0;
        foreach (GameObject body in bodies)
        {
            i++;

            if (i == bodyGapPosition)
            {
                position.x += seperationValue;
            }

            body.transform.position = position;

            position.x += seperationValue;
        }
    }

    private void Hide()
    {

    }
}
