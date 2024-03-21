using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isTrailEmitting = false;
    private bool isTrailColliding = false;
    private List<Surface> surfaces = new List<Surface>();
    private string currentSurface;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetTrailEmitterFlag(Component sender, object data)
    {
        if (data is TrailRenderer)
        {
            var trail = (TrailRenderer) data;
            if (trail.emitting)
            {
                isTrailEmitting = true;
            }
            else
            {

                isTrailEmitting = false;
            }
            Debug.Log("gamemanager setting trailemitting to:" + isTrailEmitting);
        }
    }

    public void SetTrailCollidingFlagEnter(Component sender, object data)
    {
        isTrailColliding = true;
        currentSurface = "Trail";
        Debug.Log("istrailColliding =:" + isTrailColliding);
        Debug.Log("currentsurface: " + currentSurface);
    }

    public void SetTrailCollidingFlagExit(Component sender, object data)
    {
        isTrailColliding = false;
        Debug.Log("istrailColliding =:" + isTrailColliding);
        Debug.Log("currentsurface: " + currentSurface);
    }

    public void HandleSurface(Component sender, object data)
    {
        if(data is string)
        {
            if(!isTrailColliding)
            {
                currentSurface = (string) data;
                Debug.Log("currentsurface:" + currentSurface);
            }
            else
            {
                currentSurface = "Trail";
            }
            UpdateSurfaceList();
        }
        Debug.Log("currentSurface in gameManager: " + currentSurface);
    }

    private void UpdateSurfaceList()
    {
        bool isUnique = true;
        foreach(var surface in surfaces)
        {
            if(surface.name == currentSurface)
            {
                isUnique = false;
            }
        }
        if(isUnique)
        {
            Surface newSurface = new Surface();
            newSurface.name = currentSurface;

            surfaces.Add(newSurface);
            Debug.Log("new surface added:" +  newSurface.name);
        }
    }

    private bool CalculateScoreStatus()
    {
        //TODO: algorithm for determining if ok to update score
        bool scoreStatus = false;
        if(!isTrailEmitting)
        {
            return scoreStatus;
        }

        return scoreStatus;
    }
}
