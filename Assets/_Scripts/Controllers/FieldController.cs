using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldController : MonoBehaviour
{

    Texture2D texture;

    // Start is called before the first frame update
    void Start()
    {
        var rawImage = GetComponent<RawImage>();
        texture = rawImage.texture as Texture2D;
        var pixelData = texture.GetPixels();
        Debug.Log("pixel num:" + pixelData.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
