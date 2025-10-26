using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] EndScreenManager endScreenManager;
    [SerializeField] GameObject[] GameObjectsToHide;

    [Header("Slots & Animals")]
    public List<Animal> animals = new List<Animal>();
    public List<Animal> placedAnimals = new List<Animal>();
    public List<BiomeSlot> slots = new List<BiomeSlot>();
    public bool canUpdateCua = false;

    [Header("Timer")]
    [SerializeField] TextMeshProUGUI timerText;
    float time = 0;

    Coroutine UntilEnd;

    void Awake()
    {
        instance = this;
        GameObject[] gmObj = GameObject.FindGameObjectsWithTag("Animal");
        for (int i = 0; i < gmObj.Length; i++)
        {
            animals.Add(gmObj[i].GetComponent<Animal>());
        }
        gmObj = GameObject.FindGameObjectsWithTag("Slot");
        for (int i = 0; i < gmObj.Length; i++)
        {
            slots.Add(gmObj[i].GetComponent<BiomeSlot>());
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
                if(UntilEnd != null)
                    StopCoroutine(UntilEnd);

                UntilEnd = StartCoroutine(WaitForCompletion());
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
        else if (CuaLogic.instance.IsCuaEmpty() && UntilEnd != null)
        {
            StopCoroutine(UntilEnd);
        }
    }
    public void HighLightAll(bool highlight)
    {
        BiomeSlot.AllHighLighted = highlight;
        for (int i = 0; i < slots.Count; ++i)
        {
            slots[i].OnHighLight(highlight);
        }
    }
    IEnumerator WaitForCompletion()
    {
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < placedAnimals.Count; i++)
        {
            placedAnimals[i].selectable = false;
        }
        for (int i = 0; i < GameObjectsToHide.Length; i++)
        {
            GameObjectsToHide[i].SetActive(false);
        }
        endScreenManager.StartEndScreenShow(time-3f);
        yield return null;
    }
}
