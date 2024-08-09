[System.Serializable]
public class MoneyAndUpgradeLevelsData
{
    public int dataMoney;
    public int dataSizeLevel;
    public int dataPowerLevel;
    public int dataTimeLevel;
    public int dataTime;

    public MoneyAndUpgradeLevelsData(int dataMoney, int dataSizeLevel, int dataPowerLevel, int dataTimeLevel, int dataTime)
    {
        this.dataMoney = dataMoney;
        this.dataSizeLevel = dataSizeLevel;
        this.dataPowerLevel = dataPowerLevel;
        this.dataTimeLevel = dataTimeLevel;
        this.dataTime = dataTime;
    }
}
