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
        Move();
        UpdateDebuggerProperties();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleHarvester();
        }
        if (isHarvesting && isPlayerMoving)
        {
            UpdateScore();
        }

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

    private void UpdateScore()
    {
        //check if the surface in contact is a new surface. add to list if so and keep updating while on top of it
        //use this https://www.youtube.com/watch?v=aPXvoWVabPY to help with surface handling

        List<Vector2> currentCellsCovered = GetCurrentHarvesterCoveredPositions();
        
        foreach (Vector2 cell in currentCellsCovered)
        {
            Debug.Log("scanning cell with coords:" + cell.x + "," + cell.y);
            if (!visitedPositions.Contains(cell))
            {
                visitedPositions.Add(cell);
            }
        }
        //surfaces[0].totalScore += totalDistanceTraveled * surfaces[0].scoreModifier;
    }

    private List<Vector2> GetCurrentHarvesterCoveredPositions()
    {
        List<Vector2> edgeCoordinates = new List<Vector2>();

        // Get the sprite's bounds
        Bounds spriteBounds = harvestOutlineSprite.bounds;

        // Get the position of the sprite
        Vector3 spritePosition = harvestOutlineSprite.transform.position;

        // Calculate edges
        
        float leftEdge = (float)Math.Round(spritePosition.x - spriteBounds.extents.x, 1);
        float rightEdge = (float)Math.Round(spritePosition.x + spriteBounds.extents.x, 1);
        float bottomEdge = (float)Math.Round(spritePosition.y - spriteBounds.extents.y, 1);
        float topEdge = (float)Math.Round(spritePosition.y + spriteBounds.extents.y, 1);

        // Print or use the edge coordinates
        //Debug.Log("Left Edge: " + leftEdge);
        //Debug.Log("Right Edge: " + rightEdge);
        //Debug.Log("Bottom Edge: " + bottomEdge);
        //Debug.Log("Top Edge: " + topEdge);

        for (float x = leftEdge; x <= rightEdge; x++)
        {
            // Calculate corresponding y-coordinate for each x-coordinate
            float y = spritePosition.y + (spriteBounds.extents.y * Mathf.Sin(Mathf.Acos((x - spritePosition.x) / spriteBounds.extents.x)));
            edgeCoordinates.Add(new Vector2(x, y));
        }

        return edgeCoordinates;

    }


    private Vector2 WorldToCell(Vector2 worldPos)
    {
        // rounding it to 1 decimal point for now, if more precision is needed it can always be adjusted
        float x = (float)Math.Round(worldPos.x * cellSize, 1);
        float y = (float)Math.Round(worldPos.y * cellSize, 1);
        //Debug.Log("x:" + x+ " Y:"+y);

        return new Vector2(x, y);
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
