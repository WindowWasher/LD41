using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    private Health health;
    public Image healthFg;

	// Use this for initialization
	void Start () {

        health = gameObject.GetComponentInParent<Health>();
        health.OnHealthChange += UpdateHealth;
	}

    private void OnDisable()
    {
        health.OnHealthChange -= UpdateHealth;
    }

    void UpdateHealth(int currentHealth, int damageTaken)
    {
        Debug.Log("Update health");
        healthFg.fillAmount = (float)currentHealth / health.maxHealth;
    }

    private void Update()
    {
        if(this.gameObject.transform.parent.GetComponent<Building>() == null)
        {
            this.transform.LookAt(Camera.main.transform);
        }
        
    }
}
