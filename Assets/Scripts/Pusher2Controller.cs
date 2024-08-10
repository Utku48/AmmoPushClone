using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher2Controller : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    public float forceAmount = 50f;
    void Start()
    {

    }


    void Update()
    {

    }



    private void OnTriggerEnter(Collider collision)
    {

        Vector3 oppositeDirection = collision.transform.position - PlayerMovement.Instance.transform.position;
        oppositeDirection.Normalize();
        oppositeDirection.y = 0f;

        if (collision.gameObject.TryGetComponent<Bullets>(out Bullets b))
        {
            b.GetComponent<Rigidbody>().AddForce(forceAmount * oppositeDirection, ForceMode.Impulse);
        }
    }


}