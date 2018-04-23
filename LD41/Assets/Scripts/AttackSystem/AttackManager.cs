using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager {

    Timer attackCooldownTimer = new Timer();

    AttackData attackData;
    GameObject attackerObj;

    public AttackManager(GameObject attackerObj, AttackData attackData)
    {
        this.attackerObj = attackerObj;
        this.attackData = attackData;
    }

    public bool AttackReady()
    {
        return attackData != null && attackCooldownTimer.Expired();
    }

    public bool InRange( Vector3 targetPosition)
    {
        Vector3 offset = targetPosition  - attackerObj.transform.position;
        float sqrLen = offset.sqrMagnitude;
        return sqrLen <= attackData.attackRange * attackData.attackRange;
            // return Vector3.Distance(target.transform.position, attackerObj.transform.position) <= attackData.attackRange;
    }

    public void Attack(GameObject target)
    {
        //Debug.Log("Attacked " + attackerObj.name + "=>" + target.name);
        
        attackCooldownTimer.Start(attackData.attackCooldown);

        if(attackData.rangedAttackPrefab != null)
        {
            var rangedAttackObj = GameObject.Instantiate(attackData.rangedAttackPrefab);
            rangedAttackObj.transform.position = attackerObj.GetComponentInChildren<HealthBar>().transform.position;
            rangedAttackObj.transform.LookAt(target.transform.position);
            rangedAttackObj.AddComponent<RangedAttack>();
            rangedAttackObj.GetComponent<RangedAttack>().init(target, attackData);
        }
        else
        {
            target.GetComponent<Health>().Damage(attackData.attackDamage);
        }
        

        

        
    }

    public float GetRange()
    {
        return attackData.attackRange;
    }

}

public class RangedAttack : MonoBehaviour
{
    float speed = 40f;
    GameObject target;
    GameObject targetHealth;
    AttackData attackData;
    Timer checkTimer = new Timer();
    Vector3 lastPosition = Vector3.zero;
    public void init(GameObject target, AttackData attackData)
    {
        this.target = target;
        this.attackData = attackData;
        this.targetHealth = target.GetComponentInChildren<HealthBar>().gameObject;
    }

    public void Update()
    {
        if(Target.NullTarget(target))
        {
            Destroy(this.gameObject);
            return;
        }

        if(checkTimer.Expired())
        {
            checkTimer.Start(0.2f);
            Vector3 offset = this.transform.position - targetHealth.transform.position - new Vector3(0, 2, 0);
            float sqrLen = offset.sqrMagnitude;
            if(sqrLen < 0.5f)
            {
                Hit();
            }
            
        }

        if(this.transform.position == lastPosition)
        {
            Hit();
        }
        else
        {
            lastPosition = this.transform.position;
        }

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetHealth.transform.position - new Vector3(0, 2, 0), step);
    }

    private void Hit()
    {
        target.GetComponent<Health>().Damage(attackData.attackDamage);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target)
        {
            Hit();
        }
    }

}
