using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
public class TutorialAnimalTrigger : MonoBehaviour
{
    private Animal animal;
    private Vector3 dragStartPos;

    void Awake()
    {
        animal = GetComponent<Animal>();
    }

    public void OnMouseDown()
    {
        // Avisar al TutorialManager del clic en el animal
        if (TutorialManagerExists())
        {
            TutorialManager tm = TutorialManager.instance as TutorialManager;
            if (tm != null)
                tm.OnAnimalClicked();
        }
    }

    /*public void OnEndDrag(PointerEventData eventData)
    {
        if (!TutorialManagerExists()) return;

        // Buscar el slot más cercano
        BiomeSlot[] allSlots = FindObjectsOfType<BiomeSlot>();
        BiomeSlot closest = null;
        float minDist = float.MaxValue;

        foreach (BiomeSlot s in allSlots)
        {
            float dist = Vector3.Distance(transform.position, s.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = s;
            }
        }

        // Considerar “correcto” si está suficientemente cerca del slot objetivo
        bool isCorrect = minDist < 1.0f;

        TutorialManager tm = TutorialManager.instance as TutorialManager;
        if (tm != null)
        {
            tm.OnAnimalDraggedToSlot(closest, isCorrect);
        }

        // Regresar el animal a su posición inicial (solo para tutorial)
        transform.position = dragStartPos;
    }*/

    bool TutorialManagerExists()
    {
        return LevelManager.instance != null && LevelManager.instance is TutorialManager;
    }
}