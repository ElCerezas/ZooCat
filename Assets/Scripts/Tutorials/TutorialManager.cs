using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TutorialPhase
{
    ClickSlot,
    ShowSlotUI,
    ClickAnimal,
    ShowAnimalUI,
    ShowConditionsUI,
    DragAnimalWrong,
    DragAnimalRight,
    ShowTimer,
    ShowStartCanvas
}

public class TutorialManager : LevelManager
{
    [Header("Tutorial")]
    public TutorialPhase currentPhase = TutorialPhase.ClickSlot;
    public SpriteRenderer fakeCursor;       // Cursor falso
    public TextMeshProUGUI tutorialText;    // Texto explicativo
    public GameObject startCanvas;          // Canvas final �Empezar nivel�
    public float cursorTransparencyDistance = 0.5f;
    public float cursorMinAlpha = 0.3f;
    public Image Mask;
    public Sprite[] maskPhases;
    public float yOffset = 0.75f;

    private Coroutine cursorAnimRoutine;
    private Color baseCursorColor;
    private bool cursorAnimating = false;

    // Control de progreso
    private bool clickedSlot = false;
    private bool hoveredSlotUI = false;
    private bool clickedAnimal = false;
    private bool hoveredAnimalUI = false;
    private bool hoveredConditionsUI = false;
    private bool dragToWrongSlot = false;
    private bool dragToRightSlot = false;

    public BiomeSlot[] targetSlot;
    public Animal targetAnimal;

    public Vector3 SlotUI;
    public Vector3 AnimalUI;
    public Vector3 ConditionUI;

    void Start()
    {
        Mask.sprite = maskPhases[0];
        startCanvas.SetActive(false);
        tutorialText.gameObject.SetActive(true);

        baseCursorColor = fakeCursor.color;
        fakeCursor.transform.localScale = Vector3.one;
        if(GameManager.IsPreferredDesktopPlatform())
            StartCoroutine(RunTutorialPC());
        else
            StartCoroutine(RunTutorialMobile());
    }
    IEnumerator RunTutorialPC()
    {
        // Fase 1
        Mask.sprite = maskPhases[1];
        targetAnimal.selectable = false;
        targetSlot[0].OnHighLight(true);
        currentPhase = TutorialPhase.ClickSlot;
        tutorialText.text = "Fes clic sobre l'espai per veure la seva informació";
        StartCursorAnimation(targetSlot[0].transform.position + Vector3.up * yOffset);
        yield return new WaitUntil(() => clickedSlot);
        StopCursorAnimation();

        // Fase 2
        Mask.sprite = maskPhases[2];
        currentPhase = TutorialPhase.ShowSlotUI;
        tutorialText.text = "Investiga quines propietats té l'espai";
        StartCursorAnimation(SlotUI);
        yield return new WaitUntil(() => hoveredSlotUI);
        StopCursorAnimation();

        // Fase 3
        Mask.sprite = maskPhases[3];
        currentPhase = TutorialPhase.ClickAnimal;
        tutorialText.text = "Fes clic sobre l'animal per veure informació de l'animal";
        StartCursorAnimation(targetAnimal.transform.position + Vector3.up * yOffset);
        yield return new WaitUntil(() => clickedAnimal);
        StopCursorAnimation();

        // Fase 4
        Mask.sprite = maskPhases[4];
        currentPhase = TutorialPhase.ShowAnimalUI;
        tutorialText.text = "Investiga quines propietats té l'animal";
        StartCursorAnimation(AnimalUI);
        yield return new WaitUntil(() => hoveredAnimalUI);
        StopCursorAnimation();

        // Fase 5
        Mask.sprite = maskPhases[5];
        currentPhase = TutorialPhase.ShowConditionsUI;
        tutorialText.text = "Observa les preferències de l'animal";
        StartCursorAnimation(ConditionUI);
        yield return new WaitUntil(() => hoveredConditionsUI);
        StopCursorAnimation();

        // Fase 6   
        Mask.sprite = maskPhases[6];
        targetAnimal.selectable = true;
        targetSlot[0].SlotPle = false;
        targetSlot[1].SlotPle = true;
        currentPhase = TutorialPhase.DragAnimalWrong;
        tutorialText.text = "Arrossega l'animal a l'espai per col·locar-lo";
        StartCursorAnimation(SlotPathAnimation(false));
        yield return new WaitUntil(() => dragToWrongSlot);
        StopCursorAnimation();

        // Fase 7
        Mask.sprite = maskPhases[7];
        targetSlot[1].SlotPle = false;
        currentPhase = TutorialPhase.DragAnimalRight;
        tutorialText.text = "Vaja! Ara no està content. Provem de posar-lo a un lloc que sí que vulgui estar!";
        StartCursorAnimation(SlotPathAnimation(true));
        yield return new WaitUntil(() => dragToRightSlot);
        StopCursorAnimation();

        // Fase 8
        Mask.sprite = maskPhases[8];
        fakeCursor.color = new Color(0, 0, 0, 0);
        currentPhase = TutorialPhase.ShowTimer;
        tutorialText.text = "Perfecte!\n Ara has d'intentar aconseguir que tots els animals estiguin contents en el menor temps possible!";
        startCanvas.SetActive(true);
        PlayerPrefs.SetInt("TutoComplete", 1);
        yield return null;
    }
    IEnumerator RunTutorialMobile()
    {
        // Fase 1
        Mask.sprite = maskPhases[1];
        targetAnimal.selectable = false;
        targetSlot[0].OnHighLight(true);
        currentPhase = TutorialPhase.ClickSlot;
        tutorialText.text = "Fes clic sobre l'espai per veure la seva informació";
        StartCursorAnimation(targetSlot[0].transform.position + Vector3.up * yOffset);
        yield return new WaitUntil(() => clickedSlot);
        StopCursorAnimation();

        for (int i = 0; i < targetSlot.Length; i++)
        {
            targetSlot[i].enabled = false;
        }

        // Fase 2
        Mask.sprite = maskPhases[2];
        currentPhase = TutorialPhase.ShowSlotUI;
        tutorialText.text = "Investiga quines propietats té l'espai";
        StartCursorAnimation(SlotUI);
        yield return new WaitForSeconds(3f);
        StopCursorAnimation();

        // Fase 3
        Mask.sprite = maskPhases[3];
        currentPhase = TutorialPhase.ClickAnimal;
        tutorialText.text = "Fes clic sobre l'animal per veure informació de l'animal";
        StartCursorAnimation(targetAnimal.transform.position + Vector3.up * yOffset);
        yield return new WaitUntil(() => clickedAnimal);
        StopCursorAnimation();

        // Fase 4
        Mask.sprite = maskPhases[4];
        currentPhase = TutorialPhase.ShowAnimalUI;
        tutorialText.text = "Investiga quines propietats té l'animal";
        StartCursorAnimation(AnimalUI);
        yield return new WaitForSeconds(3f);
        StopCursorAnimation();

        // Fase 5
        Mask.sprite = maskPhases[5];
        currentPhase = TutorialPhase.ShowConditionsUI;
        tutorialText.text = "Observa les preferències de l'animal";
        StartCursorAnimation(ConditionUI);
        yield return new WaitForSeconds(3f);
        StopCursorAnimation();

        for (int i = 0; i < targetSlot.Length; i++)
        {
            targetSlot[i].enabled = true;
        }

        // Fase 6   
        Mask.sprite = maskPhases[6];
        targetAnimal.selectable = true;
        targetSlot[0].SlotPle = false;
        targetSlot[1].SlotPle = true;
        currentPhase = TutorialPhase.DragAnimalWrong;
        tutorialText.text = "Arrossega l'animal a l'espai per col·locar-lo";
        StartCursorAnimation(SlotPathAnimation(false));
        yield return new WaitUntil(() => dragToWrongSlot);
        StopCursorAnimation();

        // Fase 7
        Mask.sprite = maskPhases[7];
        targetSlot[1].SlotPle = false;
        currentPhase = TutorialPhase.DragAnimalRight;
        tutorialText.text = "Vaja! Ara no està content. Provem de posar-lo a un lloc que sí que vulgui estar!";
        StartCursorAnimation(SlotPathAnimation(true));
        yield return new WaitUntil(() => dragToRightSlot);
        StopCursorAnimation();

        // Fase 8
        Mask.sprite = maskPhases[8];
        fakeCursor.color = new Color(0, 0, 0, 0);
        currentPhase = TutorialPhase.ShowTimer;
        tutorialText.text = "Perfecte!\n Ara has d'intentar aconseguir que tots els animals estiguin contents en el menor temps possible!";
        startCanvas.SetActive(true);
        PlayerPrefs.SetInt("TutoComplete", 1);
        yield return null;

    }
    void StartCursorAnimation(Vector3 target)
    {
        if (cursorAnimRoutine != null)
            StopCoroutine(cursorAnimRoutine);

        cursorAnimRoutine = StartCoroutine(CursorMoveTo(target));
    }
    void StartCursorAnimation(IEnumerator animationRoutine)
    {
        if (cursorAnimRoutine != null)
            StopCoroutine(cursorAnimRoutine);

        cursorAnimRoutine = StartCoroutine(animationRoutine);
    }
    void StopCursorAnimation()
    {
        if (cursorAnimRoutine != null)
        {
            StopCoroutine(cursorAnimRoutine);
            cursorAnimRoutine = null;
        }
    }
    IEnumerator CursorMoveTo(Vector3 target)
    {
        cursorAnimating = true;
        target.z = 0;
        while (true)
        {
            fakeCursor.transform.position = Vector3.Lerp(fakeCursor.transform.position, target, Time.deltaTime * 2f);
            fakeCursor.transform.localScale = Vector3.one * (1 + Mathf.Sin(Time.time * 6f) * 0.1f); // peque�o �latido�
            yield return null;
        }
    }
    IEnumerator SlotPathAnimation(bool correct)
    {
        cursorAnimating = true;
        Vector3 start = targetAnimal.transform.position + Vector3.up * yOffset;
        Vector3 end = (correct ? targetSlot[1].transform.position : targetSlot[0].transform.position) + Vector3.up * yOffset;

        while (true)
        {
            float t = (Mathf.Sin(Time.time * 2f) + 1f) / 2f; // ida y vuelta
            fakeCursor.transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }
    }
    public void OnSlotClicked()
    {
        if (currentPhase == TutorialPhase.ClickSlot)
            clickedSlot = true;
    }
    public void OnSlotUIHovered() { if (currentPhase == TutorialPhase.ShowSlotUI) hoveredSlotUI = true; }
    public void OnAnimalClicked() { if (currentPhase == TutorialPhase.ClickAnimal) clickedAnimal = true; }
    public void OnAnimalUIHovered() { if (currentPhase == TutorialPhase.ShowAnimalUI) hoveredAnimalUI = true; }
    public void OnConditionsUIHovered() { if (currentPhase == TutorialPhase.ShowConditionsUI) hoveredConditionsUI = true; }
    public void OnAnimalDraggedToSlot(bool isCorrect)
    {
        if (currentPhase == TutorialPhase.DragAnimalWrong && !isCorrect) dragToWrongSlot = true;
        else if (currentPhase == TutorialPhase.DragAnimalRight && isCorrect) dragToRightSlot = true;
    }
    void Update()
    {
        if (fakeCursor == null) return;

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        float dist = Vector3.Distance(mouseWorld, fakeCursor.transform.position);

        float alpha = baseCursorColor.a;
        if (dist < cursorTransparencyDistance)
            alpha = Mathf.Lerp(cursorMinAlpha, 1f, dist / cursorTransparencyDistance);

        Color c = fakeCursor.color;
        c.a = alpha;
        fakeCursor.color = c;
    }
}
