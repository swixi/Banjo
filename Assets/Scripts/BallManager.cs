using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public Transform ballTransform; //not needed? just use (this.)transform?
    public Rigidbody ballRB;
    public float kickForce = 10f;
    public float offset = 1f;

    private bool attachedToPlayer = false;
    private PlayerManager attachedPlayer;
    

    void Update()
    {
        //Kick the ball 
        //Is this in the right place?
        //TODO: kick ball toward a mouse click: see https://docs.unity3d.com/Manual/nav-MoveToClickPoint.html, maybe need to project a vector onto zx-plane
        if(Input.GetKey("f") && IsAttached())
        {
            ballRB.AddForce(kickForce * attachedPlayer.GetUnitDirection(), ForceMode.Impulse);
            RemoveAttachedPlayer();
        }
    }

    public bool IsAttached() 
    {
        return attachedToPlayer;
    }

    public void SetAttachedPlayer(PlayerManager player)
    {
        attachedPlayer = player;
        attachedToPlayer = true;
    }

    public void RemoveAttachedPlayer()
    {
        attachedPlayer = null;
        attachedToPlayer = false;
    }

    //state machine here?
    void FixedUpdate()
    {
        if(attachedToPlayer)
        {
            float offsetDistance = attachedPlayer.transform.localScale.x + offset;
            ballTransform.position = attachedPlayer.GetOffset(offsetDistance);
        }
    }
}
