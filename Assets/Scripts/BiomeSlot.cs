using UnityEngine;
enum Temperatura {Fred, Templat, Calent}
enum Bioma {Platja, Panta, Muntanya}
public class BiomeSlot : MonoBehaviour
{
    [SerializeField] Bioma biome;
    [SerializeField] Temperatura temp;
    int id;
    [SerializeField] bool slotPle = false;
    [SerializeField] Animal animalInSlot;

    public bool SlotPle { get => slotPle; set => slotPle = value; }
}
