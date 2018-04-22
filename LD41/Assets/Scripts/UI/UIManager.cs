﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Dictionary<ResourceInventory, GameObject> panels;
	// Use this for initialization
	void Start () {
        //Debug.Log("Starting");
        panels = new Dictionary<ResourceInventory, GameObject>();
        foreach (ResourceInventory resource in ResourceManager.Instance().resources.Values)
        {
            string panelName = resource.resourceName + "Panel";
            panels[resource] = GameObject.Find(panelName);
            panels[resource].transform.Find("ResourceImage").GetComponent<Image>().sprite = resource.hudImage;
        }

        ResourceManager.Instance().OnResourceChange += updateResources;
        updateResources(null);

	}

    void updateResources(ResourceInventory changedResource)
    {
        foreach (ResourceInventory resource in ResourceManager.Instance().resources.Values)
        {
            int intervalData = ResourceManager.Instance().GetIntervalDelta(resource.resourceEnum);
            string intervalDataStr = string.Format(" ({0}{1})", (intervalData <= 0 ? "" : "+"), intervalData);
            panels[resource].GetComponentInChildren<Text>().text = resource.count.ToString() + intervalDataStr;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}