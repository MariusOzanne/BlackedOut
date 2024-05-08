using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : ScriptableObject
{
    [Header("Data")]
    public string name; // Nom de l'objet
    public string description; // Description de l'objet
    public GameObject prefab; // Préfabriqué de l'objet pour l'instanciation

    [Header("Effects")]
    public int healthRegeneration; // Montant de santé régénérée par cet objet
    public int shieldAmount; // Montant du bouclier ajouté par cet objet
    public float speedMultiplier; // Multiplicateur de vitesse fourni par cet objet
    public int additionalDamage; // Dommages supplémentaires fournis par cet objet
    public float durationOfEffect; // Durée des effets de cet objet
}