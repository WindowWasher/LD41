using UnityEngine;

[CreateAssetMenu(fileName ="New Building", menuName = "Building")]
public class Building : ScriptableObject {

    public GameObject building;
    public Vector2 gridSize;
}
