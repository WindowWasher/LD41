using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Building : MonoBehaviour {

    
    //Health health;

	// Use this for initialization
	void Start () {
        //health = GetComponent<Health>();
        //health.SetInitialHealth(50);
        //health.OnDeathChange += Die;
	}

    void Die()
    { 
        Debug.Log(this.name + " died!");
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
