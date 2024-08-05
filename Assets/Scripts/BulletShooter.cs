using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] private GameObject _top;

    public GameObject _dinamitPrefab;
    public GameObject _bombPrefab;
    public GameObject _c4Prefab;
    public GameObject _varilPrefab;

    public TextMeshProUGUI _dinamitCountText;
    public TextMeshProUGUI _bombCountText;
    public TextMeshProUGUI _c4CountText;
    public TextMeshProUGUI _varilCountText;

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
        _dinamitCountText.text = _dinamitCount.ToString();
        _bombCountText.text = _dinamitCount.ToString();
        _c4CountText.text = _dinamitCount.ToString();
        _varilCountText.text = _dinamitCount.ToString();
    }

    public void DinamitButton()
    {
        if (_dinamitCount.ConvertTo<int>() > 0)
        {
            FireProjectile(_dinamitPrefab);
        }
    }

    public void BombButton()
    {
        if (_dinamitCount.ConvertTo<int>() > 0)
        {
            FireProjectile(_bombPrefab);
        }
    }

    public void C4Button()
    {
        if (_dinamitCount.ConvertTo<int>() > 0)
        {
            FireProjectile(_c4Prefab);
        }
    }
    public void VarilButton()
    {
        if (_dinamitCount.ConvertTo<int>() > 0)
        {
            FireProjectile(_varilPrefab);
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
                int blendShapeIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("Key 1");

                if (blendShapeIndex != -1)
                {

                    skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, 100f);

                    StartCoroutine(ResetBlendShapeWeightAfterDelay(skinnedMeshRenderer, blendShapeIndex, .1f));
                }
                else
                {
                    Debug.LogError("BlendShape 'key3' bulunamadı.");
                }
            }
            if (rb != null)
            {
                rb.velocity = direction * bulletSpeed;
            }
        }
    }

    private IEnumerator ResetBlendShapeWeightAfterDelay(SkinnedMeshRenderer skinnedMeshRenderer, int blendShapeIndex, float delay)
    {
        yield return new WaitForSeconds(delay);

        // BlendShape ağırlığını 0 yap
        skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, 0f);
    }
}
