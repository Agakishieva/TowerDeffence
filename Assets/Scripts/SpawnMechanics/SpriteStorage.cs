using UnityEngine;
using UnityEngine.UI;

public class SpriteStorage : MonoBehaviour
{
    [SerializeField] private Sprite _daughterSprite;
    [SerializeField] private Sprite _fermerSprite;
    [SerializeField] private Sprite _fermerRSprite;
    [SerializeField] private Sprite _villagerSprite;
    [SerializeField] private Sprite _villagerRSprite;
    [SerializeField] private Sprite _sheriffSprite;

    public Sprite GetCardSprite(Unit.UnitClass unitClass)
    {
        return unitClass switch
        {
            Unit.UnitClass.Daughter => _daughterSprite,
            Unit.UnitClass.Fermer => _fermerSprite,
            Unit.UnitClass.FermerR => _fermerRSprite,
            Unit.UnitClass.Villager => _villagerSprite,
            Unit.UnitClass.VillagerR => _villagerRSprite,
            Unit.UnitClass.Sheriff => _sheriffSprite,
            _ => _daughterSprite,
        };
    }
}
