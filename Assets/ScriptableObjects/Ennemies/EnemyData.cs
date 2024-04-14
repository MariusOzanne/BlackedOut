using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : ScriptableObject
{
    [Header("Statistiques")]
    public string enemyName;
    public int maxHealth; 
    public int health; 
    public float speed;
    public int damage;
    public float attackCooldown;
    public GameObject enemyPrefab;

}
