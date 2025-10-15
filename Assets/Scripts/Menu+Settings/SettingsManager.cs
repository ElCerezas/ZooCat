using System.Collections;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    float musicVolume, soundVolume;
    int soundToggle, musicToggle;
    public static SettingsManager instance;
    [SerializeField] Canvas settingsMenu;
    bool displayedSettings = true;
    [SerializeField] float showTime = 1f;
    [SerializeField] Vector3 hiddenPos, shownPos;
    bool animatingSettings = false;

    public AnimationCurve curve;
    private void Awake()
    {
        if (instance == null)
        {
            musicVolume = PlayerPrefs.GetFloat("musicVolume", 100);
            soundVolume = PlayerPrefs.GetFloat("soundVolume", 100);
            musicToggle = PlayerPrefs.GetInt("musicToggle", 1);
            soundToggle = PlayerPrefs.GetInt("musicToggle", 1);
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(settingsMenu.gameObject);
            instance = this;
        }
        else
        {
            Destroy(settingsMenu.gameObject);
            Destroy(this);
        }
    }
    public void SetVolumes(float MusicVolume = -1, float SoundVolume = -1)
    {
        if ((MusicVolume >= 0))
        {
            musicVolume = MusicVolume;
            PlayerPrefs.SetFloat("musicVolume", musicVolume);
        }
        if ((SoundVolume >= 0))
        {
            soundVolume = SoundVolume;
            PlayerPrefs.SetFloat("soundVolume", soundVolume);
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
