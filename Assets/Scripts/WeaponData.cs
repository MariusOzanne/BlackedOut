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
    public GameObject bulletPrefab; // Préfabriqué de la balle

    [Header("Damage & Stats")]
    public float speed; // Vitesse de la balle
    public int damage; // Dégâts infligés par la balle
    public int maxAmmo; // Munitions maximales de l'arme
    public float range; // Portée maximale de la balle
    public float spreadAngle; // Angle de dispersion des tirs (important pour les fusils à pompe)

    [Header("Visual Effects")]
    public GameObject muzzleFlashPrefab; // Préfabriqué pour l'effet de flash au canon
    public TrailRenderer bulletTrailPrefab; // Effet de traînée des balles
    public GameObject hitEffectPrefab; // Effet visuel quand une balle touche une cible

    [Header("Audio")]
    public AudioClip sound; // Son de tir de l'arme
}