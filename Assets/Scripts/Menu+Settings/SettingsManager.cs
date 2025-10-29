using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour 
{ 
    public static SettingsManager instance; 

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
    [SerializeField] int ActualScene = -1;

    float musicLinear, soundLinear; 
    float musicVolume, soundVolume;

    private void Awake() 
    { 
        if (instance == null) 
        { 
            instance = this;
            DontDestroyOnLoad(gameObject);
            musicLinear = PlayerPrefs.GetFloat("musicVolume", 1f);
            soundLinear = PlayerPrefs.GetFloat("soundVolume", 1f);
        } 
        else 
        { 
            Destroy(this); 
        } 
    } 
    private void Start() 
    {
        vcaMusica = FMODUnity.RuntimeManager.GetVCA(pathMusica);
        vcaSo = FMODUnity.RuntimeManager.GetVCA(pathSo);

        SetVolumes(musicLinear, soundLinear);
    } 
    public void SetVolumes(float MusicVolume = -1, float SoundVolume = -1) 
    {
        vcaMusica = FMODUnity.RuntimeManager.GetVCA(pathMusica);
        vcaSo = FMODUnity.RuntimeManager.GetVCA(pathSo);

        if (MusicVolume >= 0)
        {
            musicLinear = MusicVolume;
            musicVolume = (musicLinear <= 0f) ? 0f : Mathf.Pow(10f, (musicLinear - 1f) * 2f);
            PlayerPrefs.SetFloat("musicVolume", musicLinear);
            vcaMusica.setVolume(musicVolume);
        }

        if (SoundVolume >= 0)
        {
            soundLinear = SoundVolume;
            soundVolume = (soundLinear <= 0f) ? 0f : Mathf.Pow(10f, (soundLinear - 1f) * 2f);
            PlayerPrefs.SetFloat("soundVolume", soundLinear);
            vcaSo.setVolume(soundVolume);
        }
    } 
    public float GetSoundVolume() 
    { 
        return soundLinear; 
    } 
    public float GetMusicVolume() 
    { 
        return musicLinear; 
    } 
    public void OnSettings() 
    { 
        displayedSettings = !displayedSettings; 
        if (!animatingSettings) 
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
                settingsMenu.gameObject.transform.position = Vector3.Lerp(currentPos, hiddenPos, t1); elapsedTime += Time.deltaTime;
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
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        StartCoroutine(ApplyVolumeAfterDelay());
    } 
    IEnumerator ApplyVolumeAfterDelay() 
    {
        yield return new WaitForSeconds(0.1f);

        vcaMusica = FMODUnity.RuntimeManager.GetVCA(pathMusica);
        vcaSo = FMODUnity.RuntimeManager.GetVCA(pathSo);
        SetVolumes(musicLinear, soundLinear);
    } 
    private void OnEnable() { 
        SceneManager.sceneLoaded += OnSceneLoaded; 
    } 
    private void OnDisable() 
    { 
        SceneManager.sceneLoaded -= OnSceneLoaded; 
    } 
}