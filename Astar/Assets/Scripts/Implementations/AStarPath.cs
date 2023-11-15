using System.Collections.Generic;
using UnityEngine;

public class AStarPath
{
    private List<Node> openNodes;
    private List<Node> closedNodes;
    private Node[,] nodeGrid;

    public (List<Vector2Int>, PathData) FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
    {
        nodeGrid = new Node[grid.GetLength(0), grid.GetLength(1)];

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                nodeGrid[x, y] = new Node() { position = new Vector2Int(x, y) };
            }
        }

        openNodes = new List<Node>();
        closedNodes = new List<Node>();

        Node startNode = nodeGrid[startPos.x, startPos.y];
        Node endNode = nodeGrid[endPos.x, endPos.y];

        openNodes.Add(startNode);

        PathData pathData = new PathData();

        while (openNodes.Count > 0)
        {
            Node currentNode = GetNodeWithLowestF();
            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);

            pathData.checkedTiles.Add(currentNode.position);

            if (currentNode == endNode) { break; }

            foreach (Node neighbourNode in GetOpenNeighbours(grid, currentNode))
            {
                if (closedNodes.Contains(neighbourNode)) { continue; }

                if (!openNodes.Contains(neighbourNode))
                {

                    neighbourNode.parent = currentNode;
                }

                int newMovementCostToNeighbour = currentNode.GScore + GetDistance(currentNode, neighbourNode);
                if (newMovementCostToNeighbour < neighbourNode.GScore || !openNodes.Contains(neighbourNode)) 
                {
                    neighbourNode.GScore = newMovementCostToNeighbour;
                    neighbourNode.HScore = GetDistance(neighbourNode, endNode);
                    neighbourNode.parent = currentNode;

                    if (!openNodes.Contains(neighbourNode))
                    {
                        openNodes.Add(neighbourNode);
                    }
                }
            }
        }

        List<Vector2Int> path = new List<Vector2Int>();
        Node currentPathHead = endNode;

        while (currentPathHead != startNode)
        {
            path.Insert(0, currentPathHead.position);
            currentPathHead = currentPathHead.parent;
        }

        pathData.pathLength = path.Count;
        return (path, pathData);
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.position.x - nodeB.position.x);
        int distanceY = Mathf.Abs(nodeA.position.y - nodeB.position.y);

        if (distanceX > distanceY)
        {
            return 14 * distanceY + 10 * distanceX;
        }
        else
        {
            return 14 * distanceX + 10 * distanceY;
        }
    }

    private Node GetNodeWithLowestF()
    {
        if (openNodes.Count <= 0) { return null; }

        int lowestF = int.MaxValue;
        Node currentBestNode = null;

        foreach (Node node in openNodes)
        {
            if (node.FScore < lowestF || (node.FScore == currentBestNode.FScore && node.HScore < currentBestNode.HScore))
            {
                lowestF = node.FScore;
                currentBestNode = node;
            }
        }

        return currentBestNode;
    }

    private List<Node> GetOpenNeighbours(Cell[,] grid, Node node)
    {
        List<Node> neighbours = new List<Node>();
        Cell cell = grid[node.position.x, node.position.y];

        if (!cell.HasWall(Wall.RIGHT))
        {
            neighbours.Add(nodeGrid[node.position.x + 1, node.position.y]);
        }

        if (!cell.HasWall(Wall.LEFT))
        {
            neighbours.Add(nodeGrid[node.position.x - 1, node.position.y]);
        }

        if (!cell.HasWall(Wall.UP))
        {
            neighbours.Add(nodeGrid[node.position.x, node.position.y + 1]);
        }

        if (!cell.HasWall(Wall.DOWN))
        {
            neighbours.Add(nodeGrid[node.position.x, node.position.y - 1]);
        }

        return neighbours;
    }

    /// <summary>
    /// This is the Node class you can use this class to store calculated FScores for the cells of the grid, you can leave this as it is
    /// </summary>
    public class Node
    {
        public Vector2Int position; //Position on the grid
        public Node parent; //Parent Node of this node

        public int FScore
        { //GScore + HScore
            get { return GScore + HScore; }
        }
        public int GScore; //Current Travelled Distance
        public int HScore; //Distance estimated based on Heuristic

        public Node() { }
        public Node(Vector2Int position, Node parent, int GScore, int HScore)
        {
            this.position = position;
            this.parent = parent;
            this.GScore = GScore;
            this.HScore = HScore;
        }
    }
}

