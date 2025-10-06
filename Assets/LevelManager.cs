using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public List<Animal> animals = new List<Animal>();
    void Awake()
    {
        instance = this;
        GameObject[] gmObj = GameObject.FindGameObjectsWithTag("Animal");
        for (int i = 0; i < gmObj.Length; i++)
        {
            animals.Add(gmObj[i].GetComponent<Animal>());
        }
    }
}
