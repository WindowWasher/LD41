using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    NavMeshAgent agent;
    public MovementData movementData;
    public AttackData attackData;
    public GameObject target = null;

    Health health;
    AttackManager attackManager;

	// Use this for initialization
	void Start () {

        agent = GetComponent<NavMeshAgent>();

        health = GetComponent<Health>();
        health.OnDeathChange += Die;

        agent.speed = movementData.speed;

        attackManager = new AttackManager(this.gameObject, attackData);
    }

    void Die()
    {
        Debug.Log(this.name + " died!");
        Destroy(this.gameObject);
    }

    void setTarget()
    {
        target = Target.GetClosestTarget(this.transform.position, "Building");
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
