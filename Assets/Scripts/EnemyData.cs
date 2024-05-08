using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : ScriptableObject
{
    [Header("Data")]
    public string name; // Nom de l'ennemi
    public int maxHealth; // Sant� maximale de l'ennemi
    public int health; // Sant� actuelle de l'ennemi
    public float speed; // Vitesse de d�placement de l'ennemi
    public int damage; // Dommage inflig� par l'ennemi
    public float attackCooldownTime; // Temps de recharge entre les attaques
    public GameObject prefab; // Pr�fabriqu� de l'ennemi pour l'instanciation

    [Header("Awards")]
    public int minCoins; // Nombre minimum de pi�ces l�ch�es � la mort
    public int maxCoins; // Nombre maximum de pi�ces l�ch�es � la mort
    public int scoreValue; // Valeur en points lorsque l'ennemi est tu�

    // M�thode pour cloner les donn�es, utilis�e pour �viter les r�f�rences partag�es entre instances
    public EnemyData Clone()
    {
        var clone = ScriptableObject.CreateInstance<EnemyData>();
        clone.name = this.name;
        clone.maxHealth = this.maxHealth;
        clone.health = this.maxHealth; // Initialise la sant� � la valeur maximale
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