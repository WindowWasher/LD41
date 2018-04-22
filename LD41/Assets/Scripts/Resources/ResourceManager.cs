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
        if (OnResourceChange != null)
            OnResourceChange(resources[resource]);
    }

    public void UpdateResources(List<ResourceDelta> resourceDeltas, int numWorkers)
    {
        foreach(ResourceDelta delta in resourceDeltas.Where(d=>!d.oneTimeChange))
        {
            this.Add(delta.resource, (delta.amount < 0 ? delta.amount : delta.amount * numWorkers));
        }
    }

    public void RemoveOneTimeBenifits(List<ResourceDelta> resourceDeltas)
    {
        foreach(ResourceDelta delta in resourceDeltas.Where(d=>d.oneTimeChange))
        {
            if (delta.amount > 0)
            {
                this.Add(delta.resource, -delta.amount);
                //resources[delta.resource].count -= delta.amount;
                //if (OnResourceChange != null)
                //    OnResourceChange(resources[delta.resource]);
            }
        }
    }

    public void UpdateOneTimeCostResources(List<ResourceDelta> resourceDeltas)
    {
        foreach(ResourceDelta delta in resourceDeltas.Where(d=>d.oneTimeChange))
        {
            this.Add(delta.resource, delta.amount);
        }
    }

    public bool CanAffordOneTimeCost(List<ResourceDelta> resourceDeltas)
    {
        foreach(ResourceDelta delta in resourceDeltas.Where(d=>d.oneTimeChange))
        {
            if (resources[delta.resource].count + delta.amount < 0)
                return false;
        }

        return true;
    }

    public int GetIntervalDelta(Resource resource)
    {
        int delta = 0;
        foreach(var gameObj in GameObject.FindGameObjectsWithTag("Building"))
        {
            Building building = gameObj.GetComponent<Building>();
            foreach (var rDelta in gameObj.GetComponent<Building>().buildingData.resourceDeltas)
            {
                if(rDelta.resource == resource && !rDelta.oneTimeChange)
                {
                    delta += (rDelta.amount < 0 ? rDelta.amount : rDelta.amount * building.workers);
                }
            }
        }

        return delta;
    }

    public int GetAvailableWorkers()
    {
        int availableWorkers = resources[Resource.People].count;
        foreach (var gameObj in GameObject.FindGameObjectsWithTag("Building"))
        {
            Building building = gameObj.GetComponent<Building>();
            //foreach (var rDelta in gameObj.GetComponent<Building>().buildingData.resourceDeltas)
            //{
            //    if (rDelta.resource == Resource.People)
            //    {
            //        availableWorkers += rDelta.amount;
            //    }
            //}
            availableWorkers -= building.workers;

        }

        return availableWorkers;
    }
}
