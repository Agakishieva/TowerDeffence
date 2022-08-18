using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation
{
    private static readonly Stack<string> scenes = new Stack<string>();

    public const string mainSceneName = "MainMenuOur"; // for optimization
    public const string playSceneName = "PlayScene"; // 1st setting
    public const string playSceneName2 = "PlaySceneSetting2"; // 2nd setting
    public const string playSceneName3 = "PlaySceneSetting3"; // 3rd setting
    public const string chestsSceneName = "AnimationScene";
    public const string upgradeSceneName = "LevelingScene";
    public const string quickGameScene = "Dev"; // to be deleted

    // routes
    public static void NavigateBack()
    {
        if (scenes.Count == 0)
        {
            NavigateMain();
        }
        else
        {
            string sceneName = scenes.Pop();
            if (sceneName == mainSceneName)
            {
                NavigateMain();
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }

    public static void NavigateMain()
    {
        DropStack();
        SceneManager.LoadScene(mainSceneName);
    }

    public static void NavigatePlay()
    {
        LoadScene(playSceneName);
    }

    public static void NavigatePlay2()
    {
        LoadScene(playSceneName2);
    }

    public static void NavigatePlay3()
    {
        LoadScene(playSceneName3);
    }

    public static void NavigateChests()
    {
        LoadScene(chestsSceneName);
    }

    public static void NavigateUpgrade()
    {
        LoadScene(upgradeSceneName);
    }

    private static void LoadScene(string name)
    {
        AddSceneToStack();
        SceneManager.LoadScene(name);
    }

    // service
    private static void AddSceneToStack()
    {
        string currentName = SceneManager.GetActiveScene().name;
        if (currentName != mainSceneName)
        {
            scenes.Push(currentName);
        }
    }

    private static void DropStack()
    {
        scenes.Clear();
    }

    private static string GetActiveSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(GetActiveSceneName());
    }

    public static bool IsActiveScene(string name)
    {
        return GetActiveSceneName() == name;
    }

    public static bool IsPlayScene()
    {
        return GetActiveSceneName() == playSceneName || GetActiveSceneName() == playSceneName2 || GetActiveSceneName() == playSceneName3;
    }
}
