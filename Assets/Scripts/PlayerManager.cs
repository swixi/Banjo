using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Rigidbody playerRB;
    [SerializeField] private float force = 10f;
    [SerializeField] private Transform playerTransform;

    //used only for testing, sets the controls for another player
    public int controlMode = 0;

    public BallManager attachedBall;  

    void Update()
    {
        if (controlMode == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("Field")))
                {
                    Debug.Log("You clicked on the field at " + hit.point.ToString());
                    if(attachedBall != null)
                    {
                        attachedBall.KickTo(hit);
                        attachedBall = null;
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        ForceMode mode = ForceMode.Impulse;

        if(playerRB.velocity.magnitude > 0.1)
        {
            //based on the orientation of the map, the cube is moving in the zx-plane (z is horizontal)
            //get the angle of the velocity vector in this plane, then rotate around the y axis
            float angle = Mathf.Atan2(playerRB.velocity.x, playerRB.velocity.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }

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
            if (Input.GetKey("f") && attachedBall != null)
            {
                attachedBall.KickStraight();
                attachedBall = null;
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
            attachedBall = collision.collider.GetComponent<BallManager>();
            attachedBall.SetAttachedPlayer(this);
        }
    }
}