using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class BulletData
{
    public string bulletTypeName;
    public int count;
    public Bullets.BulletTypes bulletType;


    public BulletData(string bulletTypeName, int count, Bullets.BulletTypes bulletType)
    {
        this.bulletTypeName = bulletTypeName;
        this.count = count;
        this.bulletType = bulletType;
    }
}

[System.Serializable]
public class BulletInventory
{
    public List<BulletData> bullets = new List<BulletData>();

    public int GetBulletCountByType(Bullets.BulletTypes type)
    {
        return bullets.First(x => x.bulletType == type).count;
    }
}
