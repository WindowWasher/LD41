using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager {

    Timer attackCooldownTimer = new Timer();

    float attackRange;
    int attackDamage;
    float attackCooldown;

    GameObject attackerObj;

    public AttackManager(GameObject attackerObj, int attackDamage, float range, float attackCooldown)
    {
        this.attackDamage = attackDamage;
        this.attackRange = range;
        this.attackCooldown = attackCooldown;
        this.attackerObj = attackerObj;
    }

    public bool AttackReady()
    {
        return attackCooldownTimer.Expired();
    }

    public bool InRange( GameObject target)
    {
        return Vector3.Distance(target.transform.position, attackerObj.transform.position) <= attackRange;
    }

    public void Attack(GameObject target)
    {
        Debug.Log("Attacked " + attackerObj.name + "=>" + target.name);
        target.GetComponent<Health>().Damage(attackDamage);
        attackCooldownTimer.Start(attackCooldown);
    }

}
