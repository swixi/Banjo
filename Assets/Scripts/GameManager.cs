using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float restartDelay = 1f;
    public Text goalText;
    public PlayerManager player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoalScored() {
        //update scores here?

        goalText.enabled = true;
        player.Freeze();

        Invoke("Reset", restartDelay);
    }

    public void Reset() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
