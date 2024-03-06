using System;
using UnityEngine;

// COMPLETE

/// <summary>
/// The script that is placed on the health bar object to control the health bar
/// It is used to set the health bar to a certain percentage
/// </summary>
internal class HealthBarController : MonoBehaviour
{
    /// <summary>
    /// The gameObject of the green bar within the health bar
    /// </summary>
    [SerializeField] private GameObject greenBarObject;
    private Vector3 initialScale;

    // called by unity every frame
    private void Update()
    {
        // resets the rotation of the health bar, so that it is always horizontal regardless of the parent's rotation
        transform.rotation = Quaternion.identity;
    }

    /// <summary>
    /// Sets up the health bar with a certain scale
    /// </summary>
    /// <param name="scaleX">The horizontal scale of the healthbar</param>
    /// <param name="scaleY">The vertical scale of the healthbar</param>
    internal void Setup(float scaleX, float scaleY)
    {
        // set the scale of the healthbar
        transform.localScale = new Vector3 (scaleX * transform.localScale.x, scaleY * transform.localScale.y, 1);

        // sets the initialScale for a full healthBar
        initialScale = greenBarObject.transform.localScale;
    }

    /// <summary>
    /// Sets the health bar's visual representation to a certain percentage of full health
    /// </summary>
    /// <param name="percentage">The percentage to set the healthbar to (0-1)</param>
    /// <exception cref="Exception">Throws an exception when percentage is not within 0-1</exception>
    internal void SetBar(float percentage)
    {
        // throws an error if out of bounds
        if (percentage > 1)
        {
            throw new Exception("Health bar percentage cannot be greater than 1");
        }
        else if (percentage < 0)
        {
            throw new Exception("Health bar percentage cannot be less than 0");
        }

        // calculate the new position of the bar, so that it stays on the left
        float position = initialScale.x * (1 - percentage) / 2;

        // sets the new position of the green bar of the health bar
        greenBarObject.transform.localPosition = new Vector3(position, greenBarObject.transform.localPosition.y, greenBarObject.transform.localPosition.z);

        // sets the new scale of the green bar of the health bar
        greenBarObject.transform.localScale = new Vector3(initialScale.x * percentage, greenBarObject.transform.localScale.y, greenBarObject.transform.localScale.z);
    }
}
