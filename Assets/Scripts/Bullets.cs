using Unity.VisualScripting;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public BulletTypes _bulletTypes;
    public int damage;
    public enum BulletTypes
    {
        Dinamit,
        Bomb,
        C4,
        Varil
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BossMovement>() != null)
        {
            Destroy(this.gameObject);

        }
    }
}
