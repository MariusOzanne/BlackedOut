using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Joystick movementJoystick;
    [SerializeField] private Joystick rotationJoystick;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    [SerializeField] private float speed;
    [SerializeField] private float fireThreshold;
    [SerializeField] private float fireRate;

    private Rigidbody rb;
    private Animator animator;

    private Vector3 moveDirection;
    private Vector3 rotationDirection;

    private float lastYRotation;
    private float lastFireTime;

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
        float yVelocity = rb.velocity.y;
        rb.velocity = new Vector3(moveDirection.x * speed, yVelocity, moveDirection.z * speed);

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

    private void CheckFireCondition()
    {
        float distanceFromCenter = Vector2.Distance(new Vector2(rotationJoystick.Horizontal, rotationJoystick.Vertical), Vector2.zero);

        if (distanceFromCenter > fireThreshold && Time.time > lastFireTime + fireRate)
        {
            FireProjectile();
            lastFireTime = Time.time; // Mise à jour du temps pour le dernier tir
        }
    }

    private void FireProjectile()
    {
        if (projectilePrefab && projectileSpawnPoint)
        {
            // Instancie le projectile au point de départ avec la rotation du projectileSpawnPoint
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            // Obtient le Rigidbody du projectile
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

            if (projectileRb != null)
            {
                // Désactive la gravité pour le projectile
                projectileRb.useGravity = false;

                // Utilise la direction vers l'avant du projectileSpawnPoint pour la force
                Vector3 fireDirection = projectileSpawnPoint.forward;

                // Applique une force au projectile dans la direction calculée
                projectileRb.AddForce(fireDirection.normalized * 1000);

                // Détruit le projectile après un certain temps
                Destroy(projectile, 1.5f);
            }
        }
    }
}