using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    Rigidbody2D bulletRigidbody2D;


    // Start is called before the first frame update
    void Start()
    {
        bulletRigidbody2D = GetComponent<Rigidbody2D>();
        //shoot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void shoot()
    {
        bulletRigidbody2D.velocity = new Vector2(0 , moveSpeed);       
    }

    private void OnBecameInvisible()
    {
        if (!GetComponent<Renderer>().isVisible)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("enemy hit");
            //reduce hp, check if hp 0 and destroy if not
        }
    }
}
