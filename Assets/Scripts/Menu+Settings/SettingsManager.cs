using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;
    float musicVolume, soundVolume;
    [SerializeField] Canvas settingsMenu;
    bool displayedSettings = true;
    [SerializeField] float showTime = 1f;
    [SerializeField] Vector3 hiddenPos, shownPos;
    bool animatingSettings = false;

    public AnimationCurve curve;

    [Header("FMOD")]
    [SerializeField] string pathMusica = "vca:/Musica";
    [SerializeField] string pathSo = "vca:/So";
    FMOD.Studio.VCA vcaMusica;
    FMOD.Studio.VCA vcaSo;


    private void Awake()
    {
        instance = this;
        musicVolume = PlayerPrefs.GetFloat("musicVolume", 100);
        soundVolume = PlayerPrefs.GetFloat("soundVolume", 100);
    }
    private void Start()
    {
        vcaMusica = FMODUnity.RuntimeManager.GetVCA(pathMusica);
        vcaSo = FMODUnity.RuntimeManager.GetVCA(pathSo);
        vcaMusica.setVolume(musicVolume);
        vcaSo.setVolume(soundVolume);
    }
    public void SetVolumes(float MusicVolume = -1, float SoundVolume = -1)
    {
        if ((MusicVolume >= 0))
        {
            musicVolume = MusicVolume;
            PlayerPrefs.SetFloat("musicVolume", musicVolume);
            vcaMusica.setVolume(musicVolume);
        }
        if ((SoundVolume >= 0))
        {
            soundVolume = SoundVolume;
            PlayerPrefs.SetFloat("soundVolume", soundVolume);
            vcaSo.setVolume(soundVolume);
        }
    }
    public float GetSoundVolume()
    {
        return soundVolume;
    }
    public float GetMusicVolume()
    {
        return musicVolume;
    }
    public void OnSettings()
    {
        displayedSettings = !displayedSettings;
        if(!animatingSettings)
        {
            animatingSettings = true;
            StartCoroutine(ShowOrHide());
        }
    }
    IEnumerator ShowOrHide()
    {
        float elapsedTime = 0;
        float waitTime = showTime;
        Vector2 currentPos = settingsMenu.gameObject.transform.position;

        if (displayedSettings)
        {
            //Hide
            while (elapsedTime < waitTime)
            {
                float t1 = curve.Evaluate(elapsedTime / waitTime);
                settingsMenu.gameObject.transform.position = Vector3.Lerp(currentPos, hiddenPos, t1);
                elapsedTime += Time.deltaTime;

                // Yield here
                yield return null;
            }
        }
        else
        {
            //Show
            while (elapsedTime < waitTime)
            {
                float t1 = curve.Evaluate(elapsedTime / waitTime);
                settingsMenu.gameObject.transform.position = Vector3.Lerp(currentPos, shownPos, t1);
                elapsedTime += Time.deltaTime;

                // Yield here
                yield return null;
            }
        }
        animatingSettings = false;
        yield return null;
    }
}
