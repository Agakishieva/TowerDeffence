using UnityEngine;

public class PlaySfxStore : MonoBehaviour
{
    // units
    [SerializeField] private AudioSource unitMeleeAttack;
    [SerializeField] private AudioSource unitRangeAttack;
    [SerializeField] private AudioSource unitAbility;
    [SerializeField] private AudioSource unitDeath;

    // towers
    [SerializeField] private AudioSource towerAttack;
    [SerializeField] private AudioSource towerDeath;

    private void Awake()
    {
        AudioService.SetPlayStore(this);
    }

    // units
    public void MeeleAttack() => unitMeleeAttack.Play();
    public void RangeAttack() => unitRangeAttack.Play();
    public void Ability() => unitAbility.Play();
    public void UnitDeath() => unitDeath.Play();

    // towers
    public void TowerAttack() => towerAttack.Play();
    public void TowerDeath() => towerDeath.Play();
}
