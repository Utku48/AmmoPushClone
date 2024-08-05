using Unity.VisualScripting;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] Rigidbody _bossRb;
    [SerializeField] float _speed;
    [SerializeField] Transform _target;
    [SerializeField] int _health;
    [SerializeField] Animator _animator;

    public static BossMovement Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        _bossRb = GetComponent<Rigidbody>();
    }

    void Update()
    {

    }

    public void BossMove()
    {
        if (_bossRb != null && _target != null)
        {
            _animator.SetBool("run", true);
            Vector3 direction = (_target.position - _bossRb.position).normalized;
            _bossRb.velocity = direction * _speed;
        }
    }
}
