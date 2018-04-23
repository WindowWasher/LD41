using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Building : MonoBehaviour {

    public BuildingData buildingData;
    Health health;
    //Timer resourceTimer = new Timer();
    //int resourceInterval = 1;

    public AttackData attackData;
    AttackManager attackManager = null;

    public bool buildingActive = false;
    private Node buildingStartNode;

    public delegate void OnBuildingChange(Building building);
    public event OnBuildingChange OnBuildingDeath;

    public int workers;

	// Use this for initialization
	void Start () {
        health = GetComponent<Health>();
        health.OnDeathChange += Die;
        attackManager = new AttackManager(this.gameObject, attackData);
        workers = 0;
    }

    private void OnDisable()
    {
        health.OnDeathChange -= Die;
    }

    void Die()
    { 
        Debug.Log(this.name + " died!");
        ResourceManager.Instance().RemoveOneTimeBenifits(buildingData.resourceDeltas);
        GridManager.instance.SetOccupiedToValue(buildingStartNode, buildingData.gridSize, false);
        ResourceManager.Instance().Add(Resource.People, workers);
        workers = 0;
        Destroy(this.gameObject);
    }

    public void DestroyBuilding()
    {
        Die();
    }

    public void BuildingPlaced(Node startNode)
    {
        buildingStartNode = startNode;
        if (buildingActive)
            return;

        ResourceManager.Instance().UpdateOneTimeCostResources(buildingData.resourceDeltas);
        buildingActive = true;
        this.gameObject.tag = "Building";

        int peopleAvailable = ResourceManager.Instance().GetAvailableWorkers();
        // Auto Add workers if available
        workers = Mathf.Min(peopleAvailable, buildingData.maxWorkerSize);
        ResourceManager.Instance().Add(Resource.People, -workers);
        
        BuildingInfoManager.Instance().ShowBuildingInfo(this);
    }

    // Update is called once per frame
    void Update () {

        if (!buildingActive)
            return;

        //if(resourceTimer.Expired())
        //{
        //    ResourceManager.Instance().UpdateResources(buildingData.resourceDeltas, workers);
        //    resourceTimer.Start(resourceInterval);
        //}
        
        if(attackManager != null && attackManager.AttackReady() && workers > 0)
        {
            GameObject target = Target.GetClosestTarget(this.transform.position, "Enemy");
            if(target != null && attackManager.InRange(target))
            {
                attackManager.Attack(target);
            }
        }
	}

    public bool IsWall()
    {
        // TODO
        return this.buildingData.name.ToLower().Contains("wall");
    }

}

