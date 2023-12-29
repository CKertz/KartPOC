using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskManager : MonoBehaviour
{
    public GameObject Mask;
    private HashSet<Vector3> contactedPositions = new HashSet<Vector3>();


    void Start()
    {

    }

    void Update()
    {
        //partially taken from here https://www.youtube.com/watch?v=EAMUBMX5qvI and slightly optimized
        //TODO: this is terrible

        Vector3 worldPos = GameObject.Find("Car").transform.position;
        if (!contactedPositions.Contains(worldPos))
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            Vector3 pos = Camera.main.ScreenToWorldPoint(screenPos);
            pos.z = 0;

            GameObject ob = Instantiate(Mask, pos, Quaternion.identity);
            ob.transform.parent = GameObject.Find("MaskManager").transform;

            contactedPositions.Add(worldPos);
        }
    }
}
