using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Maze
{
    private int rows, columns;
    private GameObject wall;
    private GameObject destructibleWall;
    private float destructibilityRate;
    private float positionUnit;
    private Wall[,] walls;

    public Maze(int _rows, int _columns, GameObject _wall, GameObject _destructibleWall)
    {
        rows = _rows;
        columns = _columns;
        wall = _wall;
        destructibleWall = _destructibleWall;
        destructibilityRate = 0.2f;
        positionUnit = wall.transform.localScale.x - wall.transform.localScale.z;
        Initialize();
    }

    public Wall[,] getWalls()
    {
        return walls;
    }

    public void DestroyObject()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                if (walls[r, c].floor != null)
                {
                    GameObject.Destroy(walls[r, c].floor);
                }

                if (walls[r, c].northWall != null)
                {
                    GameObject.Destroy(walls[r, c].northWall);
                }

                if (walls[r, c].southWall != null)
                {
                    GameObject.Destroy(walls[r, c].southWall);
                }

                if (walls[r, c].westWall != null)
                {
                    GameObject.Destroy(walls[r, c].westWall);
                }

                if (walls[r, c].eastWall != null)
                {
                    GameObject.Destroy(walls[r, c].eastWall);
                }
            }
        }
    }

    private void Initialize()
    {
        // All walls will be generated under the GameObject called "Maze"
        GameObject maze = GameObject.Find("Maze");
            
        // Create maze
        walls = new Wall[rows, columns];
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                walls[r, c] = new Wall();
                walls[r, c].destructibleWalls = new List<GameObject>();

                // floor
                walls[r, c].floor = GameObject.Instantiate(wall, new Vector3(r * positionUnit, -(positionUnit / 2f), c * positionUnit), Quaternion.identity);
                walls[r, c].floor.name = "Floor " + r + "," + c;
                walls[r, c].floor.transform.Rotate(Vector3.right, 90f);
                walls[r, c].floor.transform.parent = maze.transform;

                GameObject wallObj = wall;
                // north wall
                if (r == 0)
                {
                    walls[r, c].northWall = GameObject.Instantiate(wall, new Vector3(r * positionUnit - positionUnit / 2f, 0, c * positionUnit), Quaternion.identity);
                    walls[r, c].northWall.name = "North Wall " + r + "," + c;
                    walls[r, c].northWall.transform.Rotate(Vector3.up, 90f);
                    walls[r, c].northWall.transform.parent = maze.transform;
                    walls[r, c].northWall.AddComponent<NavMeshObstacle>();
                    walls[r, c].northWall.GetComponent<NavMeshObstacle>().carving = true;
                    walls[r, c].northWall.GetComponent<NavMeshObstacle>().carvingMoveThreshold = 0.2f;
                }

                // south wall
                if (Random.Range(0.0f, 1.0f) <= destructibilityRate && r != rows - 1)
                {
                    wallObj = destructibleWall;
                }
                else
                {
                    wallObj = wall;
                }
                walls[r, c].southWall = GameObject.Instantiate(wallObj, new Vector3(r * positionUnit + positionUnit / 2f, 0, c * positionUnit), Quaternion.identity);
                walls[r, c].southWall.name = "South Wall " + r + "," + c;
                walls[r, c].southWall.transform.Rotate(Vector3.up, 90f);
                walls[r, c].southWall.transform.parent = maze.transform;
                walls[r, c].southWall.AddComponent<NavMeshObstacle>();
                walls[r, c].southWall.GetComponent<NavMeshObstacle>().carving = true;
                walls[r, c].southWall.GetComponent<NavMeshObstacle>().carvingMoveThreshold = 0.2f;
                if (wallObj == destructibleWall)
                {
                    walls[r, c].destructibleWalls.Add(walls[r, c].southWall);
                }

                // west wall
                if (c == 0)
                {
                    walls[r, c].westWall = GameObject.Instantiate(wall, new Vector3(r * positionUnit, 0, c * positionUnit - positionUnit / 2f), Quaternion.identity);
                    walls[r, c].westWall.name = "West Wall " + r + "," + c;
                    walls[r, c].westWall.transform.parent = maze.transform;
                    walls[r, c].westWall.AddComponent<NavMeshObstacle>();
                    walls[r, c].westWall.GetComponent<NavMeshObstacle>().carving = true;
                    walls[r, c].westWall.GetComponent<NavMeshObstacle>().carvingMoveThreshold = 0.2f;
                }

                // east wall
                if (Random.Range(0.0f, 1.0f) <= destructibilityRate && c != columns - 1)
                {
                    wallObj = destructibleWall;
                }
                else
                {
                    wallObj = wall;
                }
                walls[r, c].eastWall = GameObject.Instantiate(wallObj, new Vector3(r * positionUnit, 0, c * positionUnit + positionUnit / 2f), Quaternion.identity);
                walls[r, c].eastWall.name = "East Wall " + r + "," + c;
                walls[r, c].eastWall.transform.parent = maze.transform;
                walls[r, c].eastWall.AddComponent<NavMeshObstacle>();
                walls[r, c].eastWall.GetComponent<NavMeshObstacle>().carving = true;
                walls[r, c].eastWall.GetComponent<NavMeshObstacle>().carvingMoveThreshold = 0.2f;
                if (wallObj == destructibleWall)
                {
                    walls[r, c].destructibleWalls.Add(walls[r, c].eastWall);
                }
            }
        }
    }
}
