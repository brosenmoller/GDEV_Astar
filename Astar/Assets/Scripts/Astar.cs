using System.Collections.Generic;
using UnityEngine;

public enum PathAlogrithm
{
    BruteForce = 0,
    BruteForceEarlyExit = 1,
    GreedyBestFirst = 2,
    AStar = 3,
}

public class Astar
{
    /// <summary>
    /// TODO: Implement this function so that it returns a list of Vector2Int positions which describes a path from the startPos to the endPos
    /// Note that you will probably need to add some helper functions
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="grid"></param>
    /// <returns></returns>
    /// 

    public List<Vector2Int> FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
    {
        if (endPos.x < 0 || endPos.x >= grid.GetLength(0) || endPos.y < 0 || endPos.y >= grid.GetLength(1))
        {
            return null;
        }

        (List<Vector2Int>, PathData) resultData;

        GameManager.Instance.checkedTiles = new List<Vector2Int>();

        switch (GameManager.Instance.currentAlgorithm)
        {
            case PathAlogrithm.BruteForce:
                BruteForcePath bruteForcePath = new BruteForcePath();
                resultData = bruteForcePath.FindPathToTarget(startPos, endPos, grid);
                break;
            case PathAlogrithm.BruteForceEarlyExit:
                BruteForceEarlyExitPath bruteForceEarlyExitPath = new BruteForceEarlyExitPath();
                resultData = bruteForceEarlyExitPath.FindPathToTarget(startPos, endPos, grid);
                break;
            case PathAlogrithm.GreedyBestFirst:
                GreedyBestFirstPath bestFirstPath = new GreedyBestFirstPath();
                resultData = bestFirstPath.FindPathToTarget(startPos, endPos, grid);
                break;
            case PathAlogrithm.AStar:
                AStarPath aStarPath = new AStarPath();
                resultData = aStarPath.FindPathToTarget(startPos, endPos, grid);
                break;
            default: 
                return null;

        }

        List<Vector2Int> path = resultData.Item1;
        PathData pathData = resultData.Item2;

        GameManager.Instance.checkedTiles = pathData.checkedTiles;

        Debug.Log(GameManager.Instance.currentAlgorithm.ToString() + 
            " --- Tiles Searched: " + pathData.checkedTiles.Count.ToString() + 
            " --- Path Length: " + pathData.pathLength.ToString()
        );

        return path;
    }
}
