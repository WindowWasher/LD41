using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public float interval;
    public float waveSpawnRadius;
    public float localSpawnRadius;

    public Vector3 center;
    Timer intervalTimer = new Timer();

    public List<GameObject> enemyPrefabs;

	// Use this for initialization
	void Start () {
        enemyPrefabs = new List<GameObject>();

        enemyPrefabs.Add((GameObject)Resources.Load("Enemies/Barbarian"));

        SpawnWave();
	}
	
	// Update is called once per frame
	void Update () {
		if(intervalTimer.Expired())
        {
            SpawnWave();
        }
	}

    void SpawnWave()
    {
        Vector2 localSpawnPoint = Random.insideUnitCircle.normalized * waveSpawnRadius;
        int numberOfEnemiesToSpawn = 10;
        for(int i = 0; i < numberOfEnemiesToSpawn; ++i)
        {
            Vector2 enemySpawnPointV2 = (Random.insideUnitCircle.normalized * localSpawnRadius) + localSpawnPoint;
            Vector3 enemySpawnPoint = new Vector3(enemySpawnPointV2.x, 10, enemySpawnPointV2.y) + center;

            GameObject.Instantiate(enemyPrefabs[0], enemySpawnPoint, Quaternion.identity);

        }
        intervalTimer.Start(interval);
    }

    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(center, Vector3.up, waveSpawnRadius);
        //UnityEditor.Handles.DrawWireDisc(collider.transform.position, Vector3.back, collider.radius);
    }

    public static EnemyManager Instance()
    {
        return GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
    }

    public List<Enemy> GetAllEnemies()
    {
        List<Enemy> enemies = new List<Enemy>();
        foreach(var enemyObj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(enemyObj.GetComponent<Enemy>());
        }

        return enemies;
    }
}
