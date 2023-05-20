using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEEffectController : MonoBehaviour
{
    private float timeAlive;
    private float radius;

    // Called by the object that spawns the AOE effect to set up variables (Circle)
    internal void Setup(float timeAlive, float radius)
    {
        this.timeAlive = timeAlive;
        this.radius = radius;
    }

    // Start is called before the first frame update
    void Start()
    {
        // sets the size of the object
        transform.localScale = new Vector3(radius, radius, 1);
        
        // sets a countdown clock to destory the object
        Invoke(nameof(OnDestroy), timeAlive);
    }

    // Called when its destroyed
    private void OnDestroy()
    {
        // destroys the object
        Destroy(gameObject);
    }
}
