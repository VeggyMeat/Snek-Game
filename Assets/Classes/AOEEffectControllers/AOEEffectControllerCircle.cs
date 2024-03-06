using UnityEngine;

// COMPLETE

/// <summary>
/// The script that is placed on the circle AOEEffect objects to control its behaviour
/// </summary>
internal class AOEEffectControllerCircle : AOEEffectController
{
    /// <summary>
    /// Sets up the AOEEffect
    /// </summary>
    /// <param name="timeAlive">How long the AOEEffect should stay for</param>
    /// <param name="decay">Whether the AOEEffect should fade over time</param>
    /// <param name="colour">The AOEEffect's colour</param>
    /// <param name="radius">The radius of the circle</param>
    internal void Setup(float timeAlive, bool decay, Color colour, float radius)
    {
        // sets the scale of the AOEEffect
        transform.localScale = new Vector3 (radius * 2, radius * 2, 1);

        // calls the setup method from the parent class
        Setup(timeAlive, decay, colour);
    }
}
