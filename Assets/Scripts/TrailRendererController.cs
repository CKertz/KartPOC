using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRendererController : MonoBehaviour
{
    TrailRenderer trail;
    EdgeCollider2D edgeCollider;
    bool isTrailPointSetting = true;
    private int trailOffsetNumber = 0;
    private int trailOffsetEndNumber = 0;
    List<List<Vector2>> removalPointsTracker = new List<List<Vector2>>();

    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        edgeCollider = GameObject.Find("TrailCollider").GetComponent<EdgeCollider2D>();
    }

    void Update()
    {
        if (isTrailPointSetting)
        {
            SetColliderPointsFromTrail(trail, edgeCollider);
        }
        Debug.Log(trail.positionCount);
    }

    private void SetColliderPointsFromTrail(TrailRenderer trailRenderer, EdgeCollider2D collider)
    {
        List<Vector2> points = new List<Vector2>();
        
        // trims the top of the trailrenderer from creating collider points ahead of the trail. introducing an offset to cut off the top
        int pointOffsetNumber = 7;
        
        for(int i = trailOffsetEndNumber; i < trailRenderer.positionCount - pointOffsetNumber; i++)
        {
            points.Add(trailRenderer.GetPosition(i));
        }
        collider.SetPoints(points);
    }

    private void RemoveColliderPointsForInvisibleTrail(TrailRenderer trailRenderer)
    {
        //trailoffsetnumber - trailoffsetendnumber = the points needed to be removed.
        //each time the toggle happens, need to add a new list of these poitns to master list, and when toggled
        //back on, iterate over master list and remove each chunk of points

        //..can you remove a subset of points?
    }

    public void ToggleTrailrendererPoints(Component sender, object data)
    {
        if (data is TrailRenderer)
        {
            var trail = (TrailRenderer) data;
            if(trail.emitting)
            {
                isTrailPointSetting = true;
                trailOffsetEndNumber = trail.positionCount;
            }
            else
            {
                isTrailPointSetting = false;
                trailOffsetNumber = trail.positionCount;
                Debug.Log("OFFSET NUMBER:" +trailOffsetNumber);

            }
            Debug.Log(isTrailPointSetting);

        }
        //Debug.Log("test hit!" + data + "another test:"+ sender.gameObject.name);
    }
}
