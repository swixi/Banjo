using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider collider) 
    {
        if(collider.tag == "ball")
            FindObjectOfType<GameManager>().GoalScored();
    }
}