using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    public static void PlayOneShot(EventReference sound)
    {
        RuntimeManager.PlayOneShot(sound);
    }
}
