using UnityEngine;
using System.IO;
using Unity.VisualScripting;

public class JsonManagerBulletData : MonoBehaviour
{
    public BulletInventory inventory;

    private string filePath;


    public static JsonManagerBulletData Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        filePath = Application.persistentDataPath + "/bullets.json";
        LoadData();
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(inventory, true);
        File.WriteAllText(filePath, json);
    }

    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            inventory = JsonUtility.FromJson<BulletInventory>(json);
        }
        else
        {
            inventory = new BulletInventory(); // Dosya yoksa yeni bir envanter oluştur
        }
    }

    public void ClearList()
    {
        inventory.bullets.Clear();
    }
}
