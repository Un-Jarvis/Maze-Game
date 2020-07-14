using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    // Game
    private bool gameOver;
    private bool win;

    // Camera
    private GameObject camera;
    private Vector3 cameraOffset;

    // Display text
    public Text gameInfo;
    public Text scoreInfo;

    // Nav Mesh
    public NavMeshSurface surface;

    // Maze
    public int rows, columns;
    public GameObject wallObject;
    public GameObject destructibleWallObject;
    private Maze maze;
    private Wall[,] walls;

    // Player
    public GameObject playerObject;
    private Player player;

    // Enemies
    public GameObject enemyObject;
    public int enemyNumber;
    private Enemy[] enemies;

    // Coins
    public GameObject coinObject;
    public int coinNumber;
    private List<Coin> coins;

    // Score
    public int score;

    // Use this for initialization
    void Start()
    {
        InitializeGame();
    }

    // Update is called once per frame
    void Update()
    {
        // Display text
        gameInfo.transform.position = new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.9f, 0f);

        scoreInfo.transform.position = new Vector3((float)Screen.width * 0.85f, (float)Screen.height * 0.5f, 0f);
        scoreInfo.text = "Score\n" + score;

        // Game
        if (coins.Count == 0)
        {
            gameOver = true;
            win = true;
        }

        if (gameOver)
        {
            if (win)
            {
                gameInfo.text = "All coins are collected! Cheers!\nPress \"R\" to restart a new game.";
            }
            else
            {
                gameInfo.text = "Game over! You have collected " + score + " coins.\nPress \"R\" to restart a new game.";
                if (player.player != null)
                {
                    Destroy(player.player);
                }
            }
            for (int i = 0; i < enemyNumber; i++)
            {
                enemies[i].Update();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetGame();
            }
        }
        else
        {
            // Player
            player.Update();

            // Enemies
            for (int i = 0; i < enemyNumber; i++)
            {
                enemies[i].Update(player.player);
                if (enemies[i].ReachPlayer(player.player))
                {
                    gameOver = true;
                }
            }
        }

        // Coins
        foreach (Coin c in coins)
        {
            if (!c.isDestroyed && c.coin != null)
            {
                c.Update();
            }
        }
    }

    void LateUpdate()
    {
        // If the player object exist, the camera will follow the player as the player moves
        if (player.player != null)
        {
            camera.transform.position = player.player.transform.position + cameraOffset;
        }
    }

    private void InitializeGame()
    {
        // Display text
        gameInfo.transform.position = new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.9f, 0f);
        gameInfo.text = "";

        scoreInfo.transform.position = new Vector3((float)Screen.width * 0.85f, (float)Screen.height * 0.5f, 0f);
        scoreInfo.text = "Score\n" + score;

        // Game
        gameOver = false;
        win = false;

        // Maze
        maze = new Maze(rows, columns, wallObject, destructibleWallObject);
        walls = maze.getWalls();

        MazeAlgorithm ma = new RecursiveBacktrackingAlgorithm(walls);
        ma.CreateMaze();

        // Update NavMesh
        surface.BuildNavMesh();

        // Player
        player = new Player();

        // Camera
        camera = GameObject.Find("Main Camera");
        camera.transform.position = new Vector3(0f, 10f, -3f);
        camera.transform.rotation = Quaternion.Euler(new Vector3(60f, 0f, 0f));
        cameraOffset = camera.transform.position - player.player.transform.position;

        // Enemies
        enemies = new Enemy[enemyNumber];
        for (int i = 0; i < enemyNumber; i++)
        {
            enemies[i] = new Enemy(enemyObject, i);
        }

        // Coins
        coins = new List<Coin>();
        for (int i = 0; i < coinNumber; i++)
        {
            coins.Add(new Coin(i));
        }

        // Score
        score = 0;
    }

    private void ResetGame()
    {
        // Destroy all GameObjects existing in the game
        player.DestroyObject();
        for (int i = 0; i < enemyNumber; i++)
        {
            enemies[i].DestroyObject();
        }
        foreach (Coin c in coins)
        {
            c.DestroyObject();
        }
        maze.DestroyObject();

        // Rebuild game
        InitializeGame();
    }

    public Wall[,] GetWalls()
    {
        return walls;
    }

    public Player GetPlayer()
    {
        return player;
    }

    public List<Coin> GetCoinsList()
    {
        return coins;
    }
}
