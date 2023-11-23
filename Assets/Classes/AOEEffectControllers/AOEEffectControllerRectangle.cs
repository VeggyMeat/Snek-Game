using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEEffectControllerRectangle : AOEEffectController
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
        transform.localScale = new Vector3 (height, width, 1);

        transform.rotation = Quaternion.Euler(0, 0, angle);

        Setup(timeAlive, decay, colour);
    }
}
