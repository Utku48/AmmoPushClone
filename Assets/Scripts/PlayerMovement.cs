using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;
    [SerializeField] FloatingJoystick _floatingJs;
    public float _speed;

    [SerializeField] private Animator _anim;
    [SerializeField] private Animator _pusherAnim;
    [SerializeField] private Transform _playerTransform;



    [SerializeField] private GameObject _pusher2;
    [SerializeField] private GameObject _plane;
    [SerializeField] private Transform _planePoint;

    [SerializeField] private BoxCollider _maxCollider;
    public Transform childTransform;

    
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
        _rb.velocity = new Vector3(_horizontal, _rb.velocity.y, _vertical) * _speed;
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
        BoxCollider pusher2Collider = _pusher2.GetComponent<BoxCollider>();
        pusher2Collider.enabled = true;

        List<Transform> allObjectsInPushArea = new List<Transform>();
        foreach (var item in Physics.OverlapBox(_maxCollider.bounds.center, _maxCollider.bounds.size))
        {
            Debug.Log(item.gameObject.name);
            if (item.TryGetComponent<Bullets>(out Bullets b))
            {
                allObjectsInPushArea.Add(item.transform);
            }
        }

        _plane.transform.DOScale(new Vector3(_plane.transform.localScale.x, _plane.transform.localScale.y, 1f), .25f).OnUpdate(() =>
        {
            //Physics.OverlapBox
            Transform nearestObject = GetNearestObjectToThePlayer(allObjectsInPushArea.ToArray());
            if (nearestObject != null)
            {
                if (Vector3.Distance(_pusher2.transform.position, transform.position) + 0.5f > Vector3.Distance(nearestObject.transform.position, transform.position))
                {
                    Debug.Log("Player Going back");
                    transform.position -= transform.forward * Time.deltaTime * 15f;
                }

            }
            //box içerisindeki player'a en yakın objenin pusherr2'ye distance'sini al 
            //o mesafe kadar player'ı geri at

            //transform.position=characterStartPos-mesafe   

        }).OnComplete(() =>
        {
            _plane.transform.DOScale(new Vector3(_plane.transform.localScale.x, _plane.transform.localScale.y, 0f), .25f);
            pusher2Collider.enabled = false;
        });

    }

    public Transform GetNearestObjectToThePlayer(Transform[] objects)
    {
        float minDistance = 9999f;
        Transform nearest = null;
        foreach (var item in objects)
        {
            float pos = Vector3.Distance(transform.position, item.position);
            if (pos < minDistance)
            {
                nearest = item;
                minDistance = pos;
            }
        }
        return nearest;
    }
}
