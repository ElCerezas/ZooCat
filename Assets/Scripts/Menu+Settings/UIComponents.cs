using NaughtyAttributes;
using System;
using TMPro;
using UISwitcher;
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
        if (HasToggleTag)
        {
            if (gameObject.name == "MusicToggle") 
            { 
                gameObject.GetComponent<UISwitcher.UISwitcher>().isOn = (PlayerPrefs.GetInt("musicToggle", 1) == 1);
            }
            if (gameObject.name == "SoundToggle") 
            { 
                gameObject.GetComponent<UISwitcher.UISwitcher>().isOn = (PlayerPrefs.GetInt("soundToggle", 1) == 1);
            }
            ToggleSlider();
        }
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
    public void ToggleSlider()
    {
        bool t = gameObject.GetComponent<UISwitcher.UISwitcher>().isOn;
        slider.interactable = t;
        if (gameObject.name == "MusicToggle") { PlayerPrefs.SetInt("musicToggle", t ? 1 : 0); }
        if (gameObject.name == "SoundToggle") { PlayerPrefs.SetInt("soundToggle", t ? 1 : 0); }
    }
}
