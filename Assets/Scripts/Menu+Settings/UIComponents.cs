using UnityEngine;
using UnityEngine.SceneManagement;

public class UIComponents : MonoBehaviour
{
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
}
