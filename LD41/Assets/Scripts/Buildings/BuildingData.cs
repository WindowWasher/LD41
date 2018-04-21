using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Building")]
public class BuildingData : ScriptableObject {

    public GameObject building;
    public string buildingName;
    public Vector2 gridSize;
    public Sprite icon;
}
