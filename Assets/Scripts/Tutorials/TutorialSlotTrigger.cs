using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialSlotTrigger : MonoBehaviour
{
    private BiomeSlot biomeSlot;
    public bool isFirsSlotToClick = true;

    void Awake()
    {
        biomeSlot = GetComponent<BiomeSlot>();
    }

    public void OnMouseDown()
    {
        if (TutorialManagerExists())
        {
            TutorialManager tm = TutorialManager.instance as TutorialManager;
            if (tm != null && tm.currentPhase == TutorialPhase.ClickSlot)
                tm.OnSlotClicked();
        }
    }
    bool TutorialManagerExists()
    {
        return LevelManager.instance != null && LevelManager.instance is TutorialManager;
    }
}