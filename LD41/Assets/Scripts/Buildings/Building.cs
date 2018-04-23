using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Building : MonoBehaviour {

    public BuildingData buildingData;
    Health health;
    //Timer resourceTimer = new Timer();
    //int resourceInterval = 1;

    AttackManager attackManager = null;

    public AudioClip audioClip;

    public bool buildingActive = false;
    private Node buildingStartNode;

    public delegate void OnBuildingChange(Building building);
    public event OnBuildingChange OnBuildingDeath;

    public Canvas placementCanvas;
    public Image placementImage;

    public int workers;

    public GameObject target = null;
    //bool lookForNewTarget = false;

    public Timer checkForTargets = new Timer();

    public int enemyAttackerCount = 0;
	// Use this for initialization
	void Start () {
        health = GetComponent<Health>();
        health.OnDeathChange += Die;
        attackManager = new AttackManager(this.gameObject, buildingData.attackData);
        workers = 0;
    }

    private void OnDisable()
    {
        health.OnDeathChange -= Die;
    }

    public void Die()
    { 
        //Debug.Log(this.name + " died!");
        ResourceManager.Instance().RemoveOneTimeBenifits(buildingData.resourceDeltas);
        GridManager.instance.SetOccupiedToValue(buildingStartNode, buildingData.gridSize, false);
        ResourceManager.Instance().AddPeople(workers);
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
        
        //BuildingInfoManager.Instance().ShowBuildingInfo(this);

        //if(buildingData.attackData != null)
        //{
        //    this.gameObject.GetComponent<SphereCollider>().radius = buildingData.attackData.attackRange;
        //}
    }

    // Update is called once per frame
    void Update () {

        if (!buildingActive)
            return;

        if(buildingData.attackData != null && Target.NullTarget(target) && checkForTargets.Expired())
        {
            checkForTargets.Start(1f);
            target = Target.GetClosestTarget(this.transform.position, "Enemy", buildingData.attackData.attackRange);
            //target = Target.GetClosestTarget(this.transform.position, "Enemy");
        }

        if (buildingData.attackData != null && !Target.NullTarget(target) && attackManager.AttackReady() && workers > 0)
        {
            //Debug.Log("Shooting " + target.name);
            attackManager.Attack(target);
            AudioSource.PlayClipAtPoint(audioClip, transform.position + new Vector3(0, 35, 0), 1f);
        }
	}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!buildingActive || target != null || lookForNewTarget)
    //        return;

    //    if(other.gameObject.GetComponent<Enemy>() != null)
    //    {
    //        target = other.gameObject;
    //        target.GetComponent<Health>().OnDeathChange += targetDied;
    //    }
    //}

    //void targetDied()
    //{
    //    target.GetComponent<Health>().OnDeathChange -= targetDied;
    //    lookForNewTarget = true;
    //}

    public bool IsWall()
    {
        // TODO
        return this.buildingData.name == "Wall";
    }

}

