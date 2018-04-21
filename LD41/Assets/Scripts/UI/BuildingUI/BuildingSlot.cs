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
        placement.building = buildingData;
        placement.BeginPlacingBuilding();
    }

    private void Start()
    {
        buildingText.text = buildingData.buildingName;
        buildingImage.sprite = buildingData.icon;
        inactiveImage.enabled = false;
    }
}
