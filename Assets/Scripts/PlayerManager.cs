using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Rigidbody playerRB;
    public float force = 10f;
    public Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }    

    void FixedUpdate()
    {
        if(Input.GetKey("w") || Input.GetKey("up"))
            playerRB.AddForce(0, 0, Time.deltaTime * force, ForceMode.Impulse);
        if(Input.GetKey("a") || Input.GetKey("left"))
            playerRB.AddForce(-(Time.deltaTime * force), 0, 0, ForceMode.Impulse);
        if(Input.GetKey("s") || Input.GetKey("down"))
            playerRB.AddForce(0, 0, -(Time.deltaTime * force), ForceMode.Impulse);
        if(Input.GetKey("d") || Input.GetKey("right"))
            playerRB.AddForce(Time.deltaTime * force, 0, 0, ForceMode.Impulse);
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
}
