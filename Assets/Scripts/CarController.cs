using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    [SerializeField] 
    float moveSpeed;

    [SerializeField]
    float accelerationPower;

    [SerializeField]
    TrailRenderer trailRenderer;

    private TextMeshProUGUI distanceTraveledText;
    private Vector3 lastPosition;
    private float totalDistanceTraveled;

    private bool isOnField = false;
    private bool isTrailEmitting = true;

    private string landingZoneTag = "LandingZone";

    void Start()
    {
        lastPosition = transform.position;
    }


    void Update()
    {
        Move();
        UpdateDebuggerProperties();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleHarvester();
        }
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f) * moveSpeed * Time.deltaTime;

        transform.Translate(movement);

    }

    private void ToggleHarvester()
    {
        if (!trailRenderer.emitting)
        {
            isTrailEmitting = true;
            trailRenderer.emitting = true;
        }
        else
        {
            isTrailEmitting = false;
            trailRenderer.emitting = false;
        }
    }


    //TODO: the trail disables when the gun's circle collider collides, need to filter that out eventually
    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    Debug.Log("exiting");
    //    if(other.gameObject.tag == landingZoneTag)
    //    {
    //        gameObject.GetComponent<TrailRenderer>().enabled = true;
    //    }
    //}


    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    Debug.Log("entering");

    //    if (other.gameObject.tag == landingZoneTag)
    //    {
    //        gameObject.GetComponent<TrailRenderer>().enabled = false;
    //    }
    //}

    private void UpdateDebuggerProperties()
    {
        float distanceTraveled = Vector3.Distance(transform.position, lastPosition);


        totalDistanceTraveled += distanceTraveled;

        lastPosition = transform.position;
        var debuggerOverlayCanvas = GameObject.Find("DebuggerUICanvas");

        if (debuggerOverlayCanvas != null)
        {
            var distanceTraveledObject = debuggerOverlayCanvas.transform.Find("DistanceTraveled");
            distanceTraveledText = distanceTraveledObject.GetComponent<TextMeshProUGUI>();
            
            distanceTraveledText.text = $"Distance traveled: {totalDistanceTraveled}";
        }
        else
        {
            Debug.LogError("DebuggerUICanvas object not found!");
        }
    }
}
