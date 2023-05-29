using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Random = UnityEngine.Random;

public class ShopManager : MonoBehaviour
{
    public Canvas canvasPrefab;

    public Vector2[] buttonPositions;
    public Vector2 buttonSize;

    internal ShopState state;

    public GameObject buttonPrefab;

    private UnityAction[] actions;

    private HeadController head;

    private string[] options;

    private GameObject canvas;

    private List<string> bodies;

    private int stacks;

    private bool active;

    internal void Setup(HeadController head)
    {
        this.head = head;

        bodies = head.bodies;
    }

    internal void MakeShop(bool self = false)
    {
        if (!self)
        {
            // adds to the number of stacks
            stacks++;
        }

        if (active)
        {
            // will do the overflow later
            return;
        }

        active = true;

        // stops time
        Time.timeScale = 0;

        // creates the canvas
        canvas = Instantiate(canvasPrefab.gameObject, Vector3.zero, Quaternion.identity);

        // sets up the functions that are called when buttons are pressed
        actions = new UnityAction[3] {FuncOne, FuncTwo, FuncThree};

        // gets the options displayed
        options = new string[] { bodies[Random.Range(0, bodies.Count)], bodies[Random.Range(0, bodies.Count)], bodies[Random.Range(0, bodies.Count)] };

        // creates each button
        int i = 0;
        foreach (Vector2 buttonPos in buttonPositions)
        {
            // increments the counter
            i++;

            // creates the button
            GameObject button = Instantiate(buttonPrefab, buttonPos, Quaternion.identity);

            // sets the canvas as the button's parent
            button.transform.SetParent(canvas.transform, false);

            // sets the size of the button
            button.GetComponent<RectTransform>().sizeDelta = buttonSize;

            // gets the button component
            Button buttonBit = button.GetComponent<Button>();

            // sets the text
            buttonBit.GetComponentInChildren<TextMeshProUGUI>().text = options[i - 1];

            // adds the listener function to it
            buttonBit.onClick.AddListener(actions[i - 1]);
        }
    }

    private void FuncOne()
    {
        ButtonPressed(0);
    }

    private void FuncTwo() 
    {
        ButtonPressed(1);
    }

    private void FuncThree()
    {
        ButtonPressed(2);
    }

    private void ButtonPressed(int num)
    {
        Destroy(canvas);

        head.AddBody(options[num]);

        // restarts time
        Time.timeScale = 1;

        stacks--;

        active = false;
        
        if (stacks > 0)
        {
            MakeShop(true);
        }
    }
}
