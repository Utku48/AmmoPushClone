using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _bossRb;
    [SerializeField] private Transform _target;
    [SerializeField] private Animator _animator;

    [SerializeField] private List<GameObject> _uiPanels;
    [SerializeField] private List<TextMeshProUGUI> _uiTexts;

    [SerializeField] private float _speed;
    [SerializeField] private int _health = 300;
    [SerializeField] private int _damageMoney;

    private int firstHealth;
    private int lvlID = 1;
    private float distanceToTarget;

    [SerializeField] private HealthBar _healtBar;

    public JsonManager jsonManager;
    private MoneyAndUpgradeLevelsData moneyAndUpgradeLevelsData;

    private bool isDead = false;

    public enum BossState
    {
        Idle,
        Running,
        Die,
        FinalAttack
    }

    public BossState state = BossState.Idle;

    public static BossMovement Instance { get; private set; }

    private void Awake()
    {
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
        moneyAndUpgradeLevelsData = jsonManager.moneyAndUpgradeLevelsData;
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
            _uiPanels[3].SetActive(false); // TapToStart
        }

        HandleStateTransition();
        CalculateDamageAndMoney();
    }

    private void HandleStateTransition()
    {
        switch (state)
        {
            case BossState.Idle:
                EnterIdleState();
                break;

            case BossState.Running:
                EnterRunningState();
                break;

            case BossState.Die:
                EnterDieState();
                break;

            case BossState.FinalAttack:
                EnterFinalAttackState();
                break;

            default:
                break;
        }

        distanceToTarget = Vector3.Distance(_bossRb.position, _target.position);

        if (!_uiPanels[3].activeInHierarchy && distanceToTarget > 4f && state != BossState.FinalAttack && state != BossState.Die)
        {
            state = BossState.Running;
            _animator.SetBool("run", true);
        }
        else if (distanceToTarget <= 4f && state != BossState.FinalAttack)
        {
            state = BossState.FinalAttack;
        }
    }

    private void EnterIdleState()
    {
        _bossRb.velocity = Vector3.zero;
        _animator.SetBool("run", false);
    }

    private void EnterRunningState()
    {
        if (distanceToTarget > 4f)
        {
            BossMove();
        }
        else
        {
            state = BossState.FinalAttack;
        }
    }

    private void EnterDieState()
    {
        if (!isDead)
        {
            isDead = true;
            _animator.SetTrigger("die");
            Debug.Log("Die animasyonu tetiklendi");
            _bossRb.velocity = Vector3.zero;
            _animator.SetBool("run", false);
        }
    }

    private void EnterFinalAttackState()
    {
        _animator.SetTrigger("finalAttack");
        _bossRb.velocity = Vector3.zero;
        _uiPanels[2].SetActive(false); // FireButtonsPanel
        StartCoroutine(LoseEndPanel());
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
            state = BossState.Die;
            StartCoroutine(WinEndPanel());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_health <= 0)
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
        _uiPanels[0].SetActive(true); // WinEndPanel
    }

    IEnumerator LoseEndPanel()
    {
        yield return new WaitForSeconds(2f);
        _uiPanels[1].SetActive(true); // LoseEndPanel
    }

    private void AnimateBreathing()
    {
        _uiPanels[3].transform.DOScale(1.1f, 1f) // TapToStart
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                _uiPanels[3].transform.DOScale(.8f, 1f)
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

    private void CalculateDamageAndMoney()
    {
        int damageTaken = firstHealth - _health;
        _uiTexts[0].text = "Damage % " + ((100 * damageTaken) / firstHealth).ToString(); // PercentHealth

        _damageMoney = (((100 * damageTaken) / firstHealth)) * 5;
        _uiTexts[2].text = "+ " + _damageMoney.ToString(); // DamageMoneyText
    }

    public void NextButton()
    {
        moneyAndUpgradeLevelsData.dataMoney += _damageMoney;
        jsonManager.JsonSave();
        SceneManager.LoadScene(0);
    }
}
