using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private List<TextMeshProUGUI> buttonTexts = new List<TextMeshProUGUI>();

    private HeadController head;

    private List<string> options;

    private static List<string> bodies;

    private int stacks;

    private static bool timeActive = true;
    private static bool shopActive = true;

    internal void Setup(HeadController head)
    {
        // sets the headcontroller and gets the list of bodies
        this.head = head;
        bodies = head.bodies;

        // gets the text attatched to those buttons
        for (int i = 0; i < 3; i++)
        {
            Transform child = transform.GetChild(i);
            buttonTexts.Add(child.GetChild(0).GetComponent<TextMeshProUGUI>());
        }

        // hides the shop
        HideShop();
    }

    private void Update()
    {
        if (shopActive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // refresh shop options
                MakeBodyShop();
            }
        }
    }

    public void AddBodyShop()
    {
        // adds to the buffer of multiple level ups at once
        stacks++;

        // if a loop is currently running, it will handle it
        if (stacks > 1)
        {
            return;
        }

        // shows the shop and pauses time
        ShowShop();
        PauseTime();

        // creates the shop for bodies
        MakeBodyShop();
    }

    private void MakeBodyShop()
    {
        // makes a new set of options
        options = MakeOptions();
        UpdateButtonsText(options);
    }

    public void PauseTime()
    {
        if (timeActive)
        {
            timeActive = false;
            Time.timeScale = 0;
        }
    }

    public void ResumeTime()
    {
        if (!timeActive)
        {
            timeActive = true;
            Time.timeScale = 1;
        }
    }

    private void ShowShop()
    {
        if (!shopActive)
        {
            // shows the canvas
            gameObject.SetActive(true);
            shopActive = true;

            // pauses time
            PauseTime();
        }
    }

    private void HideShop()
    {
        if (shopActive)
        {
            // resumes time
            ResumeTime();

            // hides the canvas
            gameObject.SetActive(false);
            shopActive = false;
        }
    }

    internal void UpdateButtonsText(List<string> names)
    {
        // goes through each button and sets its text equal to the item
        int i = 0;
        foreach (TextMeshProUGUI text in buttonTexts)
        {
            text.text = names[i];
            i++;
        }
    }

    internal List<string> MakeOptions()
    {
        // creates the options displayed
        return new List<string> { bodies[Random.Range(0, bodies.Count)], bodies[Random.Range(0, bodies.Count)], bodies[Random.Range(0, bodies.Count)] };
    }

    public void ButtonPressed(int num)
    {
        // adds the body to the snake
        head.AddBody(options[num]);  

        stacks--;
        
        // if there are still stacks in the buffer, go again
        if (stacks > 0)
        {
            MakeBodyShop();
            return;
        }

        // resume time and hides the shop
        ResumeTime();
        HideShop();
    }
}
