using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    NavMeshAgent agent;
    public EnemyData enemyData;

    public float health;

    public GameObject target = null;

    Timer attackCooldownTimer = new Timer();

	// Use this for initialization
	void Start () {

        agent = GetComponent<NavMeshAgent>();

        

        health = enemyData.health;
        agent.speed = enemyData.speed;
    }

    GameObject GetTarget()
    {
        float? minDistance = null;
        GameObject target = null;
        foreach (GameObject buildingObj in GetAllBuildingGameObjs())
        {
            float currentDistance = Vector3.Distance(this.transform.position, buildingObj.transform.position);
            if(minDistance == null || currentDistance < minDistance)
            {
                minDistance = currentDistance;
                target = buildingObj;
            }
        }

        return target;
    }

    GameObject[] GetAllBuildingGameObjs()
    {
        return GameObject.FindGameObjectsWithTag("Building");
    }

    void setTarget()
    {
        target = GetTarget();
        if(target != null)
        {
            agent.SetDestination(target.transform.position);
        }
        else
        {
            // Player loses
        }
        
    }

    void Attack()
    {
        Destroy(target);
    }


    // Update is called once per frame
    void Update () {
		if(target == null)
        {
            setTarget();
        }
        if (target != null && attackCooldownTimer.Expired())
        {
            float targetDistance = Vector3.Distance(target.transform.position, this.transform.position);
            if (targetDistance <= enemyData.attackRange)
            {
                Attack();
            }
        }

        
	}
}
