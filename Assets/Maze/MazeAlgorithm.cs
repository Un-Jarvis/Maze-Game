using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MazeAlgorithm
{
    protected Wall[,] walls;
    protected int rows, columns;

    protected MazeAlgorithm(Wall[,] walls)
        : base()
    {
        this.walls = walls;
        rows = walls.GetLength(0);
        columns = walls.GetLength(1);
    }

    public abstract void CreateMaze();
}
