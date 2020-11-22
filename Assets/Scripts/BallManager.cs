using UnityEngine;
using Mirror;

public class BallManager : NetworkBehaviour
{
    public Transform ballTransform; //not needed? just use (this.)transform?
    public Rigidbody ballRB;
    public float kickForce = 10f;
    public float offset = 1f;

    private bool attachedToPlayer = false;
    private PlayerManager attachedPlayer;

    public Vector3 spawnPoint;

    public override void OnStartServer()
    {
        base.OnStartServer();

        //only simulate ball physics on server
        ballRB.isKinematic = false;

        // Serve the ball from left player
        //rigidbody2d.velocity = Vector2.right * speed;
    }
    
    void Update()
    {
        //Kick the ball 
        //Is this in the right place?
        //TODO: kick ball toward a mouse click: see https://docs.unity3d.com/Manual/nav-MoveToClickPoint.html, maybe need to project a vector onto zx-plane
        //if(Input.GetKey("f") && IsAttached())
        //{
        //    ballRB.velocity = kickForce * attachedPlayer.GetUnitDirection();
        //    RemoveAttachedPlayer();
        //}
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

    [Server]
    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "player")
        {
            SetAttachedPlayer(collision.collider.GetComponent<PlayerManager>());
        }
    }


    public void SetToSpawn()
    {
        transform.position = spawnPoint;
    }
}
