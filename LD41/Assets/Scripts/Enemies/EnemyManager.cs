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

    int waveNumber = 1;

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
        //List<GameObject> currentSpawnPoints = new List<GameObject>();
        //foreach(var spawnP in spawnPoints)
        //{
        //    currentSpawnPoints.Add(spawnP);
        //}

        List<int> rangeValues = new List<int>();
        for(int i = 0; i < spawnPoints.Length; ++i)
        {
            rangeValues.Add(i);
        }

        int numberOfSpawnLocations = 3;
        List<int> randomIndexes = new List<int>();
        for(int i = 0; i < numberOfSpawnLocations; i++)
        {
            int index = Random.Range(0, rangeValues.Count);
            randomIndexes.Add(rangeValues[index]);
            rangeValues.RemoveAt(index);
        }



        int numberOfEnemiesToSpawn = 15 * waveNumber;
        int enemiesPerWave = numberOfEnemiesToSpawn / numberOfSpawnLocations;
        //foreach(var spawnP in currentSpawnPoints)
        for(int i = 0; i < randomIndexes.Count; ++i)
        {
            var spawnP = spawnPoints[randomIndexes[i]];
            SpawnEnemiesAtPosition(spawnP.transform.position, enemiesPerWave);
        }

        waveNumber += 1;
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
                newEnemyPrefab = enemyPrefabs[0];
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
            if (enemyObj == null || enemyObj.GetComponent<Enemy>().alive == false)
                continue;
            enemies.Add(enemyObj.GetComponent<Enemy>());
        }

        return enemies;
    }
}
