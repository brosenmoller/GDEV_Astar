using System.Collections.Generic;
using UnityEngine;

public class PathData
{
    public int pathLength;
    public List<Vector2Int> checkedTiles;

    public PathData(int pathLength, List<Vector2Int> checkedTiles)
    {
        this.checkedTiles = checkedTiles;
        this.pathLength = pathLength;
    }

    public PathData()
    {
        checkedTiles = new List<Vector2Int>();
    }
}

