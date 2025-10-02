using UnityEngine;
enum Temperatura {Fred, Templat, Calent}
enum Bioma {Platja, Panta, Muntanya}
[CreateAssetMenu(fileName = "Biome", menuName = "Scriptable Objects/Biome")]
public class Biome : ScriptableObject
{
    [SerializeField] Bioma biome;
    [SerializeField] Temperatura temp;
    int id;
    bool slotPle = false;
}
