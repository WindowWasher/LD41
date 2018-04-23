using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingInfoManager : MonoBehaviour {

    public GameObject resourceDeltaListItem;

    GameObject buildingPanel;
    Image buildingImage;
    Text buildingName;
    //Image PeopleImage;
    Text PeopleUsage;
    Button PeopleMinusButton;
    Button PeoplePlusButton;
    GameObject resourceDetailPanel;

    Building building = null;

	// Use this for initialization
	void Start () {
        //buildingPanel = GameObject.Find("BuildingInfoPanel");

        //buildingImage = GameObject.Find("BuildingInfoImage").GetComponent<Image>();
        //buildingName = GameObject.Find("BuildingInfoName").GetComponent<Text>();
        ////PeopleImage = GameObject.Find("PeopleImage").GetComponent<Image>();
        //PeopleUsage = GameObject.Find("PeopleInfoUsage").GetComponent<Text>();
        //PeopleMinusButton = GameObject.Find("PeopleInfoMinusButton").GetComponent<Button>();
        //PeoplePlusButton = GameObject.Find("PeopleInfoPlusButton").GetComponent<Button>();
        //resourceDetailPanel = GameObject.Find("ResourceInfoDetailPanel");

        //PeopleMinusButton.onClick.AddListener(MinusButtonClicked);
        //PeoplePlusButton.onClick.AddListener(PlusButtonClicked);



        //DisableBuildingPanel();

    }

    void MinusButtonClicked()
    {
        Debug.Log("Clicked minus!");
        building.workers = Mathf.Max(0, building.workers - 1);
        ShowBuildingInfo(building);
    }

    void PlusButtonClicked()
    {
        building.workers = Mathf.Min(building.buildingData.maxWorkerSize, building.workers + 1);
        ShowBuildingInfo(building);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (buildingPanel.activeSelf && !EventSystem.current.IsPointerOverGameObject())
        //    {
        //        DisableBuildingPanel();
        //    }

        //    BuildingPlacement buildingPlacement = GameObject.FindObjectOfType<BuildingPlacement>();
        //    if(!buildingPlacement.isBuilding)
        //    {
        //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //        RaycastHit hit;
        //        if (Physics.Raycast(ray, out hit, 100))
        //        {
        //            Building building = hit.transform.gameObject.GetComponent<Building>();
        //            if (building)
        //            {
        //                ShowBuildingInfo(building);
        //            }
        //        }
        //    }

            
        //}
		
	}

    public void DisableBuildingPanel()
    {
        foreach (Transform child in resourceDetailPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        buildingPanel.SetActive(false);
    }

    public static BuildingInfoManager Instance()
    {
        return GameObject.Find("BuildingInfoManager").GetComponent<BuildingInfoManager>();
    }

    public void ShowBuildingInfo(Building building)
    {
        DisableBuildingPanel();
        this.building = building;
        buildingPanel.SetActive(true);
        buildingImage.sprite = building.buildingData.icon;
        buildingName.text = building.buildingData.buildingName;
        PeopleUsage.text = string.Format("{0} / {1}", building.workers, building.buildingData.maxWorkerSize);
        foreach(var delta in building.buildingData.resourceDeltas)
        {
            if ((delta.oneTimeChange && delta.resource != Resource.People))
                continue;
            var obj = GameObject.Instantiate(resourceDeltaListItem, resourceDetailPanel.transform);
            int deltaChange = delta.oneTimeChange || delta.amount < 0 ? delta.amount: building.workers * delta.amount;
            string intervalDataStr = string.Format("{0}{1}", (deltaChange <= 0 ? "" : "+"), deltaChange);
            obj.GetComponentInChildren<Text>().text = intervalDataStr;
            obj.GetComponentInChildren<Image>().sprite = ResourceManager.Instance().resources[delta.resource].hudImage;
        }
        
    }

}
