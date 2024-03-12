using System;
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

    [SerializeField]
    SpriteRenderer harvestOutlineSprite;

    [Header("Events")]
    public GameEvent onPlayerHarvesterChanged;

    private TextMeshProUGUI distanceTraveledText;
    private TextMeshProUGUI currentSurfaceText;

    private Vector3 lastPosition;
    private float totalDistanceTraveled;
    private string currentSurface;

    private List<Surface> surfaces = new List<Surface>();

    private bool isOnField = false;
    private bool isHarvesting = false;
    private bool isPlayerMoving = false;

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
        if (isHarvesting && isPlayerMoving)
        {
            //UpdateScore();
        }

    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //UpdateHarvesterRotation(horizontalInput, verticalInput);

        if (horizontalInput == 0 && verticalInput == 0)
        {
            isPlayerMoving = false;
        }
        else
        {
            isPlayerMoving = true;
            UpdateScore();
        }
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f) * moveSpeed * Time.deltaTime;

        transform.Translate(movement);

    }

    private void UpdateScore()
    {
        //check if the surface in contact is a new surface. add to list if so and keep updating while on top of it
        //use this https://www.youtube.com/watch?v=aPXvoWVabPY to help with surface handling
        
        //surfaces[0].totalScore += totalDistanceTraveled * surfaces[0].scoreModifier;
        var currentSurface = GetCurrentSurface();
        if (currentSurface != null)
        {

        }
    }

    private void UpdateHarvesterRotation(float horizontalInput, float verticalInput)
    {
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f).normalized * moveSpeed * Time.deltaTime;
        transform.position += movement;

        float angle = Mathf.Atan2(verticalInput, horizontalInput) * Mathf.Rad2Deg;

        if (horizontalInput != 0 || verticalInput != 0)
        {
            harvestOutlineSprite.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
        }
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
        onPlayerHarvesterChanged.Raise(this, trailRenderer);
    }

    public void HandleTrailColliderEnter()
    {
        Debug.Log("trail hit, playercontroller notified");
        currentSurface = "Trail"; // TODO:remove hardcoding, make a trail surface. also ignore if trail is being hit when collider.name == trailcollider?
        Debug.Log("currentsurface:" + currentSurface);
    }

    public void HandleTrailColliderExit()
    {
        Debug.Log("trail exited, playercontroller notified");
        currentSurface = GetCurrentSurface();
        Debug.Log("currentsurface:" + currentSurface);
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

    private string GetCurrentSurface()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("FieldSurface")); // Assuming sprites are on a specific layer

        if (hit.collider != null)
        {
            currentSurface = hit.collider.gameObject.tag;
            //Debug.Log(currentSurface);
            return currentSurface;
        }
        return null;
    }

}