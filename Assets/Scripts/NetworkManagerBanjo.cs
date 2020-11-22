using UnityEngine;
using System.Collections.Generic;
using Mirror;
using UnityEngine.SceneManagement;

//namespace Mirror.Examples.Pong

// Custom NetworkManager that assigns positions when
// spawning players. The built in RoundRobin spawn method wouldn't work after
// someone reconnects (both players would be on the same side).
[AddComponentMenu("")]
public class NetworkManagerBanjo : NetworkManager
{
    public Transform leftSpawn;
    public Transform rightSpawn;
    GameObject ball;
    public float restartDelay = 1.5f;
    private List<GameObject> players;

    public override void OnStartServer()
    {
        players = new List<GameObject>();
        ball = Instantiate(spawnPrefabs.Find(prefab => prefab.tag == "ball"));
        NetworkServer.Spawn(ball);
        base.Start();
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        // add player at correct spawn position
        Transform start = numPlayers == 0 ? leftSpawn : rightSpawn;
        GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
        NetworkServer.AddPlayerForConnection(conn, player);
        
        //NetworkServer.Destroy(player);
        //player = Instantiate(playerPrefab, start.position, start.rotation);
       // NetworkServer.Spawn(player, conn);

        players.Add(player);
        
        /*
        // spawn ball if two players
        if (numPlayers == 2)
        {
            ball = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"));
            NetworkServer.Spawn(ball);
        }
        */
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        /*
        // destroy ball
        if (ball != null)
            NetworkServer.Destroy(ball);
        */

        // call base functionality (actually destroys the player)
        base.OnServerDisconnect(conn);
    }

    public void KickBall()
    {
        return;
    }

    public void PrepareForNewRound() 
    {
        foreach (GameObject player in players)
            player.GetComponent<PlayerManager>().Freeze();

        Invoke("NewRound", restartDelay);
    }

    public void NewRound()
    {
        //ServerChangeScene(SceneManager.GetActiveScene().name);

        foreach (GameObject player in players)
            player.GetComponent<PlayerManager>().SetToSpawn();
            
        ball.GetComponent<BallManager>().SetToSpawn();
    }
}
