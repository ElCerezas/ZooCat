using UnityEngine;
public enum Temperatura {Fred, Templat, Calent}
public enum Bioma {Platja, Panta, Muntanya}
public class BiomeSlot : MonoBehaviour
{
    [SerializeField] Bioma biome;
    [SerializeField] Temperatura temp;
    int id;
    [SerializeField] bool slotPle = false;
    [SerializeField] Animal animalInSlot;

    Color[] colors = { Color.white, Color.cyan, Color.yellow };
    [SerializeField] BiomeSlot[] ConditionRadius;
    SpriteRenderer SpriteRenderer;
    public bool SlotPle { get => slotPle; set => slotPle = value; }

    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        SetColorProximity(0);
    }
    void SetColorProximity(int proximityLvl)
    {
        SpriteRenderer.color = colors[proximityLvl];
    }
    private void OnMouseOver()
    {
        SetColorProximity(2);
        for (int i = 0; i < ConditionRadius.Length; i++)
        {
            ConditionRadius[i].SetColorProximity(1);
        }
    }
    private void OnMouseExit()
    {
        SetColorProximity(0);
        for (int i = 0; i < ConditionRadius.Length; i++)
        {
            ConditionRadius[i].SetColorProximity(0);
        }
    }
    
    public BiomeSlot[] GetCloseSlots()
    {
        return ConditionRadius;
    }
    public Bioma GetBioma()
    {
        return biome;
    }
    public Temperatura GetTemperatura()
    {
        return temp;
    }
}
