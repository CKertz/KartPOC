using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isTrailEmitting = false;
    private bool isTrailColliding = false;
    private List<Surface> surfaces = new List<Surface>();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetTrailEmitterFlag(Component sender, object data)
    {
        if (data is TrailRenderer)
        {
            var trail = (TrailRenderer) data;
            if (trail.emitting)
            {
                isTrailEmitting = true;
            }
            else
            {

                isTrailEmitting = false;
            }
            Debug.Log("gamemanager setting trailemitting to:" + isTrailEmitting);
        }
    }

    public void SetTrailCollidingFlag(Component sender, object data)
    {
        Debug.Log("gamemanagercollision hit:" + sender.name);
        if (data is Collider2D)
        {
            var collision = (Collider2D) data;

            Debug.Log("gamemanager collision:" + collision.name);
        }
    }

    public void HandleSurface(Component sender, object data)
    {
        if(data is string)
        {
            var surface = (string) data;
            Debug.Log("handle surface:" + surface);

        }
    }
}
