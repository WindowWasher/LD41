using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    //public float waveSpawnRadius;
    //public Vector3 center;

    public float interval;
    public float localSpawnRadius;

    GameObject[] spawnPoints;
    
    Timer intervalTimer = new Timer();

    public List<GameObject> enemyPrefabs;

	// Use this for initialization
	void Start () {
        //enemyPrefabs = new List<GameObject>();

        //enemyPrefabs.Add((GameObject)Resources.Load("Enemies/Barbarian"));

        spawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");

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
        List<GameObject> currentSpawnPoints = new List<GameObject>();
        foreach(var spawnP in spawnPoints)
        {
            currentSpawnPoints.Add(spawnP);
        }

        int numberOfEnemiesToSpawn = 20;
        int enemiesPerWave = numberOfEnemiesToSpawn / spawnPoints.Length;
        foreach(var spawnP in currentSpawnPoints)
        {
            SpawnEnemiesAtPosition(spawnP.transform.position, enemiesPerWave);
        }
    }

    void SpawnEnemiesAtPosition(Vector3 spawnPoint, int numberOfEnemiesToSpawn)
    {
        //Vector2 localSpawnPoint = Random.insideUnitCircle.normalized * waveSpawnRadius;
        Vector2 localSpawnPoint = new Vector2(spawnPoint.x, spawnPoint.z);
        
        for(int i = 0; i < numberOfEnemiesToSpawn; ++i)
        {
            GameObject newEnemyPrefab;
            if (Random.Range(0, 2) == 1)
            {
                newEnemyPrefab = enemyPrefabs[1];
            }
            else
            {
                newEnemyPrefab = enemyPrefabs[0];
            }
            
            Vector2 enemySpawnPointV2 = (Random.insideUnitCircle.normalized * localSpawnRadius) + localSpawnPoint;
            Vector3 enemySpawnPoint = new Vector3(enemySpawnPointV2.x, spawnPoint.y, enemySpawnPointV2.y);

            GameObject.Instantiate(newEnemyPrefab, enemySpawnPoint, Quaternion.identity);

        }
        intervalTimer.Start(interval);
    }

    //private void OnDrawGizmos()
    //{
    //    //UnityEditor.Handles.color = Color.green;
    //    //UnityEditor.Handles.DrawWireDisc(center, Vector3.up, waveSpawnRadius);
    //    //UnityEditor.Handles.DrawWireDisc(collider.transform.position, Vector3.back, collider.radius);
    //}

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
