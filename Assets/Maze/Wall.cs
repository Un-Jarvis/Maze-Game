using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall {
    public bool visited = false;
    public bool hasCoin = false;

    public GameObject northWall, southWall, westWall, eastWall, floor;
    public List<GameObject> destructibleWalls;

    public GameObject coin = null;
}
