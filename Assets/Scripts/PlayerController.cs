using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Joystick setup")]
    [SerializeField] private Joystick movementJoystick; // Joystick pour contrôler les mouvements
    [SerializeField] private Joystick rotationJoystick; // Joystick pour contrôler la rotation

    [Header("Shooting setup")]
    [SerializeField] private Transform bulletSpawnPoint; // Point d'origine du tir
    [SerializeField] private float fireThreshold; // Seuil de distance du joystick pour activer le tir

    [Header("Weapon System")]
    [SerializeField] private WeaponData[] availableWeapons; // Tableau des données d'armes disponibles
    [HideInInspector] public int currentWeaponIndex = 0; // Index de l'arme courante dans le tableau
    [HideInInspector] public WeaponData currentWeapon; // Référence à l'arme courante
    private float lastFireTime; // Dernier moment où l'arme a tiré

    [Header("Speed")]
    public float speed; // Vitesse de déplacement du joueur
    
    [HideInInspector] public float defaultSpeed; // Vitesse par défaut pour restaurer après le mode rage
    [HideInInspector] public int defaultDamage; // Dégâts par défaut pour restaurer après le mode rage

    private Rigidbody rb; // Composant Rigidbody pour le mouvement physique
    private Animator animator; // Composant Animator pour la gestion des animations
    private Health health; // Composant Health pour gérer la santé du joueur

    private Vector3 moveDirection; // Direction de déplacement actuelle
    private Vector3 rotationDirection; // Direction de rotation actuelle

    private float lastYRotation; // Dernier angle de rotation en degrés

    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); // Récupération du composant Rigidbody pour le mouvement physique
        animator = GetComponent<Animator>(); // Récupération du composant Animator pour la gestion des animations
        health = GetComponent<Health>(); // Récupération du composant Health pour gérer la santé du joueur

        defaultSpeed = speed; // Sauvegarde de la vitesse initiale

        if (availableWeapons.Length > 0)
        {
            InitializeWeapon(currentWeaponIndex); // Initialise avec l'index d'arme par défaut ou chargé
        }
    }

    private void Update()
    {
        ProcessInputs(); // Traitement des entrées du joueur
        Rotate(); // Rotation du personnage
        CheckFireCondition(); // Vérification des conditions de tir
    }

    private void FixedUpdate()
    {
        Move(); // Déplacement physique basé sur les inputs
    }

    public void InitializeWeapon(int weaponIndex)
    {
        // Vérifie si l'index spécifié est dans les limites du tableau des armes disponibles
        if (weaponIndex >= 0 && weaponIndex < availableWeapons.Length)
        {
            currentWeaponIndex = weaponIndex; // Met à jour l'index courant de l'arme
            currentWeapon = availableWeapons[weaponIndex]; // Met à jour l'arme courante
            defaultDamage = currentWeapon.damage; // Sauvegarde des dégâts initiaux de l'arme
        }
    }

    private void ProcessInputs()
    {
        // Lecture des valeurs de déplacement du joystick et normalisation
        float moveX = movementJoystick.Horizontal;
        float moveZ = movementJoystick.Vertical;
        moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        // Lecture des valeurs de rotation du joystick et calcul de l'angle de rotation
        if (rotationJoystick.Horizontal != 0 || rotationJoystick.Vertical != 0)
        {
            float rotateX = rotationJoystick.Horizontal;
            float rotateZ = rotationJoystick.Vertical;
            rotationDirection = new Vector3(rotateX, 0, rotateZ).normalized;

            lastYRotation = Mathf.Atan2(rotationDirection.x, rotationDirection.z) * Mathf.Rad2Deg;
        }
    }

    private void Move()
    {
        // Application de la vélocité basée sur la direction et la vitesse actuelle
        rb.velocity = new Vector3(moveDirection.x * speed, rb.velocity.y, moveDirection.z * speed);

        // Mise à jour de l'état de l'animation de marche
        animator.SetBool("isWalking", moveDirection.magnitude > 0);
    }

    private void Rotate()
    {
        // Application de la rotation calculée au personnage
        transform.rotation = Quaternion.Euler(0, lastYRotation, 0);
    }

    private void CheckFireCondition()
    {
        // Calcul de la distance du joystick depuis le centre pour déterminer si le joueur est en train de viser
        float distanceFromCenter = Vector2.Distance(new Vector2(rotationJoystick.Horizontal, rotationJoystick.Vertical), Vector2.zero);

        // Vérification de la condition de tir (distance et intervalle de temps depuis le dernier tir)
        if (distanceFromCenter > fireThreshold && Time.time > lastFireTime + currentWeapon.fireRate)
        {
            FireWeapon();
            lastFireTime = Time.time;
            animator.SetBool("isShooting", true);
        }
        else
        {
            animator.SetBool("isShooting", false);
        }
    }

    private void FireWeapon()
    {
        // Vérifie si le point de tir et le préfab de la balle sont bien définis
        if (bulletSpawnPoint == null || currentWeapon.bulletPrefab == null)
        {
            return;
        }

        // Calcule la rotation de la balle basée sur la rotation actuelle du joueur
        Quaternion bulletRotation = Quaternion.Euler(90, lastYRotation, 0);
        // Crée un nouvel objet balle à la position de tir avec la rotation calculée
        GameObject bullet = Instantiate(currentWeapon.bulletPrefab, bulletSpawnPoint.position, bulletRotation);

        // Récupère le composant Bullet de la nouvelle balle pour accéder à ses propriétés
        Bullet bulletComponent = bullet.GetComponent<Bullet>();

        if (bulletComponent != null)
        {
            // Définit les dégâts de la balle en fonction des dégâts définis dans les données de l'arme actuelle
            bulletComponent.damage = currentWeapon.damage;

            // Récupère le Rigidbody de la balle pour appliquer des forces
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            if (bulletRb != null)
            {
                // Désactive la gravité pour la balle
                bulletRb.useGravity = false;

                // Définit la direction du tir basée sur l'orientation de la balle
                Vector3 fireDirection = bullet.transform.up;

                // Applique la force de tir
                bulletRb.AddForce(fireDirection.normalized * currentWeapon.speed, ForceMode.VelocityChange);

                // Détruit la balle après que celle-ci a parcouru sa portée maximale
                Destroy(bullet, currentWeapon.range / currentWeapon.speed);
            }

            // Gère l'affichage du flash de tir au canon
            if (currentWeapon.muzzleFlashPrefab != null)
            {
                GameObject muzzleFlash = Instantiate(currentWeapon.muzzleFlashPrefab, bulletSpawnPoint.position, Quaternion.identity);
                Destroy(muzzleFlash, 0.2f); // Détruit l'effet de flash après un court instant
            }

            // Joue le son du tir si un AudioClip est assigné
            if (currentWeapon.sound != null)
            {
                AudioSource.PlayClipAtPoint(currentWeapon.sound, bulletSpawnPoint.position);
            }
        }
    }

    public void ChangeWeapon()
    {
        currentWeapon.damage = defaultDamage; // Réinitialise les dégâts avant de changer d'arme pour s'assurer que les dégâts précédents ne sont pas conservés

        currentWeaponIndex = (currentWeaponIndex + 1) % availableWeapons.Length; // Incrémente l'index de l'arme, boucle au début si nécessaire
        currentWeapon = availableWeapons[currentWeaponIndex]; // Met à jour l'arme actuelle
        defaultDamage = currentWeapon.damage; // Réinitialise les dégâts par défaut lors du changement d'arme

        // Réapplique les effets de rage si nécessaire
        if (GetComponent<RageManager>().isRageActive)
        {
            GetComponent<RageManager>().UpdateRageEffectsOnWeaponChange();
        }
    }

    public void ResetSpeedAndDamage()
    {
        // Restaure la vitesse à sa valeur par défaut
        speed = defaultSpeed;
        // Restaure les dégâts de l'arme actuelle à leur valeur par défaut
        currentWeapon.damage = defaultDamage;
    }

    public void ResetToDefaultSettings()
    {
        // Réinitialisation à l'arme par défaut
        InitializeWeapon(0);
        // Réinitialisation de la vitesse et des dégâts
        ResetSpeedAndDamage();
    }

    public void TakeDamage(int damage)
    {
        health.TakeDamage(damage); // Application des dégâts au joueur

        // Vérifie si la santé du joueur est épuisée
        if (health.currentHealth <= 0)
        {
            // ResetSpeedAndDamage(); // Réinitialise la vitesse et les dégâts avant de montrer le panneau de défaite
            GameManager.Instance.ShowDefeatPanel(); // Affichage du panneau de défaite
        }
    }

    private void OnApplicationQuit()
    {
        ResetSpeedAndDamage(); // S'assure que les valeurs par défaut sont rétablies à la fermeture de l'application
    }

}