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
        cursorHotspot = new Vector2(4f, 4f);
        Cursor.SetCursor(DefaultCursorTexture, cursorHotspot, CursorMode.Auto);
    }
    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1))
            SetToMode(ModeOfCursor.Interact);
        else
            SetToMode(ModeOfCursor.Default);
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
