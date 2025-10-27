using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one AudioManager in scene");
        }
        instance = this;
    }

    public static void PlayOneShot(EventReference sound, Vector3 position)
    {
        RuntimeManager.PlayOneShot(sound, position);
    }
}
