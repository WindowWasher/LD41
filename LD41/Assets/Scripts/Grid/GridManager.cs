using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

    public static GridManager instance;

    public Vector2 gridWorldSize;
    public LayerMask occupiedMask;
    public float nodeRadius;

    private Node[,] grid;
    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    private Vector3 worldMousePosition;

    private void Start()
    {
        if (!instance)
            instance = this;

        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];

        Vector3 worldBottomLeft = transform.position - Vector3.right * (gridWorldSize.x / 2) - Vector3.forward * (gridWorldSize.y/2);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                Vector3 bottomLeft = worldBottomLeft + Vector3.right * (x * nodeDiameter) + Vector3.forward * (y * nodeDiameter);

                bool occupied = Physics.CheckSphere(worldPoint, nodeRadius, occupiedMask);
                grid[x, y] = new Node(occupied, worldPoint, bottomLeft, x, y);
            }
        }
    }

    public void SetOccupiedToFalse(Node buildingStartNode, Vector2 size)
    {
        int x = buildingStartNode.xPos;
        int y = buildingStartNode.yPos;

        // Subtract 1 from size.x and size.y to be sure that we can make checks along the grid boundaries
        if (size.x - 1 + x < gridSizeX && size.y - 1 + y < gridSizeY)
        {
            for (int xPos = x; xPos < x + size.x; xPos++)
            {
                for (int yPos = y; yPos < y + size.y; yPos++)
                {
                    grid[xPos, yPos].occupied = false;
                }
            }
        }
    }

    public void UpdateGrid()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                grid[x, y].occupied = Physics.CheckSphere(grid[x,y].worldPosition, nodeRadius, occupiedMask);
            }
        }
    }

    public Node GetNodeUnderMouse()
    {
        return NodeFromWorldPoint(worldMousePosition);
    }

    public void UpdateGridCurrentMousePosition(Vector2 size)
    {
        Node mouseNode = NodeFromWorldPoint(worldMousePosition);
        UpdateGridFromNode(mouseNode, size);
    }

    public void UpdateGridFromNode(Node startNode, Vector2 size)
    {
        Debug.Log("Called");
        int x = startNode.xPos;
        int y = startNode.yPos;

        // Subtract 1 from size.x and size.y to be sure that we can make checks along the grid boundaries
        if (size.x - 1 + x < gridSizeX && size.y - 1 + y < gridSizeY)
        {
            for (int xPos = x; xPos < x + size.x; xPos++)
            {
                for (int yPos = y; yPos < y + size.y; yPos++)
                {
                    grid[xPos, yPos].occupied = Physics.CheckSphere(grid[xPos, yPos].worldPosition, nodeRadius, occupiedMask);
                }
            }
        }
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null)
        {
            foreach (Node n in grid)
            {
                Color color = new Color(1, 1, 1, 0.5f);
                Gizmos.color = color;
                Gizmos.DrawCube(n.worldPosition, new Vector3(1,0,1) * (nodeDiameter - .1f)); 
            }
        }
    }

    public Vector3 getMouseToNodeWorldPos()
    {
        worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 30));
        Node mouseNode = NodeFromWorldPoint(worldMousePosition);

        return mouseNode.worldPosition;
    }

    public Vector3 getMouseToNodeBottomLeftWorld()
    {
        worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 30));
        Node mouseNode = NodeFromWorldPoint(worldMousePosition);

        return mouseNode.worldBottomLeft;
    }

    public bool CanPlaceBuilding(Vector2 size)
    {
        Node mouseNode = NodeFromWorldPoint(worldMousePosition);

        int x = mouseNode.xPos;
        int y = mouseNode.yPos;

        // Subtract 1 from size.x and size.y to be sure that we can make checks along the grid boundaries
        if (size.x - 1 + x < gridSizeX && size.y - 1 + y < gridSizeY)
        {
            for (int xPos = x; xPos < x + size.x; xPos++)
            {
                for (int yPos = y; yPos < y + size.y; yPos++)
                {
                    if (grid[xPos, yPos].occupied)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}
