using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EndScreenManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] GameObject Bg;
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text txt1;
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text txt2;
    [SerializeField] TMP_Text scoreText;

    [Header("Animation Settings")]
    [SerializeField] float fadeInDuration = 0.5f;
    [SerializeField] float numberAnimDuration = 3f;
    [SerializeField] AnimationCurve numberCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] AnimationCurve popCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Debug")]
    [SerializeField] bool debugTest = false;
    [SerializeField] float testTime = 135.7f;
    [SerializeField] int testScore = 8742;

    int maxScorePossible = 1714; //Visca Catalunya ostiaaaaaa

    private void Start()
    {
        if (debugTest)
        {
            StartEndScreenShow(testTime);
        }
    }
    public void StartEndScreenShow(float time, float minTime = 2f * 60f, float maxTime = 20f * 60f)
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
        yield return AnimatePop(timerText.transform, 1.1f);
        yield return StartCoroutine(AnimateTimer(timerText, totalTime));

        yield return new WaitForSeconds(0.6f);

        yield return AnimatePop(txt2.transform, 1.1f);
        yield return AnimatePop(scoreText.transform, 1.1f);
        yield return StartCoroutine(AnimateScore(scoreText, finalScore));
    }
    private IEnumerator AnimateTimer(TMP_Text text, float finalSeconds)
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
    private IEnumerator AnimateScore(TMP_Text text, int finalScore)
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
    private string FormatTime(float seconds)
    {
        int mins = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);
        return $"{mins:00}:{secs:00}";
    }
    private IEnumerator FadeCanvasGroup()
    {
        float t = 0;
        //TO DO ApareixerMillor
        Bg.SetActive(true);
        yield return null;
    }
    private IEnumerator AnimatePop(Transform target, float scaleUp)
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
}
