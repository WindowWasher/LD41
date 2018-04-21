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
        target.GetComponent<Health>().Damage(attackData.attackDamage);
        attackCooldownTimer.Start(attackData.attackCooldown);
    }

    public float GetRange()
    {
        return attackData.attackRange;
    }

}
