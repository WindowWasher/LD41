using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public enum Resource
{
    People=0,
    Gold=1,
    Wood=2,
    Iron=3,
    Goods=4,
    Food=5
}

public class ResourceManager:MonoBehaviour  {

    public delegate void OnResourceChangeAction(ResourceInventory resource);
    public event OnResourceChangeAction OnResourceChange;

    public List<ResourceInventory> resourceList;
    public Dictionary<Resource, ResourceInventory> resources =null;

    public Timer peopleStarveTimer = new Timer();
    private float starveLength;

    public Timer resourceIntervalTimer = new Timer();
    private float resourceTick = 1f;

    public bool awakened = false;

    public Sprite AttackDamageIcon;
    public Sprite AttackRangeIcon;

    public static ResourceManager Instance()
    {
        return GameObject.Find("ResourceManager").GetComponent<ResourceManager>();
    }

    void Awake()
    {
        resources = new Dictionary<Resource, ResourceInventory>();
        foreach (ResourceInventory inventory in resourceList)
        {
            resources[inventory.resourceEnum] = inventory;
        }
    }


    public void Add(Resource resource, int addition)
    {
        resources[resource].count += addition;
        if (OnResourceChange != null)
            OnResourceChange(resources[resource]);
    }

    public void KillPeople(int peopleToKill)
    {
        Debug.Log("Killing " + peopleToKill);
        int availableWorkers = GetAvailableWorkers();
        int killedAvailableWorkers = Mathf.Min(availableWorkers, peopleToKill);
        peopleToKill -= killedAvailableWorkers;
        Add(Resource.People, -killedAvailableWorkers);
        while (peopleToKill > 0)
        {
            bool changeMade = false;
            foreach (var gameObj in Target.GetActiveBuildingObjs())
            {
                Building building = gameObj.GetComponent<Building>();
                if (building.workers > 0)
                {
                    changeMade = true;
                    building.workers -= 1;
                    peopleToKill -= 1;
                    //Add(Resource.People, -1);
                    if (peopleToKill <= 0)
                        break;
                }

            }

            //
            if (!changeMade)
            {
                // this means there are no people left (can happen if people are starving)
                return;
                //throw new KeyNotFoundException("Something went horribly wrong! No people to kill");
            }
                
        }
    }

    public void AddPeople(int peopleToAdd)
    {
        foreach (var gameObj in Target.GetActiveBuildingObjs())
        {
            Building building = gameObj.GetComponent<Building>();
            if (building.workers < building.buildingData.maxWorkerSize)
            {
                int buildingAddtion = Mathf.Min(building.buildingData.maxWorkerSize - building.workers, peopleToAdd);
                building.workers += buildingAddtion;
                peopleToAdd -= buildingAddtion;
            }
        }

        if(peopleToAdd > 0)
        {
            Add(Resource.People, peopleToAdd);

        }
    }



    public void UpdateResources()
    {
        Dictionary<Resource, int> updates = new Dictionary<Resource, int>();
        foreach (ResourceInventory inventory in resourceList)
        {
            updates[inventory.resourceEnum] = 0;
        }
        foreach (var gameObj in Target.GetActiveBuildingObjs())
        {
            Building building = gameObj.GetComponent<Building>();
            foreach (ResourceDelta delta in building.buildingData.resourceDeltas.Where(d => !d.oneTimeChange))
            {
                updates[delta.resource] += (delta.amount < 0 ? delta.amount : delta.amount * building.workers);
                
            }
        }

        foreach(var keyPair in updates)
        {
            this.Add(keyPair.Key, keyPair.Value);
        }
    }

    public void RemoveOneTimeBenifits(List<ResourceDelta> resourceDeltas)
    {
        foreach(ResourceDelta delta in resourceDeltas.Where(d=>d.oneTimeChange))
        {
            if (delta.amount > 0)
            {
                if(delta.resource == Resource.People)
                {
                    KillPeople(delta.amount);
                }
                else
                {
                    this.Add(delta.resource, -delta.amount);
                }
                
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
            if (delta.resource == Resource.People)
            {
                AddPeople(delta.amount);
            }
            else
            {
                this.Add(delta.resource, delta.amount);
            }
        }
    }

    public bool CanAffordOneTimeCost(BuildingData data)
    {
        if (data.maxWorkerSize > GetAvailableWorkers())
            return false;
        foreach(ResourceDelta delta in data.resourceDeltas.Where(d=>d.oneTimeChange))
        {
            if (resources[delta.resource].count + delta.amount < 0)
                return false;
        }

        return true;
    }

    public int GetIntervalDelta(Resource resource)
    {
        int delta = 0;
        foreach(var gameObj in Target.GetActiveBuildingObjs())
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

    public int GetTotalWorkers()
    {
        int totalWorkers = GetAvailableWorkers();
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
            totalWorkers += building.workers;

        }
        return totalWorkers;
    }

    public int GetAvailableWorkers()
    {
        return resources[Resource.People].count;
        //int availableWorkers = resources[Resource.People].count;


        //return availableWorkers;
    }

    private void Update()
    {
        if(resources[Resource.Food].count < 0 && peopleStarveTimer.Expired())
        {
            KillPeople(1);
            starveLength -= 1;
            peopleStarveTimer.Start(starveLength);
        }
        else
        {
            starveLength = 30;
        }

        if(resourceIntervalTimer.Expired())
        {
            UpdateResources();
            resourceIntervalTimer.Start(resourceTick);
        }
    }

    public static List<ResourceDelta> SortResourceList(List<ResourceDelta> resourceDeltas)
    {
        //List<ResourceDelta> sortedList;
        //foreach(Resource resource in )

        List<ResourceDelta> negatives = resourceDeltas.Where(r=>r.amount<0).OrderBy(r => (int)(r.resource)).ToList();
        List<ResourceDelta> positives = resourceDeltas.Where(r => r.amount > 0).OrderBy(r => (int)(r.resource)).ToList();

        positives.AddRange(negatives);

        return positives;

    }
}
