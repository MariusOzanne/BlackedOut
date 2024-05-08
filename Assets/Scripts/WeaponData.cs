using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : ScriptableObject
{
    [Header("Data")]
    public string name; // Nom de l'arme
    public string description; // Description de l'arme
    public float fireRate; // Cadence de tir (tirs par seconde)

    [Header("Prefab Settings")]
    public GameObject bulletPrefab; // Pr�fabriqu� de la balle

    [Header("Damage & Stats")]
    public float speed; // Vitesse de la balle
    public int damage; // D�g�ts inflig�s par la balle
    public int maxAmmo; // Munitions maximales de l'arme
    public float range; // Port�e maximale de la balle
    public float spreadAngle; // Angle de dispersion des tirs (important pour les fusils � pompe)

    [Header("Visual Effects")]
    public GameObject muzzleFlashPrefab; // Pr�fabriqu� pour l'effet de flash au canon
    public TrailRenderer bulletTrailPrefab; // Effet de tra�n�e des balles
    public GameObject hitEffectPrefab; // Effet visuel quand une balle touche une cible

    [Header("Audio")]
    public AudioClip sound; // Son de tir de l'arme
}