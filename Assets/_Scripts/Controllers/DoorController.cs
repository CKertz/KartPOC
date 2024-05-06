using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Events")]
    public GameEvent onPlayerEnterDoor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("player entered door " + gameObject.name);
            onPlayerEnterDoor.Raise(this, gameObject);
            //trigger entering another room, blacking out current room, realign camera
        }
    }
}
