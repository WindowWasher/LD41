﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObject/EnemyData")]
public class EnemyData : ScriptableObject {

    public float speed;
    public float attackRange;
    public int attackDamage;
    public float attackCooldown;
}
