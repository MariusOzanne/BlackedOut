using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : ScriptableObject
{
    [Header("Data")]
    public string name; // Nom de l'ennemi
    public int maxHealth; // Santé maximale de l'ennemi
    public int health; // Santé actuelle de l'ennemi
    public float speed; // Vitesse de déplacement de l'ennemi
    public int damage; // Dommage infligé par l'ennemi
    public float attackCooldownTime; // Temps de recharge entre les attaques
    public GameObject prefab; // Préfabriqué de l'ennemi pour l'instanciation

    [Header("Awards")]
    public int minCoins; // Nombre minimum de pièces lâchées à la mort
    public int maxCoins; // Nombre maximum de pièces lâchées à la mort
    public int scoreValue; // Valeur en points lorsque l'ennemi est tué

    // Méthode pour cloner les données, utilisée pour éviter les références partagées entre instances
    public EnemyData Clone()
    {
        var clone = ScriptableObject.CreateInstance<EnemyData>();
        clone.name = this.name;
        clone.maxHealth = this.maxHealth;
        clone.health = this.maxHealth; // Initialise la santé à la valeur maximale
        clone.speed = this.speed;
        clone.damage = this.damage;
        clone.attackCooldownTime = this.attackCooldownTime;
        clone.prefab = this.prefab;
        clone.minCoins = this.minCoins;
        clone.maxCoins = this.maxCoins;
        clone.scoreValue = this.scoreValue;
        return clone;
    }
}