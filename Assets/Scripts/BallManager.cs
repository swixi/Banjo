using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public Transform ballTransform; //not needed? just use (this.)transform?
    public Rigidbody ballRB;
    private bool attachedToPlayer = false;
    private PlayerManager attachedPlayer;
    public float kickForce = 10f;

    void Update()
    {
        //Kick the ball 
        //Is this in the right place?
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
            float offsetDistance = this.transform.localScale.x+0.5f;
            ballTransform.position = attachedPlayer.GetOffset(offsetDistance);
        }
    }
}
