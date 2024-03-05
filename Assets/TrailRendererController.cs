using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRendererController : MonoBehaviour
{
    TrailRenderer trail;
    EdgeCollider2D trailCollider;

    // Start is called before the first frame update
    void Start()
    {
        trail = GetComponent<TrailRenderer>();

        //GameObject colliderGameObject = new GameObject("TrailCollider", typeof(EdgeCollider2D));
        //trailCollider = colliderGameObject.GetComponent<EdgeCollider2D>();
        trailCollider = GameObject.Find("TrailCollider").GetComponent<EdgeCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        SetColliderPointsFromTrail(trail, trailCollider);
    }

    private void SetColliderPointsFromTrail(TrailRenderer trailRenderer, EdgeCollider2D collider)
    {
        List<Vector2> points = new List<Vector2>();
        
        // trims the top of the trailrenderer from creating collider points ahead of the trail. introducing an offset to cut off the top
        int pointOffsetNumber = 5;
        
        for(int i = 0; i < trailRenderer.positionCount - pointOffsetNumber; i++)
        {
            //Debug.Log("positioncount: " + trailRenderer.positionCount);
            points.Add(trailRenderer.GetPosition(i));
        }
        collider.SetPoints(points);
        if (Input.GetKeyDown(KeyCode.Space))
        {

            LogTrailData(points);
        }
    }

    private void LogTrailData(List<Vector2> points)
    {
        for(int i = 0; i < points.Count; i++)
        {
            Debug.Log(points[i].x + "," + points[i].y);
        }
    }
}
