using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target {

    public static GameObject GetClosestTarget(Vector3 position, string tag, float? range=null)
    {
        float? minDistance = null;
        GameObject target = null;
        foreach (GameObject buildingObj in GameObject.FindGameObjectsWithTag(tag))
        {
            Vector3 offset = position - buildingObj.transform.position;
            float currentDistanceSq = offset.sqrMagnitude;
            //float currentDistance = Vector3.Distance(position, buildingObj.transform.position);
            if (range != null && currentDistanceSq > range * range)
                continue;

            if (minDistance == null || currentDistanceSq < minDistance * minDistance)
            {
                minDistance = currentDistanceSq;
                target = buildingObj;
            }
        }

        return target;
    }

    public static List<GameObject> GetBuildingsInRange(Vector3 position, float range)
    {
        List<GameObject> buildingObjs = new List<GameObject>();
        foreach (GameObject buildingObj in GetActiveBuildingObjs())
        {
            //float currentDistance = Vector3.Distance(position, buildingObj.transform.position);
            Vector3 offset = position - buildingObj.transform.position;
            float currentDistanceSq = offset.sqrMagnitude;
            if (currentDistanceSq <= range * range)
            {
                buildingObjs.Add(buildingObj);
            }
        }

        return buildingObjs;
    }

    public static List<GameObject> GetActiveBuildingObjs()
    {
        List < GameObject > buildingObjs = new List<GameObject>();
        foreach(var bObj in GameObject.FindGameObjectsWithTag("Building"))
        {
            if(bObj.GetComponent<Building>().buildingActive)
            {
                buildingObjs.Add(bObj);
            }
        }

        return buildingObjs;
        
    }
    //GameObject[] GetAllBuildingGameObjs()
    //{
    //    return GameObject.FindGameObjectsWithTag("Building");
    //}

    //GameObject[] GetAllEnemyGameObjs()
    //{
    //    return GameObject.FindGameObjectsWithTag("Enemy");
    //}
}
