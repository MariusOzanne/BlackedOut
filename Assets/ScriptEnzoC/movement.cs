using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeplacementJoueur : MonoBehaviour
{
    public float vitesseDeplacement = 5f;

    void Update()
    {
        // Obtient les entrées de l'axe horizontal et vertical
        float deplacementHorizontal = Input.GetAxis("Horizontal");
        float deplacementVertical = Input.GetAxis("Vertical");

        // Calcule le vecteur de déplacement en fonction des entrées
        Vector3 deplacement = new Vector3(deplacementHorizontal, 0f, deplacementVertical) * vitesseDeplacement * Time.deltaTime;

        // Applique le déplacement à la position de l'objet
        transform.Translate(deplacement, Space.Self);
    }
}