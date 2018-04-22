using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Enemy : MonoBehaviour {

    NavMeshAgent agent;
    public MovementData movementData;
    public AttackData attackData;
    public GameObject target = null;

    Health health;
    AttackManager attackManager;

    int numTargetsPerBuilding = 4;

    // Use this for initialization
    void Start () {

        agent = GetComponent<NavMeshAgent>();

        health = GetComponent<Health>();
        health.OnDeathChange += Die;

        agent.speed = movementData.speed;

        attackManager = new AttackManager(this.gameObject, attackData);
        BuildingPlacement buildingPlacement = GameObject.FindObjectOfType<BuildingPlacement>();
        buildingPlacement.OnBuildingCreationAction += newBuilding;

        foreach(var buildingObj in Target.GetActiveBuildingObjs())
        {
            buildingObj.GetComponent<Building>().OnBuildingDeath += buildingDestroyed;
        }
    }

    void Die()
    {
        Debug.Log(this.name + " died!");
        Destroy(this.gameObject);
    }

    void newBuilding(Building newBuilding)
    {
        newBuilding.OnBuildingDeath += buildingDestroyed;
        if(target != null)
        {
            Building currentBuilding = target.GetComponent<Building>();
            List<Enemy> allEnemies = EnemyManager.Instance().GetAllEnemies();
            int enemyTargetCount = allEnemies.Where(e => e.target == target).Count();
            //Debug.Log("NewBuilding, but currentBuildingTargetCount " + enemyTargetCount.ToString());
            if (newBuilding.IsWall() || (!currentBuilding.IsWall() && enemyTargetCount <= numTargetsPerBuilding))
                return;
        }

        resetTarget();
    }

    void buildingDestroyed(Building destroyedBuilding)
    {
        if (target != null)
        {
            Building currentBuilding = target.GetComponent<Building>();
            List<Enemy> allEnemies = EnemyManager.Instance().GetAllEnemies();
            int enemyTargetCount = allEnemies.Where(e => e.target == target).Count();

            if (enemyTargetCount <= numTargetsPerBuilding)
            {
                if (destroyedBuilding.IsWall() || !currentBuilding.IsWall())
                    return;
            }


        }

        resetTarget();
    }

    void resetTarget()
    {
        setTarget(GetTarget());
    }

    void setTarget(GameObject target)
    {
        //target = Target.GetClosestTarget(this.transform.position, "Building");
        this.target = target;
        if (target != null)
        {
            //agent.SetDestination(target.transform.position);
            agent.SetDestination(GetRandomTargetPosition(target));
            target.GetComponent<Health>().OnDeathChange += targetDestroyed;
        }
        else
        {
            // Player loses
        }
        
    }

    public Vector3 GetRandomTargetPosition(GameObject buildObj)
    {
        float xVal = buildObj.GetComponent<Renderer>().bounds.size.x / 2;
        float zVal = buildObj.GetComponent<Renderer>().bounds.size.z / 2;
        //Debug.Log("X: " + xVal);
        //Debug.Log("Z: " + zVal);
        Vector3 randomPoint =  new Vector3(
            Random.Range(-xVal, xVal),
            buildObj.transform.position.y,
            Random.Range(-zVal, zVal)
            ) + buildObj.transform.position;

        //Debug.Log("Random " + randomPoint.ToString());
        return randomPoint;
        //Vector2 randomPoint = Random.insideUnitCircle.normalized * Mathf.Min(building.buildingData.gridSize.x, building.buildingData.gridSize.y);
        //return new Vector3(randomPoint.x, building.transform.position.y, randomPoint.y) + building.transform.position;
        //return newVec;
    }

    void targetDestroyed()
    {
        target = null;
    }


    // Update is called once per frame
    void Update () {
		if(target == null)
        {
            resetTarget();
        }
        if (target != null && attackManager.AttackReady() && attackManager.InRange(target))
        {
            attackManager.Attack(target);
        }

        
	}

    private void OnCollisionEnter(Collision collision)
    {
        Building building = collision.gameObject.GetComponent<Building>();
        if(building != null)
        {
            setTarget(collision.gameObject);
        }
    }

    GameObject GetTarget()
    {
        //int sightRange = 50;
        
        //List<GameObject> gameObjects = Target.GetBuildingsInRange(this.transform.position, sightRange);
        List<GameObject> buildingObjects = Target.GetActiveBuildingObjs();
        List<Enemy> allEnemies = EnemyManager.Instance().GetAllEnemies();

        //List<GameObject> buildObjsNotFullyTargted = new List<GameObject>();

        float? closestUnTargetedDistance = null;
        GameObject closestUnTargtedBuildingObj = null;

        float? closestTargedDistance = null;
        GameObject closestTargetdBulidngObj = null;

        foreach(var bObj in buildingObjects)
        {
            int enemyTargetCount = allEnemies.Where(e => e.target == bObj).Count();
            float distance = Vector3.Distance(this.transform.position, bObj.transform.position);
            if (enemyTargetCount < numTargetsPerBuilding)
            {
                if(closestUnTargetedDistance == null || distance < closestUnTargetedDistance)
                {
                    closestUnTargetedDistance = distance;
                    closestUnTargtedBuildingObj = bObj;
                }
            }
            else
            {
                if (closestTargedDistance == null || distance < closestTargedDistance)
                {
                    closestTargedDistance = distance;
                    closestTargetdBulidngObj = bObj;
                }
            }
        }

        if(closestUnTargtedBuildingObj != null)
        {
            //Debug.Log("Found untraged building " + closestUnTargtedBuildingObj.name);
            return closestUnTargtedBuildingObj;
        }
        else
        {
            //Debug.Log("Did not find untraged building " + closestTargetdBulidngObj.name);
            return closestTargetdBulidngObj;
        }

        //Dictionary<GameObject, float> pathAbleBuildingObjsDistance = new Dictionary<GameObject, float>();
        //NavMeshPath path = new NavMeshPath();
        //foreach(var bObj in buildObjsNotFullyTargted)
        //{
        //    // TODO optimize by breaking early?
        //    if(agent.CalculatePath(bObj.transform.position, path))
        //    {
        //        agent.SetPath(path);
        //        pathAbleBuildingObjsDistance[bObj] = agent.remainingDistance;
        //    }
        //}

        //float? closestPriorityDistance = null;
        //GameObject closestPriority = null;
        //float? closestWallDistance = null;
        //GameObject closestWall = null;
        //foreach(var keyVal in pathAbleBuildingObjsDistance)
        //{
        //    float distance = keyVal.Value;
        //    if(keyVal.Key.GetComponent<Building>().IsWall())
        //    {
        //        if(closestWallDistance == null || distance < closestWallDistance)
        //        {
        //            closestWall = keyVal.Key;
        //            closestWallDistance = distance;
        //        }
        //    }
        //    else
        //    {
        //        if (closestPriorityDistance == null || distance < closestPriorityDistance)
        //        {
        //            closestPriority = keyVal.Key;
        //            closestPriorityDistance = distance;
        //        }
        //    }
        //}

        //GameObject target;

        //if (closestPriority != null)
        //    target = closestPriority;
        //else if (closestWall != null)
        //    target = closestWall;
        //else
        //{
        //    Debug.Log("Could not find a priority");
        //    target = Target.GetClosestTarget(this.transform.position, "Building");
        //}

        //Debug.Log("Enemy " + this.name + " choose " + target.name);


        //return target;


    }


}
