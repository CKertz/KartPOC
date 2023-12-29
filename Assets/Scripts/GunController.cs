using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    public float bulletSpeed = 5f; // Speed of the bullet


    private bool inRange = false;
    private float timer = 0f;
    private float interval = 0.5f;
    private Transform target; 


    void Update()
    {
        if (inRange)
        {
            timer += Time.deltaTime; // Increment the timer while in range

            if (timer >= interval)
            {
                // Perform actions after every 1 second in range
                FireGun();

                timer = 0f; // Reset the timer after executing the function
            }
        }
    }

    private void FireGun()
    {
        if (target != null && bulletPrefab != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * bulletSpeed;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            inRange = true;
            target = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            inRange = false;
            timer = 0f; // Reset the timer when the objects exit collision range
            target = null;
        }
    }
}
