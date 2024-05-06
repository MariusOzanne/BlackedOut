/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPortalDestroyed : MonoBehaviour
{
    [SerializeField] private GameObject[] portals; // Liste des portails sur la sc�ne
    [SerializeField] private GameObject victoryPanel; // Panel � afficher lorsque tous les portails sont d�truits

    void Update()
    {
        // V�rifie si tous les portails ont �t� d�truits
        bool allPortalsDestroyed = true;
        foreach (GameObject portal in portals)
        {
            if (portal != null)
            {
                allPortalsDestroyed = false;
                break;
            }
        }

        // Si tous les portails ont �t� d�truits, affiche le panel de victoire
        if (allPortalsDestroyed)
        {
            victoryPanel.SetActive(true);
        }
    }
}

*/