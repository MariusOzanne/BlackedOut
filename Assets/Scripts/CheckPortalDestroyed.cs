/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPortalDestroyed : MonoBehaviour
{
    [SerializeField] private GameObject[] portals; // Liste des portails sur la scène
    [SerializeField] private GameObject victoryPanel; // Panel à afficher lorsque tous les portails sont détruits

    void Update()
    {
        // Vérifie si tous les portails ont été détruits
        bool allPortalsDestroyed = true;
        foreach (GameObject portal in portals)
        {
            if (portal != null)
            {
                allPortalsDestroyed = false;
                break;
            }
        }

        // Si tous les portails ont été détruits, affiche le panel de victoire
        if (allPortalsDestroyed)
        {
            victoryPanel.SetActive(true);
        }
    }
}

*/