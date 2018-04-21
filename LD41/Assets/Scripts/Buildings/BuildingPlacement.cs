using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingPlacement : MonoBehaviour {

    [HideInInspector]
    public BuildingData buildingData;
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
        buildingRef = GameObject.Instantiate(buildingData.building, gridRef.getMouseToNodeBottomLeftWorld(), Quaternion.identity);
        canvas.transform.position = gridRef.getMouseToNodeBottomLeftWorld();
        canvas.enabled = true;
        placementImage.enabled = true;
        canvas.transform.localScale = buildingData.gridSize;
    }

    // Update is called once per frame
    void Update() {

        // Click to place building
        if (Input.GetMouseButtonDown(0) && isBuilding && gridRef.CanPlaceBuilding(buildingData.gridSize) && ResourceManager.Instance().CanAffordOneTimeCost(buildingData.resourceDeltas))
        {
            // Make any updates before disabling everything
            gridRef.UpdateGridCurrentMousePosition(buildingData.gridSize);
            buildingRef.GetComponent<Building>().BuildingPlaced(gridRef.GetNodeUnderMouse());

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
        else if (Input.GetMouseButtonDown(1) && !isBuilding)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.Log(hit.collider.name);
                Building building = hit.collider.GetComponent<Building>();

                if (building)
                {
                    building.DestroyBuilding();
                }
            }
        }

        if (isBuilding && buildingRef)
        {
            Vector3 mouseNodeBottomLeftPos = gridRef.getMouseToNodeBottomLeftWorld();
            buildingRef.transform.position = mouseNodeBottomLeftPos;
            canvas.transform.position = mouseNodeBottomLeftPos;

            if (gridRef.CanPlaceBuilding(buildingData.gridSize))
                placementImage.sprite = canPlaceGraphic;
            else
                placementImage.sprite = cantPlaceGraphic;
        }
    }
}
