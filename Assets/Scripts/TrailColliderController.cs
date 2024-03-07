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
        Debug.Log("collision entered by " + collision.name);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("collision exited by " + collision.name);
    }
}
