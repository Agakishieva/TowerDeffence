public class Configs
{
    public enum Setting
    {
        Zombie,
        Animals,
        Bugs,
    }

    public static Setting SelectedSetting()
    {
        return Setting.Animals;
    }
}
