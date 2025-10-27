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

    [Header("Particles")]
    [SerializeField]ParticleSystem particlesHappy;
    [SerializeField]ParticleSystem particlesAngry;
    [SerializeField]ParticleSystem particlesPop;

    [Header("Sound")]
    [SerializeField] AudioSource audioHappy;

    [SerializeField] Mood actualMood = Mood.Idle;

    Vector3 headOrigScale;
    Quaternion bodyOrigRot;
    Vector3 headOrigPos;
    Vector3 bodyOrigPos;
    private void Start()
    {
        particlesHappy.Stop(); particlesAngry.Stop();
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
            switch (newMood)
            {
                case Mood.Happy:
                    particlesHappy.Play(); particlesAngry.Stop();
                    if (audioHappy != null)
                    {
                        audioHappy.Play();
                    }
                    break;
                case Mood.Angry:
                    particlesHappy.Stop(); particlesAngry.Play(); break;
                default:
                    particlesHappy.Stop(); particlesAngry.Stop(); break;
            }
        }
    }
    private void Update()
    {
        switch (actualMood)
        {
            case Mood.Happy:
                break;
            case Mood.Angry:
                float shake = Mathf.Sin(Time.time * angryShakeSpeed) * angryShakeAmplitude;
                headRenderer.transform.localScale = headOrigScale * angryUpScaler;
                headRenderer.transform.localRotation = Quaternion.Euler(0, 0, shake);
                break;
            case Mood.Grabed:
                headRenderer.transform.localScale = Vector3.Lerp(headRenderer.transform.localScale, headOrigScale * pickUpScaler, Time.deltaTime * 10f);
                float swing = Mathf.Sin(Time.time * grabSwingSpeed) * grabSwingAmplitude;
                bodyRenderer.transform.localRotation = Quaternion.Euler(0, 0, swing);
                break;
            default: // Idle
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
