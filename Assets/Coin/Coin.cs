using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin
{
    public GameObject coin;

    private Game game;
    private Wall[,] walls;

    private float rotateSpeed = 10f;

    public bool isDestroyed = false;

    public Coin(int _number)
    {
        game = GameObject.Find("Game Generator").GetComponent<Game>();
        walls = game.GetWalls();

        coin = GameObject.Instantiate(game.coinObject);
        int r = Random.Range(0, game.rows);
        int c = Random.Range(0, game.columns);
        // If the random position already contains a coin, generate a new position as the coin's position
        while (walls[r, c].hasCoin)
        {
            r = Random.Range(0, game.rows);
            c = Random.Range(0, game.columns);
        }
        walls[r, c].coin = coin;
        walls[r, c].hasCoin = true;
        coin.transform.position = walls[r, c].floor.transform.position + new Vector3(0f, 1.4f, 0f);
        coin.name = "Coin " + _number;
        coin.layer = 8;
        coin.transform.parent = GameObject.Find("Coins").transform;
    }

    public void Update()
    {
        coin.transform.Rotate(Vector3.left * rotateSpeed);
        if (coin == null)
        {
            isDestroyed = true;
        }
    }

    public void DestroyObject()
    {
        if (coin != null)
        {
            GameObject.Destroy(coin);
        }
        isDestroyed = true;
    }
}
