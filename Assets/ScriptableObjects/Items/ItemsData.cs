using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsData : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public int regenPts;
    public int shieldPts;
    public GameObject itemPrefab;

    public ItemsData Clone()
    {
        var clone = ScriptableObject.CreateInstance<ItemsData>();
        clone.itemName = this.itemName;
        clone.itemDescription = this.itemDescription;
        clone.regenPts = this.regenPts;
        clone.shieldPts = this.shieldPts;
        clone.itemPrefab = this.itemPrefab;
        return clone;
    }
}