using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Rigidbody playerRB;
    public float force = 10f;
    public Transform playerTransform;

    //used only for testing, sets the controls for another player
    public int controlMode = 0;

    // Start is called before the first frame update
    void Start()
    {

    }    

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        ForceMode mode = ForceMode.Impulse;

        if(controlMode == 0)
        {
            if(Input.GetKey("w"))
            {
                playerRB.AddForce(0, 0, Time.deltaTime * force, mode);
            }
            if(Input.GetKey("a"))
            {
                playerRB.AddForce(-(Time.deltaTime * force), 0, 0, mode);
            }
            if(Input.GetKey("s"))
            {
                playerRB.AddForce(0, 0, -(Time.deltaTime * force), mode);
            }
            if(Input.GetKey("d"))
            {
                playerRB.AddForce(Time.deltaTime * force, 0, 0, mode);
            }
        }
        else
        {
            if(Input.GetKey("up"))
            {
                playerRB.AddForce(0, 0, Time.deltaTime * force, mode);
            }
            if(Input.GetKey("left"))
            {
                playerRB.AddForce(-(Time.deltaTime * force), 0, 0, mode);
            }
            if(Input.GetKey("down"))
            {
                playerRB.AddForce(0, 0, -(Time.deltaTime * force), mode);
            }
            if(Input.GetKey("right"))
            {
                playerRB.AddForce(Time.deltaTime * force, 0, 0, mode);
            }
        }
    }

    public void SetControlMode(int controlMode)
    {
        this.controlMode = controlMode;
    }

    public void Freeze() 
    {
        force = 0f;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "ball")
        {
            collision.collider.GetComponent<BallManager>().SetAttachedPlayer(this);
        }
    }

    //returns a vector which signifies the position of an object which is offset "distance" units from the player 
    //in the direction the player is running
    
    //TODO: there is an issue with using the player velocity direction to position the ball: if the player is sliding to the right but the controller is holding left, 
    //the player can collide with the ball on the right and it will teleport to the left side.
    public Vector3 GetOffset(float distance)
    {
        return this.transform.position + distance * GetUnitDirection();
    }

    //TODO: never return 0? this makes rendering go bonkers. really the point of this function is to give DIRECTION
    //if the velocity magnitude drops below, say 0.01, then it is a sign the player is coming to a standstill
    //in this case, return the last known velocity vector.
    public Vector3 GetUnitDirection()
    {
        return playerRB.velocity.normalized;
    }
}
