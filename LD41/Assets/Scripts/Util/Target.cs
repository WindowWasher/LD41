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
            float currentDistance = Vector3.Distance(position, buildingObj.transform.position);
            if (range != null && currentDistance > range)
                continue;

            if (minDistance == null || currentDistance < minDistance)
            {
                minDistance = currentDistance;
                target = buildingObj;
            }
        }

        return target;
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
