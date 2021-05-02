using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public Transform ballTransform; //not needed? just use (this.)transform?
    public Rigidbody ballRB;
    [SerializeField] private float kickForce = 10f;
    [SerializeField] private float playerOffset = 2f;

    private bool attachedToPlayer = false;
    private PlayerManager attachedPlayer;
    

    void Update()
    {
        //Kick the ball 
        //Is this in the right place?
        //TODO: kick ball toward a mouse click: see https://docs.unity3d.com/Manual/nav-MoveToClickPoint.html, maybe need to project a vector onto zx-plane
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
        this.transform.parent = null;
        attachedToPlayer = false;
    }

    //state machine here?
    void FixedUpdate()
    {
        if(attachedToPlayer)
        {
            // make the ball a child of the player so the ball moves with them
            this.transform.parent = attachedPlayer.transform;
            this.transform.localPosition = new Vector3(0, 0, playerOffset);
        }
    }

    // TODO there's a problem somewhere in here, probably around the vector addition. The ball seems to go in random directions sometimes
    // See console for click coords
    public void KickTo(RaycastHit target)
    {
        // Get the kick vector by subtracting the destination vector from the ball's vector. Also add the player's velocity vector 
        // (if you're running with the ball and kick it forwards it goes faster than if you were standing still)
        Vector3 forceDirection = target.point - ballTransform.position + attachedPlayer.GetComponent<Rigidbody>().velocity;
        forceDirection.y = 0;
        RemoveAttachedPlayer();
        // I think I actually want to normalize & multiply the vector before adding the player's velocity vector?
        ballRB.AddForce(kickForce * forceDirection.normalized, ForceMode.Impulse);
    }

    public void KickStraight()
    {
        ballRB.AddForce(kickForce * attachedPlayer.GetComponent<Rigidbody>().velocity.normalized, ForceMode.Impulse);
        RemoveAttachedPlayer();
    }
}
