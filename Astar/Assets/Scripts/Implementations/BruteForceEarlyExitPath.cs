using System.Collections.Generic;
using UnityEngine;

public class BruteForceEarlyExitPath
{
    private Queue<Cell> frontier;
    private Dictionary<Cell, Cell> checkedCells;

    public (List<Vector2Int>, PathData) FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
    {
        checkedCells = new Dictionary<Cell, Cell>();
        frontier = new Queue<Cell>();

        Cell startCell = grid[startPos.x, startPos.y];
        Cell endCell = grid[endPos.x, endPos.y];

        frontier.Enqueue(startCell);

        PathData pathData = new PathData();

        while (frontier.Count > 0)
        {
            Cell currentCell = frontier.Dequeue();

            pathData.checkedTiles.Add(currentCell.gridPosition);

            if (currentCell == endCell) { break; }

            foreach (Cell neighbourCell in GetOpenNeighbours(grid, currentCell))
            {
                if (checkedCells.ContainsKey(neighbourCell)) { continue; }

                frontier.Enqueue(neighbourCell);
                checkedCells.Add(neighbourCell, currentCell);
            }
        }

        List<Vector2Int> path = new List<Vector2Int>();
        Cell currentPathHead = endCell;

        while (currentPathHead != startCell)
        {
            path.Insert(0, currentPathHead.gridPosition);
            currentPathHead = checkedCells[currentPathHead];
        }

        pathData.pathLength = path.Count;
        return (path, pathData);
    }

    private List<Cell> GetOpenNeighbours(Cell[,] grid, Cell cell)
    {
        List<Cell> result = new List<Cell>();

        if (!cell.HasWall(Wall.RIGHT))
        {
            result.Add(grid[cell.gridPosition.x + 1, cell.gridPosition.y]);
        }

        if (!cell.HasWall(Wall.LEFT))
        {
            result.Add(grid[cell.gridPosition.x - 1, cell.gridPosition.y]);
        }

        if (!cell.HasWall(Wall.UP))
        {
            result.Add(grid[cell.gridPosition.x, cell.gridPosition.y + 1]);
        }

        if (!cell.HasWall(Wall.DOWN))
        {
            result.Add(grid[cell.gridPosition.x, cell.gridPosition.y - 1]);
        }

        return result;
    }
}

