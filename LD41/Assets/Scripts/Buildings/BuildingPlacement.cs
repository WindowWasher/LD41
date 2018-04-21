using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPlacement : MonoBehaviour {

    public Building building;
    public GridManager gridRef;
    public Canvas placementCanvas;
    public Image placementImage;
    public Sprite canPlaceGraphic;
    public Sprite cantPlaceGraphic;

    public bool isBuilding = false;

    private GameObject buildingRef;
	// Use this for initialization
	void Start () {
        placementCanvas.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0) && isBuilding && gridRef.CanPlaceBuilding(building.gridSize))
        {
            isBuilding = false;
            placementCanvas.enabled = false;
            gridRef.UpdateGridCurrentMousePosition(building.gridSize);
        }
        else if (Input.GetMouseButtonDown(0) && !isBuilding)
        {
            isBuilding = true;
            buildingRef = GameObject.Instantiate(building.building, Vector3.zero, Quaternion.identity);
            placementCanvas.enabled = true;
            placementCanvas.transform.localScale = building.gridSize;
        }

        if (isBuilding && buildingRef)
        {
            Vector3 mouseNodeWorldPos = gridRef.getMouseToNodeWorldPos();
            buildingRef.transform.position = gridRef.getMouseToNodeBottomLeftWorld();
            placementCanvas.transform.position = mouseNodeWorldPos + new Vector3(0.5f, 0, 0.5f);

            if (gridRef.CanPlaceBuilding(building.gridSize))
                placementImage.sprite = canPlaceGraphic;
            else
                placementImage.sprite = cantPlaceGraphic;
        }
    }
}
