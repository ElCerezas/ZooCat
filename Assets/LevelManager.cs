using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public List<Animal> animals = new List<Animal>();
    public List<Animal> placedAnimals = new List<Animal>();
    void Awake()
    {
        instance = this;
        GameObject[] gmObj = GameObject.FindGameObjectsWithTag("Animal");
        for (int i = 0; i < gmObj.Length; i++)
        {
            animals.Add(gmObj[i].GetComponent<Animal>());
        }
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
            CuaLogic.instance.UpdateCua();
        }
    }
}
