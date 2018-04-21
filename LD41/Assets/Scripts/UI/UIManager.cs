using System.Collections;
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
            updateResource(resource);
        }

        ResourceManager.Instance().OnResourceChange += updateResource;

	}

    void updateResource(ResourceInventory resource)
    {
        panels[resource].GetComponentInChildren<Text>().text = resource.name + " " + resource.count.ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
