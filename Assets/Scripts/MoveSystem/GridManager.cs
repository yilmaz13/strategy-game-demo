using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    private static GridManager _instance = null;
    public static GridManager Instance => _instance;

    public bool displayGridGizmos;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    public Transform Transform;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Awake()
    {
        //singleton pattern
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }

        _instance = this;

        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public int MaxSize
    {
        get { return gridSizeX * gridSizeY; }
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 -
                                  Vector3.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) +
                                     Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask));

                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public bool CheckGrid(Vector3 pos, float height, float width)
    {
        float column = height / 32;
        float line = width / 32;
        Vector2 horizontalVector = new Vector2(pos.x - (line / 2 * 0.5f), 0);
        Vector2 verticalVector = new Vector2(0, pos.y - (column / 2 * 0.5f));
        Vector2 mapLengthX = new Vector2(-gridWorldSize.x / 2, 0);
        Vector2 mapLengthY = new Vector2(0, -gridWorldSize.y / 2);
        var horizontalLenght = Vector2.Distance(horizontalVector, mapLengthX);
        var verticalLenght = Vector2.Distance(verticalVector, mapLengthY);

        double originX = Math.Round((horizontalLenght * 2.0f));
        double originY = Math.Round(verticalLenght * 2.0f);

        for (int ix = 0; ix < line; ix++)
        {
            for (int jy = 0; jy < column; jy++)
            {
                float x = (int)originX + ix;
                float y = (int)originY + jy;

                Vector2 test = new Vector2(-gridWorldSize.x / 2 + x * 0.5f + 0.2f,
                    -gridWorldSize.y / 2 + y * 0.5f + 0.2f);

                if (!grid[(int)x, (int)y].walkable)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private IEnumerator _updateGriEnumerator(Vector3 pos, float height, float width, bool clearAll = false)
    {
        yield return new WaitForSeconds(0.1f);
        _updateGrid(pos, height, width, clearAll);
        yield return null;
    }

    public void UpdateGrid(Vector3 pos, float height, float width, bool clearAll = false)
    {
        StartCoroutine(_updateGriEnumerator(pos, height, width, clearAll));
    }

    private void _updateGrid(Vector3 spawnPos, float height, float width, bool clearAll = false)
    {
        float column = height / 32;
        float line = width / 32;
        Vector2 horizontalVector = new Vector2(spawnPos.x - (line / 2 * 0.5f), 0);
        Vector2 verticalVector = new Vector2(0, spawnPos.y - (column / 2 * 0.5f));
        Vector2 mapLengthX = new Vector2(-gridWorldSize.x / 2, 0);
        Vector2 mapLengthY = new Vector2(0, -gridWorldSize.y / 2);
        var horizontalLenght = Vector2.Distance(horizontalVector, mapLengthX);
        var verticalLenght = Vector2.Distance(verticalVector, mapLengthY);

        double originX = Math.Round((horizontalLenght * 2.0f));
        double originY = Math.Round(verticalLenght * 2.0f);

        for (int ix = 0; ix < line; ix++)
        {
            for (int jy = 0; jy < column; jy++)
            {
                float x = (int)originX + ix;
                float y = (int)originY + jy;

                Vector2 test = new Vector2(-gridWorldSize.x / 2 + x * 0.5f + 0.2f,
                    -gridWorldSize.y / 2 + y * 0.5f + 0.2f);

                bool walkable = !(Physics2D.OverlapCircle(test, nodeRadius, unwalkableMask));

                if (clearAll)
                    walkable = true;

                grid[(int)x, (int)y] = new Node(walkable, test, (int)x, (int)y);
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }


    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
        if (grid != null && displayGridGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                var size = Vector3.one * (nodeDiameter - .1f);
                Vector3 sizeZf = new Vector3(size.x, size.y, 0.01f);
                Gizmos.DrawCube(n.worldPosition, sizeZf);
            }
        }
    }
}