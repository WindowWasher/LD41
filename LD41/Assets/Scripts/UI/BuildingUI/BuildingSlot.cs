using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSlot : MonoBehaviour {

    public BuildingData buildingData;
    public Image buildingImage;
    public Image inactiveImage;
    public Button buildButton;
    public Text buildingText;

    public BuildingPlacement placement;

    public void OnClickBuilding()
    {
        placement.buildingData = buildingData;
        placement.BeginPlacingBuilding();
    }

    private void Start()
    {
        buildingText.text = buildingData.buildingName;
        buildingImage.sprite = buildingData.icon;
        inactiveImage.enabled = false;
        buildButton.interactable = true;
    }

    public void Update()
    {
        if (!ResourceManager.Instance().CanAffordOneTimeCost(buildingData.resourceDeltas))
        {
            inactiveImage.enabled = true;
            buildButton.interactable = false;
        }
        else
        {
            inactiveImage.enabled = false;
            buildButton.interactable = true;
        }
    }
}
