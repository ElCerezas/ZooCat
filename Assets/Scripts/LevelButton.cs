using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [Header("Level")]
    public bool isUnlockedByDefault;
    public int LevelToGo;
    [SerializeField] bool isFake = false;

    [Header("Button")]
    Button button;
    Image image;

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    public void SetUp(bool isUnlocked)
    {
        button.interactable = isUnlocked;

        if(isUnlocked)
        {
            button.onClick.AddListener(LoadLevel);
            image.color = Color.white;
            //gameObject.transform.parent = GameObject.FindGameObjectWithTag("Manager").transform;
        }
        else
        {
            image.color = Color.black;

        }
    }
    public void Unlock()
    {
        if (isFake)
        {
            return;
        }
        button.interactable = true;
        button.onClick.AddListener(LoadLevel);
        image.color = Color.white;
    }
    public void LoadLevel()
    {
        GameManager.instance.GoToScene($"Level {LevelToGo}");
    }
}