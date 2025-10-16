using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIIconShow : MonoBehaviour
{
    public static UIIconShow instance;

    [Header("UI")]
    [SerializeField] Image[] animalIcons; //Taxon - Food - Transport
    [SerializeField] Image[] biomeIcons;  //Temperature - Biome
    [SerializeField] GameObject animalUI, biomeUI, animalName;
    [SerializeField] Image conditionIcon1, conditionIcon2;

    [Header("Sprites")]
    [SerializeField] Sprite[] iconsTaxon;       // Mamífero, Reptil, Ave, etc.
    [SerializeField] Sprite[] iconsFood;        // Carnívoro, Herbívoro, Omnívoro
    [SerializeField] Sprite[] iconsTransport;   // Volador, Marino, Terrestre
    [SerializeField] Sprite[] iconsTemperature; // Frío, Templado, Cálido
    [SerializeField] Sprite[] iconsBiome;       // Agua, Tierra, Árbol, Aire

    [Header("Animation")]
    [SerializeField] Vector3 DisplayedT;
    [SerializeField] Vector3 HideT;
    [SerializeField] float upTime = 0.3f;
    [SerializeField] float downTime = 0.3f;
    [SerializeField] AnimationCurve curve;
    private Vector3 currentPos;

    [Header("Local Saves")]
    Animal sAnimal = null;
    BiomeSlot sBiome = null;

    bool sNegative1 = false;
    TypeOfConditions sCondition1 = TypeOfConditions.Null;
    int sIndexSprite1 = 0;

    bool sNegative2 = false;
    TypeOfConditions sCondition2 = TypeOfConditions.Null;
    int sIndexSprite2 = 0;

    Coroutine activeAnimation = null;

    void Awake()
    {
        instance = this;
    }
    public void NewInfo(Animal animal = null, BiomeSlot biomeSlot = null, bool isNegative = false, TypeOfConditions condition = TypeOfConditions.Null, int Index = 0, bool isNegative2 = false, TypeOfConditions condition2 = TypeOfConditions.Null, int Index2 = 0)
    {
        bool newInfo = (animal != sAnimal || biomeSlot != sBiome);

        // Guardar referencias nuevas
        if (animal != null)
        {
            sAnimal = animal;
            if (condition != TypeOfConditions.Null)
            {
                sCondition1 = condition;
                sNegative1 = isNegative;
                sIndexSprite1 = Index;
            }
            if (condition2 != TypeOfConditions.Null)
            {
                sCondition2 = condition2;
                sNegative2 = isNegative2;
                sIndexSprite2 = Index2;
            }
        }
        else sAnimal = null;

        if (biomeSlot != null) sBiome = biomeSlot;
        else sBiome = null;

        // Control de animación
        if (newInfo)
        {
            if (activeAnimation != null)
            {
                StopCoroutine(activeAnimation);
                activeAnimation = null;
            }
            activeAnimation = StartCoroutine(StartUIAnimation());
        }
    }
    IEnumerator StartUIAnimation()
    {
        float elapsedTime = 0;
        float waitTime = upTime;
        currentPos = transform.position;

        // Animación de ocultar
        while (elapsedTime < waitTime)
        {
            float t1 = curve.Evaluate(elapsedTime / waitTime);
            transform.position = Vector3.Lerp(currentPos, HideT, t1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = HideT;
        UpdateInfo();

        elapsedTime = 0;
        waitTime = downTime;
        currentPos = transform.position;

        // Animación de mostrar
        while (elapsedTime < waitTime)
        {
            float t2 = curve.Evaluate(elapsedTime / waitTime);
            transform.position = Vector3.Lerp(currentPos, DisplayedT, t2);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = DisplayedT;
        activeAnimation = null;
    }
    void UpdateInfo()
    {
        if (sAnimal != null)
        {
            animalUI.SetActive(true);
            animalName.GetComponent<TMP_Text>().text = sAnimal.animalName;
            animalIcons[0].sprite = iconsTaxon[sAnimal.taxon.GetHashCode()];
            animalIcons[1].sprite = iconsFood[sAnimal.dieta.GetHashCode()];
            animalIcons[2].sprite = iconsTransport[sAnimal.medi.GetHashCode()];

            // Condición 1
            if (sCondition1 != TypeOfConditions.Null)
            {
                conditionIcon1.gameObject.SetActive(true);
                conditionIcon1.sprite = GetSpriteForCondition(sCondition1, sIndexSprite1);
                conditionIcon1.color = sNegative1 ? Color.red : Color.green;
            }
            else conditionIcon1.gameObject.SetActive(false);

            // Condición 2
            if (sCondition2 != TypeOfConditions.Null)
            {
                conditionIcon2.gameObject.SetActive(true);
                conditionIcon2.sprite = GetSpriteForCondition(sCondition2, sIndexSprite2);
                conditionIcon2.color = sNegative2 ? Color.red : Color.green;
            }
            else conditionIcon2.gameObject.SetActive(false);
        }
        else
        {
            animalUI.SetActive(false);
        }

        if (sBiome != null)
        {
            biomeIcons[0].sprite = iconsTemperature[sBiome.temp.GetHashCode()];
            biomeUI.SetActive(true);
        }
        else biomeUI.SetActive(false);
    }
    Sprite GetSpriteForCondition(TypeOfConditions type, int index)
    {
        switch (type)
        {
            case TypeOfConditions.CompanionTaxon: return iconsTaxon[index];
            case TypeOfConditions.CompanionTransport: return iconsTransport[index];
            case TypeOfConditions.CompanionFood: return iconsFood[index];
            case TypeOfConditions.Temperature: return iconsTemperature[index];
            //case TypeOfConditions.Biome: return iconsBiome[index];
            default: return null;
        }
    }
}
