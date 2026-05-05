using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public struct LevelProgressData
{
    public string unlockedLevels;
}

public class LevelProgressManager : MonoBehaviour
{
    public static LevelProgressManager Instance { get; private set; }

    private const string SaveKey = "LevelProgress";
    private HashSet<int> unlockedLevels = new HashSet<int>();

    public event Action<int> OnLevelUnlocked;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadData();

        if (unlockedLevels.Count == 0)
            UnlockLevel(1);
    }

    public bool IsLevelUnlocked(int levelIndex) => unlockedLevels.Contains(levelIndex);

    public void UnlockLevel(int levelIndex)
    {
        if (unlockedLevels.Add(levelIndex))
        {
            SaveData();
            OnLevelUnlocked?.Invoke(levelIndex);
        }
    }

    public void UnlockNextLevel() => UnlockLevel(SceneManager.GetActiveScene().buildIndex + 1);

    private void LoadData()
    {
        LevelProgressData data = SaveManager.Load<LevelProgressData>(SaveKey);

        if (!string.IsNullOrEmpty(data.unlockedLevels))
        {
            string[] savedLevels = data.unlockedLevels.Split(',');
            foreach (string level in savedLevels)
            {
                if (int.TryParse(level, out int levelIndex))
                    unlockedLevels.Add(levelIndex);
            }
        }
    }

    private void SaveData()
    {
        LevelProgressData data = new LevelProgressData { unlockedLevels = string.Join(',', unlockedLevels) };
        SaveManager.Save(SaveKey, data);
    }
}
