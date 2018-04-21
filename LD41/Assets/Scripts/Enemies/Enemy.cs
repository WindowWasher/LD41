using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    NavMeshAgent agent;
    public EnemyData enemyData;
    public GameObject target = null;

    Health health;
    AttackManager attackManager;

	// Use this for initialization
	void Start () {

        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        

        health.SetInitialHealth(enemyData.health);
        health.OnDeathChange += Die;
        agent.speed = enemyData.speed;

        attackManager = new AttackManager(this.gameObject, enemyData.attackDamage, enemyData.attackRange, enemyData.attackCooldown);
    }

    void Die()
    {
        Debug.Log(this.name + " died!");
        Destroy(this.gameObject);
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
            target.GetComponent<Health>().OnDeathChange += targetDestroyed;
        }
        else
        {
            // Player loses
        }
        
    }

    void targetDestroyed()
    {
        target = null;
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
        if (target != null && attackManager.AttackReady() && attackManager.InRange(target))
        {
            attackManager.Attack(target);
        }

        
	}
}
