using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPlacement : MonoBehaviour {

    public BuildingData building;
    public GridManager gridRef;
    public Canvas canvas;
    public Image placementImage;
    public Sprite canPlaceGraphic;
    public Sprite cantPlaceGraphic;

    public bool isBuilding = false;

    private GameObject buildingRef;
	// Use this for initialization
	void Start () {
        canvas.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0) && isBuilding && gridRef.CanPlaceBuilding(building.gridSize))
        {
            isBuilding = false;
            canvas.enabled = false;
            placementImage.enabled = false;
            gridRef.UpdateGridCurrentMousePosition(building.gridSize);
        }
        else if (Input.GetMouseButtonDown(0) && !isBuilding)
        {
            isBuilding = true;
            buildingRef = GameObject.Instantiate(building.building, Vector3.zero, Quaternion.identity);
            canvas.enabled = true;
            placementImage.enabled = true;
            canvas.transform.localScale = building.gridSize;
        }

        if (isBuilding && buildingRef)
        {
            Vector3 mouseNodeBottomLeftPos = gridRef.getMouseToNodeBottomLeftWorld();
            buildingRef.transform.position = mouseNodeBottomLeftPos;
            canvas.transform.position = mouseNodeBottomLeftPos;

            if (gridRef.CanPlaceBuilding(building.gridSize))
                placementImage.sprite = canPlaceGraphic;
            else
                placementImage.sprite = cantPlaceGraphic;
        }
    }
}
