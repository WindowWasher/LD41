using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingPlacement : MonoBehaviour {

    [HideInInspector]
    public BuildingData buildingData;
    public GridManager gridRef;
    public Sprite canPlaceGraphic;
    public Sprite cantPlaceGraphic;

    public bool isBuilding = false;

    public bool startBuildingsCreated = false;

    public delegate void OnBuildingCreation(Building building);
    public event OnBuildingCreation OnBuildingCreationAction;

    private GameObject buildingRef;
    private Building buildingComp;

    public void BeginPlacingBuilding()
    {
        if (buildingRef != null)
            Destroy(buildingRef);
        isBuilding = true;
        buildingRef = GameObject.Instantiate(buildingData.building, gridRef.getMouseToNodeBottomLeftWorld(), Quaternion.identity);
        buildingComp = buildingRef.GetComponent<Building>();
        buildingComp.placementCanvas.enabled = true;
    }

    public void PlaceStartBuildings()
    {
        List<GameObject> houseObjs = new List<GameObject>();
        List<GameObject> nonHouseObjs = new List<GameObject>();
        foreach(GameObject buildingObj in GameObject.FindGameObjectsWithTag("Building"))
        {
            if(buildingObj.GetComponent<Building>().buildingData.buildingName == "House")
            {
                houseObjs.Add(buildingObj);
            }
            else
            {
                nonHouseObjs.Add(buildingObj);
            }
            
        }

        // Do houses first
        foreach(GameObject bObj in houseObjs)
        {
            PlaceBeginningBuilding(bObj);
        }

        foreach (GameObject bObj in nonHouseObjs)
        {
            PlaceBeginningBuilding(bObj);
        }

        // reset initial values
        foreach(var keyVal in ResourceManager.Instance().resources)
        {
            if(keyVal.Value.resourceEnum != Resource.People)
            {
                keyVal.Value.count = keyVal.Value.initialCount;
            }
        }

        GameObject.FindObjectOfType<UIManager>().updateResources(null);

        BuildingInfoManager.Instance().DisableBuildingPanel();
        //gridRef.UpdateGrid();
        startBuildingsCreated = true;
    }

    public void PlaceBeginningBuilding(GameObject buildingObj)
    {
        Building newBuilding = buildingObj.GetComponent<Building>();
        newBuilding.placementCanvas.enabled = false;

        Node startNode = gridRef.NodeFromWorldPoint(buildingObj.transform.position);
        buildingObj.transform.position = startNode.worldBottomLeft;
        gridRef.UpdateGridFromNode(startNode, newBuilding.buildingData.gridSize, true);
        newBuilding.BuildingPlaced(startNode);

        if (OnBuildingCreationAction != null)
        {
            OnBuildingCreationAction(buildingObj.GetComponent<Building>());
        }
    }

    // Update is called once per frame
    void Update() {

        if(!startBuildingsCreated)
        {
            PlaceStartBuildings();
        }

        Node mouseNode = gridRef.GetNodeUnderMouse();

        if (mouseNode == null)
            return;

        // Click to place building
        if (Input.GetMouseButton(0) && isBuilding && gridRef.CanPlaceBuilding(buildingData.gridSize) && ResourceManager.Instance().CanAffordOneTimeCost(buildingData) && !EventSystem.current.IsPointerOverGameObject())
        {
            // Make any updates before disabling everything
            buildingRef.GetComponent<Building>().BuildingPlaced(mouseNode);
            gridRef.UpdateGridFromNode(mouseNode, buildingData.gridSize, true);
            buildingRef.transform.position = mouseNode.worldBottomLeft; 

            if (OnBuildingCreationAction != null)
            {
                OnBuildingCreationAction(buildingRef.GetComponent<Building>());
            }

            // Disable everything and set buildingRef to null
            buildingComp.placementCanvas.enabled = false;
            buildingRef = null;
            buildingComp = null;

            if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift) || !ResourceManager.Instance().CanAffordOneTimeCost(buildingData))
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
            buildingComp.placementCanvas.enabled = false;
            buildingRef = null;
            buildingComp = null;
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
            buildingRef.transform.position = mouseNode.worldBottomLeft; 

            if (gridRef.CanPlaceBuilding(buildingData.gridSize, mouseNode))
                buildingComp.placementImage.sprite = canPlaceGraphic;
            else
                buildingComp.placementImage.sprite = cantPlaceGraphic;
        }
    }
}
