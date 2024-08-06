using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] private GameObject _top;

    public List<GameObject> bullets = new List<GameObject>();
    public List<TextMeshProUGUI> _counts = new List<TextMeshProUGUI>();

    public int _dinamitCount;
    public int _bombCount;
    public int _c4Count;
    public int _varilCount;

    public Transform _firingPoint;
    public float bulletSpeed = 20f;
    public Transform target;

    SkinnedMeshRenderer skinnedMeshRenderer;



    private void Start()
    {
        skinnedMeshRenderer = _top.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();

    }

    private void Update()
    {
        _counts[0].text = _dinamitCount.ToString();
        _counts[1].text = _bombCount.ToString();
        _counts[2].text = _c4Count.ToString();
        _counts[3].text = _varilCount.ToString();
    }

    public void DinamitButton()
    {
        if (_dinamitCount.ConvertTo<int>() > 0)
        {
            FireProjectile(bullets[0]);
            _dinamitCount--;
        
        }
    }

    public void BombButton()
    {
        if (_bombCount.ConvertTo<int>() > 0)
        {
            FireProjectile(bullets[1]);
            _bombCount--;
        }
    }

    public void C4Button()
    {
        if (_c4Count.ConvertTo<int>() > 0)
        {
            FireProjectile(bullets[2]);
            _c4Count--;
        }
    }
    public void VarilButton()
    {
        if (_varilCount.ConvertTo<int>() > 0)
        {
            FireProjectile(bullets[3]);
            _varilCount--;
        }
    }



    private void FireProjectile(GameObject projectilePrefab)
    {
        if (projectilePrefab != null && _firingPoint != null && target != null)
        {
            BossMovement.Instance.BossMove();

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
                    // Start the coroutine to set blend shape weights sequentially
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
