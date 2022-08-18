using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    private const string zombieTag = "ZombieTag";
    private const string animalsTag = "AnimalsTag";
    private const string bugsTag = "BugsTag";

    [SerializeField] private GameObject[] settingObjects;

    private void Awake()
    {
        foreach (GameObject obj in settingObjects)
        {
            if (obj.CompareTag(zombieTag))
            {
                obj.SetActive(Configs.SelectedSetting() == Configs.Setting.Zombie);
            } else if (obj.CompareTag(animalsTag))
            {
                obj.SetActive(Configs.SelectedSetting() == Configs.Setting.Animals);
            } else if (obj.CompareTag(bugsTag))
            {
                obj.SetActive(Configs.SelectedSetting() == Configs.Setting.Bugs);
            }
        }
    }
}
