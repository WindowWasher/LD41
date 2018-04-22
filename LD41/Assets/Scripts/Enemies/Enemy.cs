using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Enemy : MonoBehaviour {

    NavMeshAgent agent;
    Timer NavMeshTimer = new Timer();

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
        if (!agent.isOnNavMesh)
        {
            NavMeshHit hit = new NavMeshHit();
            if (NavMesh.SamplePosition(this.transform.position, out hit, 10f, NavMesh.AllAreas))
            {
                this.transform.position = hit.position;
            }
            else
            {
                Die();
                return;
            }
        }

        //if (agent.enabled == false)
        //    return;
        //target = Target.GetClosestTarget(this.transform.position, "Building");
        this.target = target;
        if (target != null)
        {
            //agent.SetDestination(target.transform.position);
            agent.SetDestination(GetRandomTargetPosition(target));
            if (!attackManager.InRange(target))
            {
                agent.isStopped = false;
            }
            else
            {
                agent.isStopped = true;
            }
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
        if (!agent.isOnNavMesh)
        {
            NavMeshHit hit = new NavMeshHit();
            if(NavMesh.SamplePosition(this.transform.position, out hit, 10f, NavMesh.AllAreas))
            {
                this.transform.position = hit.position;
            }
            else
            {
                Die();
                return;
            }
        }

		if(target == null)
        {
            resetTarget();
        }
        if(target!= null)
        {
            if(attackManager.InRange(target))
            {
                if (!agent.isStopped)
                {
                    agent.isStopped = true;
                }
                if (attackManager.AttackReady())
                {
                    attackManager.Attack(target);
                }
            }
            else
            {
                if (agent.isStopped)
                {
                    agent.isStopped = false;
                }
            }
            
            
        }
        

        
	}

    private void OnTriggerEnter(Collider other)
    {
        Building building = other.gameObject.GetComponent<Building>();
        if (building != null)
        {
            setTarget(other.gameObject);
        }
    }

    GameObject GetTarget()
    {
        //int sightRange = 50;
        
        //List<GameObject> gameObjects = Target.GetBuildingsInRange(this.transform.position, sightRange);
        List<GameObject> buildingObjects = Target.GetActiveBuildingObjs();
        List<Enemy> allEnemies = EnemyManager.Instance().GetAllEnemies();

        //List<GameObject> buildObjsNotFullyTargted = new List<GameObject>();

        float? priorityDistance = null;
        GameObject priorityObj = null;

        float? closestTargedDistance = null;
        GameObject closestTargetdBulidngObj = null;

        foreach(var bObj in buildingObjects)
        {
            int enemyTargetCount = allEnemies.Where(e => e.target == bObj).Count();
            float distance = Vector3.Distance(this.transform.position, bObj.transform.position);
            if (enemyTargetCount < numTargetsPerBuilding && !bObj.GetComponent<Building>().IsWall())
            {
                if(priorityDistance == null || distance < priorityDistance)
                {
                    priorityDistance = distance;
                    priorityObj = bObj;
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

        if(priorityObj != null)
        {
            //Debug.Log("Found untraged building " + closestUnTargtedBuildingObj.name);
            return priorityObj;
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
