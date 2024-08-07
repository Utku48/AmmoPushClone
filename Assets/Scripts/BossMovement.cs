using DG.Tweening;
using System.Collections;
using System.Security.Cryptography;
using TMPro;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

public class BossMovement : MonoBehaviour
{
    [SerializeField] Rigidbody _bossRb;
    [SerializeField] Transform _target;
    [SerializeField] Animator _animator;

    [SerializeField] GameObject _WinEndPanel;
    [SerializeField] GameObject _LoseEndPanel;
    [SerializeField] GameObject _fireButtonsPanel;
    [SerializeField] GameObject _tapToStart;
    [SerializeField] TextMeshProUGUI _percentHealth;


    [SerializeField] float _speed;
    [SerializeField] int _health = 300;
    int firstHealth;


    float distanceToTarget;

    [SerializeField] private HealthBar _healtBar;

    public static BossMovement Instance { get; private set; }


    public enum BossState
    {
        Idle,
        Running,
        FinalAttack
    }
    public BossState state = BossState.Idle;


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

        AnimateBreathing();

        _bossRb = GetComponent<Rigidbody>();
        firstHealth = _health;

        _healtBar.UpdateHealthBar(firstHealth, _health);
    }

    void Update()
    {
        _healtBar.UpdateHealthBar(firstHealth, _health);

        if (Input.touchCount > 0)
        {
            _tapToStart.SetActive(false);
        }

        switch (state)
        {
            case BossState.Idle:

                _bossRb.velocity = Vector3.zero;
                _animator.SetBool("run", false);
                break;

            case BossState.Running:

                if (distanceToTarget > 4f)
                {
                    BossMove();
                }
                else
                {
                    state = BossState.FinalAttack;
                }
                break;

            case BossState.FinalAttack:

                _animator.SetTrigger("finalAttack");
                _bossRb.velocity = Vector3.zero;
                _fireButtonsPanel.SetActive(false);
                StartCoroutine(LoseEndPanel());
                break;

            default:
                break;
        }

        distanceToTarget = Vector3.Distance(_bossRb.position, _target.position);
        if (!_tapToStart.activeInHierarchy && distanceToTarget > 4f && state != BossState.FinalAttack)
        {
            state = BossState.Running;
            _animator.SetBool("run", true);
        }
        else if (distanceToTarget <= 4f && state != BossState.FinalAttack)
        {
            state = BossState.FinalAttack;
        }
    }


    public void BossMove()
    {

        if (_health > 0)
        {
            if (_bossRb != null && _target != null)
            {
                Vector3 direction = (_target.position - _bossRb.position).normalized;
                _bossRb.velocity = direction * _speed;

            }
        }
        else
        {
            int azalan = firstHealth - _health;
            _percentHealth.text = "Damage % " + ((100 * azalan) / 300).ToString();

            _animator.SetTrigger("die");
            _bossRb.velocity = Vector3.zero;

            StartCoroutine(WinEndPanel());

        }
    }




    private void OnTriggerEnter(Collider other)
    {
        if (_health < 0)
        {
            return;
        }

        Bullets bulletsComponent = other.gameObject.GetComponent<Bullets>();

        if (bulletsComponent != null)
        {
            _health -= bulletsComponent.damage;
            _animator.SetTrigger("hitReaction");
        }

    }



    IEnumerator WinEndPanel()
    {
        yield return new WaitForSeconds(2f);

        _WinEndPanel.SetActive(true);
    }

    IEnumerator LoseEndPanel()
    {
        yield return new WaitForSeconds(2f);

        _LoseEndPanel.SetActive(true);
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

    public void DoShakeCamera()
    {
        Camera.main.DOShakePosition(.3f, .15f);
    }

    public void AttackEffect()
    {
        Camera.main.DOShakePosition(.1f, .2f);
    }

    public void ShowFailFanel()
    {

    }
}
