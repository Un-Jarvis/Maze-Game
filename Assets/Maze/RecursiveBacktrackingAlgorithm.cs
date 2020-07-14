using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursiveBacktrackingAlgorithm : MazeAlgorithm
{
    public RecursiveBacktrackingAlgorithm(Wall[,] walls)
        : base(walls)
    {
    }

    public override void CreateMaze()
    {
        RecursiveBacktracking();
    }

    private void RecursiveBacktracking()
    {
        // Randomly choose a start point.
        int row = Random.Range(0, rows);
        int column = Random.Range(0, columns);

        // Carve a passage from the start point.
        walls[row, column].visited = true;
        CarvePassage(row, column);
    }

    private void CarvePassage(int row, int column)
    {
        // While there exists any adjacent cell that has not been visited yet, carve a passage through the adjacent cell.
        while (HasAnAdjacentNotVisitedCell(row, column))
        {
            int adjacentCellRow = row;
            int adjacentCellColumn = column;

            bool carveDone = false;

            while (!carveDone)
            {
                // The variable direction indicates which side of wall to remove in order to carve a passage.
                int direction = Random.Range(1, 5);

                carveDone = true;
                if (direction == 1 && CanBeCarved(row - 1, column))
                {
                    // north
                    adjacentCellRow = row - 1;
                    RemoveWall(walls[row, column].northWall, walls[row, column].destructibleWalls);
                    RemoveWall(walls[adjacentCellRow, adjacentCellColumn].southWall, walls[adjacentCellRow, adjacentCellColumn].destructibleWalls);
                }
                else if (direction == 2 && CanBeCarved(row + 1, column))
                {
                    // south
                    adjacentCellRow = row + 1;
                    RemoveWall(walls[row, column].southWall, walls[row, column].destructibleWalls);
                    RemoveWall(walls[adjacentCellRow, adjacentCellColumn].northWall, walls[adjacentCellRow, adjacentCellColumn].destructibleWalls);
                }
                else if (direction == 3 && CanBeCarved(row, column - 1))
                {
                    // west
                    adjacentCellColumn = column - 1;
                    RemoveWall(walls[row, column].westWall, walls[row, column].destructibleWalls);
                    RemoveWall(walls[adjacentCellRow, adjacentCellColumn].eastWall, walls[adjacentCellRow, adjacentCellColumn].destructibleWalls);
                }
                else if (direction == 4 && CanBeCarved(row, column + 1))
                {
                    // east
                    adjacentCellColumn = column + 1;
                    RemoveWall(walls[row, column].eastWall, walls[row, column].destructibleWalls);
                    RemoveWall(walls[adjacentCellRow, adjacentCellColumn].westWall, walls[adjacentCellRow, adjacentCellColumn].destructibleWalls);
                }
                else
                {
                    carveDone = false;
                }
            }

            walls[adjacentCellRow, adjacentCellColumn].visited = true;

            // Recusively call method CarvePassage.
            CarvePassage(adjacentCellRow, adjacentCellColumn);
        }
        // If all adjacent cells have been visited, back up to the last cell that has uncarved wall(s).
    }

    private bool HasAnAdjacentNotVisitedCell(int row, int column)
    {
        int NotVisitedCells = 0;
        // northern side
        // The current cell should not be at the uppermost row.
        if (row > 0 && !walls[row - 1, column].visited)
        {
            NotVisitedCells++;
        }
        // southern side
        // The current cell should not be at the lowermost row.
        if (row < (rows - 1) && !walls[row + 1, column].visited)
        {
            NotVisitedCells++;
        }
        // western side
        // The current cell should not be at the leftmost column.
        if (column > 0 && !walls[row, column - 1].visited)
        {
            NotVisitedCells++;
        }
        // eastern side
        // The current cell should not be at the rightmost column.
        if (column < (columns - 1) && !walls[row, column + 1].visited)
        {
            NotVisitedCells++;
        }
        return NotVisitedCells > 0;
    }

    private bool CanBeCarved(int row, int column)
    {
        // Return true if the cell is not out of maze range and is not visited.
        if (row >= 0 && row < rows && column >= 0 && column < columns && !walls[row, column].visited)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void RemoveWall(GameObject wall, List<GameObject> destructibleWalls)
    {
        if (wall != null)
        {
            GameObject.Destroy(wall);
            destructibleWalls.Remove(wall);
        }
    }
}
