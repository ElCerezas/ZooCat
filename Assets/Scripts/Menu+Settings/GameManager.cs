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
}
