using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public Rigidbody2D selfRigid;
    public Vector3 movement;
    public float lifeSpan;
    private float timeAlive = 0f;

    // Start is called before the first frame update
    void Start()
    {
        selfRigid.velocity = movement;
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;
        UnityEngine.Debug.Log(timeAlive);
        UnityEngine.Debug.Log(lifeSpan);
        if (timeAlive >= lifeSpan)
        {
            Destroy(gameObject);
        }
    }
}
