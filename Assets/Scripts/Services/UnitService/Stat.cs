public class Stat
{
    const int maxLevel = 3;

    public int baseValue { get; private set; }
    public int level { get; private set; }

    public int value => baseValue * level;

    public Stat(int baseValue)
    {
        this.baseValue = baseValue;
        this.level = 1;
    }

    public bool LevelUp(bool demoMode = false)
    {
        if (demoMode)
        {
            level = 3;
            return true;
        }

        if (level >= maxLevel)
        {
            return false;
        }

        level++;
        return true;
    }
}
