[System.Serializable]
public class MoneyAndUpgradeLevelsData
{
    public int dataMoney;
    public int dataSizeLevel;
    public int dataPowerLevel;
    public int dataTimeLevel;
    public int dataTime;
    public float speed = 7;
    public float pusherScale = 1f;
    public int datalevelID;

    public MoneyAndUpgradeLevelsData(float speed, float pusherScale)
    {
        this.speed = speed;
        this.pusherScale = pusherScale;
    }

    public MoneyAndUpgradeLevelsData(int dataMoney, int dataSizeLevel, int dataPowerLevel, int dataTimeLevel, int dataTime, int dataevelID)
    {
        this.dataMoney = dataMoney;
        this.dataSizeLevel = dataSizeLevel;
        this.dataPowerLevel = dataPowerLevel;
        this.dataTimeLevel = dataTimeLevel;
        this.dataTime = dataTime;
        this.datalevelID = dataevelID;
    }
}
