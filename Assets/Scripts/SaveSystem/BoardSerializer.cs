using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class BoardSerializer
{
    public IEnumerable<FileContext> GetAllSaves()
    {
        var path = $"{Application.persistentDataPath}/{Serialization.DEFEND_PATH}";
        if (Directory.Exists(path) == false)
            yield break;
        foreach (var file in Directory.GetFiles(path))
        {
            if (Path.HasExtension(file) == false || Path.GetExtension(file) != Serialization.DEFEND_EXTENSION)
                continue;
            yield return new FileContext(new FileInfo(file));
        }
    }

    public void Delete(string fileName)
    {
        var savePath = $"{Application.persistentDataPath}/{Serialization.DEFEND_PATH}/{fileName}{Serialization.DEFEND_EXTENSION}";
        if (File.Exists(savePath))
            File.Delete(savePath);
    }

    public void Save(BoardData data, string fileName)
    {
        var formatter = new BinaryFormatter();
        string json = JsonUtility.ToJson(data); // JSON for android test
        // var directoryPath = $"{Application.persistentDataPath}/{Serialization.DEFEND_PATH}";
        var directoryPath = $"{Application.dataPath}/Resources/{Serialization.LEVELS_PATH}";
        if (Directory.Exists(directoryPath) == false)
            Directory.CreateDirectory(directoryPath);
        // var savePath = $"{directoryPath}/{fileName}{Serialization.DEFEND_EXTENSION}";
        var savePath = $"{directoryPath}/{fileName}{Serialization.LEVELS_EXTENSION}";
        // File.WriteAllBytes(savePath, data.Serialize());
        File.WriteAllText(savePath, json);
#if UNITY_EDITOR
        UnityEditor.EditorUtility.RevealInFinder(savePath);
#endif

    }

    public BoardData Load(string fileName)
    {
        //var savePath = $"{Application.persistentDataPath}/{Serialization.DEFEND_PATH}/{fileName}{Serialization.DEFEND_EXTENSION}";
        var savePath = $"{Application.dataPath}/Resources/{Serialization.LEVELS_PATH}/{fileName}{Serialization.LEVELS_EXTENSION}";
        if (File.Exists(savePath) == false)
            return null;
        string json = File.ReadAllText(savePath); // JSON for android test
        // return BoardData.Deserialize(File.ReadAllBytes(savePath));
        return JsonUtility.FromJson<BoardData>(json);
    }

    public BoardData LoadRes(string filename)
    {
        var json = Resources.Load<TextAsset>($"Levels/{filename}");
        if (!json)
        {
            return null;
        }
        return JsonUtility.FromJson<BoardData>(json.text);
    }
}