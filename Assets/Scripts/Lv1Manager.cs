using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lv1Manager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyCount;
    [SerializeField] private GameObject _tapToStart;
    [SerializeField] private GameObject panel;

    public float money;

    [SerializeField] private List<Button> buttons = new List<Button>();
    [SerializeField] private List<Sprite> greyUpgrades = new List<Sprite>();
    [SerializeField] private List<Sprite> greenUpgrades = new List<Sprite>();

    [SerializeField] private List<int> upgradePrices = new List<int>();
    [SerializeField] private List<int> upgradeLevels = new List<int>();

    [SerializeField] private List<TextMeshProUGUI> upgradePricesText = new List<TextMeshProUGUI>();
    [SerializeField] private List<TextMeshProUGUI> upgradeLvlText = new List<TextMeshProUGUI>();

    public JsonManager jsonManager;
    private MoneyAndUpgradeLevelsData moneyAndUpgradeLevelsData;

    private void Start()
    {

        if (jsonManager != null)
        {
            moneyAndUpgradeLevelsData = jsonManager.moneyAndUpgradeLevelsData;
            money = moneyAndUpgradeLevelsData.dataMoney;
            upgradeLevels[0] = moneyAndUpgradeLevelsData.dataSizeLevel;
            upgradeLevels[1] = moneyAndUpgradeLevelsData.dataPowerLevel;
            upgradeLevels[2] = moneyAndUpgradeLevelsData.dataTimeLevel;

            upgradeLvlText[0].text = "Lvl " + moneyAndUpgradeLevelsData.dataSizeLevel.ToString();
            upgradeLvlText[1].text = "Lvl " + moneyAndUpgradeLevelsData.dataPowerLevel.ToString();
            upgradeLvlText[2].text = "Lvl " + moneyAndUpgradeLevelsData.dataTimeLevel.ToString();
        }

        UpdateButtonImage();
        AnimateBreathing();
    }

    private void Update()
    {
        _moneyCount.text = money + "$".ToString();

        for (int i = 0; i < upgradePrices.Count; i++)
        {
            upgradePricesText[i].text = upgradePrices[i].ToString();
        }
    }

    private void UpdateButtonImage()
    {
        for (int i = 0; i < upgradePrices.Count; i++)
        {
            if (money >= upgradePrices[i])
            {
                buttons[i].image.sprite = greenUpgrades[i];
            }
            else
            {
                buttons[i].image.sprite = greyUpgrades[i];
            }

            upgradePricesText[i].text = upgradePrices[i].ToString();
        }
    }

    public void OnUpgradeButton(int index, int priceIncrease)
    {
        if (money >= upgradePrices[index])
        {
            money -= upgradePrices[index];
            upgradePrices[index] += priceIncrease;
            upgradeLevels[index]++;
            upgradeLvlText[index].text = "Lvl " + upgradeLevels[index].ToString();

            SaveData();
            UpdateButtonImage();
        }
    }

    public void OnSizeButton()
    {
        OnUpgradeButton(0, 25);
    }

    public void OnPowerButton()
    {
        OnUpgradeButton(1, 35);
    }

    public void OnTimeButton()
    {
        OnUpgradeButton(2, 15);
    }

    private void AnimateBreathing()
    {
        _tapToStart.transform.DOScale(1.1f, 1f)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                _tapToStart.transform.DOScale(.8f, 1f)
                    .SetEase(Ease.InOutSine)
                    .OnComplete(() =>
                    {
                        AnimateBreathing();
                    });
            });
    }

    public void OnPointerClick()
    {
        TimerHandler.Instance.StartTimer();
        _tapToStart.SetActive(false);

        foreach (var item in buttons)
        {
            item.gameObject.SetActive(false);
        }
        panel.SetActive(false);
    }

    private void SaveData()
    {
        if (jsonManager != null)
        {
            moneyAndUpgradeLevelsData.dataMoney = (int)money;
            moneyAndUpgradeLevelsData.dataSizeLevel = upgradeLevels[0];
            moneyAndUpgradeLevelsData.dataPowerLevel = upgradeLevels[1];
            moneyAndUpgradeLevelsData.dataTimeLevel = upgradeLevels[2];

            jsonManager.JsonSave();
        }
    }

    public void MoneyLevelIncrease()
    {
        money++;
        SaveData();
    }
}
