using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Resource
{
    People,
    Food,
    Happiness,
    Gold,
    Wood,
    Iron
}


public class ResourceManager:MonoBehaviour  {

    public delegate void OnResourceChangeAction(ResourceInventory resource);
    public event OnResourceChangeAction OnResourceChange;

    public static ResourceManager Instance()
    {
        return GameObject.Find("ResourceManager").GetComponent<ResourceManager>();
    }

    void Start()
    {
        resources = new Dictionary<Resource, ResourceInventory>();
        foreach(ResourceInventory inventory in resourceList)
        {
            resources[inventory.resourceEnum] = inventory;
        }
    }


    public List<ResourceInventory> resourceList;
    public Dictionary<Resource, ResourceInventory> resources;

    public void Add(Resource resource, int addition)
    {
        resources[resource].count += addition;
        OnResourceChange(resources[resource]);
    }

    public void Remove(Resource resource, int subtraction)
    {
        resources[resource].count -= subtraction;
        OnResourceChange(resources[resource]);
    }


}
