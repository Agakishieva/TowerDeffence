using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField] private UnitsSpawnUI _unitUI;

    private void OnDisable()
    {
        _unitUI.spawnUnit -= NormalGame.SpawnEnemy;
    }

    public void Init()
    {
        UnitService.round.Start();

        HudInit();
    }

    private void HudInit()
    {
        _unitUI.Init();
        _unitUI.spawnUnit += NormalGame.SpawnEnemy;
    }
}
