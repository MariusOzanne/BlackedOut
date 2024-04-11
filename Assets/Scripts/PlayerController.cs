using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Joystick movementJoystick;
    [SerializeField] private Joystick rotationJoystick;

    [SerializeField] private float speed;

    private Rigidbody rb;

    private Vector3 moveDirection;
    private Vector3 rotationDirection;

    private float lastYRotation = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        ProcessInputs();
        Rotate();
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
        rb.velocity = new Vector3(moveDirection.x * speed, rb.velocity.y, moveDirection.z * speed);
    }

    private void Rotate()
    {
        transform.rotation = Quaternion.Euler(0, lastYRotation, 0);
    }
}