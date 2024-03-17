using System.Collections.Generic;
using TMPro;
using UnityEngine;

// COMPLETE

/// <summary>
/// The base class for all managers that control the canvas
/// </summary>
public abstract class CanvasManager : MonoBehaviour
{
    /// <summary>
    /// The canvas that the manager controls
    /// </summary>
    protected Canvas canvas;

    /// <summary>
    /// The text attatched to the buttons
    /// </summary>
    protected List<TextMeshProUGUI> buttonTexts = new List<TextMeshProUGUI>();

    /// <summary>
    /// Whether the canvas is active or not
    /// </summary>
    protected bool active = false;

    /// <summary>
    /// The number of options in the canvas
    /// </summary>
    public int optionsNum;

    // Called by unity when the object is created
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

    /// <summary>
    /// Called to show the canvas
    /// </summary>
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

    /// <summary>
    /// Called to hide the canvas
    /// </summary>
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

    /// <summary>
    /// Sets the text of a button
    /// </summary>
    /// <param name="index">The index of the button in the list of texts</param>
    /// <param name="text">The text</param>
    public void SetButtonText(int index, string text)
    {
        buttonTexts[index].text = text;
    }

    /// <summary>
    /// Called when a button is clicked
    /// </summary>
    /// <param name="button">The number to indicate which button</param>
    public virtual void ButtonClicked(int button)
    {

    }
}
