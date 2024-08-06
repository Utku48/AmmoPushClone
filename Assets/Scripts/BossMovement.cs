using System.Collections;
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
    [SerializeField] TextMeshProUGUI _percentHealth;



    [SerializeField] float _speed;
    [SerializeField] int _health = 300;
    int firstHealth;


    [SerializeField] private HealthBar _healtBar;

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
        firstHealth = _health;

        _healtBar.UpdateHealthBar(firstHealth, _health);
    }

    void Update()
    {
        _healtBar.UpdateHealthBar(firstHealth, _health);



        BossMove();
    }

    public void BossMove()
    {

        if (_health > 0)
        {
            if (_bossRb != null && _target != null)
            {
                float distanceToTarget = Vector3.Distance(_bossRb.position, _target.position);
                if (distanceToTarget > 3.5f)
                {
                    _animator.SetBool("run", true);
                    Vector3 direction = (_target.position - _bossRb.position).normalized;
                    _bossRb.velocity = direction * _speed;
                }
                else
                {
                    _bossRb.velocity = Vector3.zero;
                    _animator.SetBool("run", false);
                    _animator.SetBool("hit", true);
                    _fireButtonsPanel.SetActive(false);

                    int azalan = firstHealth - _health;
                    _percentHealth.text = "Damage % " + ((100 * azalan) / 300).ToString();

                    StartCoroutine(LoseEndPanel());


                }
            }
        }
        else
        {
            _animator.SetBool("die", true);
            _animator.SetBool("run", false);
            _bossRb.velocity = Vector3.zero;

            StartCoroutine(WinEndPanel());



        }
    }


    private void OnTriggerEnter(Collider other)
    {

        Bullets bulletsComponent = other.gameObject.GetComponent<Bullets>();

        if (bulletsComponent != null)
        {

            Bullets.BulletTypes bulletType = bulletsComponent._bulletTypes;

            switch (bulletType)
            {
                case Bullets.BulletTypes.Dinamit:

                    _health -= 25;
                    Debug.Log("Hit Dinamit");

                    break;
                case Bullets.BulletTypes.Bomb:

                    _health -= 50;

                    Debug.Log("Hit Bomb");
                    break;
                case Bullets.BulletTypes.C4:

                    _health -= 75;

                    Debug.Log("Hit C4");
                    break;
                case Bullets.BulletTypes.Varil:

                    _health -= 100;

                    Debug.Log("Hit Varil");
                    break;
                default:

                    Debug.Log("Unknown bullet type");
                    break;
            }
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

}
