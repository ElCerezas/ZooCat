using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [Header("Level")]
    public bool isUnlockedByDefault;
    public int LevelToGo;
    [SerializeField] bool isFake = false;
    public bool IsFake => isFake; // getter público

    [Header("Button")]
    Button button;
    Image image;

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    // isUnlocked: estado que venga del manager (PlayerPrefs o default)
    public void SetUp(bool isUnlocked)
    {
        // Si es fake, forzamos a locked visualmente
        if (isFake)
        {
            isUnlocked = false;
        }

        button.interactable = isUnlocked;

        // evitar listeners duplicados
        button.onClick.RemoveAllListeners();

        if (isUnlocked)
        {
            button.onClick.AddListener(LoadLevel);
            image.color = Color.white;
        }
        else
        {
            // Si quieres mostrar un icono de candado pon un sprite o similar
            image.color = Color.black;
        }
    }

    // Activa el botón (usado por manager)
    public void Unlock()
    {
        if (isFake) return;

        button.interactable = true;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(LoadLevel);
        image.color = Color.white;
    }

    public void LoadLevel()
    {
        GameManager.instance.GoToScene($"Level {LevelToGo}");
    }
}
