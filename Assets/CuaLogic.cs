using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuaLogic : MonoBehaviour
{
    public static CuaLogic instance;
    public List<Transform> animalCua = new List<Transform>();
    public Transform slot1, slot2, storage;

    private void Start()
    {
        instance = this;
        UpdateCua();
    }
    public void UpdateCua()
    {
        for (int i = 0; i < animalCua.Count; i++)
        {
            if (i == 0)
            {
                animalCua[i].position = slot1.position;
                animalCua[i].GetComponent<Animal>().selectable = true;
                animalCua[i].parent = slot1;
            }
            else if (i == 1)
            {
                animalCua[i].position = slot2.position;
                animalCua[i].parent = slot2;
            }
            else
            {
                animalCua[i].position = storage.position;
                animalCua[i].parent = storage;
            }
        }
    }
}
