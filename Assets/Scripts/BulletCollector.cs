using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

public class BulletCollector : MonoBehaviour
{
    public BulletInventory inventory = new BulletInventory();
    public List<Sprite> bulletImages = new List<Sprite>();
    public Transform panelImages;

    public List<TextMeshProUGUI> panelBulletCount = new List<TextMeshProUGUI>();

    private Dictionary<int, Image> imageIndexMap = new Dictionary<int, Image>();

    List<Bullets.BulletTypes> bulletImageIndex = new List<Bullets.BulletTypes>();


    public JsonManagerBulletData dataManager;

    private void Start()
    {
        dataManager.ClearList();


        if (dataManager != null)
        {
            inventory = dataManager.inventory;
        }
    }

    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Bullets bullet = other.GetComponent<Bullets>();
        if (bullet != null)
        {
            string bulletTypeName = bullet.bulletType.ToString();
            CollectBullet(bulletTypeName, 1, bullet.bulletType);

            foreach (Transform item in panelImages)
            {
                int index = item.GetSiblingIndex();

                if (item.GetComponent<Image>().sprite != null)
                {
                    item.GetComponentInChildren<TextMeshProUGUI>().text = inventory.GetBulletCountByType(bulletImageIndex[index]).ToString();
                }

            }

            Destroy(other.gameObject);
            FindFirstEmptyImage(bullet.imageIndex, bullet.bulletType);
        }
    }

    private void CollectBullet(string typeName, int countIncrement, Bullets.BulletTypes type)
    {


        var bulletData = inventory.bullets.Find(b => b.bulletTypeName == typeName);
        if (bulletData != null)
        {
            bulletData.count += countIncrement;
        }
        else
        {
            inventory.bullets.Add(new BulletData(typeName, countIncrement, type));
        }

        if (dataManager != null)
        {
            dataManager.SaveData();
        }
    }

    public void FindFirstEmptyImage(int index, Bullets.BulletTypes type)
    {
        foreach (Transform child in panelImages)
        {
            Image image = child.GetComponent<Image>();
            if (image != null)
            {
                if (image.sprite == null)
                {
                    if (!imageIndexMap.ContainsKey(index))
                    {
                        image.gameObject.transform.DOScale((Vector3.one * 2.25f), .2f).OnComplete(() => image.gameObject.transform.DOScale(new Vector3(1.75f, 1.75f, 1.75f), .3f));
                        image.sprite = bulletImages[index];
                        imageIndexMap[index] = image;

                        if (!bulletImageIndex.Contains(type))
                        {
                            bulletImageIndex.Add(type);
                        }
                        return;
                    }
                    else
                    {
                        Debug.Log("Sprite with index " + index + " already placed.");
                        return;
                    }
                }
            }
        }
    }
}
