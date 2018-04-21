using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


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

    void Awake()
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

    public void UpdateResources(List<ResourceDelta> resourceDeltas)
    {
        foreach(ResourceDelta delta in resourceDeltas.Where(d=>!d.oneTimeChange))
        {
            this.Add(delta.resource, delta.amount);
        }
    }


}
