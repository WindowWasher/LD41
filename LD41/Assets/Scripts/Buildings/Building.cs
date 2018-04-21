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

	// Use this for initialization
	void Start () {
        health = GetComponent<Health>();
        health.OnDeathChange += Die;
        attackManager = new AttackManager(this.gameObject, attackData);
    }

    void Die()
    { 
        Debug.Log(this.name + " died!");
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update () {
        if(resourceTimer.Expired())
        {
            ResourceManager.Instance().UpdateResources(buildingData.resourceDeltas);
            resourceTimer.Start(resourceInterval);
        }
        
        if(attackManager.AttackReady())
        {
            GameObject target = Target.GetClosestTarget(this.transform.position, "Enemy");
            if(target != null)
            {
                attackManager.Attack(target);
            }
        }
	}

}
