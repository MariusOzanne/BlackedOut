using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Joystick setup")]
    [SerializeField] private Joystick movementJoystick;
    [SerializeField] private Joystick rotationJoystick;
    [Header("Shooting setup")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
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
        // Utilise la position du movementJoystick pour d�finir la direction du mouvement
        float moveX = movementJoystick.Horizontal;
        float moveZ = movementJoystick.Vertical;
        moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        // Utilise la position du rotationJoystick pour d�finir la direction de la rotation seulement si le joueur bouge le rotationJoystick
        if (rotationJoystick.Horizontal != 0 || rotationJoystick.Vertical != 0)
        {
            float rotateX = rotationJoystick.Horizontal;
            float rotateZ = rotationJoystick.Vertical;
            rotationDirection = new Vector3(rotateX, 0, rotateZ).normalized;

            // Calcul de l'angle de rotation et stockage comme derni�re rotation valide
            lastYRotation = Mathf.Atan2(rotationDirection.x, rotationDirection.z) * Mathf.Rad2Deg;
        }
    }

    private void Move()
    {
        float currentSpeed = GameManager.Instance.speed;
        rb.velocity = new Vector3(moveDirection.x * currentSpeed, rb.velocity.y, moveDirection.z * currentSpeed);

        // Si le joueur se d�place, active l'animation de marche
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
            FireBullet();
            lastFireTime = Time.time; // Mise � jour du temps pour le dernier tir
            animator.SetBool("isShooting", true);
        }
        else
        {
            animator.SetBool("isShooting", false);
        }
    }

    private void FireBullet()
    {
        if (bulletPrefab && bulletSpawnPoint)
        {
            // Instancie la balle au point de d�part avec la rotation ajust�e de 90 degr�s en X
            Quaternion bulletRotation = Quaternion.Euler(90, lastYRotation, 0);
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletRotation);

            // Obtient le Rigidbody de la balle
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            if (bulletRb != null)
            {
                // D�sactive la gravit� pour la balle
                bulletRb.useGravity = false;

                // Utilise la direction avant de la balle pour la force
                Vector3 fireDirection = bullet.transform.up;

                // Applique une force � la balle dans la direction calcul�e
                bulletRb.AddForce(fireDirection.normalized * 1000);

                // D�truit la balle apr�s un certain temps
                Destroy(bullet, 5f);
            }
        }
    }
}