using System.Collections.Generic;
using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
    public LevelButton[] levelsToLoad;
    public static LevelButton[] levelButtons;
    public static bool[] unlockLevels;

    public static LevelSelectManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        levelButtons = levelsToLoad;
        unlockLevels = new bool[levelsToLoad.Length];
        LoadLevels();
    }
    void LoadLevels()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            Debug.Log($"{i+1}/{levelButtons.Length}");
            unlockLevels[i] = levelButtons[i].isUnlockedByDefault;
            levelButtons[i].SetUp(unlockLevels[i]);
        }
    }
    public void UnlockLevel(int LevelId)
    {
        LevelId -= 1;
        unlockLevels[LevelId] = levelButtons[LevelId].isUnlockedByDefault;
        levelButtons[LevelId].Unlock();
    }
}
