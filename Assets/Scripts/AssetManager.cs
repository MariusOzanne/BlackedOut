using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

// Classe statique AssetManager pour g�rer la cr�ation de ScriptableObjects via des menus personnalis�s
public static class AssetManager
{
    // Ajoute une option de menu pour cr�er des EnemyData sous le menu "Assets/Create/Game Data"
    [MenuItem("Assets/Create/Game Data/Enemy Data")]
    public static void CreateEnemyData()
    {
        CreateScriptableObject<EnemyData>("NewEnemy", "EnemiesInstances");
    }

    // Ajoute une option de menu pour cr�er des WeaponData sous le menu "Assets/Create/Game Data"
    [MenuItem("Assets/Create/Game Data/Weapon Data")]
    public static void CreateWeaponData()
    {
        CreateScriptableObject<WeaponData>("NewWeapon", "WeaponsInstances");
    }

    // Ajoute une option de menu pour cr�er des ItemData sous le menu "Assets/Create/Game Data"
    [MenuItem("Assets/Create/Game Data/Item Data")]
    public static void CreateItemData()
    {
        CreateScriptableObject<ItemData>("NewItem", "ItemsInstances");
    }

    // M�thode g�n�rique pour cr�er un ScriptableObject du type sp�cifi�
    // 'T' est le type de ScriptableObject � cr�er
    // 'fileName' est le nom initial sugg�r� pour le fichier
    // 'folderName' est le dossier dans lequel le fichier sera cr��
    private static void CreateScriptableObject<T>(string fileName, string folderName) where T : ScriptableObject
    {
        // Cr�e une nouvelle instance du ScriptableObject sp�cifi�
        T asset = ScriptableObject.CreateInstance<T>();

        // Construit le chemin complet en se basant sur le dossier "Scripts" principal
        string basePath = "Assets/Scripts";
        string fullPath = Path.Combine(basePath, folderName);

        // V�rifie si le dossier sp�cifi� existe, sinon le cr�e
        if (!AssetDatabase.IsValidFolder(fullPath))
        {
            AssetDatabase.CreateFolder(basePath, folderName);
        }

        // G�n�re un chemin unique pour le nouvel asset dans le dossier sp�cifi�
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(fullPath, $"{fileName}.asset"));

        // Cr�e l'asset au chemin sp�cifi� et l'enregistre
        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        
        // Rafra�chit la base de donn�es d'assets pour mettre � jour l'�diteur
        AssetDatabase.Refresh();
        
        // Met le focus sur la fen�tre du projet et s�lectionne le nouvel objet
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}