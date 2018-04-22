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
    Image PeopleImage;
    Text PeopleUsage;
    Button PeopleMinusButton;
    Button PeoplePlusButton;
    GameObject resourceDetailPanel;

	// Use this for initialization
	void Start () {
        buildingPanel = GameObject.Find("BuildingPanel");

        buildingImage = GameObject.Find("BuildingImage").GetComponent<Image>();
        buildingName = GameObject.Find("BuildingName").GetComponent<Text>();
        PeopleImage = GameObject.Find("PeopleImage").GetComponent<Image>();
        PeopleUsage = GameObject.Find("PeopleUsage").GetComponent<Text>();
        PeopleMinusButton = GameObject.Find("PeopleMinusButton").GetComponent<Button>();
        PeoplePlusButton = GameObject.Find("PeoplePlusButton").GetComponent<Button>();
        resourceDetailPanel = GameObject.Find("ResourceDetailPanel");


        DisableBuildingPanel();

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (buildingPanel.activeSelf && !EventSystem.current.IsPointerOverGameObject())
            {
                DisableBuildingPanel();
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                Building building = hit.transform.gameObject.GetComponent<Building>();
                if(building)
                {
                    ShowBuildingInfo(building);
                }
            }
        }
		
	}

    void DisableBuildingPanel()
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
        buildingPanel.SetActive(true);
        buildingName.text = building.buildingData.name;
        PeopleUsage.text = "(5/18)";
        foreach(var delta in building.buildingData.resourceDeltas)
        {
            if (delta.oneTimeChange && delta.resource != Resource.People)
                continue;
            var obj = GameObject.Instantiate(resourceDeltaListItem, resourceDetailPanel.transform);
            string intervalDataStr = string.Format("{0}{1}", (delta.amount <= 0 ? "" : "+"), delta.amount);
            obj.GetComponentInChildren<Text>().text = intervalDataStr;
            obj.GetComponentInChildren<Image>().sprite = ResourceManager.Instance().resources[delta.resource].hudImage;
        }
        
    }
}
