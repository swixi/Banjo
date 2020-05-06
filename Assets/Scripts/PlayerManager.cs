using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Rigidbody playerRB;
    public float force = 10f;
    public Transform playerTransform;

    //used only for testing, sets the controls for another player
    public int controlMode = 0;

    //arbitrary default, gets used if e.g. player has never moved or if there is crazy friction that would cause a drop to 0 vel in less than a frame
    private Vector3 lastNonzeroUnitDirection = Vector3.right; 

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

        
        float angle = Mathf.Atan2(playerRB.velocity.x, playerRB.velocity.z) * Mathf.Rad2Deg;
        Debug.Log(angle);
        transform.rotation = Quaternion.Euler(0, angle, 0);

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

    //never return the zero vector: this makes rendering go bonkers
    //if the velocity magnitude drops below, say 0.01, then it is a sign the player is coming to a standstill
    //in this case, return the last known nonzero velocity vector direction
    public Vector3 GetUnitDirection()
    {
        float vel = playerRB.velocity.magnitude;
        Vector3 unitDirection = playerRB.velocity.normalized;

        if(vel > 0)
            lastNonzeroUnitDirection = unitDirection;

        Debug.Log(vel);

        return vel < 5 ? lastNonzeroUnitDirection : unitDirection;
    }
}