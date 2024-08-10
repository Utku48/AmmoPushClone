
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueScript : MonoBehaviour
{
    public JsonManager jsonManager;
    private MoneyAndUpgradeLevelsData moneyAndUpgradeLevelsData;

    void Start()
    {

        int lvlID = jsonManager.moneyAndUpgradeLevelsData.datalevelID;
        SceneManager.LoadScene("Level" + lvlID);
    }

    void Update()
    {

    }
}
