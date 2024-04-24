using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsData : ScriptableObject
{
    [Header("Variables general")]
    public string itemName;
    public string itemDescription;
    public GameObject itemPrefab;
    public GameObject particlePrefab;
    [Header("Potions stats")]
    public int regenPts;
    public int shieldPts;
    public float speedBoost;
    public int damageBoost;
    public float effectDuration;
}