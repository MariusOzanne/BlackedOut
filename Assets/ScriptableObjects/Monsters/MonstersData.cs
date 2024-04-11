using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstersData : ScriptableObject
{
    [Header("Statistiques")]
    public string monsterName;
    public int health;
    public float speed;
    public int damage;
    public float attackCooldown;
    public GameObject monsterPrefab;
    //[Header("Zone de detection")]
    //public float detectionRange;
    //public float attackRange;
}
