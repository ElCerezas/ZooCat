using NaughtyAttributes;
using System;
using UISwitcher;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIComponents : MonoBehaviour
{
    private bool HasToggleTag => gameObject.CompareTag("toggle");
    [ShowIf("HasToggleTag")][SerializeField] Slider slider;
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
        float s = gameObject.GetComponent<Slider>().value;
        SettingsManager.instance.SetVolumes(SoundVolume:s);
    }
    public void OnSliderMusic()
    {
        float m = gameObject.GetComponent<Slider>().value;
        SettingsManager.instance.SetVolumes(m);
    }
    public void ToggleSlider()
    {
        slider.interactable = gameObject.GetComponent<UISwitcher.UISwitcher>().isOn;
    }
}
