using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Building : MonoBehaviour {

    public BuildingData buildingData;
    Health health;
    Timer resourceTimer = new Timer();
    int resourceInterval = 1;

    public AttackData attackData;
    AttackManager attackManager = null;

    private bool buildingActive = false;
    private Node buildingStartNode;

	// Use this for initialization
	void Start () {
        health = GetComponent<Health>();
        health.OnDeathChange += Die;
        attackManager = new AttackManager(this.gameObject, attackData);
    }

    private void OnDisable()
    {
        health.OnDeathChange -= Die;
    }

    void Die()
    { 
        Debug.Log(this.name + " died!");
        ResourceManager.Instance().RemoveOneTimeBenifits(buildingData.resourceDeltas);
        GridManager.instance.SetOccupiedToFalse(buildingStartNode, buildingData.gridSize);
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
    }

    // Update is called once per frame
    void Update () {

        if (!buildingActive)
            return;

        if(resourceTimer.Expired())
        {
            ResourceManager.Instance().UpdateResources(buildingData.resourceDeltas);
            resourceTimer.Start(resourceInterval);
        }
        
        if(attackManager != null && attackManager.AttackReady())
        {
            GameObject target = Target.GetClosestTarget(this.transform.position, "Enemy");
            if(target != null)
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
