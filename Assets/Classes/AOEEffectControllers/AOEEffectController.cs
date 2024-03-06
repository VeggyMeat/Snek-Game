using UnityEngine;

// COMPLETE

/// <summary>
/// Inherited by all AOEEffect controllers, that are placed on objects to control their behaviour
/// </summary>
internal abstract class AOEEffectController : MonoBehaviour
{
    /// <summary>
    /// The length of time in seconds that the AOEEffect will stay for
    /// </summary>
    protected float timeAlive;

    /// <summary>
    /// The sprite renderer of the AOEEffect
    /// </summary>
    protected SpriteRenderer spriteRenderer;

    /// <summary>
    /// The amount of game time (non-paused time) elapsed since the AOEEffect was created
    /// </summary>
    protected float elapedTime = 0;

    /// <summary>
    /// The starting colour of the AOEEffect
    /// </summary>
    protected Color startColour;

    /// <summary>
    /// Whether the AOEEffect's colour should fade over time
    /// </summary>
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

        // grabs the sprite renderer and sets the colour
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = colour;
    }

    // Called by unity before the first frame
    private void Start()
    {
        // sets a countdown clock to destory the object
        Invoke(nameof(Destroy), timeAlive);
    }

    // Called to destroy the object
    protected virtual void Destroy()
    {
        // destroys the object
        Destroy(gameObject);
    }

    // Called by unity every frame
    private void Update()
    {
        // if the game is paused, do nothing
        if (Time.timeScale == 0)
        {
            return;
        }

        // if the object is decaying
        if (decay)
        {   
            // adds the time since the last frame to the elapsed time
            elapedTime += Time.deltaTime;

            // gets the percentage of life lived
            float lifePercentage = (float)elapedTime / timeAlive;

            // gets the new opacity of the AOEEffect
            float newOpacity = (1 - lifePercentage) * startColour.a;

            // reduces the alpha value of the colour
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newOpacity);
        }
    }
}
