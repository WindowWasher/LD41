using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "ScriptableObject/AttackData")]
public class AttackData : ScriptableObject
{
    public float attackRange;
    public int attackDamage;
    public float attackCooldown;
}
