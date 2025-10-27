using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance {get; private set;}
        
    [SerializeField] private Texture2D DefaultCursorTexture;
    [SerializeField] private Texture2D InteractCursorTexture;
    
    private Vector2 cursorHotspot;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        cursorHotspot = new Vector2(DefaultCursorTexture.width / 2, DefaultCursorTexture.height / 2);
        Cursor.SetCursor(DefaultCursorTexture, cursorHotspot, CursorMode.Auto);
    }

    public void SetToMode (ModeOfCursor mode)
    {
        switch(mode)
        {
            case ModeOfCursor.Default:
                Cursor.SetCursor(DefaultCursorTexture, cursorHotspot, CursorMode.Auto);
                break;
            case ModeOfCursor.Interact:
                Cursor.SetCursor(InteractCursorTexture, cursorHotspot, CursorMode.Auto);
                break;
        }
    }
}

public enum ModeOfCursor
{
    Default,
    Interact
}
