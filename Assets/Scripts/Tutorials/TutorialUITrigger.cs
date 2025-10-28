using FMODUnity;
using UnityEngine;

public class TutorialUITrigger : MonoBehaviour
{
    enum typeSlot {Slot, Animal, Condition}
    [SerializeField]typeSlot uiType;
    bool hovered = false;
    static int[] slotHovers = { 0, 0, 0 };
    TutorialManager tm;
    void OnMouseEnter()
    {
        Debug.Log("Entered");
        if (tm == null)
            tm = TutorialManager.instance as TutorialManager;
        switch (uiType.GetHashCode())
        {
            case 0:
                if (!hovered && tm != null && tm.currentPhase == TutorialPhase.ShowSlotUI)
                {
                    hovered = true;
                    slotHovers[0]++;
                    if (slotHovers[0] >= 2)
                        tm.OnSlotUIHovered();

                }
                break;
            case 1:
                if (!hovered && tm != null && tm.currentPhase == TutorialPhase.ShowAnimalUI)
                {
                    hovered = true;
                    slotHovers[1]++;
                    if (slotHovers[1] >= 3)
                        tm.OnAnimalUIHovered();

                }
                break;
            case 2:
                if (!hovered && tm != null && tm.currentPhase == TutorialPhase.ShowConditionsUI)
                {
                    hovered = true;
                    slotHovers[2]++;
                    if (slotHovers[2] >= 1)
                        tm.OnConditionsUIHovered();

                }
                break;
        }
    }
}
