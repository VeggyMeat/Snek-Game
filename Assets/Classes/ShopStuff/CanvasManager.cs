using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    protected Canvas canvas;
    protected List<TextMeshProUGUI> buttonTexts = new List<TextMeshProUGUI>();
    protected bool active = false;

    public int optionsNum;

    public void Awake()
    {
        // grabs the canvas and hides it
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;

        // gets the text attatched to the buttons
        for (int i = 0; i < optionsNum; i++)
        {
            Transform child = transform.GetChild(i);
            buttonTexts.Add(child.GetChild(0).GetComponent<TextMeshProUGUI>());
        }
    }

    public virtual void ShowButtons()
    {
        if (active)
        {
            // if the canvas is already active, don't do anything
            return;
        }

        // shows the canvas
        canvas.enabled = true;
        active = true;
    }

    public virtual void HideButtons()
    {
        if (!active)
        {
            // canvas already not active, don't do anything
            return;
        }
        // hides the canvas
        canvas.enabled = false;
        active = false;
    }

    public void SetButtonText(int index, string text)
    {
        buttonTexts[index].text = text;
    }

    public virtual void ButtonClicked(int button)
    {

    }
}
