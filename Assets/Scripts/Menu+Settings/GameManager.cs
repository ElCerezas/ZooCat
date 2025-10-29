using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.P)&& Input.GetKey(KeyCode.U)&&Input.GetKey(KeyCode.T)&& Input.GetKey(KeyCode.A))
        {
            PlayerPrefs.DeleteAll();
        }
    }
    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

#if !UNITY_EDITOR && UNITY_WEBGL

        [System.Runtime.InteropServices.DllImport("__Internal")]
        public static extern bool IsMobileBrowser();
      
        [System.Runtime.InteropServices.DllImport("__Internal")]
        public static extern bool IsPreferredDesktopPlatform();
#else
    public static bool IsMobileBrowser() => false;
    public static bool IsPreferredDesktopPlatform() => true;
#endif
}
