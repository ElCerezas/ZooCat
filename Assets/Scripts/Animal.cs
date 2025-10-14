using UnityEngine;
using NaughtyAttributes;
using System.Runtime.InteropServices.WindowsRuntime;
using NUnit.Framework;
using System.Collections.Generic;
public enum Taxon {Mamifer, Reptil, Ocell, Anfibi, Peix}
public enum Dieta {Carnivor, Herbivor, Omnivor}
public enum Medi { Volador, Mari, Terrestre}
public enum TypeOfConditions { Biome, Temperature, CompanionFood, CompanionTaxon, CompanionTransport, Null }
public class Animal : MonoBehaviour
{
    [Header("Animal Stats")]
    public string animalName;
    public Taxon taxon;
    public Medi medi;
    public Dieta dieta;

    [Header("Condition 1")]
    [SerializeField] bool isNegative;
    [SerializeField] TypeOfConditions condition = TypeOfConditions.Null;
    [ShowIf("condition", TypeOfConditions.Biome)][SerializeField] Bioma biomeCon;
    [ShowIf("condition", TypeOfConditions.CompanionFood)][SerializeField] Dieta dietaCon;
    [ShowIf("condition", TypeOfConditions.Temperature)][SerializeField] Temperatura temperaturaCon;
    [ShowIf("condition", TypeOfConditions.CompanionTaxon)][SerializeField] Taxon taxonCon;
    [ShowIf("condition", TypeOfConditions.CompanionTransport)][SerializeField] Medi transportCon;

    [Header("Condition 2")]
    [SerializeField] bool isNegative2;
    [SerializeField] TypeOfConditions condition2 = TypeOfConditions.Null;
    [ShowIf("condition2", TypeOfConditions.Biome)][SerializeField] Bioma biomeCon2;
    [ShowIf("condition2", TypeOfConditions.CompanionFood)][SerializeField] Dieta dietaCon2;
    [ShowIf("condition2", TypeOfConditions.Temperature)][SerializeField] Temperatura temperaturaCon2;
    [ShowIf("condition2", TypeOfConditions.CompanionTaxon)][SerializeField] Taxon taxonCon2;
    [ShowIf("condition2", TypeOfConditions.CompanionTransport)][SerializeField] Medi transportCon2;

    [Header("Test Tools")]
    BiomeSlot parentSlot;
    BiomeSlot slot;
    public bool isOnQueue = true;
    public bool selectable = false;
    object workingVar = null;
    object workingVar2 = null;
    SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameObject.name = animalName;
        SetWorkingVariables();
    }
    private void OnMouseEnter()
    {
        int var1 = workingVar.GetHashCode(); //(workingVar.GetHashCode() == null) ? 0 : workingVar.GetHashCode();
        int var2 = workingVar2.GetHashCode(); //(workingVar2.GetHashCode() == null) ? 0 : workingVar2.GetHashCode();
        UIIconShow.instance.NewInfo(this, parentSlot, isNegative, condition, var1 ,isNegative2 ,condition2, var2 );
    }
    private void OnMouseExit()
    {
        //bubbleIcon.ToggleOffBubble();
    }
    private void OnMouseDrag()
    {
        if (!selectable) { return; }
        Vector3 worldPosition;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

        gameObject.transform.position = new Vector2(worldPosition.x, worldPosition.y);
    }
    private void OnMouseUp()
    {
        if (!selectable) { return; }
        if (slot != null && !slot.SlotPle)
        {
            if (parentSlot != null) { parentSlot.SlotPle = false; }
            parentSlot = slot;
            parentSlot.SlotPle = true;
            

            transform.parent = slot.gameObject.transform;
            if (isOnQueue)
            {
                LevelManager.instance.placedAnimals.Add(this);
                isOnQueue = false;
                CuaLogic.instance.animalCua.Remove(gameObject.transform);
            }
            transform.position = transform.parent.transform.position;
            LevelManager.instance.OverallHappiness();
        }
        else
        {
                transform.position = transform.parent.transform.position;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        slot = collision.gameObject.GetComponent<BiomeSlot>();
        Debug.Log("Entered Slot");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (slot == collision.gameObject.GetComponent<BiomeSlot>())
        {
            slot = null;
            Debug.Log("Exit All Slots");
        }
    }
    public bool CheckIfHappy ()
    {
        List <BiomeSlot> nearSlots = slot.GetCloseSlots();

        //Condicio 1
        if (workingVar == null) {}
        else if (workingVar.GetType() == typeof(Bioma))
        {
            return (slot.GetBioma() == biomeCon && !isNegative);
        }
        else if (workingVar.GetType() == typeof(Temperatura))
        {
            return (slot.GetTemperatura() == temperaturaCon && !isNegative);
        }
        else if (workingVar.GetType() == typeof(Dieta))
        {
            for (int i = 0; i < nearSlots.Count; i++)
            {
                if (!nearSlots[i].SlotPle) { }
                else if (!(nearSlots[i].GetComponentInChildren<Animal>().dieta == dietaCon && !isNegative)) { return false;}
            }
        }
        else if (workingVar.GetType() == typeof(Taxon))
        {
            for (int i = 0; i < nearSlots.Count; i++)
            {
                if (!nearSlots[i].SlotPle) { Debug.Log("Slot buit al costat"); }
                else if (!(nearSlots[i].GetComponentInChildren<Animal>().taxon == taxonCon && !isNegative)) { Debug.Log("Condició no cumplida"); return false; }
            }
        }
        else if (workingVar.GetType() == typeof(Medi))
        {
            for (int i = 0; i < nearSlots.Count; i++)
            {
                if (!nearSlots[i].SlotPle) { }
                else if(!(nearSlots[i].GetComponentInChildren<Animal>().medi == transportCon && !isNegative)) { return false; }
            }
        }

        //Condicio 2
        if (workingVar2 == null) {}
        else if (workingVar2.GetType() == typeof(Bioma))
        {
            return (slot.GetBioma() == biomeCon2 && !isNegative2);
        }
        else if (workingVar2.GetType() == typeof(Temperatura))
        {
            return (slot.GetTemperatura() == temperaturaCon2 && !isNegative2);
        }
        else if (workingVar2.GetType() == typeof(Dieta))
        {
            for (int i = 0; i < nearSlots.Count; i++)
            {
                if (!nearSlots[i].SlotPle) { }
                else if (!(nearSlots[i].GetComponentInChildren<Animal>().dieta == dietaCon2 && !isNegative2)) { return false; }
            }
        }
        else if (workingVar2.GetType() == typeof(Taxon))
        {
            for (int i = 0; i < nearSlots.Count; i++)
            {
                if (!nearSlots[i].SlotPle) {}
                else if (!(nearSlots[i].GetComponentInChildren<Animal>().taxon == taxonCon2 && !isNegative2)) { Debug.Log("Condició no cumplida"); return false; }
            }
        }
        else if (workingVar2.GetType() == typeof(Medi))
        {
            for (int i = 0; i < nearSlots.Count; i++)
            {
                if (!nearSlots[i].SlotPle) { }
                else if (!(nearSlots[i].GetComponentInChildren<Animal>().medi == transportCon2 && !isNegative2)) { return false; }
            }
        }
        return true;
    }
    void SetWorkingVariables()
    {
        switch (condition)
        {
            case TypeOfConditions.Biome:
                workingVar = biomeCon;
                break;
            case TypeOfConditions.CompanionFood:
                workingVar = dietaCon;
                break;
            case TypeOfConditions.Temperature:
                workingVar = temperaturaCon;
                break;
            case TypeOfConditions.CompanionTaxon:
                workingVar = taxonCon;
                break;
            case TypeOfConditions.CompanionTransport:
                workingVar = transportCon;
                break;
            default:
                workingVar = TypeOfConditions.Null;
                break;
        }
        switch (condition2)
        {
            case TypeOfConditions.Biome:
                workingVar2 = biomeCon2;
                break;
            case TypeOfConditions.CompanionFood:
                workingVar2 = dietaCon2;
                break;
            case TypeOfConditions.Temperature:
                workingVar2 = temperaturaCon2;
                break;
            case TypeOfConditions.CompanionTaxon:
                workingVar2 = taxonCon2;
                break;
            case TypeOfConditions.CompanionTransport:
                workingVar2 = transportCon2;
                break;
            default:
                workingVar2 = TypeOfConditions.Null;
                break;
        }
    }
    public void SetMood(bool isHappy)
    {
        spriteRenderer.color = isHappy ? Color.green : Color.red;
    }
}
