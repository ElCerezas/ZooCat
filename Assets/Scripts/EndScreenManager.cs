using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.tvOS;
using UnityEngine.UI;

public class EndScreenManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] GameObject Bg;
    [SerializeField] GameObject titleText;
    [SerializeField] GameObject txt1;
    [SerializeField] GameObject timer;
    [SerializeField] GameObject score;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text timerText;
    [SerializeField] Image SealOfAproval,SealOfAprovalShadow;
    [SerializeField] GameObject returnButton;

    [Header("Animation Settings")]
    [SerializeField] float fadeInDuration = 0.5f;
    [SerializeField] float numberAnimDuration = 3f;
    [SerializeField] AnimationCurve numberCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] AnimationCurve popCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Stamps")]
    [SerializeField] Sprite[] stamps;

    [Header("Debug")]
    [SerializeField] bool debugTest = false;
    [SerializeField] float testTime = 135.7f;
    [SerializeField] int testScore = 8742;

    int maxScorePossible = 1714; //Visca Catalunya ostiaaaaaa
    int howManyStars;

    private void Start()
    {
        if (debugTest)
        {
            StartEndScreenShow(testTime);
        }
    }
    public void StartEndScreenShow(float time, float minTime = 1f * 60f, float maxTime = 5f * 60f)
    {
        StopAllCoroutines();

        StartCoroutine(EndScreenSequence(time, CalculateFinalScore(time, minTime, maxTime)));
    }
    int CalculateFinalScore(float t, float minTime, float maxTime)
    {
        int Score = 0;
        if (t <= minTime)
        {
            Score = maxScorePossible;
        }
        else if (t >= maxTime)
        {
            Score = 50;
        }
        else
        {
            float timeRange = maxTime - minTime;
            float timeRatio = (t - minTime) / timeRange;

            float scoreFactor = (1.0f - timeRatio) * (1.0f - timeRatio);

            Score = Mathf.Max((int)(maxScorePossible * scoreFactor), 50);
        }

        if (Score <= (maxScorePossible / 3f))
            howManyStars = 1;
        else if (Score > (maxScorePossible / 3) && Score <= (maxScorePossible / 3) * 2)
            howManyStars = 2;
        else
            howManyStars = 3;

            return Score;
    }
    IEnumerator EndScreenSequence(float totalTime, int finalScore)
    {
        timerText.text = "00:00";
        scoreText.text = "0000 punts";

        yield return FadeCanvasGroup();

        yield return AnimatePop(titleText.transform, 1.2f);
        yield return new WaitForSeconds(0.4f);

        yield return AnimatePop(txt1.transform, 1.1f);
        yield return AnimatePop(timer.transform, 1.1f);
        yield return StartCoroutine(AnimateTimer(timerText, totalTime));

        yield return new WaitForSeconds(0.6f);

        yield return AnimatePop(score.transform, 1.1f);
        yield return StartCoroutine(AnimateScore(scoreText, finalScore));

        yield return new WaitForSeconds(0.3f);
        SealOfAproval.sprite = stamps[howManyStars - 1];
        SealOfAprovalShadow.sprite = stamps[howManyStars - 1];
        yield return ApprovalSeal(SealOfAproval.transform, SealOfAprovalShadow.transform);
        yield return AnimatePop(returnButton.transform, 1.1f);
    }
    IEnumerator AnimateTimer(TMP_Text text, float finalSeconds)
    {
        float elapsed = 0;
        while (elapsed < numberAnimDuration)
        {
            elapsed += Time.deltaTime;
            float t = numberCurve.Evaluate(elapsed / numberAnimDuration);
            float current = Mathf.Lerp(0, finalSeconds, t);
            text.text = FormatTime(current);
            yield return null;
        }
        text.text = FormatTime(finalSeconds);
    }
    IEnumerator AnimateScore(TMP_Text text, int finalScore)
    {
        float elapsed = 0;
        while (elapsed < numberAnimDuration)
        {
            elapsed += Time.deltaTime;
            float t = numberCurve.Evaluate(elapsed / numberAnimDuration);
            int current = Mathf.RoundToInt(Mathf.Lerp(0, finalScore, t));
            text.text = current.ToString("0000") + " punts";
            yield return null;
        }
        text.text = finalScore.ToString("0000") + " punts";
    }
    string FormatTime(float seconds)
    {
        int mins = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);
        return $"{mins:00}:{secs:00}";
    }
    IEnumerator FadeCanvasGroup()
    {
        Bg.SetActive(true);
        yield return null;
    }
    IEnumerator AnimatePop(Transform target, float scaleUp)
    {
        Vector3 original = target.localScale;
        target.localScale = Vector3.zero;
        target.gameObject.SetActive(true);

        float duration = fadeInDuration;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            float curveValue = popCurve.Evaluate(t);

            float scale = Mathf.Lerp(0, scaleUp, curveValue);
            target.localScale = original * scale;

            yield return null;
        }

        float finalScale = Mathf.Lerp(target.localScale.x, original.x, 0.1f);
        target.localScale = new Vector3(finalScale, finalScale, finalScale);
    }
    IEnumerator ApprovalSeal(Transform Seal, Transform Shadow)
    {
        float startScaleMultiplier = 2f;
        float endScaleMultiplier = 1f;
        float duration = 0.55f;
        float dropHeight = 200f;
        float shadowTargetAlpha = 0.75f;
        float sealLateralOffset = -100f;
        float shadowLateralOffset = 160f;
        float shadowDepthOffset = -40f;
        float shadowStartScaleMul = 0.8f;
        float rotationMax = 8f;
        float elapsed = 0f;

        Image shadowImage = Shadow.GetComponent<Image>();
        SpriteRenderer shadowSprite = Shadow.GetComponent<SpriteRenderer>();

        Vector3 finalSealScale = Seal.localScale;
        Vector3 finalSealPos = Seal.localPosition;
        Quaternion finalSealRot = Seal.localRotation;

        Vector3 startSealScale = finalSealScale * startScaleMultiplier;
        Vector3 initialPos = finalSealPos + Vector3.up * dropHeight + Vector3.right * sealLateralOffset;

        Vector3 finalShadowScale = (Shadow.localScale == Vector3.zero) ? Vector3.one : Shadow.localScale;
        Vector3 startShadowScale = finalShadowScale * shadowStartScaleMul;
        Vector3 initialShadowPos = finalSealPos + Vector3.up * (dropHeight * 0.4f) + new Vector3(shadowLateralOffset, shadowDepthOffset, 0f);

        Quaternion startRot = Quaternion.Euler(0f, 0f, rotationMax * Mathf.Sign(sealLateralOffset));

        Seal.localScale = startSealScale;
        Seal.localPosition = initialPos;
        Seal.localRotation = startRot;

        Shadow.localScale = startShadowScale;
        Shadow.localPosition = initialShadowPos;
        SetGraphicAlpha(shadowImage, shadowSprite, 0f);

        Seal.gameObject.SetActive(true);
        Shadow.gameObject.SetActive(true);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float eased = t * t * t;

            Seal.localScale = Vector3.Lerp(startSealScale, finalSealScale * endScaleMultiplier, eased);
            Seal.localPosition = Vector3.Lerp(initialPos, finalSealPos, eased);
            Seal.localRotation = Quaternion.Slerp(startRot, finalSealRot, eased);

            Shadow.localPosition = Vector3.Lerp(initialShadowPos, finalSealPos + new Vector3(20f, -20f, 0f), eased);
            Shadow.localScale = Vector3.Lerp(startShadowScale, finalShadowScale, eased);
            SetGraphicAlpha(shadowImage, shadowSprite, Mathf.Lerp(0f, shadowTargetAlpha, eased));

            yield return null;
        }

        Seal.localScale = finalSealScale * endScaleMultiplier;
        Seal.localPosition = finalSealPos;
        Seal.localRotation = finalSealRot;
        Shadow.localPosition = finalSealPos + new Vector3(20f, -20f, 0f);
        Shadow.localScale = finalShadowScale;
        SetGraphicAlpha(shadowImage, shadowSprite, shadowTargetAlpha);

        yield return StartCoroutine(StampImpact(Seal));
    }
    IEnumerator StampImpact(Transform Seal)
    {
        float duration = 0.16f;
        float half = duration * 0.5f;
        float elapsed = 0f;

        Vector3 baseScale = Seal.localScale;
        Vector3 overshootScale = baseScale * 1.12f;

        Quaternion baseRot = Seal.localRotation;
        float rotAmount = 4f;
        Quaternion rotLeft = Quaternion.Euler(0f, 0f, rotAmount);
        Quaternion rotRight = Quaternion.Euler(0f, 0f, -rotAmount);

        while (elapsed < half)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / half);
            float eased = 1f - Mathf.Pow(1f - t, 3f);
            Seal.localScale = Vector3.Lerp(baseScale, overshootScale, eased);
            Seal.localRotation = Quaternion.Slerp(baseRot, rotLeft, eased);
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < half)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / half);
            float eased = 1f - Mathf.Pow(1f - t, 3f);
            Seal.localScale = Vector3.Lerp(overshootScale, baseScale, eased);
            Seal.localRotation = Quaternion.Slerp(rotLeft, baseRot, eased);
            yield return null;
        }

        Seal.localScale = baseScale;
        Seal.localRotation = baseRot;
    }
    void SetGraphicAlpha(Image img, SpriteRenderer sr, float alpha)
    {
        if (img != null)
        {
            Color c = img.color;
            c.a = alpha;
            img.color = c;
        }
        else if (sr != null)
        {
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }
    }

}

