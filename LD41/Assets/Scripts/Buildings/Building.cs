using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Building", menuName = "Building")]
public class Building : MonoBehaviour {

    public GameObject building;
    public Vector2 gridSize;
    Health health;

	// Use this for initialization
	void Start () {
        health = GetComponent<Health>();
        health.SetInitialHealth(50);
        health.OnDeathChange += Die;
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
