using UnityEngine;

// COMPLETE

/// <summary>
/// The script that is placed on the rectangle AOEEffect objects to control its behaviour
/// </summary>
internal class AOEEffectControllerRectangle : AOEEffectController
{
    /// <summary>
    /// Sets up the AOEEffect
    /// </summary>
    /// <param name="timeAlive">How long the AOEEffect should stay for</param>
    /// <param name="decay">Whether the AOEEffect should fade over time</param>
    /// <param name="colour">The AOEEffect's colour</param>
    /// <param name="angle">Angle in Degrees that its rotated</param>
    /// <param name="height">The height of the AOEEffect</param>
    /// <param name="width">The width of the AOEEffect</param>
    internal void Setup(float timeAlive, bool decay, Color colour, float angle, float height, float width)
    {
        // sets the scale of the AOEEffect
        transform.localScale = new Vector3 (height, width, 1);

        // sets the rotation of the AOEEffect
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // calls the setup method from the parent class
        Setup(timeAlive, decay, colour);
    }
}
