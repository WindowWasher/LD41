using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public bool occupied;
    public Vector3 worldPosition;
    public Vector3 worldBottomLeft;

    public int xPos;
    public int yPos;

    public Node(bool _occupied, Vector3 _worldPosition, Vector3 _worldBottomLeft, int _xPos, int _yPos)
    {
        occupied = _occupied;
        worldPosition = _worldPosition;
        worldBottomLeft = _worldBottomLeft;
        xPos = _xPos;
        yPos = _yPos;
    }
}
