using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using Unity.VisualScripting;

public enum Taxon { Mamifer, Reptil, Ocell, Anfibi, Peix }
public enum Dieta { Carnivor, Herbivor, Omnivor }
public enum Medi { Volador, Mari, Terrestre }
public enum TypeOfConditions { Biome, Temperature, CompanionFood, CompanionTaxon, CompanionTransport, Null }

public class Animal : MonoBehaviour
{
    [Header("Animal Stats")]
    public string animalName;
    public Taxon taxon;
    public Medi medi;
    public Dieta dieta;

    [Header("Conditions")]
    [SerializeField] bool isNegative;
    [SerializeField] TypeOfConditions condition = TypeOfConditions.Null;
    [ShowIf("condition", TypeOfConditions.Biome)][SerializeField] Bioma biomeCon;
    [ShowIf("condition", TypeOfConditions.CompanionFood)][SerializeField] Dieta dietaCon;
    [ShowIf("condition", TypeOfConditions.Temperature)][SerializeField] Temperatura temperaturaCon;
    [ShowIf("condition", TypeOfConditions.CompanionTaxon)][SerializeField] Taxon taxonCon;
    [ShowIf("condition", TypeOfConditions.CompanionTransport)][SerializeField] Medi transportCon;

    [Header("Second Condition")]
    [SerializeField] bool isNegative2;
    [SerializeField] TypeOfConditions condition2 = TypeOfConditions.Null;
    [ShowIf("condition2", TypeOfConditions.Biome)][SerializeField] Bioma biomeCon2;
    [ShowIf("condition2", TypeOfConditions.CompanionFood)][SerializeField] Dieta dietaCon2;
    [ShowIf("condition2", TypeOfConditions.Temperature)][SerializeField] Temperatura temperaturaCon2;
    [ShowIf("condition2", TypeOfConditions.CompanionTaxon)][SerializeField] Taxon taxonCon2;
    [ShowIf("condition2", TypeOfConditions.CompanionTransport)][SerializeField] Medi transportCon2;

    [Header("Runtime Data")]
    public BiomeSlot parentSlot;   // Slot actual
    public bool isOnQueue = true;
    public bool selectable = false;

    [Header("Transforms")]
    public Transform head, body;

    private object workingVar;
    private object workingVar2;
    private SpriteRenderer spriteRenderer;
    public AnimalAnimator aAnimator;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        aAnimator = GetComponent<AnimalAnimator>();
        gameObject.name = animalName;
        SetWorkingVariables();
    }
    public void SetParentSlot(BiomeSlot newSlot, bool transformImediatly = false)
    {
        if (parentSlot != null) { parentSlot.SlotPle = false; }

        parentSlot = newSlot;

        if (newSlot != null)
        {
            newSlot.SlotPle = true;
            transform.SetParent(newSlot.transform);
            transform.localScale = Vector3.one;
            if (transformImediatly)
                transform.localPosition = Vector3.zero;

            SetMood(CheckIfHappy());
        }

        if (isOnQueue && parentSlot != null)
        {
            LevelManager.instance.placedAnimals.Add(this);
            isOnQueue = false;
            LevelManager.instance.canUpdateCua = true;
            LevelManager.instance.OverallHappiness();
        }
        LevelManager.instance.OverallHappiness();
    }
    public void SetMood(bool isHappy)
    {
        if (!isHappy)
        {
            aAnimator.OnMoodSwap(Mood.Angry);
        }
        else
        {
            aAnimator.OnMoodSwap(Mood.Happy);
        }
    }
    void SetWorkingVariables()
    {
        workingVar = condition switch
        {
            TypeOfConditions.Biome => biomeCon,
            TypeOfConditions.CompanionFood => dietaCon,
            TypeOfConditions.Temperature => temperaturaCon,
            TypeOfConditions.CompanionTaxon => taxonCon,
            TypeOfConditions.CompanionTransport => transportCon,
            _ => TypeOfConditions.Null
        };

        workingVar2 = condition2 switch
        {
            TypeOfConditions.Biome => biomeCon2,
            TypeOfConditions.CompanionFood => dietaCon2,
            TypeOfConditions.Temperature => temperaturaCon2,
            TypeOfConditions.CompanionTaxon => taxonCon2,
            TypeOfConditions.CompanionTransport => transportCon2,
            _ => TypeOfConditions.Null
        };
    }
    public bool CheckIfHappy()
    {
        if (parentSlot == null)
            return false;

        List<BiomeSlot> nearSlots = parentSlot.GetCloseSlots();

        bool happy1 = EvaluateCondition(condition, isNegative, biomeCon, temperaturaCon, dietaCon, taxonCon, transportCon, parentSlot, nearSlots);
        bool happy2 = EvaluateCondition(condition2, isNegative2, biomeCon2, temperaturaCon2, dietaCon2, taxonCon2, transportCon2, parentSlot, nearSlots);
        return happy1 && happy2;
    }
    private bool EvaluateCondition(TypeOfConditions conditionType, bool isNegative, Bioma bioma, Temperatura temperatura, Dieta dieta, Taxon taxon, Medi medi, BiomeSlot slot, List<BiomeSlot> nearSlots)
    {
        switch (conditionType)
        {
            case TypeOfConditions.Null:
                return true;

            case TypeOfConditions.Biome:
                return (slot.GetBioma() == bioma) != isNegative;

            case TypeOfConditions.Temperature:
                return (slot.GetTemperatura() == temperatura) != isNegative;

            case TypeOfConditions.CompanionFood:
                bool hasMatch = false;
                foreach (var s in nearSlots)
                {
                    if (!s.SlotPle) continue;
                    var other = s.GetComponentInChildren<Animal>();
                    if (other != null && other.dieta == dieta)
                    {
                        hasMatch = true;
                        break;
                    }
                }
                return isNegative ? !hasMatch : hasMatch;

            case TypeOfConditions.CompanionTaxon:
                bool hasMatch2 = false;
                foreach (var s in nearSlots)
                {
                    if (!s.SlotPle) continue;
                    var other = s.GetComponentInChildren<Animal>();
                    if (other != null && other.taxon == taxon)
                    {
                        hasMatch2 = true;
                        break;
                    }
                }
                return isNegative ? !hasMatch2 : hasMatch2;

            case TypeOfConditions.CompanionTransport:
                bool hasMatch3 = false;
                foreach (var s in nearSlots)
                {
                    if (!s.SlotPle) continue;
                    var other = s.GetComponentInChildren<Animal>();
                    if (other != null && other.medi == medi)
                    {
                        hasMatch3 = true;
                        break;
                    }
                }
                return isNegative ? !hasMatch3 : hasMatch3;

            default:
                return true;
        }
    }
    /*private void OnMouseEnter()
    {
        int var1 = workingVar.GetHashCode(); //(workingVar.GetHashCode() == null) ? 0 : workingVar.GetHashCode();
        int var2 = workingVar2.GetHashCode(); //(workingVar2.GetHashCode() == null) ? 0 : workingVar2.GetHashCode();
        UIIconShow.instance.NewInfo(this, parentSlot, isNegative, condition, var1, isNegative2, condition2, var2);
    }*/
    private void OnMouseDown()
    {
        int var1 = workingVar.GetHashCode(); //(workingVar.GetHashCode() == null) ? 0 : workingVar.GetHashCode();
        int var2 = workingVar2.GetHashCode(); //(workingVar2.GetHashCode() == null) ? 0 : workingVar2.GetHashCode();
        UIIconShow.instance.NewInfo(this, parentSlot, isNegative, condition, var1, isNegative2, condition2, var2);
    }
}

