using UnityEngine;

public class ConditionBubbleManager : MonoBehaviour
{
    [SerializeField] GameObject conditionBubble;
    [SerializeField] SpriteRenderer icon;
    [SerializeField] Sprite[] icons;
    int conditionGrupMod, numCon;
    void Start()
    {
    }

    public void ReciveCondition(bool isNegative, TypeOfConditions type, int Index)
    {
        icon.color = isNegative ? Color.red : Color.green;
        switch (type)
        {
            case TypeOfConditions.CompanionTaxon: conditionGrupMod = 0; break;
            case TypeOfConditions.CompanionTransport: conditionGrupMod = 5; break;
            case TypeOfConditions.CompanionFood: conditionGrupMod = 8; break;
            case TypeOfConditions.Temperature: conditionGrupMod = 11; break;
            case TypeOfConditions.Biome: conditionGrupMod = 14; break;
        }
        numCon = Index;
        icon.sprite = icons[conditionGrupMod+numCon];
    }
    public void ToggleOnBubble()
    {
        conditionBubble.SetActive(true);
    }
    public void ToggleOffBubble()
    {
        conditionBubble.SetActive(false);
    }
}
