using UnityEngine;
using System.IO;

public class JsonManagerBulletData : MonoBehaviour
{
    public BulletInventory inventory;

    private string filePath;

    private void Awake()
    {
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
