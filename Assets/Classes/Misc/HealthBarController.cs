using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    // created by an object that wants a healthBar, as a child of that object
    [SerializeField] private GameObject greenBarObject;
    private Vector3 initialScale;

    private void Update()
    {
        // resets rotation
        transform.rotation = Quaternion.identity;
    }

    internal void Setup(float scaleX, float scaleY)
    {
        // set the scale of the healthbar
        transform.localScale = new Vector3 (scaleX * transform.localScale.x, scaleY * transform.localScale.y, 1);

        // sets the initialScale for a full healthBar
        initialScale = greenBarObject.transform.localScale;
    }

    internal void SetBar(float percentage)
    {
        // TEMP fixes out of bounds values
        if (percentage > 1)
        {
            percentage = 1;
        }
        else if (percentage < 0)
        {
            percentage = 0;
        }

        // calculate the new position of the bar, so that it stays on the left
        float position = initialScale.x * (1 - percentage) / 2;

        // sets the new position of the green bar of the health bar
        greenBarObject.transform.localPosition = new Vector3(position, greenBarObject.transform.localPosition.y, greenBarObject.transform.localPosition.z);

        // sets the new scale of the green bar of the health bar
        greenBarObject.transform.localScale = new Vector3(initialScale.x * percentage, greenBarObject.transform.localScale.y, greenBarObject.transform.localScale.z);
    }
}
