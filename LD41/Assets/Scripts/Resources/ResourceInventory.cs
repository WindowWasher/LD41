using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Resource", menuName = "ScriptableObject/Resource")]
public class ResourceInventory: ScriptableObject
{
    public string resourceName;
    public Resource resourceEnum;
    public int initialCount;
    [HideInInspector]
    public int count;
    public Sprite hudImage;

    /// <summary>
    ///TODO: remove this 
    /// </summary>
    public void OnEnable()
    {
        count = initialCount;
    }
    // Color?

    //public void Add(int addition)
    //{
    //    count += addition;
    //}

    //public void Remove(int subtraction)
    //{
    //    count -= subtraction;
    //    // TODO what happens if count is less than 0?
    //}

}
