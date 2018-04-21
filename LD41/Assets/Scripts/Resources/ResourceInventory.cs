using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Resource", menuName = "New Resource")]
public class ResourceInventory: ScriptableObject
{
    public string resourceName;
    public Resource resourceEnum;
    public int count;
    public Sprite hudImage;

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