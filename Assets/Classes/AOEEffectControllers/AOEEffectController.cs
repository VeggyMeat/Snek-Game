using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AOEEffectController : MonoBehaviour
{
    protected float timeAlive;

    protected SpriteRenderer spriteRenderer;

    protected DateTime startTime;

    protected Color startColour;

    protected bool decay;

    /// <summary>
    /// Sets up the AOEEffect
    /// </summary>
    /// <param name="timeAlive">How long the AOEEffect should stay for</param>
    /// <param name="decay">Whether the AOEEffect should fade over time</param>
    /// <param name="colour">The AOEEffect's colour</param>
    protected void Setup(float timeAlive, bool decay, Color colour)
    {
        // sets up the variables
        this.timeAlive = timeAlive;
        this.decay = decay;
        startColour = colour;

        startTime = DateTime.Now;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = colour;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // sets a countdown clock to destory the object
        Invoke(nameof(OnDestroy), timeAlive);
    }

    // Called when its destroyed
    protected virtual void OnDestroy()
    {
        // destroys the object
        Destroy(gameObject);
    }

    protected virtual void Update()
    {
        // if the object is decaying
        if (decay)
        {
            // gets the time alive so far
            TimeSpan difference = DateTime.Now - startTime;
            
            // gets the percentage of life lived
            float lifePercentage = (float)difference.TotalSeconds / timeAlive;

            // gets the new opacity of the AOEEffect
            float newOpacity = (1 - lifePercentage) * startColour.a;

            // reduces the alpha value of the colour
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newOpacity);
        }
    }
}
