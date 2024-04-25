using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Joystick setup")]
    [SerializeField] private Joystick movementJoystick;
    [SerializeField] private Joystick rotationJoystick;
    [Header("Shooting setup")]    
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float fireThreshold;
    //[SerializeField] private float fireRate;
    [SerializeField] private int currentGunIndex = 0;
    [SerializeField] private float maxAngleDeviation = 10f;

    // Rigidbody and Animator components
    private Rigidbody rb;
    private Animator animator;

    // Movement and rotation directions
    private Vector3 moveDirection;
    private Vector3 rotationDirection;

    // Last rotation Y-axis
    private float lastYRotation;

    // Tracer related variables
    private LineRenderer currentTracer;
    public LineRenderer tracerPrefab;
    private Vector3 tracerEndPoint;
    private float lastFireTime;
    public float tracerWidth = 0.05f;
    public float maxTracerLength = 10f;
    public GameObject spotlightPrefab;


    // Trail renderer variables
    public TrailRenderer trailPrefab;
    private TrailRenderer currentTrail;
    public float trailTime = 0.05f;

    // Weapon data
    public WeaponData AssaultRifle;
    public WeaponData Shotgun;
    public int assaultRifleDamage = 10;




    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        ProcessInputs();
        Rotate();
        CheckFireCondition();
        if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchGun();
            Debug.Log("switching");
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void ProcessInputs()
    {
        // Utilise la position du movementJoystick pour définir la direction du mouvement
        float moveX = movementJoystick.Horizontal;
        float moveZ = movementJoystick.Vertical;
        moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        // Utilise la position du rotationJoystick pour définir la direction de la rotation seulement si le joueur bouge le rotationJoystick
        if (rotationJoystick.Horizontal != 0 || rotationJoystick.Vertical != 0)
        {
            float rotateX = rotationJoystick.Horizontal;
            float rotateZ = rotationJoystick.Vertical;
            rotationDirection = new Vector3(rotateX, 0, rotateZ).normalized;

            // Calcul de l'angle de rotation et stockage comme dernière rotation valide
            lastYRotation = Mathf.Atan2(rotationDirection.x, rotationDirection.z) * Mathf.Rad2Deg;
        }
    }

    private void Move()
    {
        float currentSpeed = GameManager.Instance.speed;
        rb.velocity = new Vector3(moveDirection.x * currentSpeed, rb.velocity.y, moveDirection.z * currentSpeed);

        // Si le joueur se déplace, active l'animation de marche
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
        transform.rotation = Quaternion.Euler(0, lastYRotation, 0);
    }

    private void SwitchGun()
    {        
        currentGunIndex = (currentGunIndex + 1) % 3;
    }


    private void CheckFireCondition()
    {
        float distanceFromCenter = Vector2.Distance(new Vector2(rotationJoystick.Horizontal, rotationJoystick.Vertical), Vector2.zero);

        if (distanceFromCenter > fireThreshold)
        {                    
            // Call the appropriate method to fire the gun based on the current gun index
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
        }
        else
        {
            animator.SetBool("isShooting", false);
        }
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

        

    //private void FireBullet()
    //{      
    //    if (bulletPrefab && bulletSpawnPoint)
    //    {
    //        // Instancie la balle au point de départ avec la rotation ajustée de 90 degrés en X
    //        Quaternion bulletRotation = Quaternion.Euler(90, lastYRotation, 0);
    //        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletRotation);

    //        // Obtient le Rigidbody de la balle
    //        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

    //        if (bulletRb != null)
    //        {
    //            // Désactive la gravité pour la balle
    //            bulletRb.useGravity = false;

    //            // Utilise la direction avant de la balle pour la force
    //            Vector3 fireDirection = bullet.transform.up;

    //            // Applique une force à la balle dans la direction calculée
    //            bulletRb.AddForce(fireDirection.normalized * 1000);

    //            // Détruit la balle après un certain temps
    //            Destroy(bullet, 5f);
    //        }
    //    }
    //}
}