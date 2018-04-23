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

    public bool InRange( GameObject target)
    {
        return Vector3.Distance(target.transform.position, attackerObj.transform.position) <= attackData.attackRange;
    }

    public void Attack(GameObject target)
    {
        Debug.Log("Attacked " + attackerObj.name + "=>" + target.name);
        
        attackCooldownTimer.Start(attackData.attackCooldown);

        if(attackData.rangedAttackPrefab != null)
        {
            var rangedAttackObj = GameObject.Instantiate(attackData.rangedAttackPrefab);
            rangedAttackObj.transform.position = attackerObj.transform.position;
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
    AttackData attackData;
    public void init(GameObject target, AttackData attackData)
    {
        this.target = target;
        this.attackData = attackData;
    }

    public void Update()
    {
        if(target == null)
        {
            Destroy(this.gameObject);
            return;
        }

        if(Vector3.Distance(this.transform.position, target.transform.position) < 0.1)
        {
            Hit();
        }

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
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
