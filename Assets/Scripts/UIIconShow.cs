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
    [SerializeField] Image conditionIcon1, conditionIcon2;

    [Header("Sprites")]
    [SerializeField] Sprite[] iconsTaxon; //Mamifer, Reptil, Ocell, Anfibi, Peix
    [SerializeField] Sprite[] iconsFood; //Carnivor, Herbivor, Omnivor
    [SerializeField] Sprite[] iconsTransport; //Volador, Mari, Terrestre
    [SerializeField] Sprite[] iconsTemperature; //Fred, Templat, Calent
    [SerializeField] Sprite[] iconsBiome; //Aigua, Terra, Arbre, Aire

    [Header("Animation")]
    [SerializeField]Vector3 DisplayedT;
    [SerializeField]Vector3 HideT;
    [SerializeField]float upTime, downTime;
    Vector3 currentPos;

    [Header("Local Saves")]
    Animal sAnimal = null;
    BiomeSlot sBiome = null;

    bool sNegative1 = false;
    TypeOfConditions sCondition1 = TypeOfConditions.Null;
    int sIndexSprite1 = 0;

    bool sNegative2 = false;
    TypeOfConditions sCondition2 = TypeOfConditions.Null;
    int sIndexSprite2 = 0;


    void Awake()
    {
        instance = this;
    }
    public void NewInfo(Animal animal = null, BiomeSlot biomeSlot = null, bool isNegative = false, TypeOfConditions condition = TypeOfConditions.Null, int Index = 0, bool isNegative2 = false, TypeOfConditions condition2 = TypeOfConditions.Null, int Index2 = 0)
    {
        bool newInfo = (animal != sAnimal || biomeSlot != sBiome);
        if(animal != null)
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
            //UpdateInfo();
        }
    }
    IEnumerator StartUIAnimation()
    {
        float elapsedTime = 0;
        float waitTime = upTime;
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
        waitTime = downTime;
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
            animalName.GetComponent<TMP_Text>().text = sAnimal.animalName;
            animalIcons[0].sprite = iconsTaxon[sAnimal.taxon.GetHashCode()]; //Taxon
            animalIcons[1].sprite = iconsFood[sAnimal.dieta.GetHashCode()]; //Food
            animalIcons[2].sprite = iconsTransport[sAnimal.medi.GetHashCode()];//Terrain

            if(sCondition1 != TypeOfConditions.Null)
            {
                conditionIcon1.gameObject.SetActive(true);
                switch (sCondition1)
                {
                    case TypeOfConditions.CompanionTaxon:
                        conditionIcon1.sprite = iconsTaxon[sIndexSprite1];
                        break;
                    case TypeOfConditions.CompanionTransport:
                        conditionIcon1.sprite = iconsTransport[sIndexSprite1];
                        break;
                    case TypeOfConditions.CompanionFood:
                        conditionIcon1.sprite = iconsFood[sIndexSprite1];
                        break;
                    case TypeOfConditions.Temperature:
                        conditionIcon1.sprite = iconsTemperature[sIndexSprite1];
                        break;
                    case TypeOfConditions.Biome:
                        break; //TO DELETE
                        conditionIcon1.sprite = iconsBiome[sIndexSprite1];
                        break;
                }
                conditionIcon1.color = sNegative1 ? Color.red : Color.green;
            }
            else
            {
                conditionIcon1.gameObject.SetActive(false);
            }

            if (sCondition2 != TypeOfConditions.Null)
            {
                conditionIcon2.gameObject.SetActive(true);
                switch (sCondition2)
                {
                    case TypeOfConditions.CompanionTaxon:
                        conditionIcon2.sprite = iconsTaxon[sIndexSprite2];
                        break;
                    case TypeOfConditions.CompanionTransport:
                        conditionIcon2.sprite = iconsTransport[sIndexSprite2];
                        break;
                    case TypeOfConditions.CompanionFood:
                        conditionIcon2.sprite = iconsFood[sIndexSprite2];
                        break;
                    case TypeOfConditions.Temperature:
                        conditionIcon2.sprite = iconsTemperature[sIndexSprite2];
                        break;
                    case TypeOfConditions.Biome:
                        break; //TO DELETE
                        conditionIcon2.sprite = iconsBiome[sIndexSprite2];
                        break;
                }
                conditionIcon2.color = sNegative2 ? Color.red : Color.green;
            }
            else
            {
                conditionIcon2.gameObject.SetActive(false);
            }
        }
        else
        {
            animalUI.SetActive(false);
        }
        if (sBiome != null)
        {
            biomeIcons[0].sprite = iconsTemperature[sBiome.temp.GetHashCode()]; //Temperature
            //biomeIcons[0].sprite = iconsBiome[sBiome.biome.GetHashCode()]; //Biome

            biomeUI.SetActive(true);
        }
        else
        {
            biomeUI.SetActive(false);
        }
    }

}
