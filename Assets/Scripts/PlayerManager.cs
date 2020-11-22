using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    public Rigidbody playerRB;
    public float force = 10f;
    public Transform playerTransform;

    //arbitrary default, gets used if e.g. player has never moved or if there is crazy friction that would cause a drop to 0 vel in less than a frame
    private Vector3 lastNonzeroUnitDirection = Vector3.right; 

    Vector3 spawnPoint;

    //public Text debugText;

    enum Direction
    {
        UP=0,
        DOWN=1,
        LEFT=2,
        RIGHT=3
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.position;
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        

        if(playerRB.velocity.magnitude > 0.1)
        {
            //based on the orientation of the map, the cube is moving in the zx-plane (z is horizontal)
            //get the angle of the velocity vector in this plane, then rotate around the y axis
            float angle = Mathf.Atan2(playerRB.velocity.x, playerRB.velocity.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }

        // only let the local player control their player
        // don't control other players
        if(isLocalPlayer) 
        {
            //debugText.text = "pressed";
            //ForceMode mode = ForceMode.Impulse;
            if(Input.GetKey("w") || Input.GetKey("up"))
            {
                //playerRB.AddForce(0, 0, Time.deltaTime * force, mode);
                CmdMove(0);
            }
            if(Input.GetKey("a") || Input.GetKey("left"))
            {
                //playerRB.AddForce(-(Time.deltaTime * force), 0, 0, mode);
                CmdMove(1);
            }
            if(Input.GetKey("s") || Input.GetKey("down"))
            {
                //playerRB.AddForce(0, 0, -(Time.deltaTime * force), mode);
                CmdMove(2);
            }
            if(Input.GetKey("d") || Input.GetKey("right"))
            {
                //playerRB.AddForce(Time.deltaTime * force, 0, 0, mode);
                CmdMove(3);
            }
            if(Input.GetKey("f"))
            {
                CmdKick();
            }
        }
    }

    [Command]
    private void CmdMove(int dir)
    {
        Debug.Log("command " + dir + " " + playerRB.velocity.magnitude + " " + playerRB.position);
        //RpcMove(dir);
        ForceMode mode = ForceMode.Impulse;
        switch(dir)
        {
            case 0:
                playerRB.AddForce(0, 0, Time.deltaTime * force, mode);
                break;
            case 1:
                playerRB.AddForce(-(Time.deltaTime * force), 0, 0, mode);
                break;
            case 2:
                playerRB.AddForce(0, 0, -(Time.deltaTime * force), mode);
                break;
            case 3:
                playerRB.AddForce(Time.deltaTime * force, 0, 0, mode);
                break;
        }
    }

    [ClientRpc]
    private void RpcMove(Direction dir)
    {
        ForceMode mode = ForceMode.Impulse;
        switch(dir)
        {
            case Direction.UP:
                playerRB.AddForce(0, 0, Time.deltaTime * force, mode);
                break;
            case Direction.LEFT:
                playerRB.AddForce(-(Time.deltaTime * force), 0, 0, mode);
                break;
            case Direction.DOWN:
                playerRB.AddForce(0, 0, -(Time.deltaTime * force), mode);
                break;
            case Direction.RIGHT:
                playerRB.AddForce(Time.deltaTime * force, 0, 0, mode);
                break;
        }
    }

    [Command]
    private void CmdKick()
    {
        //validate that this player has the ball
        RpcKick();
    }

    [ClientRpc]
    private void RpcKick()
    {
        FindObjectOfType<NetworkManagerBanjo>().KickBall();
    }

    public void SetToSpawn()
    {
        transform.position = spawnPoint;
    }

    public void Freeze() 
    {
        force = 0f;
    }

    //returns a vector which signifies the position of an object which is offset "distance" units from the player 
    //in the direction the player is running
    
    //TODO: there is an issue with using the player velocity direction to position the ball: if the player is sliding to the right but the controller is holding left, 
    //the player can collide with the ball on the right and it will teleport to the left side.
    public Vector3 GetOffset(float distance)
    {
        return this.transform.position + distance * GetUnitDirection();
    }

    //never return the zero vector: this makes rendering go bonkers
    //if the velocity magnitude drops below, say 0.1, then it is a sign the player is coming to a standstill
    //in this case, return the last known nonzero velocity vector direction
    public Vector3 GetUnitDirection()
    {
        float vel = playerRB.velocity.magnitude;
        Vector3 unitDirection = playerRB.velocity.normalized;

        //if you make this vel > 0, it will sometimes set lastNonzeroUnitDirection to the zero vector; probably rounding error
        if(vel > 0.2)
            lastNonzeroUnitDirection = unitDirection;

        return vel < 0.5 ? lastNonzeroUnitDirection : unitDirection;
    }
}