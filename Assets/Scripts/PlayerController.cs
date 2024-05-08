using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Joystick setup")]
    [SerializeField] private Joystick movementJoystick; // Joystick pour contr�ler les mouvements
    [SerializeField] private Joystick rotationJoystick; // Joystick pour contr�ler la rotation

    [Header("Shooting setup")]
    [SerializeField] private Transform bulletSpawnPoint; // Point d'origine du tir
    [SerializeField] private float fireThreshold; // Seuil de distance du joystick pour activer le tir

    [Header("Weapon System")]
    [SerializeField] private WeaponData[] availableWeapons; // Tableau des donn�es d'armes disponibles
    [HideInInspector] public int currentWeaponIndex = 0; // Index de l'arme courante dans le tableau
    [HideInInspector] public WeaponData currentWeapon; // R�f�rence � l'arme courante
    private float lastFireTime; // Dernier moment o� l'arme a tir�

    [Header("Speed")]
    public float speed; // Vitesse de d�placement du joueur
    
    [HideInInspector] public float defaultSpeed; // Vitesse par d�faut pour restaurer apr�s le mode rage
    [HideInInspector] public int defaultDamage; // D�g�ts par d�faut pour restaurer apr�s le mode rage

    private Rigidbody rb; // Composant Rigidbody pour le mouvement physique
    private Animator animator; // Composant Animator pour la gestion des animations
    private Health health; // Composant Health pour g�rer la sant� du joueur

    private Vector3 moveDirection; // Direction de d�placement actuelle
    private Vector3 rotationDirection; // Direction de rotation actuelle

    private float lastYRotation; // Dernier angle de rotation en degr�s

    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); // R�cup�ration du composant Rigidbody pour le mouvement physique
        animator = GetComponent<Animator>(); // R�cup�ration du composant Animator pour la gestion des animations
        health = GetComponent<Health>(); // R�cup�ration du composant Health pour g�rer la sant� du joueur

        defaultSpeed = speed; // Sauvegarde de la vitesse initiale

        if (availableWeapons.Length > 0)
        {
            InitializeWeapon(currentWeaponIndex); // Initialise avec l'index d'arme par d�faut ou charg�
        }
    }

    private void Update()
    {
        ProcessInputs(); // Traitement des entr�es du joueur
        Rotate(); // Rotation du personnage
        CheckFireCondition(); // V�rification des conditions de tir
    }

    private void FixedUpdate()
    {
        Move(); // D�placement physique bas� sur les inputs
    }

    public void InitializeWeapon(int weaponIndex)
    {
        // V�rifie si l'index sp�cifi� est dans les limites du tableau des armes disponibles
        if (weaponIndex >= 0 && weaponIndex < availableWeapons.Length)
        {
            currentWeaponIndex = weaponIndex; // Met � jour l'index courant de l'arme
            currentWeapon = availableWeapons[weaponIndex]; // Met � jour l'arme courante
            defaultDamage = currentWeapon.damage; // Sauvegarde des d�g�ts initiaux de l'arme
        }
    }

    private void ProcessInputs()
    {
        // Lecture des valeurs de d�placement du joystick et normalisation
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
        // Application de la v�locit� bas�e sur la direction et la vitesse actuelle
        rb.velocity = new Vector3(moveDirection.x * speed, rb.velocity.y, moveDirection.z * speed);

        // Mise � jour de l'�tat de l'animation de marche
        animator.SetBool("isWalking", moveDirection.magnitude > 0);
    }

    private void Rotate()
    {
        // Application de la rotation calcul�e au personnage
        transform.rotation = Quaternion.Euler(0, lastYRotation, 0);
    }

    private void CheckFireCondition()
    {
        // Calcul de la distance du joystick depuis le centre pour d�terminer si le joueur est en train de viser
        float distanceFromCenter = Vector2.Distance(new Vector2(rotationJoystick.Horizontal, rotationJoystick.Vertical), Vector2.zero);

        // V�rification de la condition de tir (distance et intervalle de temps depuis le dernier tir)
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
        // V�rifie si le point de tir et le pr�fab de la balle sont bien d�finis
        if (bulletSpawnPoint == null || currentWeapon.bulletPrefab == null)
        {
            return;
        }

        // Calcule la rotation de la balle bas�e sur la rotation actuelle du joueur
        Quaternion bulletRotation = Quaternion.Euler(90, lastYRotation, 0);
        // Cr�e un nouvel objet balle � la position de tir avec la rotation calcul�e
        GameObject bullet = Instantiate(currentWeapon.bulletPrefab, bulletSpawnPoint.position, bulletRotation);

        // R�cup�re le composant Bullet de la nouvelle balle pour acc�der � ses propri�t�s
        Bullet bulletComponent = bullet.GetComponent<Bullet>();

        if (bulletComponent != null)
        {
            // D�finit les d�g�ts de la balle en fonction des d�g�ts d�finis dans les donn�es de l'arme actuelle
            bulletComponent.damage = currentWeapon.damage;

            // R�cup�re le Rigidbody de la balle pour appliquer des forces
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            if (bulletRb != null)
            {
                // D�sactive la gravit� pour la balle
                bulletRb.useGravity = false;

                // D�finit la direction du tir bas�e sur l'orientation de la balle
                Vector3 fireDirection = bullet.transform.up;

                // Applique la force de tir
                bulletRb.AddForce(fireDirection.normalized * currentWeapon.speed, ForceMode.VelocityChange);

                // D�truit la balle apr�s que celle-ci a parcouru sa port�e maximale
                Destroy(bullet, currentWeapon.range / currentWeapon.speed);
            }

            // G�re l'affichage du flash de tir au canon
            if (currentWeapon.muzzleFlashPrefab != null)
            {
                GameObject muzzleFlash = Instantiate(currentWeapon.muzzleFlashPrefab, bulletSpawnPoint.position, Quaternion.identity);
                Destroy(muzzleFlash, 0.2f); // D�truit l'effet de flash apr�s un court instant
            }

            // Joue le son du tir si un AudioClip est assign�
            if (currentWeapon.sound != null)
            {
                AudioSource.PlayClipAtPoint(currentWeapon.sound, bulletSpawnPoint.position);
            }
        }
    }

    public void ChangeWeapon()
    {
        currentWeapon.damage = defaultDamage; // R�initialise les d�g�ts avant de changer d'arme pour s'assurer que les d�g�ts pr�c�dents ne sont pas conserv�s

        currentWeaponIndex = (currentWeaponIndex + 1) % availableWeapons.Length; // Incr�mente l'index de l'arme, boucle au d�but si n�cessaire
        currentWeapon = availableWeapons[currentWeaponIndex]; // Met � jour l'arme actuelle
        defaultDamage = currentWeapon.damage; // R�initialise les d�g�ts par d�faut lors du changement d'arme

        // R�applique les effets de rage si n�cessaire
        if (GetComponent<RageManager>().isRageActive)
        {
            GetComponent<RageManager>().UpdateRageEffectsOnWeaponChange();
        }
    }

    public void ResetSpeedAndDamage()
    {
        // Restaure la vitesse � sa valeur par d�faut
        speed = defaultSpeed;
        // Restaure les d�g�ts de l'arme actuelle � leur valeur par d�faut
        currentWeapon.damage = defaultDamage;
    }

    public void ResetToDefaultSettings()
    {
        // R�initialisation � l'arme par d�faut
        InitializeWeapon(0);
        // R�initialisation de la vitesse et des d�g�ts
        ResetSpeedAndDamage();
    }

    public void TakeDamage(int damage)
    {
        health.TakeDamage(damage); // Application des d�g�ts au joueur

        // V�rifie si la sant� du joueur est �puis�e
        if (health.currentHealth <= 0)
        {
            // ResetSpeedAndDamage(); // R�initialise la vitesse et les d�g�ts avant de montrer le panneau de d�faite
            GameManager.Instance.ShowDefeatPanel(); // Affichage du panneau de d�faite
        }
    }

    private void OnApplicationQuit()
    {
        ResetSpeedAndDamage(); // S'assure que les valeurs par d�faut sont r�tablies � la fermeture de l'application
    }

}