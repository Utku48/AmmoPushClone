using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LvlManager : MonoBehaviour
{
    public float money;


    public List<Button> buttons = new List<Button>();
    [SerializeField] private List<Sprite> greyUpgrades = new List<Sprite>();
    [SerializeField] private List<Sprite> greenUpgrades = new List<Sprite>();

    public List<int> upgradePrices = new List<int>();
    [SerializeField] private List<int> upgradeLevels = new List<int>();

    [SerializeField] private List<TextMeshProUGUI> upgradePricesText = new List<TextMeshProUGUI>();
    [SerializeField] private List<TextMeshProUGUI> upgradeLvlText = new List<TextMeshProUGUI>();

    [SerializeField] private ParticleSystem _sizeParticle;
    [SerializeField] private GameObject _pusher;



    public JsonManager jsonManager;
    private MoneyAndUpgradeLevelsData moneyAndUpgradeLevelsData;

    private void Start()
    {
        if (jsonManager != null)
        {
            moneyAndUpgradeLevelsData = jsonManager.moneyAndUpgradeLevelsData;
            money = moneyAndUpgradeLevelsData.dataMoney;
            TimerHandler.Instance.duration = jsonManager.moneyAndUpgradeLevelsData.dataTime;

            PlayerMovement.Instance._speed = jsonManager.moneyAndUpgradeLevelsData.speed;

            Vector3 newScale = _pusher.transform.localScale;
            newScale.x = jsonManager.moneyAndUpgradeLevelsData.pusherScale;
            newScale.y = 1f;
            newScale.z = 1f;
            _pusher.transform.localScale = newScale;

            var dataLevels = new int[]
            {
                moneyAndUpgradeLevelsData.dataSizeLevel,
                moneyAndUpgradeLevelsData.dataPowerLevel,
                moneyAndUpgradeLevelsData.dataTimeLevel
            };

            for (int i = 0; i < dataLevels.Length; i++)
            {
                upgradeLevels[i] = dataLevels[i];
                upgradeLvlText[i].text = "Lvl " + dataLevels[i].ToString();
            }
        }

        UpdateButtonImage();
    }

    private void Update()
    {
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
        if (money >= upgradePrices[0])
        {
            _pusher.transform.DOScale(new Vector3(_pusher.transform.localScale.x + .05f, _pusher.transform.localScale.y, _pusher.transform.localScale.z), .2f);
            _sizeParticle.Play();
        }
        OnUpgradeButton(0, 65);

        SaveData();
    }

    public void OnPowerButton()
    {
        OnUpgradeButton(1, 75);
        if (money > upgradePrices[1])
        {
            PlayerMovement.Instance._speed += .7f;
        }

        SaveData();
    }

    public void OnTimeButton()
    {
        OnUpgradeButton(2, 55);
        if (money >= upgradePrices[2])
        {
            TimerHandler.Instance.duration += 2;
            TimerHandler.Instance.timer_Text.text = TimerHandler.Instance.duration.ToString();

        }
        SaveData();
    }

    private void SaveData()
    {
        if (jsonManager != null)
        {
            moneyAndUpgradeLevelsData.dataMoney = (int)money;
            moneyAndUpgradeLevelsData.dataSizeLevel = upgradeLevels[0];
            moneyAndUpgradeLevelsData.dataPowerLevel = upgradeLevels[1];
            moneyAndUpgradeLevelsData.dataTimeLevel = upgradeLevels[2];
            moneyAndUpgradeLevelsData.dataTime = TimerHandler.Instance.duration;
            moneyAndUpgradeLevelsData.speed = PlayerMovement.Instance._speed;
            moneyAndUpgradeLevelsData.pusherScale = _pusher.transform.localScale.x;
            jsonManager.JsonSave();
        }
    }

    public void MoneyLevelIncrease()
    {
        money += 50;
        SaveData();
    }
}
