using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] EndScreenManager endScreenManager;

    [Header("Slots & Animals")]
    public List<Animal> animals = new List<Animal>();
    public List<Animal> placedAnimals = new List<Animal>();
    public bool canUpdateCua = false;

    [Header("Timer")]
    [SerializeField] TextMeshProUGUI timerText;
    float time = 0;
    void Awake()
    {
        instance = this;
        GameObject[] gmObj = GameObject.FindGameObjectsWithTag("Animal");
        for (int i = 0; i < gmObj.Length; i++)
        {
            animals.Add(gmObj[i].GetComponent<Animal>());
        }
    }
    private void Update()
    {
        time += Time.deltaTime;
        timerText.text = TimerFormat(time);
    }
    string TimerFormat(float t)
    {
        string min = Mathf.Floor(t / 60).ToString("00");
        string sec = Mathf.Floor(t % 60).ToString("00");
        return min + ":" + sec;
    }
    public void  OverallHappiness()
    {
        bool allHappy = true;
        for (int i = 0; i < placedAnimals.Count;i++)
        {
            if (placedAnimals[i].CheckIfHappy() == false)
            {
                placedAnimals[i].SetMood(false);
                allHappy = false;
            }else
            {
                placedAnimals[i].SetMood(true);
            }
        }
        if (allHappy)
        {
            if (CuaLogic.instance.IsCuaEmpty())
            {
                endScreenManager.StartEndScreenShow(time);
            }
            else
            {
                if (canUpdateCua)
                {
                    CuaLogic.instance.AdvanceCua();
                    canUpdateCua = false;
                }
            }
        }
    }
}
