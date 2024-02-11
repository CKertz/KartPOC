using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    [SerializeField] 
    float moveSpeed;

    [SerializeField]
    float accelerationPower;

    private TextMeshProUGUI distanceTraveledText;
    private Vector3 lastPosition;
    private float totalDistanceTraveled;



    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        Move();
        UpdateDebuggerProperties();
    }

    void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f) * moveSpeed * Time.deltaTime;

        transform.Translate(movement);

    }

    private void UpdateDebuggerProperties()
    {
        float distanceTraveled = Vector3.Distance(transform.position, lastPosition);


        totalDistanceTraveled += distanceTraveled;

        lastPosition = transform.position;
        var debuggerOverlayCanvas = GameObject.Find("DebuggerUICanvas");

        if (debuggerOverlayCanvas != null)
        {
            var distanceTraveledObject = debuggerOverlayCanvas.transform.Find("DistanceTraveled");
            distanceTraveledText = distanceTraveledObject.GetComponent<TextMeshProUGUI>();
            
            distanceTraveledText.text = $"Distance traveled: {totalDistanceTraveled}";
        }
        else
        {
            Debug.LogError("DebuggerUICanvas object not found!");
        }
    }
}
