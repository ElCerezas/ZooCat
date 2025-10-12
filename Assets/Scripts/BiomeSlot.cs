using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
public enum Temperatura {Fred, Templat, Calent}
public enum Bioma {Aigua, Terra, Arbre, Aire}
public class BiomeSlot : MonoBehaviour
{
    [Header("Biome Stats")]
    public Bioma biome;
    public Temperatura temp;

    [Header("Animal in")]
    [SerializeField] bool slotPle = false;
    [SerializeField] Animal animalInSlot;

    [Header("Near Slots")]
    Color[] colors = { Color.white, Color.cyan, Color.yellow };
    [SerializeField] float radius = 1f;
    [SerializeField] List<BiomeSlot> NearSlots = new List<BiomeSlot>();
    SpriteRenderer SpriteRenderer;
    public bool SlotPle { get => slotPle; set => slotPle = value; }

    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        SetColorProximity(0);
        Collider2D[] t = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), radius);
        for (int i = 0; i < t.Length; i++)
        {
            if (t[i].gameObject.tag == "Slot" && t[i].gameObject != this.gameObject)
            {
                NearSlots.Add(t[i].gameObject.GetComponent<BiomeSlot>());
            }
        }
    }
    void SetColorProximity(int proximityLvl)
    {
        SpriteRenderer.color = colors[proximityLvl];
    }
    private void OnMouseOver()
    {
        SetColorProximity(2);
        for (int i = 0; i < NearSlots.Count; i++)
        {
            NearSlots[i].SetColorProximity(1);
        }
    }
    private void OnMouseExit()
    {
        SetColorProximity(0);
        for (int i = 0; i < NearSlots.Count; i++)
        {
            NearSlots[i].SetColorProximity(0);
        }
    }
    private void OnMouseEnter()
    {
        UIIconShow.instance.NewInfo(animalInSlot, this);
    }
    public List<BiomeSlot> GetCloseSlots()
    {
        return NearSlots;
    }
    public Bioma GetBioma()
    {
        return biome;
    }
    public Temperatura GetTemperatura()
    {
        return temp;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y), radius);
    }
}
