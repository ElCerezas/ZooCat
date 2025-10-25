using UnityEngine;
using UnityEngine.Rendering;

public enum Mood { Happy, Angry, Grabed, Idle}
public class AnimalAnimator : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite head;
    public Sprite body;

    [Header("AnimationSettings")]
    [SerializeField] float pickUpScaler = 1.3f;
    [SerializeField] float grabSwingSpeed = 3f;
    [SerializeField] float grabSwingAmplitude = 10f;
    [SerializeField] float angryUpScaler = 1.5f;
    [SerializeField] float angryShakeSpeed = 15f;
    [SerializeField] float angryShakeAmplitude = 5f;

    [Header("Renderer")]
    public SpriteRenderer headRenderer;
    public SpriteRenderer bodyRenderer;

    [SerializeField] Mood actualMood = Mood.Idle;

    Vector3 headOrigScale;
    Quaternion bodyOrigRot;
    Vector3 headOrigPos;
    Vector3 bodyOrigPos;
    private void Start()
    {
        headRenderer.sprite = head;
        bodyRenderer.sprite = body;

        headOrigScale = headRenderer.transform.localScale;
        bodyOrigRot = bodyRenderer.transform.localRotation;
        headOrigPos = headRenderer.transform.localPosition;
        bodyOrigPos = bodyRenderer.transform.localPosition;
    }
    public void OnMoodSwap(Mood newMood)
    {
        if (newMood != actualMood)
        {
            ResetProperties();
            actualMood = newMood;
        }

    }
    private void Update()
    {
        switch (actualMood)
        {
            case Mood.Happy:
                //No ce
                break;
            case Mood.Angry:
                //headRenderer.color = Color.red;
                float shake = Mathf.Sin(Time.time * angryShakeSpeed) * angryShakeAmplitude;
                headRenderer.transform.localScale = headOrigScale * angryUpScaler;
                headRenderer.transform.localRotation = Quaternion.Euler(0, 0, shake);
                break;
            case Mood.Grabed:
                headRenderer.color = Color.white;

                headRenderer.transform.localScale = Vector3.Lerp(headRenderer.transform.localScale, headOrigScale * pickUpScaler, Time.deltaTime * 10f);

                // cuerpo balance�ndose
                float swing = Mathf.Sin(Time.time * grabSwingSpeed) * grabSwingAmplitude;
                bodyRenderer.transform.localRotation = Quaternion.Euler(0, 0, swing);
                break;
            default: // Idle
                headRenderer.color = Color.white;
                break;
        }
    }
    private void ResetProperties()
    {
        headRenderer.transform.localScale = headOrigScale;
        headRenderer.transform.localPosition = headOrigPos;
        headRenderer.transform.localRotation = Quaternion.identity;

        bodyRenderer.transform.localRotation = bodyOrigRot;
        bodyRenderer.transform.localPosition = bodyOrigPos;
        bodyRenderer.transform.localScale = Vector3.one;
    }
}
