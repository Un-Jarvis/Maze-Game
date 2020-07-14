using UnityEngine;
using UnityEngine.AI;

public class Player
{
    public GameObject player;

    private Game game;
    private Wall[,] walls;

    private Vector3 currentPos;
    private NavMeshAgent agent;

    public Player()
    {
        game = GameObject.Find("Game Generator").GetComponent<Game>();
        walls = game.GetWalls();

        player = GameObject.Instantiate(game.playerObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
        player.name = "Player";

        player.AddComponent<NavMeshAgent>();
        agent = player.GetComponent<NavMeshAgent>();
        agent.radius = 0.55f;
    }

    public void Update()
    {
        UpdateMovement();
        BreakDestructibleWall();
        ConsumeCoin();
    }

    public void DestroyObject()
    {
        if (player != null)
        {
            GameObject.Destroy(player);
        }
    }

    private void UpdateMovement()
    {
        // Moving left
        if (Input.GetKey(KeyCode.A))
        { 
            player.transform.localPosition += new Vector3(-5, 0, 0) * Time.deltaTime;
        }
        // Moving right
        if (Input.GetKey(KeyCode.D))
        { 
            player.transform.localPosition += new Vector3(5, 0, 0) * Time.deltaTime;
        }
        // Moving forward
        if (Input.GetKey(KeyCode.W))
        { 
            player.transform.localPosition += new Vector3(0, 0, 5) * Time.deltaTime;
        }
        // Moving back
        if (Input.GetKey(KeyCode.S))
        { 
            player.transform.localPosition += new Vector3(0, 0, -5) * Time.deltaTime;
        }
    }

    private void BreakDestructibleWall()
    {
        // Determine the maze cell which the player is at
        int r = Mathf.FloorToInt((player.transform.position.x + 1.5f) / 2.8f);
        int c = Mathf.FloorToInt((player.transform.position.z + 1.5f) / 2.8f);

        // Determine if there is any destructible wall near the player
        // If the player hit the destructible wall, the wall will be destroyed
        if (walls[r, c].destructibleWalls.Count != 0)
        {
            if (player.transform.position.x >= walls[r, c].floor.transform.position.x + 0.5f
                && walls[r, c].destructibleWalls.Contains(walls[r, c].southWall)
                && walls[r, c].southWall != null)
            {
                GameObject.Destroy(walls[r, c].southWall);
                walls[r, c].destructibleWalls.Remove(walls[r, c].southWall);
            }

            if (player.transform.position.z >= walls[r, c].floor.transform.position.z + 0.5f
                && walls[r, c].destructibleWalls.Contains(walls[r, c].eastWall)
                && walls[r, c].eastWall != null)
            {
                GameObject.Destroy(walls[r, c].eastWall);
                walls[r, c].destructibleWalls.Remove(walls[r, c].eastWall);
            }
        }

        if (player.transform.position.x <= walls[r, c].floor.transform.position.x - 0.5f
            && r - 1 >= 0
            && walls[r - 1, c].destructibleWalls.Contains(walls[r - 1, c].southWall)
            && walls[r - 1, c].southWall != null)
        {
            GameObject.Destroy(walls[r - 1, c].southWall);
            walls[r - 1, c].destructibleWalls.Remove(walls[r - 1, c].southWall);
        }

        if (player.transform.position.z <= walls[r, c].floor.transform.position.z - 0.5f
            && c - 1 >= 0
            && walls[r, c - 1].destructibleWalls.Contains(walls[r, c - 1].eastWall)
            && walls[r, c - 1].eastWall != null)
        {
            GameObject.Destroy(walls[r, c - 1].eastWall);
            walls[r, c - 1].destructibleWalls.Remove(walls[r, c - 1].eastWall);
        }
    }

    private void ConsumeCoin()
    {
        // Determine the maze cell which the player is at
        int r = Mathf.FloorToInt((player.transform.position.x + 1.5f) / 2.8f);
        int c = Mathf.FloorToInt((player.transform.position.z + 1.5f) / 2.8f);

        // Determine if there is a coin on the floor
        if (walls[r, c].hasCoin)
        {
            Coin coinToBeDestroyed = null;
            foreach (Coin coin in game.GetCoinsList()){
                if (coin.coin.name == walls[r, c].coin.name){
                    coinToBeDestroyed = coin;
                }
            }
            game.GetCoinsList().Remove(coinToBeDestroyed);
            GameObject.Destroy(coinToBeDestroyed.coin);
            walls[r, c].hasCoin = false;

            // Update score
            game.score++;
        }
    }
}
