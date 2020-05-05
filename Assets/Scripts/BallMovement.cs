using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public Rigidbody ballRB;
    public Transform ballTransform;
    public Vector3 ballStartingPos;

    // Start is called before the first frame update
    void Start()
    {
        ballTransform.position = ballStartingPos;
    }    
}
