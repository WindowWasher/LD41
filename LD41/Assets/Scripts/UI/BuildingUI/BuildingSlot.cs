using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingSlot : MonoBehaviour {

    public BuildingData buildingData;
    public Image buildingImage;
    public Image inactiveImage;
    public Button buildButton;
    public Text buildingText;

    public BuildingPlacement placement;


    GameObject toolTip;
    Text ToolTipBuildingName;
    //Image PeopleImage;
    Text toolTipPeopleUsage;
    GameObject toolTipCostPanel;
    GameObject toolTipOutputPanel;
    GameObject toolTipUpkeepPanel;
    public GameObject resourceDeltaListItem;
    Timer toolTipTimer = new Timer();
    bool hovering = false;
    float toolTipTimerWait = 0.5f;


    public void OnClickBuilding()
    {
        placement.buildingData = buildingData;
        placement.BeginPlacingBuilding();
    }

    public void Awake()
    {
        toolTip = GameObject.Find("ToolTip");

        ToolTipBuildingName = GameObject.Find("ToolTipBuildingName").GetComponent<Text>();
        //PeopleImage = GameObject.Find("PeopleImage").GetComponent<Image>();
        toolTipPeopleUsage = GameObject.Find("ToolTipPeopleUsage").GetComponent<Text>();
        toolTipCostPanel = GameObject.Find("ToolTipCostPanel");
        toolTipOutputPanel = GameObject.Find("ToolTipOutputPanel");
        toolTipUpkeepPanel = GameObject.Find("ToolTipUpkeepPanel");

        EventTrigger.Entry eventtype = new EventTrigger.Entry();
        eventtype.eventID = EventTriggerType.PointerEnter;
        eventtype.callback.AddListener((eventData) => { OnPointerEnter(); });

        EventTrigger.Entry eventtype2 = new EventTrigger.Entry();
        eventtype2.eventID = EventTriggerType.PointerExit;
        eventtype2.callback.AddListener((eventData) => { OnPointerExit(); });

        buildButton.gameObject.AddComponent<EventTrigger>();
        buildButton.gameObject.GetComponent<EventTrigger>().triggers.Add(eventtype);
        buildButton.gameObject.GetComponent<EventTrigger>().triggers.Add(eventtype2);

        inactiveImage.gameObject.AddComponent<EventTrigger>();
        inactiveImage.gameObject.GetComponent<EventTrigger>().triggers.Add(eventtype);
        inactiveImage.gameObject.GetComponent<EventTrigger>().triggers.Add(eventtype2);
    }

    private void Start()
    {
        //Debug.Log(buildingData.buildingName);

        buildingText.text = buildingData.buildingName;
        buildingImage.sprite = buildingData.icon;
        inactiveImage.enabled = false;
        buildButton.interactable = true;


        HideToolTipeInfo();
    }

    public void Update()
    {
        if (!ResourceManager.Instance().CanAffordOneTimeCost(buildingData))
        {
            inactiveImage.enabled = true;
            buildButton.interactable = false;
        }
        else
        {
            inactiveImage.enabled = false;
            buildButton.interactable = true;
        }
        if(hovering && toolTipTimer.Expired() && !toolTip.activeSelf)
        {
            ShowToolTipInfo();
        }
    }

    public void OnPointerEnter()
    {
        //Debug.Log("Enter!");
        hovering = true;
        toolTipTimer.Start(toolTipTimerWait);
        
        
    }

    public void OnPointerExit()
    {
        //Debug.Log("Exit!");
        hovering = false;
        HideToolTipeInfo();
    }

    public void HideToolTipeInfo()
    {
        toolTip.SetActive(false);
        foreach (Transform child in toolTipCostPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in toolTipOutputPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Transform child in toolTipUpkeepPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }



    public void ShowToolTipInfo()
    {
        toolTip.SetActive(true);
        toolTip.transform.position = this.transform.position + new Vector3(175, 250, 0);

        ToolTipBuildingName.text = buildingData.buildingName;
        toolTipPeopleUsage.text = "Max: " + buildingData.maxWorkerSize.ToString();

        if(buildingData.maxWorkerSize > 0)
        {
            var obj = GameObject.Instantiate(resourceDeltaListItem, toolTipCostPanel.transform);
            obj.GetComponentInChildren<Text>().text = buildingData.maxWorkerSize.ToString();
            if(ResourceManager.Instance().resources[Resource.People].count - buildingData.maxWorkerSize < 0)
            {
                obj.GetComponentInChildren<Text>().color = Color.red;
            }
            else
            {
                obj.GetComponentInChildren<Text>().color = Color.white;
            }
            obj.GetComponentInChildren<Image>().sprite = ResourceManager.Instance().resources[Resource.People].hudImage;
        }

        // Cost
        foreach (var delta in ResourceManager.SortResourceList(buildingData.resourceDeltas))
        {
            if (!delta.oneTimeChange || delta.amount > 0)
                continue;
            var obj = GameObject.Instantiate(resourceDeltaListItem, toolTipCostPanel.transform);
            obj.GetComponentInChildren<Text>().text = (-1 * delta.amount).ToString();
            if(ResourceManager.Instance().resources[delta.resource].count + delta.amount < 0)
            {
                obj.GetComponentInChildren<Text>().color = Color.red;
            }
            else
            {
                obj.GetComponentInChildren<Text>().color = Color.white;
            }
            obj.GetComponentInChildren<Image>().sprite = ResourceManager.Instance().resources[delta.resource].hudImage;
        }

        // Weapon Output
        if(buildingData.attackData != null)
        {
            var obj = GameObject.Instantiate(resourceDeltaListItem, toolTipOutputPanel.transform);
            obj.GetComponentInChildren<Text>().text = buildingData.attackData.attackDamage.ToString();
            obj.GetComponentInChildren<Image>().sprite = ResourceManager.Instance().AttackDamageIcon;

            var obj2 = GameObject.Instantiate(resourceDeltaListItem, toolTipOutputPanel.transform);
            obj2.GetComponentInChildren<Text>().text = buildingData.attackData.attackRange.ToString();
            obj2.GetComponentInChildren<Image>().sprite = ResourceManager.Instance().AttackRangeIcon;
        }


        // Output
        foreach (var delta in ResourceManager.SortResourceList(buildingData.resourceDeltas))
        {
            if (delta.oneTimeChange && delta.amount <0)
                continue;

            int workerSize = Mathf.Max(buildingData.maxWorkerSize, 1);
            string intervalDataStr = string.Format("{0}{1}", (delta.amount <= 0 ? "" : ""), Mathf.Abs(delta.amount) * workerSize);
            //if (!delta.oneTimeChange)
            //    intervalDataStr += "/worker";
            GameObject panel;
            if (delta.amount < 0)
                panel = toolTipUpkeepPanel;
            else
                panel = toolTipOutputPanel;

            var obj = GameObject.Instantiate(resourceDeltaListItem, panel.transform);
            obj.GetComponentInChildren<Text>().text = intervalDataStr;
            obj.GetComponentInChildren<Image>().sprite = ResourceManager.Instance().resources[delta.resource].hudImage;
                        
        }

        

    }
}
