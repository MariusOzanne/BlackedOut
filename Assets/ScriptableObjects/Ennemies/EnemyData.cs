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

    [Header("R�compenses")]
    public int minCoins;
    public int maxCoins;
    public int scoreValue;

    /// <summary>
    /// Cr�e une copie de cet EnemyData avec une nouvelle sant� maximale et des valeurs r�initialis�es.
    /// </summary>
    /// <returns>Une nouvelle instance de EnemyData avec des valeurs copi�es.</returns>
    public EnemyData Clone()
    {
        var clone = ScriptableObject.CreateInstance<EnemyData>();
        clone.enemyName = this.enemyName;
        clone.maxHealth = this.maxHealth;
        clone.health = this.maxHealth; // R�initialiser la sant� � la valeur maximale lors du clonage
        clone.speed = this.speed;
        clone.damage = this.damage;
        clone.attackCooldown = this.attackCooldown;
        clone.enemyPrefab = this.enemyPrefab;
        clone.minCoins = this.minCoins;
        clone.maxCoins = this.maxCoins;
        clone.scoreValue = this.scoreValue;
        clone.enemySound = this.enemySound;
        clone.wizardnoiseSound = this.wizardnoiseSound;
        return clone;
    }
}