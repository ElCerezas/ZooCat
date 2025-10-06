using UnityEngine;
using NaughtyAttributes;
using System.Runtime.InteropServices.WindowsRuntime;
public enum Taxon {Mamifer, Reptil, Ocell, Anfibi, Insecte, Peix}
public enum Dieta {Carnivor, Herbivor, Omnivor}
public enum Medi { Volador, Mari, Terrestre}
enum TypeOfConditions { Biome, Temperature, CompanionFood, CompanionTaxon, CompanionTransport, Null }
public class Animal : MonoBehaviour
{
    //AnimalStats
    public string name;
    [SerializeField] Taxon taxon;
    [SerializeField] Medi medi;
    [SerializeField] Dieta dieta;
    
    //Condition Vars
    [SerializeField] bool isNegative;
    [SerializeField] TypeOfConditions condition;
    [ShowIf("condition", TypeOfConditions.Biome)][SerializeField] Bioma biomeCon;
    [ShowIf("condition", TypeOfConditions.CompanionFood)][SerializeField] Dieta dietaCon;
    [ShowIf("condition", TypeOfConditions.Temperature)][SerializeField] Temperatura temperaturaCon;
    [ShowIf("condition", TypeOfConditions.CompanionTaxon)][SerializeField] Taxon taxonCon;
    [ShowIf("condition", TypeOfConditions.CompanionTransport)][SerializeField] Medi transportCon;

    BiomeSlot slot;
    public bool isOnQueue = true;
    public bool selectable = false;
    object workingVar = null;
    private void Start()
    {
        SetWorkingVariable();
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
            if (!isOnQueue)
            {

            }
            transform.parent = slot.gameObject.transform;
            slot.SlotPle = false;
            if (isOnQueue)
            {
                isOnQueue = false;
                CuaLogic.instance.animalCua.Remove(gameObject.transform);
                CuaLogic.instance.UpdateCua();
            }
            transform.position = transform.parent.transform.position;
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
    bool CheckIfHappy ()
    {
        BiomeSlot[] nearSlots = slot.GetCloseSlots();
        if (workingVar == null) { return true; }
        else if (workingVar.GetType() == typeof(Bioma))
        {
            return (slot.GetBioma() == biomeCon && isNegative);
        }
        else if (workingVar.GetType() == typeof(Temperatura))
        {
            return (slot.GetTemperatura() == temperaturaCon && isNegative);
        }
        else if (workingVar.GetType() == typeof(Dieta))
        {
            for (int i = 0; i < nearSlots.Length; i++)
            {
                if (!(nearSlots[i].GetComponentInChildren<Animal>().dieta == dietaCon && isNegative)) { return false;}
            }
        }
        else if (workingVar.GetType() == typeof(Taxon))
        {
            for (int i = 0; i < nearSlots.Length; i++)
            {
                if (!(nearSlots[i].GetComponentInChildren<Animal>().taxon == taxonCon && isNegative)) { return false; }
            }
        }
        else if (workingVar.GetType() == typeof(Medi))
        {
            for (int i = 0; i < nearSlots.Length; i++)
            {
                if (!(nearSlots[i].GetComponentInChildren<Animal>().medi == transportCon && isNegative)) { return false; }
            }
        }
        return true;
    }
    void SetWorkingVariable()
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
                workingVar = null;
                break;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log(CheckIfHappy());
        }
    }
}
