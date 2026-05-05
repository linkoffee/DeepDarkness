using BayatGames.SaveGameFree;
using System.Collections.Generic;
using UnityEngine;

public static class SaveManager
{
    private const string SaveFileName = "data.sav";

    public static bool HasData(string key)
    {
        if (!SaveGame.Exists(SaveFileName))
            return false;

        string json = SaveGame.Load<string>(SaveFileName);
        var saveDict = MiniJSON.Json.Deserialize(json) as Dictionary<string, object>;

        return saveDict != null && saveDict.ContainsKey(key);
    }

    public static void Save<T>(string key, T data)
    {
        string json = "";
        if (SaveGame.Exists(SaveFileName))
            json = SaveGame.Load<string>(SaveFileName);

        Dictionary<string, object> saveDict;

        if (string.IsNullOrEmpty(json))
        {
            saveDict = new Dictionary<string, object>();
        }
        else
        {
            saveDict = MiniJSON.Json.Deserialize(json) as Dictionary<string, object>;
            if (saveDict == null)
                saveDict = new Dictionary<string, object>();
        }

        saveDict[key] = JsonUtility.ToJson(data);

        SaveGame.Save<string>(SaveFileName, MiniJSON.Json.Serialize(saveDict));
    }

    public static T Load<T>(string key, T defaultValue = default)
    {
        if (!SaveGame.Exists(SaveFileName))
            return defaultValue;

        string json = SaveGame.Load<string>(SaveFileName);
        var saveDict = MiniJSON.Json.Deserialize(json) as Dictionary<string, object>;

        if (saveDict != null && saveDict.ContainsKey(key))
            return JsonUtility.FromJson<T>(saveDict[key].ToString());

        return defaultValue;
    }

    public static void Delete(string key)
    {
        if (!SaveGame.Exists(SaveFileName))
            return;

        string json = SaveGame.Load<string>(SaveFileName);
        var saveDict = MiniJSON.Json.Deserialize(json) as Dictionary<string, object>;

        if (saveDict != null && saveDict.ContainsKey(key))
        {
            saveDict.Remove(key);
            SaveGame.Save<string>(SaveFileName, MiniJSON.Json.Serialize(saveDict));
        }
    }

    public static void DeleteAll() { if (SaveGame.Exists(SaveFileName)) SaveGame.Delete(SaveFileName); }
}