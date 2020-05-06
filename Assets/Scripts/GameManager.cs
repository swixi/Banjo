using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float restartDelay = 1f;
    public Text goalText;
    private List<GameObject> players;
    public int playerCount = 1;
    public GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        players = new List<GameObject>();
        
        for(int i = 0; i < playerCount; i++) 
        {
            GameObject player = Instantiate(playerPrefab, new Vector3(10+(10*i), 0, 0), Quaternion.identity);
            //the top of the field is y=0.5, this places the player on the ground
            player.transform.position = new Vector3(10+(10*i), player.transform.localScale.y/2 + 0.5f, 0);
            player.GetComponent<PlayerManager>().SetControlMode(i);
            players.Add(player);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoalScored() 
    {
        //update scores here?

        goalText.enabled = true;
        foreach (GameObject player in players)
            player.GetComponent<PlayerManager>().Freeze();

        Invoke("Reset", restartDelay);
    }

    public void Reset() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
