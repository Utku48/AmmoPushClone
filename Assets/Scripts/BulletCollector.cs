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
            CollectBullet(bulletTypeName, 1);
            Destroy(other.gameObject);
            FindFirstEmptyImage(bullet.imageIndex);
        }
    }

    private void CollectBullet(string typeName, int countIncrement)
    {
        var bulletData = inventory.bullets.Find(b => b.bulletTypeName == typeName);
        if (bulletData != null)
        {
            bulletData.count += countIncrement;
        }
        else
        {
            inventory.bullets.Add(new BulletData(typeName, countIncrement));
        }

        if (dataManager != null)
        {
            dataManager.SaveData();
        }
    }

    public void FindFirstEmptyImage(int index)
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
                        Debug.Log(image.name);
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
