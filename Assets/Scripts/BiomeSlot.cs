using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] float radius = 1f;
    [SerializeField] List<BiomeSlot> NearSlots = new List<BiomeSlot>();

    [Header("Highlighter")]
    [SerializeField] Color highLight = Color.white;
    [SerializeField] Color lowLight = Color.darkGray;
    [SerializeField] float highlightSpeed = 1f;
    Coroutine highlightCoroutine;
    [SerializeField]SpriteRenderer SpriteRenderer;
    public static bool AllHighLighted = false;
    public bool SlotPle { get => slotPle; set => slotPle = value; }

    private void Start()
    {
        OnHighLight(false);
        SetColorProximity(0);
        Collider2D[] t = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y+ 0.75f), radius);
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
        //SpriteRenderer.color = colors[proximityLvl];
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
        OnHighLight(false);
        //SetColorProximity(0);
        for (int i = 0; i < NearSlots.Count; i++)
        {
            NearSlots[i].SetColorProximity(0);
        }
    }
    private void OnMouseEnter()
    {
        OnHighLight(true);
    }
    private void OnMouseDown()
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
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y + 0.75f), radius);
    }
    public void OnHighLight(bool inHighlight)
    {
        if (highlightCoroutine != null)
            StopCoroutine(highlightCoroutine);
        Color targetColor;
        if (AllHighLighted)
        {
            targetColor = highLight;
            highlightCoroutine = StartCoroutine(SmoothHighlight(targetColor));
            return;
        }

        targetColor = inHighlight ? highLight : lowLight;

        highlightCoroutine = StartCoroutine(SmoothHighlight(targetColor));
    }
    private IEnumerator SmoothHighlight(Color targetColor)
    {
        Color startColor = SpriteRenderer.color;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * highlightSpeed;
            SpriteRenderer.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        SpriteRenderer.color = targetColor;
    }
}
