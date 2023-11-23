using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEEffectControllerCircle : AOEEffectController
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="timeAlive">How long the AOEEffect should stay for</param>
    /// <param name="decay">Whether the AOEEffect should fade over time</param>
    /// <param name="colour">The AOEEffect's colour</param>
    /// <param name="radius"></param>
    internal void Setup(float timeAlive, bool decay, Color colour, float radius)
    {
        transform.localScale = new Vector3 (radius * 2, radius * 2, 1);

        Setup(timeAlive, decay, colour);
    }
}
