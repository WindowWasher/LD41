using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceDelta
{
    public Resource resource;
    public int amount;
    public bool oneTimeChange; // for things like people and cost of resources
}

[CreateAssetMenu(fileName = "New Building", menuName = "Building")]
public class BuildingData : ScriptableObject {

    public GameObject building;
    public string buildingName;
    public Vector2 gridSize;
    public Sprite icon;
    public List<ResourceDelta> resourceDeltas;
}
