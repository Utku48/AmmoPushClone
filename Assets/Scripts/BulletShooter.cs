﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] private GameObject _top;
    public JsonManagerBulletData dataManager;

    public List<GameObject> bullets = new List<GameObject>();
    public List<TextMeshProUGUI> _counts = new List<TextMeshProUGUI>();
    public List<int> _bulletCounts = new List<int>();

    public Transform _firingPoint;
    public float bulletSpeed = 20f;
    public Transform target;

    private SkinnedMeshRenderer skinnedMeshRenderer;

    private void Start()
    {
        // Verileri yükle
        if (dataManager != null)
        {
            dataManager.LoadData(); // Verileri yükle
            UpdateBulletCounts();
        }
        else
        {
            Debug.LogError("dataManager referansı atanmış değil.");
        }

        skinnedMeshRenderer = _top.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
    }

    private void Update()
    {
        for (int i = 0; i < _counts.Count; i++)
        {
            if (i < _bulletCounts.Count)
            {
                _counts[i].text = _bulletCounts[i].ToString();
            }
        }
    }

    private void UpdateBulletCounts()
    {
        _bulletCounts.Clear(); // Önceki sayıları temizle

        // Liste uzunluğunu ayarla
        _bulletCounts.AddRange(new int[4]);

        foreach (var bulletData in dataManager.inventory.bullets)
        {
            int index = -1;
            switch (bulletData.bulletTypeName)
            {
                case "Dinamit":
                    index = 0;
                    break;
                case "Bomb":
                    index = 1;
                    break;
                case "C4":
                    index = 2;
                    break;
                case "Varil":
                    index = 3;
                    break;
            }

            if (index != -1 && index < _bulletCounts.Count)
            {
                _bulletCounts[index] = bulletData.count;
            }
        }
    }

    public void DinamitButton()
    {
        if (_bulletCounts.Count > 0 && _bulletCounts[0] > 0)
        {
            FireProjectile(bullets[0]);
            _bulletCounts[0]--;
            dataManager.SaveData(); // Veriyi kaydet
        }
    }

    public void C4Button()
    {
        if (_bulletCounts.Count > 1 && _bulletCounts[1] > 0)
        {
            FireProjectile(bullets[1]);
            _bulletCounts[1]--;
            dataManager.SaveData(); // Veriyi kaydet
        }
    }

    public void BombButton()
    {
        if (_bulletCounts.Count > 2 && _bulletCounts[2] > 0)
        {
            FireProjectile(bullets[2]);
            _bulletCounts[2]--;
            dataManager.SaveData(); // Veriyi kaydet
        }
    }

    public void VarilButton()
    {
        if (_bulletCounts.Count > 3 && _bulletCounts[3] > 0)
        {
            FireProjectile(bullets[3]);
            _bulletCounts[3]--;
            dataManager.SaveData(); // Veriyi kaydet
        }
    }

    private void FireProjectile(GameObject projectilePrefab)
    {
        if (projectilePrefab != null && _firingPoint != null && target != null)
        {
            GameObject bullet = Instantiate(projectilePrefab, _firingPoint.position, _firingPoint.rotation);
            Vector3 direction = (target.position - _firingPoint.position);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            if (skinnedMeshRenderer != null)
            {
                int key1 = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("Key 1");
                int key2 = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("Key 2");
                int key3 = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("Key 3");

                if (key1 != -1 && key2 != -1 && key3 != -1)
                {
                    StartCoroutine(SetBlendShapeWeightsSequentially(skinnedMeshRenderer, key1, key2, key3, 0.075f));
                }
                else
                {
                    Debug.LogError("One or more BlendShapes not found.");
                }
            }

            if (rb != null)
            {
                rb.velocity = direction * bulletSpeed;
            }
        }
    }

    private IEnumerator SetBlendShapeWeightsSequentially(SkinnedMeshRenderer skinnedMeshRenderer, int key1, int key2, int key3, float delay)
    {
        skinnedMeshRenderer.SetBlendShapeWeight(key1, 100f);
        yield return new WaitForSeconds(delay);
        skinnedMeshRenderer.SetBlendShapeWeight(key1, 0f);

        skinnedMeshRenderer.SetBlendShapeWeight(key2, 100f);
        yield return new WaitForSeconds(delay);
        skinnedMeshRenderer.SetBlendShapeWeight(key2, 0f);

        skinnedMeshRenderer.SetBlendShapeWeight(key3, 100f);
        yield return new WaitForSeconds(delay);
        skinnedMeshRenderer.SetBlendShapeWeight(key3, 0f);
    }
}
