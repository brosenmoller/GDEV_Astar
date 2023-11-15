using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PathAlogrithm currentAlgorithm;
    public List<Vector2Int> checkedTiles;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (int i = 0; i < checkedTiles.Count; i++)
        {
            Gizmos.DrawSphere(new Vector3(checkedTiles[i].x, 0, checkedTiles[i].y), .2f);
        }
    }
}
