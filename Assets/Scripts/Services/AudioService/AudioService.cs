public class AudioService
{
    // UI
    private static UISfxStore uiStore;
    public static UISounds ui = new UISounds();

    // Units and Towers
    private static PlaySfxStore playStore;
    public static UnitSounds unit = new UnitSounds();
    public static TowerSounds tower = new TowerSounds();

    public static void SetUIStore(UISfxStore uiStore)
    {
        AudioService.uiStore = uiStore;
    }

    public static void SetPlayStore(PlaySfxStore playStore)
    {
        AudioService.playStore = playStore;
    }

    public class UISounds
    {
        public void ChestOpen() => uiStore?.ChestOpen();
    }

    public class UnitSounds
    {
        public void MeeleAttack() => playStore?.MeeleAttack();
        public void RangeAttack() => playStore?.RangeAttack();
        public void Ability() => playStore?.Ability();
        public void Death() => playStore?.UnitDeath();
    }

    public class TowerSounds
    {
        public void TowerAttack() => playStore?.TowerAttack();
        public void Death() => playStore?.TowerDeath();
    }
}
