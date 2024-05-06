using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.SimpleSpinner
{
    [RequireComponent(typeof(Image))] // N�cessite que le GameObject poss�de un composant Image
    public class SpinnerAnimator : MonoBehaviour
    {
        [Header("Rotation Settings")]
        [SerializeField] private bool enableRotation = true; // Active ou d�sactive la rotation
        [Range(-10, 10), Tooltip("Speed in Hz (revolutions per second).")]
        [SerializeField] private float rotationSpeed = 1; // Vitesse de rotation en Hz
        [SerializeField] private AnimationCurve rotationCurve = AnimationCurve.Linear(0, 0, 1, 1); // Courbe de vitesse de rotation

        [Header("Color Change Settings")]
        [SerializeField] private bool enableColorChange = true; // Active ou d�sactive le changement de couleur
        [Range(-10, 10), Tooltip("Speed in Hz (revolutions per second).")]
        [SerializeField] private float colorChangeSpeed = 0.5f; // Vitesse de changement de couleur en Hz
        [Range(0, 1)]
        [SerializeField] private float colorSaturation = 1f; // Saturation de la couleur
        [SerializeField] private AnimationCurve colorChangeCurve = AnimationCurve.Linear(0, 0, 1, 1); // Courbe de changement de couleur

        [Header("General Options")]
        [SerializeField] private bool randomizeStart = true; // Commence l'animation � un point al�atoire

        private Image imageComponent; // Composant Image de l'�l�ment
        private float animationOffset; // D�calage initial pour les animations

        private void Start()
        {
            imageComponent = GetComponent<Image>(); // R�cup�re le composant Image
            animationOffset = randomizeStart ? Random.Range(0f, 1f) : 0; // D�termine l'offset de d�part
        }

        private void Update()
        {
            if (enableRotation) // Si la rotation est activ�e
            {
                float rotationAngle = -360 * rotationCurve.Evaluate((rotationSpeed * Time.time + animationOffset) % 1); // Calcule l'angle de rotation
                transform.localEulerAngles = new Vector3(0, 0, rotationAngle); // Applique la rotation
            }

            if (enableColorChange) // Si le changement de couleur est activ�
            {
                float hue = colorChangeCurve.Evaluate((colorChangeSpeed * Time.time + animationOffset) % 1); // Calcule la teinte
                imageComponent.color = Color.HSVToRGB(hue, colorSaturation, 1); // Applique la couleur
            }
        }
    }
}