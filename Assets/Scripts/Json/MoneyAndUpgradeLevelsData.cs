[System.Serializable]
public class MoneyAndUpgradeLevelsData
{
    public int dataMoney;
    public int dataSizeLevel;
    public int dataPowerLevel;
    public int dataTimeLevel;

    public MoneyAndUpgradeLevelsData(int dataMoney, int dataSizeLevel, int dataPowerLevel, int dataTimeLevel)
    {
        this.dataMoney = dataMoney;
        this.dataSizeLevel = dataSizeLevel;
        this.dataPowerLevel = dataPowerLevel;
        this.dataTimeLevel = dataTimeLevel;
    }
}
