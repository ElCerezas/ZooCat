using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;
    float musicVolume, soundVolume;
    [SerializeField] Canvas settingsMenu;
    [SerializeField] float showTime = 1f;
    [SerializeField] Vector3 hiddenPos, shownPos;
    bool animatingSettings = false;

    public AnimationCurve curve;

    [Header("FMOD")]
    [SerializeField] string pathMusica = "vca:/Musica";
    [SerializeField] string pathSo = "vca:/So";
    FMOD.Studio.VCA vcaMusica;
    FMOD.Studio.VCA vcaSo;

    [Header("PauseMenu")]
    bool displayedSettings = true;
    public delegate void GamePaused(bool isPaused);
    public static event GamePaused OnGamePaused;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            musicVolume = PlayerPrefs.GetFloat("musicVolume", 100);
            soundVolume = PlayerPrefs.GetFloat("soundVolume", 100);
            SetVolumes(musicVolume,soundVolume);
        }
        else
        {
            Destroy(this);
        }
    }
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            OnSettings();
        }
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
            float musicLog = Mathf.Pow(10f, (MusicVolume - 1f) * 2f);
            musicVolume = musicLog;
            PlayerPrefs.SetFloat("musicVolume", musicVolume);
            vcaMusica.setVolume(musicVolume);
        }
        if ((SoundVolume >= 0))
        {
            float soundLog = Mathf.Pow(10f, (SoundVolume - 1f) * 2f);
            soundVolume = soundLog;
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
            OnGamePaused?.Invoke(displayedSettings);
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
