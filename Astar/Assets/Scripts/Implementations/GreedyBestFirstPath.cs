using System.Collections.Generic;
using UnityEngine;

public class GreedyBestFirstPath
{
    private Dictionary<Cell, int> priorityQueue;
    private Dictionary<Cell, Cell> checkedCells;

    public (List<Vector2Int>, PathData) FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
    {
        priorityQueue = new Dictionary<Cell, int>();
        checkedCells = new Dictionary<Cell, Cell>();

        Cell startCell = grid[startPos.x, startPos.y];
        Cell endCell = grid[endPos.x, endPos.y];

        priorityQueue.Add(startCell, 0);

        PathData pathData = new PathData();

        while (priorityQueue.Count > 0)
        {
            Cell currentCell = DeQueueCell();

            pathData.checkedTiles.Add(currentCell.gridPosition);

            if (currentCell == endCell) { break; }

            foreach (Cell neighbourCell in GetOpenNeighbours(grid, currentCell))
            {
                if (checkedCells.ContainsKey(neighbourCell)) { continue; }

                priorityQueue.Add(neighbourCell, GetHeuristicCost(endCell, currentCell));
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

    private Cell DeQueueCell()
    {
        if (priorityQueue.Count <= 0) { return null; }

        int lowestPriority = int.MaxValue;
        Cell currentCell = null;

        foreach (KeyValuePair<Cell, int> item in priorityQueue)
        {
            if (item.Value < lowestPriority)
            {
                lowestPriority = item.Value;
                currentCell = item.Key;
            }
        }

        priorityQueue.Remove(currentCell);

        return currentCell;
    }

    private int GetHeuristicCost(Cell endCell, Cell cell)
    {
        return Mathf.Abs(endCell.gridPosition.x - cell.gridPosition.x) + Mathf.Abs(endCell.gridPosition.y - cell.gridPosition.y);
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

