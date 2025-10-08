using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIIconShow : MonoBehaviour
{
    public static UIIconShow instance;

    [Header("UI")]
    [SerializeField] Image[] animalIcons; //Taxon - Food - Transport
    [SerializeField] Image[] biomeIcons; //Temperature - Biome
    [SerializeField] GameObject animalUI, biomeUI, animalName;
    [SerializeField] Image conditionIcon;

    [Header("Sprites")]
    [SerializeField] Sprite[] iconsTaxon; //Mamifer, Reptil, Ocell, Anfibi, Peix
    [SerializeField] Sprite[] iconsFood; //Carnivor, Herbivor, Omnivor
    [SerializeField] Sprite[] iconsTransport; //Volador, Mari, Terrestre
    [SerializeField] Sprite[] iconsTemperature; //Fred, Templat, Calent
    [SerializeField] Sprite[] iconsBiome; //

    [Header("Animation")]
    [SerializeField]Vector3 DisplayedT, HideT;
    Vector3 currentPos;

    [Header("Local Saves")]
    Animal sAnimal = null;
    BiomeSlot sBiome = null;
    bool sNegative = false;
    TypeOfConditions sCondition = TypeOfConditions.Null;
    int sIndexSprite = 0;
    
    void Awake()
    {
        instance = this;
    }
    public void NewInfo(Animal animal = null, BiomeSlot biomeSlot = null, bool isNegative = false, TypeOfConditions condition = TypeOfConditions.Null, int Index = 0)
    {
        bool newInfo = (animal != sAnimal || biomeSlot != sBiome);
        if(animal != null)
        {
            sAnimal = animal;
            if (condition != TypeOfConditions.Null)
            {
                sCondition = condition;
                sNegative = isNegative;
                sIndexSprite = Index;
            }
        }
        else
        {
            sAnimal = null;
        }
        if (biomeSlot != null)
        {
            sBiome = biomeSlot;
        }
        else
        {
            sBiome = null;
        }
        if (newInfo)
        {
            StopCoroutine(StartUIAnimation());
            StartCoroutine(StartUIAnimation());
        }
    }
    IEnumerator StartUIAnimation()
    {
        float elapsedTime = 0;
        float waitTime = 1f;
        currentPos = transform.position;
        //Hide
        while (elapsedTime < waitTime)
        {
            transform.position = Vector3.Lerp(currentPos, HideT, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }

        // Make sure we got there
        transform.position = HideT;

        UpdateInfo();

        elapsedTime = 0;
        waitTime = 1f;
        currentPos = transform.position;
        //Show
        while (elapsedTime < waitTime)
        {
            transform.position = Vector3.Lerp(currentPos, DisplayedT, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }
        yield return null;
    }
    void UpdateInfo()
    {
        if (sAnimal != null)
        {
            animalUI.SetActive(true);
            animalName.GetComponent<TMP_Text>().text = sAnimal.name;
            animalIcons[0] = iconsTaxon[sAnimal.]
        }
        else
        {
            animalUI.SetActive(false);
        }
        if (sBiome != null)
        {
            biomeUI.SetActive(true);
        }
        else
        {
            biomeUI.SetActive(false);
        }

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
            icon.sprite = icons[conditionGrupMod + numCon];
    }

}
