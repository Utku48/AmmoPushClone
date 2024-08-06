using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooterAnim : MonoBehaviour
{

    [SerializeField] GameObject _top;
    SkinnedMeshRenderer skinnedMeshRenderer;
    void Start()
    {
        skinnedMeshRenderer = _top.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartAnimation() { 
    }
}
