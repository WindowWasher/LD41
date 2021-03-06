﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health: MonoBehaviour {

    /// <summary>
    /// Event that can be registered against and is called anytime health is changed
    /// </summary>
    public delegate void OnHealthChangeAction(int currentHealth, int damageTaken);
    public event OnHealthChangeAction OnHealthChange;

    public delegate void OnDeathChangeAction();
    public event OnDeathChangeAction OnDeathChange;

    public int maxHealth;

    public void Awake()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// The current health of the entity
    /// </summary>
    public int currentHealth { get; set; }

 //   // Use this for initialization
 //   public void InitHealth (int health, OnDeathChangeAction deathAction) {
 //       currentHealth = health;
 //       OnDeathChange += deathAction;
	//}

    public void Damage(int damage)
    {
        currentHealth -= damage;
        if(OnHealthChange != null)
            OnHealthChange(currentHealth, damage);
        if (currentHealth <= 0)
        {
            OnDeathChange();
        }
    }
}
