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
    public AudioClip enemySound;
    public AudioSource EnemySource;
    public AudioClip wizardnoiseSound;
    public AudioSource WizardSource;

    public AudioClip slimeNoiseSound;
    public AudioSource SlimeNoiseSource;
    public AudioClip slimeAttackSound;
    public AudioSource SlimeAttackSource;

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
        clone.enemySound = this.enemySound;
        clone.wizardnoiseSound = this.wizardnoiseSound;
        clone.slimeNoiseSound = this.slimeNoiseSound;
        clone.slimeAttackSound = this.slimeAttackSound;
        return clone;
    }
}