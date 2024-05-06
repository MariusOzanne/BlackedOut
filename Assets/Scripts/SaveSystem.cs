using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public void SaveData()
    {
        SavedData savedData = new SavedData
        {
            coins = GameManager.Instance.coins,
            score = GameManager.Instance.score
        };

        string jsonData = JsonUtility.ToJson(savedData);
        string filePath = Application.persistentDataPath + "/SavedData.json";
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, jsonData);
        Debug.Log("Sauvegarde effectuée");
    }

    public void LoadData()
    {
        string filePath = Application.persistentDataPath + "/SavedData.json";

        if (System.IO.File.Exists(filePath))
        {
            string jsonData = System.IO.File.ReadAllText(filePath);
            SavedData savedData = JsonUtility.FromJson<SavedData>(jsonData);

            // Chargement des données
            GameManager.Instance.coins = savedData.coins;
            GameManager.Instance.score = savedData.score;

            Debug.Log("Chargement terminé");
        }
    }
}

public class SavedData
{
    public int coins;
    public int score;
}