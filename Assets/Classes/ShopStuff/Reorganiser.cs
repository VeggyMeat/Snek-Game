using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Reorganiser : MonoBehaviour
{
    List<GameObject> bodies = new List<GameObject>();
    List<int> bodiesHashes = new List<int>();

    [SerializeField] private HeadController headController;
    [SerializeField] private GameObject body;
    [SerializeField] private Camera mainCamera;

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


    private bool active = false;

    private bool bodyChanged = false;

    private GameObject selectedBody = null;
    private int selectedBodyHash = 0;

    private int selectedBodyPreviousSpot = -1;

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

            Debug.Log("x3 nuzzles owo your so warm");

            // sets its color
            bodyObject.GetComponent<SpriteRenderer>().color = bodyController.Color;

            // adds it to the body list
            bodies.Add(bodyObject);
            bodiesHashes.Add(bodyController.gameObject.GetHashCode());

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
        Debug.Log("hello");

        // no gap is currently selected
        bodyGapPosition = -1;

        // gets the mouse position
        Vector2 mousePos = Input.mousePosition;
        bool mouseClicked = Input.GetMouseButtonDown(0);

        Vector2 mousePosCoords = Camera.main.ScreenToWorldPoint(mousePos);

        // looks at each space in the bodies places
        float y = StartingSpot.y;
        float x = StartingSpot.x;

        // shows that no body is being hovered over
        bool hovered = false;
        
        for (int i = 0; i <= bodies.Count; i++)
        {
            // Debug.Log(Vector2.Distance(mousePosCoords, new Vector2(x, y)));

            if (!selectedBody && i < bodies.Count)
            {
                // makes the body opaque again
                Color oldColour = bodies[i].GetComponent<SpriteRenderer>().color;
                bodies[i].GetComponent<SpriteRenderer>().color = new Color(oldColour.r, oldColour.g, oldColour.b, 1f);
            }

            // checks if the mouse is over the current space
            bool hovering = Vector2.Distance(mousePosCoords, new Vector2(x, y)) < mouseDistanceValue;

            // if its hovering, then an object has been hovered over this cycle
            if (hovering)
            {
                hovered = true;
            }

            // if the mouse is over the space
            if (hovering)
            {
                // if a body has been selected
                if (selectedBody is not null)
                {
                    // shows the mouse is over the space
                    bodyGapPosition = i;

                    // if the mouse is pressed
                    if (mouseClicked)
                    {
                        // adds the body back into the list
                        bodies.Insert(i, selectedBody);
                        bodiesHashes.Insert(i, selectedBodyHash);

                        // shows that a body has been changed
                        bodyChanged = true;

                        // resets the selected body
                        selectedBody = null;
                        selectedBodyHash = 0;
                    }
                }
                
                // if no body has been selected
                else if (i < bodies.Count)
                {
                    // if button pressed
                    if (mouseClicked)
                    {
                        // select that body
                        selectedBody = bodies[i];
                        selectedBodyHash = bodiesHashes[i];

                        selectedBodyPreviousSpot = i;

                        // remove that body from the list
                        bodies.Remove(selectedBody);
                        bodiesHashes.Remove(selectedBodyHash);
                    }
                    else
                    {
                        // get the old colour
                        Color oldColour = bodies[i].GetComponent<SpriteRenderer>().color;

                        // makes the object partially transparent
                        bodies[i].GetComponent<SpriteRenderer>().color = new Color(oldColour.r, oldColour.g, oldColour.b, 0.7f);
                    }
                }
            }

            // increases the value of the x for the next space
            x += seperationValue;
        }

        // if a body is selected
        if (selectedBody is not null)
        {
            // if no space is being hovered over
            if (!hovered)
            {
                // if mouse is clicked
                if (mouseClicked)
                {
                    // adds the body back into the list at previous spot
                    bodies.Insert(selectedBodyPreviousSpot, selectedBody);
                    bodiesHashes.Insert(selectedBodyPreviousSpot, selectedBodyHash);

                    selectedBody = null;
                    selectedBodyHash = 0;
                }
            }
        }
    }

    private void SetBodiesPositions()
    {
        Vector2 position = StartingSpot;

        // goes through each body
        int i = -1;
        foreach (GameObject body in bodies)
        {
            i++;

            if (i == bodyGapPosition && selectedBody is not null)
            {
                position.x += seperationValue;
            }

            body.transform.position = position;

            position.x += seperationValue;
        }

        // if there is a selected body, draws it at the mouse position
        if (selectedBody is not null)
        {
            selectedBody.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void Hide()
    {
        // if a body is still selected, put it back
        if (selectedBody is not null)
        {
            bodies.Insert(selectedBodyPreviousSpot, selectedBody);
            bodiesHashes.Insert(selectedBodyPreviousSpot, selectedBodyHash);
        }


        // if something has been changed
        if (bodyChanged)
        {
            TriggerManager.PreBodyMoveTrigger.CallTrigger(0);

            // gets the current arrangment of bodies
            List<BodyController> order = new List<BodyController>();

            foreach (int hash in bodiesHashes)
            {
                BodyController currentBody = ItemManager.headController.head;
                bool found = false;

                while (currentBody is not null)
                {
                    if (currentBody.gameObject.GetHashCode() == hash)
                    {
                        found = true;
                        break;
                    }

                    currentBody = currentBody.next;
                }

                if (!found)
                {
                    throw new System.Exception("Body not found with hash");
                }

                order.Add(currentBody);
            }
            
            ItemManager.headController.Rearrange(order);

            TriggerManager.PostBodyMoveTrigger.CallTrigger(0);
        }

        // kills all the bodies
        foreach (GameObject bodyObj in bodies)
        {
            Destroy(bodyObj);
        }

        if (selectedBody)
        {
            Destroy(selectedBody);
        }

        bodies.Clear();
        bodiesHashes.Clear();
        selectedBody = null;
        selectedBodyHash = 0;
    }
}
