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

    [Header("Récompenses")]
    public int minCoins;
    public int maxCoins;
    public int scoreValue;

    public EnemyData Clone()
    {
        var clone = ScriptableObject.CreateInstance<EnemyData>();
        clone.enemyName = this.enemyName;
        clone.maxHealth = this.maxHealth;
        clone.health = this.maxHealth; // Réinitialiser la santé à la valeur maximale lors du clonage
        clone.speed = this.speed;
        clone.damage = this.damage;
        clone.attackCooldown = this.attackCooldown;
        clone.enemyPrefab = this.enemyPrefab;
        clone.minCoins = this.minCoins;
        clone.maxCoins = this.maxCoins;
        clone.scoreValue = this.scoreValue;
        return clone;
    }
}