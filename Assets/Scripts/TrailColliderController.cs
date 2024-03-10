using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailColliderController : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.name == "TrailCollider") // && !trailrenderer.emitting .. set up eventlistener?
        {
            Debug.Log("trailcollider collision, this is OK");
        }
        else
        {
            Debug.Log("NOT OK");
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("collision exited by " + collision.name);
    }
}
