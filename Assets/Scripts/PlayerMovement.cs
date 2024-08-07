using DG.Tweening;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;
    [SerializeField] FloatingJoystick _floatingJs;
    [SerializeField] private float _speed;

    [SerializeField] private Animator _anim;
    [SerializeField] private Animator _pusherAnim;
    [SerializeField] private Transform _playerTransform;


    [SerializeField] private GameObject _pusher1;
    [SerializeField] private GameObject _pusher2;
    [SerializeField] private GameObject _plane;
    [SerializeField] private Transform _planePoint;



    private float _horizontal;
    private float _vertical;

    private bool _joystickHeld;

    public static PlayerMovement Instance { get; private set; }
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


    private void Start()
    {
        _joystickHeld = false;

    }

    private void Update()
    {
        GetMovementInputs();
        CheckJoystickRelease();
        _pusher2.transform.position = _planePoint.position;
    }

    private void FixedUpdate()
    {
        SetMovement();
        SetRotation();
    }

    private void SetMovement()
    {
        _rb.velocity = new Vector3(_horizontal, _rb.velocity.y, _vertical) * _speed * Time.deltaTime;
    }

    private void SetRotation()
    {
        if (_horizontal != 0 || _vertical != 0)
        {
            _playerTransform.rotation = Quaternion.LookRotation(_rb.velocity);
            _anim.SetBool("pushing", true);
        }
        else
        {
            _anim.SetBool("pushing", false);
        }
    }

    private void GetMovementInputs()
    {
        _horizontal = _floatingJs.Horizontal;
        _vertical = _floatingJs.Vertical;
    }

    private void CheckJoystickRelease()
    {
        if (_horizontal == 0 && _vertical == 0 && _joystickHeld)
        {

            OnJoystickRelease();
            _pusherAnim.SetBool("push", false);
            _joystickHeld = false;
        }
        else if (_horizontal != 0 || _vertical != 0)
        {
            _pusherAnim.SetBool("push", true);
            _joystickHeld = true;
        }

    }

    private void OnJoystickRelease()
    {
        Vector3 characterStartPos = transform.position;

        _plane.transform.DOScale(new Vector3(_plane.transform.localScale.x, _plane.transform.localScale.y, 1f), .25f).OnUpdate(() =>
        {
            //Physics.OverlapBox
            //box içerisindeki player'a en yakın objenin pusherr2'ye distance'sini al 
            //o mesafe kadar player'ı geri at

            //transform.position=characterStartPos-mesafe   

        }).OnComplete(() =>
         _plane.transform.DOScale(new Vector3(_plane.transform.localScale.x, _plane.transform.localScale.y, 0f), .25f)
        );

    }
}
