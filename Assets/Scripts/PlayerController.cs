using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Joystick setup")]
    [SerializeField] private Joystick movementJoystick; // Joystick pour contrôler les mouvements
    [SerializeField] private Joystick rotationJoystick; // Joystick pour contrôler la rotation

    [Header("Shooting setup")]
    [SerializeField] private GameObject bulletPrefab; // Préfabriqué de la balle à tirer
    [SerializeField] private Transform bulletSpawnPoint; // Point d'origine du tir
    [SerializeField] private float fireThreshold; // Seuil de distance du joystick pour activer le tir
    [SerializeField] private float fireRate; // Cadence de tir

    // [SerializeField] private int currentGunIndex = 0;
    // [SerializeField] private float maxAngleDeviation = 10f;

    private Rigidbody rb;
    private Animator animator;
    private Health health;

    private Vector3 moveDirection;
    private Vector3 rotationDirection;

    private float lastYRotation;
    private float lastFireTime;

    /*

    private LineRenderer currentTracer;
    public LineRenderer tracerPrefab;
    private Vector3 tracerEndPoint;
    public float tracerWidth = 0.05f;
    public float maxTracerLength = 10f;
    public GameObject spotlightPrefab;


    public TrailRenderer trailPrefab;
    private TrailRenderer currentTrail;
    public float trailTime = 0.05f;

    public WeaponData AssaultRifle;
    public WeaponData Shotgun;
    public int assaultRifleDamage = 10;

    */

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Récupération du composant Rigidbody pour le mouvement physique
        animator = GetComponent<Animator>(); // Récupération du composant Animator pour la gestion des animations
        health = GetComponent<Health>(); // Récupération du composant Health pour gérer la santé du joueur
    }

    private void Update()
    {
        ProcessInputs(); // Traitement des entrées du joueur
        Rotate(); // Rotation du personnage
        CheckFireCondition(); // Vérification des conditions de tir

        /*
        
        if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchGun();
            Debug.Log("switching");
        }

        */
    }

    private void FixedUpdate()
    {
        Move(); // Déplacement physique basé sur les inputs
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
        float currentSpeed = GameManager.Instance.speed;
        rb.velocity = new Vector3(moveDirection.x * currentSpeed, rb.velocity.y, moveDirection.z * currentSpeed);

        // Mise à jour de l'état de l'animation de marche
        if (moveDirection.magnitude > 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
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
        if (distanceFromCenter > fireThreshold && Time.time > lastFireTime + fireRate)
        {
            FireBullet();
            lastFireTime = Time.time;
            animator.SetBool("isShooting", true);

            /*

            switch (currentGunIndex)
            {
                case 0:
                    FireGun1();
                    break;
                case 1:
                    FireGun2();
                    break;
                case 2:
                    FireGun3();
                    break;
                default:
                    Debug.LogError("Invalid gun index!");
                    break;
            }

            */
        }
        else
        {
            animator.SetBool("isShooting", false);
        }
    }

    private void FireBullet()
    {
        // Vérification et instantiation de la balle
        if (bulletPrefab && bulletSpawnPoint)
        {
            Quaternion bulletRotation = Quaternion.Euler(90, lastYRotation, 0);
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletRotation);

            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            if (bulletRb != null)
            {
                // La balle ne subit pas la gravité
                bulletRb.useGravity = false;

                // Direction de tir basée sur l'orientation de la balle
                Vector3 fireDirection = bullet.transform.up;

                // Application de la force de tir
                bulletRb.AddForce(fireDirection.normalized * 1000);

                // Destruction de la balle après 5 secondes
                Destroy(bullet, 5f);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health.TakeDamage(damage); // Application des dégâts au joueur

        // Vérifie si la santé du joueur est épuisée
        if (health.currentHealth <= 0)
        {
            GameManager.Instance.ShowDefeatPanel(); // Affichage du panneau de défaite
        }
    }

    /*

    private void SwitchGun()
    {
        currentGunIndex = (currentGunIndex + 1) % 3;
    }

    private void FireGun1()
    {
        if (Time.time > lastFireTime + AssaultRifle.fireRate)
        {
            GameObject spotlight = Instantiate(spotlightPrefab, transform.position, Quaternion.identity);
            Destroy(spotlight, 0.05f);
            lastFireTime = Time.time;
            Vector3 fireDirection = transform.forward;
            int layerMask = ~(1 << LayerMask.NameToLayer("ZoneAttack"));
            RaycastHit hitInfo;
            if (Physics.Raycast(bulletSpawnPoint.position, fireDirection, out hitInfo, Mathf.Infinity, layerMask))
            {
                Debug.Log("Bullet hit: " + hitInfo.collider.gameObject.name);
                Debug.DrawLine(bulletSpawnPoint.position, hitInfo.point, Color.green, 0.1f);
                EnemyController enemy = hitInfo.collider.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(5);
                    Debug.Log("HIT");
                }
                tracerEndPoint = hitInfo.point; // Set tracer end point to the hit point
            }
            else
            {
                Vector3 endPoint = bulletSpawnPoint.position + fireDirection * 100f;
                Debug.DrawLine(bulletSpawnPoint.position, endPoint, Color.red, 0.1f);
                tracerEndPoint = endPoint; // Set tracer end point to the maximum range
            }

            // Instantiate a trail object
            currentTrail = Instantiate(trailPrefab, bulletSpawnPoint.position, Quaternion.identity);
            // Set the time the trail should be visible
            currentTrail.time = trailTime;
            // Set the starting position of the trail
            currentTrail.transform.position = bulletSpawnPoint.position;
            // Set the end position of the trail
            currentTrail.AddPosition(tracerEndPoint);
            // Destroy the trail after the specified time
            Destroy(currentTrail.gameObject, 0.05f);
        }
    }


    private void FireGun2()
    {
        Vector3 forwardDirection = transform.forward;
        int layerMask = ~(1 << LayerMask.NameToLayer("ZoneAttack"));
               
        if (Time.time > lastFireTime + 0.99)
        {
            GameObject spotlight = Instantiate(spotlightPrefab, transform.position, Quaternion.identity);
            Destroy(spotlight, 0.05f);
            animator.SetBool("isShooting", true);
            lastFireTime = Time.time;
            for (int i = 0; i < 9; i++)
            {
                Vector3 randomDirection = Quaternion.Euler(0, Random.Range(-maxAngleDeviation, maxAngleDeviation), 0) * forwardDirection;
                RaycastHit hitInfo;
                if (Physics.Raycast(bulletSpawnPoint.position, randomDirection, out hitInfo, Mathf.Infinity, layerMask))
                {
                    Debug.Log("Bullet hit: " + hitInfo.collider.gameObject.name);
                    Debug.DrawLine(bulletSpawnPoint.position, hitInfo.point, Color.green, 0.1f);
                    EnemyController enemy = hitInfo.collider.GetComponent<EnemyController>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(5);
                        Debug.Log("HIT");
                    }
                    tracerEndPoint = hitInfo.point; // Set tracer end point to the hit point
                }
                else
                {
                    Vector3 endPoint = bulletSpawnPoint.position + randomDirection * 100f;
                    Debug.DrawLine(bulletSpawnPoint.position, endPoint, Color.red, 0.1f);
                    tracerEndPoint = endPoint; // Set tracer end point to the maximum range
                }

                // Instantiate a trail object
                currentTrail = Instantiate(trailPrefab, bulletSpawnPoint.position, Quaternion.identity);
                // Set the time the trail should be visible
                currentTrail.time = trailTime;
                // Set the starting position of the trail
                currentTrail.transform.position = bulletSpawnPoint.position;
                // Set the end position of the trail
                currentTrail.AddPosition(tracerEndPoint);
                // Destroy the trail after the specified time
                Destroy(currentTrail.gameObject, 0.05f);
            }
        }
    }

    private void FireGun3()
    {
        if (Time.time > lastFireTime + 1)
        {
            GameObject spotlight = Instantiate(spotlightPrefab, transform.position, Quaternion.identity);
            Destroy(spotlight, 0.05f);
            lastFireTime = Time.time;
            Vector3 fireDirection = transform.forward;
            int layerMask = ~(1 << LayerMask.NameToLayer("ZoneAttack"));
            RaycastHit hitInfo;
            if (Physics.Raycast(bulletSpawnPoint.position, fireDirection, out hitInfo, Mathf.Infinity, layerMask))
            {
                Debug.Log("Bullet hit: " + hitInfo.collider.gameObject.name);
                Debug.DrawLine(bulletSpawnPoint.position, hitInfo.point, Color.green, 0.1f);
                EnemyController enemy = hitInfo.collider.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(10);
                    Debug.Log("HIT");
                }
                tracerEndPoint = hitInfo.point; // Set tracer end point to the hit point
            }
            else
            {
                Vector3 endPoint = bulletSpawnPoint.position + fireDirection * 100f;
                Debug.DrawLine(bulletSpawnPoint.position, endPoint, Color.red, 0.1f);
                tracerEndPoint = endPoint; // Set tracer end point to the maximum range
            }

            // Instantiate a trail object
            currentTrail = Instantiate(trailPrefab, bulletSpawnPoint.position, Quaternion.identity);
            // Set the time the trail should be visible
            currentTrail.time = trailTime;
            // Set the starting position of the trail
            currentTrail.transform.position = bulletSpawnPoint.position;
            // Set the end position of the trail
            currentTrail.AddPosition(tracerEndPoint);
            // Destroy the trail after the specified time
            Destroy(currentTrail.gameObject, 0.05f);
        }

    }

    */
}