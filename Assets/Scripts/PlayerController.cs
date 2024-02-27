using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] 
    float moveSpeed;

    [SerializeField]
    float accelerationPower;

    [SerializeField]
    TrailRenderer trailRenderer;

    private TextMeshProUGUI distanceTraveledText;
    private TextMeshProUGUI currentSurfaceText;

    private Vector3 lastPosition;
    private float totalDistanceTraveled;
    private string currentSurface;

    private List<Surface> surfaces = new List<Surface>();

    private bool isOnField = false;
    private bool isHarvesting = true;
    private bool isVehicleMoving = false;

    void Start()
    {
        lastPosition = transform.position;

        //TODO: remove temp test surface, just hardcoding one in for now
        Surface testSurface = new Surface();
        testSurface.name = "Basic Field";
        testSurface.isHarvestable = true;
        testSurface.scoreModifier = 0.1f;
        surfaces.Add(testSurface);
    }


    void Update()
    {
        Move();
        UpdateDebuggerProperties();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleHarvester();
        }
        if (isHarvesting)
        {
            UpdateScore();
        }

    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if(horizontalInput == 0 && verticalInput == 0)
        {
            isVehicleMoving = false;
            //Debug.Log("vehiclemoving:" + isVehicleMoving);
        }
        else
        {
            isVehicleMoving = true;
            //Debug.Log("vehiclemoving:" + isVehicleMoving);

        }
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f) * moveSpeed * Time.deltaTime;

        transform.Translate(movement);

    }

    private void ToggleHarvester()
    {
        if (!trailRenderer.emitting)
        {
            isHarvesting = true;
            trailRenderer.emitting = true;
        }
        else
        {
            isHarvesting = false;
            trailRenderer.emitting = false;
        }
    }

    // only called when isHarvesting == true
    private void UpdateScore()
    {
        //check if the surface in contact is a new surface. add to list if so and keep updating while on top of it
        //use this https://www.youtube.com/watch?v=aPXvoWVabPY to help with surface handling

        //if vehicle is moving and on never touched area -- i think a rendered trail can be set as a layer? 
        if(isVehicleMoving)
        {
            surfaces[0].totalScore += totalDistanceTraveled * surfaces[0].scoreModifier;
            //Debug.Log("total score for "+ surfaces[0].name+ ":"+surfaces[0].totalScore);
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
        GetCurrentSurface();
        if(isHarvesting)
        {
            totalDistanceTraveled += distanceTraveled;
        }

        lastPosition = transform.position;
        var debuggerOverlayCanvas = GameObject.Find("DebuggerUICanvas");

        if (debuggerOverlayCanvas != null)
        {
            var distanceTraveledObject = debuggerOverlayCanvas.transform.Find("DistanceTraveled");
            distanceTraveledText = distanceTraveledObject.GetComponent<TextMeshProUGUI>();
            distanceTraveledText.text = $"Distance traveled: {totalDistanceTraveled}";

            var currentSurfaceObject = debuggerOverlayCanvas.transform.Find("CurrentSurface");
            currentSurfaceText = currentSurfaceObject.GetComponent<TextMeshProUGUI>();
            currentSurfaceText.text = $"Current surface: {currentSurface}";
        }
        else
        {
            Debug.LogError("DebuggerUICanvas object not found!");
        }
    }

    private void GetCurrentSurface()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("FieldSurface")); // Assuming sprites are on a specific layer

        if (hit.collider != null)
        {
            currentSurface = hit.collider.gameObject.tag;
        }
    }

}
