using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailColliderController : MonoBehaviour
{
    private bool isTrailEmitting = false;
    [Header("Events")]
    public GameEvent onTrailEntered;
    public GameEvent onTrailExited;

    void Start()
    {
        
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.name == "TrailCollider" && !isTrailEmitting) 
        {
            Debug.Log("trailcollider collision, this is OK");
        }
        else
        {
            Debug.Log("NOT OK");
        }
        onTrailEntered.Raise(this, "");

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("collision exited by " + collision.name);
        onTrailExited.Raise(this, "");
    }

    public void SetTrailEmitterFlag(Component sender, object data)
    {
        if (data is TrailRenderer)
        {
            var trail = (TrailRenderer)data;
            if (trail.emitting)
            {
                isTrailEmitting = true;
            }
            else
            {

                isTrailEmitting = false;
            }

        }
    }
}
