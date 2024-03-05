using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailColliderController : MonoBehaviour
{
    bool isHit = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("trail hit:" + isHit + "by: " + collidingObjectName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collision entered by " + collision.name);
        //if (collision.gameObject.name == "Player")
        //{
        //    isHit = true;
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("collision exited by " + collision.name);
        //collidingObjectName = collision.name;
        //if (collision.gameObject.name == "Player")
        //{
        //    isHit = false;
        //}
    }
}
