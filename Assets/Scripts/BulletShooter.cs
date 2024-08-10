using Newtonsoft.Json;
using System.Collections;
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
    public List<Transform> instantiatePositions = new List<Transform>();

    public float bulletSpeed = 20f;
    public Transform target;

    public int levelID;
    private SkinnedMeshRenderer skinnedMeshRenderer;

    private void Start()
    {

        if (dataManager != null)
        {
            dataManager.LoadData();
            UpdateBulletCounts();
        }
        else
        {
            Debug.LogError("dataManager referansı atanmış değil.");
        }

        StartCoroutine(SpawnBullets());

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
        _bulletCounts.Clear();


        _bulletCounts.AddRange(new int[6]);

        foreach (var bulletData in dataManager.inventory.bullets)
        {
            int index = -1;
            switch (bulletData.bulletTypeName)
            {
                case "YBullet":
                    index = 0;
                    break;
                case "Marshmallow":
                    index = 1;
                    break;
                case "Dinamit":
                    index = 2;
                    break;
                case "C4":
                    index = 3;
                    break;
                case "Bomb":
                    index = 4;
                    break;
                case "Varil":
                    index = 5;
                    break;
            }

            if (index != -1 && index < _bulletCounts.Count)
            {
                _bulletCounts[index] = bulletData.count;
            }
        }
    }
    public void YBulletButton()
    {
        if (_bulletCounts.Count > 0 && _bulletCounts[0] > 0)
        {
            FireProjectile(bullets[0]);
            _bulletCounts[0]--;
            dataManager.SaveData();
        }
    }
    public void MarshmallowButton()
    {
        if (_bulletCounts.Count > 0 && _bulletCounts[1] > 0)
        {
            FireProjectile(bullets[1]);
            _bulletCounts[1]--;
            dataManager.SaveData();
        }
    }
    public void DinamitButton()
    {
        if (_bulletCounts.Count > 0 && _bulletCounts[2] > 0)
        {
            FireProjectile(bullets[2]);
            _bulletCounts[2]--;
            dataManager.SaveData();
        }
    }

    public void C4Button()
    {
        if (_bulletCounts.Count > 3 && _bulletCounts[3] > 0)
        {
            FireProjectile(bullets[3]);
            _bulletCounts[3]--;
            dataManager.SaveData();
        }
    }

    public void BombButton()
    {
        if (_bulletCounts.Count > 4 && _bulletCounts[4] > 0)
        {
            FireProjectile(bullets[4]);
            _bulletCounts[4]--;
            dataManager.SaveData();
        }
    }

    public void VarilButton()
    {
        if (_bulletCounts.Count > 5 && _bulletCounts[5] > 0)
        {
            FireProjectile(bullets[5]);
            _bulletCounts[5]--;
            dataManager.SaveData();
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


    private IEnumerator SpawnBullets()
    {
        int positionIndex = 0;

        for (int j = 0; j < bullets.Count; j++)
        {
            for (int i = 0; i < _bulletCounts[j]; i++)
            {

                Instantiate(bullets[j], instantiatePositions[positionIndex].position, instantiatePositions[positionIndex].rotation);


                positionIndex = (positionIndex + 1) % instantiatePositions.Count;


                yield return new WaitForSeconds(.001f);
            }
        }


    }
}
