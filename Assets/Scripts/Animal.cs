using UnityEngine;
using NaughtyAttributes;
enum Taxon {Mamifer, Reptil, Ocell, Anfibi, Insecte, Peix}
enum Dieta {Carnivor, Herbivor, Omnivor}
enum Medi { Volador, Mari, Terrestre}
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

    Transform SlotFather;
    private void Start()
    {
        
    }
    private void OnMouseDrag()
    {
        
    }
    private void OnMouseUp()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        SlotFather = collision.gameObject.transform;
    }

}
