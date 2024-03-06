using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRendererController : MonoBehaviour
{
    TrailRenderer trail;
    EdgeCollider2D trailCollider;

    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        trailCollider = GameObject.Find("TrailCollider").GetComponent<EdgeCollider2D>();
    }

    void Update()
    {
        SetColliderPointsFromTrail(trail, trailCollider);
    }

    private void SetColliderPointsFromTrail(TrailRenderer trailRenderer, EdgeCollider2D collider)
    {
        List<Vector2> points = new List<Vector2>();
        
        // trims the top of the trailrenderer from creating collider points ahead of the trail. introducing an offset to cut off the top
        int pointOffsetNumber = 7;
        
        for(int i = 0; i < trailRenderer.positionCount - pointOffsetNumber; i++)
        {
            //Debug.Log("positioncount: " + trailRenderer.positionCount);
            points.Add(trailRenderer.GetPosition(i));
        }
        collider.SetPoints(points);
    }
}
