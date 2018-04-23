using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour {

    //public float waveSpawnRadius;
    //public Vector3 center;

    public float interval;
    public float localSpawnRadius;

    GameObject[] spawnPoints;
    
    Timer intervalTimer = new Timer();

    public List<GameObject> enemyPrefabs;

    GameObject waveCountPanel;

    int waveNumber = 0;
    int numPerWave = 5;
    bool waveRunning = false;

	// Use this for initialization
	void Start () {
        //enemyPrefabs = new List<GameObject>();

        //enemyPrefabs.Add((GameObject)Resources.Load("Enemies/Barbarian"));

        //intervalTimer.Start(10f);
        waveCountPanel = GameObject.Find("WaveCountPanel");

        spawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");

        SpawnWave();
	}
	
	// Update is called once per frame
	void Update () {
		if(intervalTimer.Expired() && !waveRunning)
        {
            SpawnWave();
        }

        if(GameObject.FindGameObjectsWithTag("Building").Where(b=>!b.GetComponent<Building>().IsWall()).Count() <= 0)
        {
            PlayerLose();
        }
	}

    void PlayerLose()
    {
        //throw new KeyNotFoundException("You lost!");
    }

    void SpawnWave()
    {
        //List<GameObject> currentSpawnPoints = new List<GameObject>();
        //foreach(var spawnP in spawnPoints)
        //{
        //    currentSpawnPoints.Add(spawnP);
        //}
        waveRunning = true;

        List<int> rangeValues = new List<int>();
        for(int i = 0; i < spawnPoints.Length; ++i)
        {
            rangeValues.Add(i);
        }

        int numberOfSpawnLocations = 3;
        //int numberOfSpawnLocations = spawnPoints.Length;
        List<int> randomIndexes = new List<int>();
        for(int i = 0; i < numberOfSpawnLocations; i++)
        {
            int index = Random.Range(0, rangeValues.Count);
            randomIndexes.Add(rangeValues[index]);
            rangeValues.RemoveAt(index);
        }



        int numberOfEnemiesToSpawn = 200* 5 + numPerWave * waveNumber;
        if(waveNumber == 0)
        {
            numberOfSpawnLocations = 2;
            numberOfEnemiesToSpawn = 2;
        }
        //int numberOfEnemiesToSpawn = 20000 + 5 * waveNumber;
        numPerWave += waveNumber; //Add some addtiona scaling
        numPerWave += waveNumber / 10 * 10;
        int enemiesPerWave = numberOfEnemiesToSpawn / numberOfSpawnLocations;
        //foreach(var spawnP in currentSpawnPoints)
        List<GameObject> selectedSpawnPoints = new List<GameObject>();
        for(int i = 0; i < randomIndexes.Count; ++i)
        {
            var spawnP = spawnPoints[randomIndexes[i]];
            selectedSpawnPoints.Add(spawnP);
            StartCoroutine(SpawnEnemiesAtPosition(spawnP.transform.position, enemiesPerWave));
            
        }
        //StartCoroutine(DoSpawning(selectedSpawnPoints, enemiesPerWave));

        intervalTimer.Start(interval);
        waveCountPanel.GetComponentInChildren<Text>().text = "Wave: " + waveNumber.ToString();
        waveNumber += 1;
    }

    //IEnumerator DoSpawning(List<GameObject> spawnPoints, int enemiesPerWave)
    //{
    //    Debug.Log("Enemies per wave" + enemiesPerWave.ToString());
    //    foreach (GameObject spawnPoint in spawnPoints)
    //    {
    //        Vector2 localSpawnPoint = new Vector2(spawnPoint.transform.position.x, spawnPoint.transform.position.z);

    //        for (int i = 0; i < enemiesPerWave; ++i)
    //        {
    //            //if (i % 2 == 0)
    //            {
    //                yield return new WaitForSeconds(5f);
    //            }

    //            GameObject newEnemyPrefab;
    //            if (Random.Range(0, 2) == 1)
    //            {
    //                newEnemyPrefab = enemyPrefabs[1];
    //            }
    //            else
    //            {
    //                newEnemyPrefab = enemyPrefabs[0];
    //            }

    //            Vector2 enemySpawnPointV2 = (Random.insideUnitCircle.normalized * localSpawnRadius) + localSpawnPoint;
    //            Vector3 enemySpawnPoint = new Vector3(enemySpawnPointV2.x, spawnPoint.transform.position.y, enemySpawnPointV2.y);

    //            GameObject.Instantiate(newEnemyPrefab, enemySpawnPoint, Quaternion.identity);

    //        }
    //    }

    //}



    IEnumerator SpawnEnemiesAtPosition(Vector3 spawnPoint, int numberOfEnemiesToSpawn)
    {
        //Vector2 localSpawnPoint = Random.insideUnitCircle.normalized * waveSpawnRadius;
        Vector2 localSpawnPoint = new Vector2(spawnPoint.x, spawnPoint.z);

        for (int i = 0; i < numberOfEnemiesToSpawn; ++i)
        {
            if (i % 2 == 0)
            {
                yield return new WaitForSeconds(1f);
            }

            GameObject newEnemyPrefab;
            if (Random.Range(0, 2) == 1 && waveNumber > 1)
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

        waveRunning = false;

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
