using System.Collections.Generic;

[System.Serializable]
public class BulletData
{
    public string bulletTypeName;
    public int count;

    public BulletData(string typeName, int initialCount)
    {
        bulletTypeName = typeName;
        count = initialCount;
    }

    public BulletData() { }
}

[System.Serializable]
public class BulletInventory
{
    public List<BulletData> bullets = new List<BulletData>();
}
