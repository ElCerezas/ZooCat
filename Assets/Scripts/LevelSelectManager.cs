using System.Linq;
using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
    public LevelButton[] levelsToLoad;
    const string PREF_KEY_PREFIX = "LevelUnlocked_";
    private void Start()
    {
        LoadLevels();
    }

    void LoadLevels()
    {
        for (int i = 0; i < levelsToLoad.Length; i++)
        {
            LevelButton lb = levelsToLoad[i];
            if (lb == null)
            {
                Debug.LogWarning($"LevelSelectManager: LevelButton null at index {i}");
                continue;
            }

            string key = PREF_KEY_PREFIX + lb.LevelToGo;

            bool isUnlocked;
            if (PlayerPrefs.HasKey(key))
            {
                isUnlocked = PlayerPrefs.GetInt(key) == 1;
            }
            else
            {
                isUnlocked = lb.isUnlockedByDefault && !lb.IsFake;
                PlayerPrefs.SetInt(key, isUnlocked ? 1 : 0);
            }

            if (lb.IsFake)
            {
                isUnlocked = false;
                PlayerPrefs.SetInt(key, 0);
            }

            lb.SetUp(isUnlocked);
        }

        PlayerPrefs.Save();
    }
    public void UnlockLevel(int LevelId)
    {
        LevelButton lb = levelsToLoad.FirstOrDefault(x => x.LevelToGo == LevelId);
        if (lb == null)
        {
            return;
        }

        if (lb.IsFake)
        {
            return;
        }

        string key = PREF_KEY_PREFIX + LevelId;
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();

        lb.Unlock();
    }
}
public static class LevelProgress
{
    const string PREF_KEY_PREFIX = "LevelUnlocked_";

    public static void UnlockLevel(int levelId)
    {
        // Evita valores inválidos
        if (levelId <= 0) return;

        string key = PREF_KEY_PREFIX + levelId;
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();
    }

    public static bool IsUnlocked(int levelId)
    {
        string key = PREF_KEY_PREFIX + levelId;
        return PlayerPrefs.GetInt(key, 0) == 1;
    }

    public static void ResetAll(int maxLevel)
    {
        for (int i = 1; i <= maxLevel; i++)
        {
            PlayerPrefs.DeleteKey(PREF_KEY_PREFIX + i);
        }
        PlayerPrefs.Save();
    }
}
