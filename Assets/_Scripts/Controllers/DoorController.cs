using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Events")]
    public GameEvent onPlayerEnterDoor;

    private bool isOpened = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isOpened)
        {
            isOpened = true;
            Debug.Log("player entered door " + gameObject.name);
            onPlayerEnterDoor.Raise(this, gameObject);
            //trigger entering another room, blacking out current room, realign camera           
        }
    }
}
