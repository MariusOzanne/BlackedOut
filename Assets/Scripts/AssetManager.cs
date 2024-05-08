using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

// Classe statique AssetManager pour gérer la création de ScriptableObjects via des menus personnalisés
public static class AssetManager
{
    // Ajoute une option de menu pour créer des EnemyData sous le menu "Assets/Create/Game Data"
    [MenuItem("Assets/Create/Game Data/Enemy Data")]
    public static void CreateEnemyData()
    {
        CreateScriptableObject<EnemyData>("NewEnemy", "EnemiesInstances");
    }

    // Ajoute une option de menu pour créer des WeaponData sous le menu "Assets/Create/Game Data"
    [MenuItem("Assets/Create/Game Data/Weapon Data")]
    public static void CreateWeaponData()
    {
        CreateScriptableObject<WeaponData>("NewWeapon", "WeaponsInstances");
    }

    // Ajoute une option de menu pour créer des ItemData sous le menu "Assets/Create/Game Data"
    [MenuItem("Assets/Create/Game Data/Item Data")]
    public static void CreateItemData()
    {
        CreateScriptableObject<ItemData>("NewItem", "ItemsInstances");
    }

    // Méthode générique pour créer un ScriptableObject du type spécifié
    // 'T' est le type de ScriptableObject à créer
    // 'fileName' est le nom initial suggéré pour le fichier
    // 'folderName' est le dossier dans lequel le fichier sera créé
    private static void CreateScriptableObject<T>(string fileName, string folderName) where T : ScriptableObject
    {
        // Crée une nouvelle instance du ScriptableObject spécifié
        T asset = ScriptableObject.CreateInstance<T>();

        // Construit le chemin complet en se basant sur le dossier "Scripts" principal
        string basePath = "Assets/Scripts";
        string fullPath = Path.Combine(basePath, folderName);

        // Vérifie si le dossier spécifié existe, sinon le crée
        if (!AssetDatabase.IsValidFolder(fullPath))
        {
            AssetDatabase.CreateFolder(basePath, folderName);
        }

        // Génère un chemin unique pour le nouvel asset dans le dossier spécifié
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(fullPath, $"{fileName}.asset"));

        // Crée l'asset au chemin spécifié et l'enregistre
        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        
        // Rafraîchit la base de données d'assets pour mettre à jour l'éditeur
        AssetDatabase.Refresh();
        
        // Met le focus sur la fenêtre du projet et sélectionne le nouvel objet
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}