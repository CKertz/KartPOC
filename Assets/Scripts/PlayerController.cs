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

    private TextMeshProUGUI distanceTraveledText;
    private TextMeshProUGUI currentSurfaceText;

    private Vector3 lastPosition;
    private float totalDistanceTraveled;
    private string currentSurface;

    private List<Surface> surfaces = new List<Surface>();

    private bool isOnField = false;
    private bool isHarvesting = true;
    private bool isPlayerMoving = false;

    public float cellSize = 0.5f; 
    private HashSet<Vector2> visitedPositions = new HashSet<Vector2>();

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
        GetHarvesterEdges();
        Vector2 currentCell = WorldToCell(transform.position);
        if (!visitedPositions.Contains(currentCell))
        {
            //todo: this will get currentpos of player, but not entire trailrenderer. find a way to get trailrenderer width.
            // cannot attach a bnoxcollider to trailrenderer. maybe get dimensions of the trail texture? possibly just the x1 and x2
            // coords because the y can just be 1 pixel tall
            Debug.Log("Unique position detected at: " + currentCell);
            visitedPositions.Add(currentCell);
        }

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

    void GetHarvesterEdges()
    {
        // Get the sprite's bounds
        Bounds spriteBounds = harvestOutlineSprite.bounds;

        // Get the position of the sprite
        Vector3 spritePosition = harvestOutlineSprite.transform.position;

        // Calculate edges
        float leftEdge = spritePosition.x - spriteBounds.extents.x;
        float rightEdge = spritePosition.x + spriteBounds.extents.x;
        float bottomEdge = spritePosition.y - spriteBounds.extents.y;
        float topEdge = spritePosition.y + spriteBounds.extents.y;

        // Print or use the edge coordinates
        Debug.Log("Left Edge: " + leftEdge);
        Debug.Log("Right Edge: " + rightEdge);
        Debug.Log("Bottom Edge: " + bottomEdge);
        Debug.Log("Top Edge: " + topEdge);
    }


    private Vector2 WorldToCell(Vector3 worldPos)
    {
        // rounding it to 1 decimal point for now, if more precision is needed it can always be adjusted
        float x = (float)Math.Round(worldPos.x * cellSize, 1);
        float y = (float)Math.Round(worldPos.y * cellSize, 1);
        //Debug.Log("x:" + x+ " Y:"+y);

        return new Vector2(x, y);
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        UpdateHarvesterRotation(horizontalInput, verticalInput);

        if (horizontalInput == 0 && verticalInput == 0)
        {
            isPlayerMoving = false;
        }
        else
        {
            isPlayerMoving = true;
        }
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f) * moveSpeed * Time.deltaTime;

        transform.Translate(movement);

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
    }

    // only called when isHarvesting == true
    private void UpdateScore()
    {
        //check if the surface in contact is a new surface. add to list if so and keep updating while on top of it
        //use this https://www.youtube.com/watch?v=aPXvoWVabPY to help with surface handling

        //if vehicle is moving and on never touched area -- i think a rendered trail can be set as a layer? 
        if(isPlayerMoving)
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
