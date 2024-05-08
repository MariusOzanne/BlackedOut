using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : ScriptableObject
{
    [Header("Data")]
    public string name; // Nom de l'objet
    public string description; // Description de l'objet
    public GameObject prefab; // Pr�fabriqu� de l'objet pour l'instanciation

    [Header("Effects")]
    public int healthRegeneration; // Montant de sant� r�g�n�r�e par cet objet
    public int shieldAmount; // Montant du bouclier ajout� par cet objet
    public float speedMultiplier; // Multiplicateur de vitesse fourni par cet objet
    public int additionalDamage; // Dommages suppl�mentaires fournis par cet objet
    public float durationOfEffect; // Dur�e des effets de cet objet
}