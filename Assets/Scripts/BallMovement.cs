using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public float force = 10f;
    public Rigidbody ballRB;
    public Transform ballTransform;
    public Vector3 ballStartingPos;

    // Start is called before the first frame update
    void Start()
    {
        ballTransform.position = ballStartingPos;
    }

    // FixedUpdate is good for physics apparently
    void FixedUpdate()
    {
        if(Input.GetKey("w") || Input.GetKey("up"))
            ballRB.AddForce(0, 0, Time.deltaTime * force, ForceMode.Impulse);
        if(Input.GetKey("a") || Input.GetKey("left"))
            ballRB.AddForce(-(Time.deltaTime * force), 0, 0, ForceMode.Impulse);
        if(Input.GetKey("s") || Input.GetKey("down"))
            ballRB.AddForce(0, 0, -(Time.deltaTime * force), ForceMode.Impulse);
        if(Input.GetKey("d") || Input.GetKey("right"))
            ballRB.AddForce(Time.deltaTime * force, 0, 0, ForceMode.Impulse);
    }

    public void Freeze() {
        force = 0f;
    }
}
