using UnityEngine;

public class Bullets : MonoBehaviour
{
    public BulletTypes bulletType; // Enum olarak tanımlanır
    public int damage;
    public int imageIndex;
    public enum BulletTypes
    {
        Dinamit,
        Bomb,
        C4,
        Varil
    }


}
