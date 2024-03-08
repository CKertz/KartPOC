using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRendererController : MonoBehaviour
{
    [SerializeField]
    private GameObject trailColliderPrefab;
    TrailRenderer trail;
    EdgeCollider2D edgeCollider;
    bool isTrailPointSetting = false;
    private int trailOffsetNumber = 0;
    private int trailOffsetEndNumber = 0;
    List<List<Vector2>> pointTracker = new List<List<Vector2>>();

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

    private void CreateColliderPointSegment(TrailRenderer trailRenderer)
    {
        List<Vector2> segment = new List<Vector2>();
        for (int i = trailOffsetEndNumber; i < trailRenderer.positionCount; i++)
        {
            segment.Add(trailRenderer.GetPosition(i));
        }
        pointTracker.Add(segment);
        Debug.Log(string.Format("segment from points {0} to {1} added. pointtracker count: {2}", trailOffsetEndNumber, trailRenderer.positionCount, pointTracker.Count));
    }

    private void BuildColliderPointsFromPointTracker(EdgeCollider2D collider)
    {
        if(pointTracker.Count <= 0)
        {
            Debug.Log("pointtracker count = 0");
            return;
        }
        GameObject newObject = Instantiate(trailColliderPrefab);
        //GameObject newEdgeColliderObject = new GameObject("EdgeColliderObject");
        //EdgeCollider2D newEdgeCollider = newEdgeColliderObject.AddComponent<EdgeCollider2D>();
        //newEdgeCollider.edgeRadius = 0.5f;
        //newEdgeCollider.isTrigger = true;

        foreach (List<Vector2> segment in pointTracker)
        {
            newObject.GetComponent<EdgeCollider2D>().SetPoints(segment);

            //newEdgeCollider.SetPoints(segment);
        }
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
                BuildColliderPointsFromPointTracker(edgeCollider);
            }
            else
            {
                isTrailPointSetting = false;
                trailOffsetNumber = trail.positionCount;
                // form a segment in master list
                CreateColliderPointSegment(trail);
                Debug.Log("OFFSET NUMBER:" +trailOffsetNumber);

            }
            Debug.Log(isTrailPointSetting);

        }
        //Debug.Log("test hit!" + data + "another test:"+ sender.gameObject.name);
    }
}
