using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    public float bulletSpeed = 5f; // Speed of the bullet

    private List<GameObject> enemiesInRange = new List<GameObject>();

    private bool inRange = false;
    private float timer = 0f;
    private float interval = 0.5f;
    private Transform target;
    private TextMeshProUGUI enemiesInRangeText;

    void Start()
    {
        UpdateGunDebuggerProperties();
    }

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
    //TODO: remove enemiesInRange when they die. event/subscription OnEnemyDeath?
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            inRange = true;
            if (!enemiesInRange.Contains(other.gameObject))
            {
                enemiesInRange.Add(other.gameObject);
                UpdateGunDebuggerProperties();

                if (enemiesInRange.Count > 1)
                {
                    target = GetClosestEnemy().transform;
                    Debug.Log("multiple enemies in range, target selected is" +  target.name);
                }
                //other.gameObject.GetComponent<EnemyController>().InflictDamage();
            }
            //target = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (enemiesInRange.Contains(other.gameObject))
            {
                enemiesInRange.Remove(other.gameObject);
                UpdateGunDebuggerProperties();

            }
            inRange = false;
            timer = 0f; // Reset the timer when the objects exit collision range
            target = null;
        }
    }

    private GameObject GetClosestEnemy()
    {
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemiesInRange)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    private void UpdateGunDebuggerProperties()
    {
        var debuggerOverlayCanvas = GameObject.Find("DebuggerUICanvas");

        if (debuggerOverlayCanvas != null)
        {
            var enemiesInRangeObject = debuggerOverlayCanvas.transform.Find("EnemiesInRange");
            enemiesInRangeText = enemiesInRangeObject.GetComponent<TextMeshProUGUI>();

            string objectsNames = "";
            foreach (GameObject obj in enemiesInRange)
            {
                objectsNames += obj.name + ", ";
            }
            enemiesInRangeText.text = $"Enemies In Range: {objectsNames}";
        }
        else
        {
            Debug.LogError("DebuggerUICanvas object not found!");
        }
    }
}
