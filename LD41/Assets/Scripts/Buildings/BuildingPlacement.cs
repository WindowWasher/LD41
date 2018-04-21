using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingPlacement : MonoBehaviour {

    [HideInInspector]
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

    public void BeginPlacingBuilding()
    {
        if (buildingRef != null)
            Destroy(buildingRef);
        isBuilding = true;
        buildingRef = GameObject.Instantiate(building.building, gridRef.getMouseToNodeBottomLeftWorld(), Quaternion.identity);
        canvas.transform.position = gridRef.getMouseToNodeBottomLeftWorld();
        canvas.enabled = true;
        placementImage.enabled = true;
        canvas.transform.localScale = building.gridSize;
    }

    // Update is called once per frame
    void Update() {

        // Click to place building
        if (Input.GetMouseButtonDown(0) && isBuilding && gridRef.CanPlaceBuilding(building.gridSize) && ResourceManager.Instance().CanAfford(building.resourceDeltas))
        {
            // Make any updates before disabling everything
            buildingRef.GetComponent<Building>().ActivateBuilding();
            gridRef.UpdateGridCurrentMousePosition(building.gridSize);

            // Disable everything and set buildingRef to null
            canvas.enabled = false;
            placementImage.enabled = false;
            buildingRef = null;

            if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
                isBuilding = false;
            else
                BeginPlacingBuilding();
        }

        // Right click to clear building selection
        if (Input.GetMouseButtonDown(1) && isBuilding)
        {
            if (buildingRef != null)
                Destroy(buildingRef);
            isBuilding = false;
            canvas.enabled = false;
            placementImage.enabled = false;
            buildingRef = null;
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
