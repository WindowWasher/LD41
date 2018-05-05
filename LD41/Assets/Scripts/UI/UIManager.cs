using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Dictionary<ResourceInventory, GameObject> panels;
    Timer nextUpdate = new Timer();

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

    public void updateResources(ResourceInventory changedResource)
    {
        // start slowing down after 3 minutes
        if (!nextUpdate.Expired() && Time.deltaTime > 180)
            return;
        nextUpdate.Start(0.25f);
        foreach (ResourceInventory resource in ResourceManager.Instance().resources.Values)
        {
            int intervalData = ResourceManager.Instance().GetIntervalDelta(resource.resourceEnum);
            
            if (resource.resourceEnum == Resource.People)
            {
                panels[resource].GetComponentInChildren<Text>().text = resource.count.ToString() + "/" + ResourceManager.Instance().GetTotalWorkers().ToString();
            }
            else
            {
                string intervalDataStr = string.Format(" ({0}{1})", (intervalData <= 0 ? "" : "+"), intervalData);
                panels[resource].GetComponentInChildren<Text>().text = resource.count.ToString() + intervalDataStr;
            }
            if (resource.count < 0 || intervalData < 0 || (resource.resourceEnum == Resource.People && ResourceManager.Instance().resources[Resource.Food].count <= 0))
            {
                panels[resource].GetComponentInChildren<Text>().color = Color.red;
            }
            else
            {
                panels[resource].GetComponentInChildren<Text>().color = Color.white;
            }

        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
