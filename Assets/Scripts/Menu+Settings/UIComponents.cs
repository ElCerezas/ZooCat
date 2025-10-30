using NaughtyAttributes;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIComponents : MonoBehaviour
{
    bool HasToggleTag => gameObject.CompareTag("toggle");
    [ShowIf("HasToggleTag")][SerializeField] Slider slider;
    bool IsSlider => gameObject.GetComponent<Slider>() != null;
    [ShowIf("IsSlider")][SerializeField] TextMeshProUGUI percentage;
    private void Start()
    {
        if (gameObject.name == "MusicSlider") 
        { 
            gameObject.GetComponent<Slider>().value = SettingsManager.instance.GetMusicVolume() * 100;
            OnSliderMusic();
        }
        if (gameObject.name == "SoundSlider") 
        { 
            gameObject.GetComponent<Slider>().value = SettingsManager.instance.GetSoundVolume() *100;
            OnSliderSound();
        }
    }
    public void OnSettings()
    {
        SettingsManager.instance.OnSettings();
        
    }
    public void OnGoToScene(string scene)
    {
        GameManager.instance.GoToScene(scene);
    }
    public void OnExitGame()
    {
        Application.Quit();
    }
    public void OnSliderSound()
    {
        float s = gameObject.GetComponent<Slider>().value /  100;
        percentage.text = $"{(s * 100).ToString("00")}%";
        SettingsManager.instance.SetVolumes(SoundVolume:s);
    }
    public void OnSliderMusic()
    {
        float m = gameObject.GetComponent<Slider>().value / 100;
        percentage.text = $"{(m * 100).ToString("00")}%";
        SettingsManager.instance.SetVolumes(MusicVolume:m);
    }
}
