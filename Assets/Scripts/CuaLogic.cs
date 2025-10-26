using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CuaLogic : MonoBehaviour
{
    public static CuaLogic instance;

    public List<Transform> animalCua = new List<Transform>();
    public Transform slot1, slot2, storage;
    public UIIconShow shower;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateCua();
    }

    public void UpdateCua()
    {
        for (int i = 0; i < animalCua.Count; i++)
        {
            Transform animal = animalCua[i];
            Animal a = animal.GetComponent<Animal>();
            if (i == 0)
            {
                animal.SetParent(slot1);
                animal.position = slot1.position;
                a.selectable = true;
                a.parentSlot = null;
                a.ShowAnimalInfo();
            }
            else if (i == 1)
            {
                animal.SetParent(slot2);
                animal.position = slot2.position;
                a.selectable = false;
                a.parentSlot = null;
            }
            else
            {
                animal.SetParent(storage);
                animal.position = storage.position;
                a.selectable = false;
                a.parentSlot = null;
            }
        }
        foreach (var t in animalCua)
        {
            t.GetComponent<AnimalDragHandler>().ResetPosition();
        }

    }
    public void AdvanceCua()
    {
        if (animalCua.Count == 0) return;
        animalCua.RemoveAt(0);
        UpdateCua();
    }
    public void RegisterAnimal(Transform newAnimal)
    {
        if (!animalCua.Contains(newAnimal))
        {
            animalCua.Add(newAnimal);
            UpdateCua();
        }
    }
    public bool IsCuaEmpty()
    {
        return animalCua.Count == 0;
    }
}
