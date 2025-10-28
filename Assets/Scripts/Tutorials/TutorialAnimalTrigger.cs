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
            if (tm != null && tm.currentPhase == TutorialPhase.ClickAnimal)
                tm.OnAnimalClicked();
        }
    }
    public void OnMouseUp()
    {
        if (TutorialManagerExists())
        {
            TutorialManager tm = TutorialManager.instance as TutorialManager;
            if (tm != null && (tm.currentPhase == TutorialPhase.DragAnimalWrong || tm.currentPhase == TutorialPhase.DragAnimalRight))
            {
                if (animal.parentSlot != null)
                {
                    tm.OnAnimalDraggedToSlot(!(tm.currentPhase == TutorialPhase.DragAnimalWrong));
                }
            } 
        }
    }



    bool TutorialManagerExists()
    {
        return LevelManager.instance != null && LevelManager.instance is TutorialManager;
    }
}