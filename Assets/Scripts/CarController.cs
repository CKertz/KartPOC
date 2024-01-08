using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    //float vertical, horizontal;
    Rigidbody2D carRigidbody2D;
    [SerializeField]
    float accelerationPower = 5f;
    [SerializeField]
    float steeringPower = 5f;
    float steeringAmount, direction;


    // Start is called before the first frame update
    void Start()
    {
        carRigidbody2D = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        Move();

    }

    void Move()
    {
        //this works but gets weighed down with the maskmanager objects piling up, the longer the game, the slower it goes
        steeringAmount = -Input.GetAxis("Horizontal");
        moveSpeed = Input.GetAxis("Vertical") * accelerationPower;
        direction = Mathf.Sign(Vector2.Dot(carRigidbody2D.velocity, carRigidbody2D.GetRelativeVector(Vector2.up)));
        carRigidbody2D.rotation += steeringAmount * steeringPower * carRigidbody2D.velocity.magnitude * direction;
        carRigidbody2D.AddRelativeForce(Vector2.up * moveSpeed);

        carRigidbody2D.AddRelativeForce(-Vector2.right * carRigidbody2D.velocity.magnitude * steeringAmount / 2);


        // old, just saving for now
        //horizontal = Input.GetAxis("Horizontal");
        //vertical = Input.GetAxis("Vertical");
        //carRigidbody2D.velocity = new Vector2(horizontal * moveSpeed, vertical * moveSpeed);
        //
    }
}
