using System.IO;
using UnityEngine;

public class JsonManager : MonoBehaviour
{
    public MoneyAndUpgradeLevelsData moneyAndUpgradeLevelsData;

    private void Awake()
    {
        LoadDatas();
    }

    private void LoadDatas()
    {
        if (File.Exists(Application.persistentDataPath + "/MoneyLevelData.json"))
        {
            JsonLoad();
        }
        else
        {
            moneyAndUpgradeLevelsData = new MoneyAndUpgradeLevelsData(0, 0, 0, 0,23);
        }
    }

    [ContextMenu("Save")]
    public void JsonSave()
    {
        string jsonString = JsonUtility.ToJson(moneyAndUpgradeLevelsData, true);
        Directory.CreateDirectory(Application.persistentDataPath );
        File.WriteAllText(Application.persistentDataPath + "/MoneyLevelData.json", jsonString);
    }

    public void JsonLoad()
    {
        string loadJson = File.ReadAllText(Application.persistentDataPath + "/MoneyLevelData.json");
        moneyAndUpgradeLevelsData = JsonUtility.FromJson<MoneyAndUpgradeLevelsData>(loadJson);
    }
}
