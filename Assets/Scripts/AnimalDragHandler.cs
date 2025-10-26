using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Animal))]
public class AnimalDragHandler : MonoBehaviour
{
    [Header("Drag settings")]
    public float followSpeed = 10f;
    public float grabOffset = 1.5f;

    [Header("PickUp Effect")]
    [SerializeField] float zoomScale = 1.5f;
    [SerializeField] float zoomDuration = 1f;
    Vector3 originalScale;
    
    Coroutine scaleRoutine = null;

    private Vector3 targetPosition;
    private Vector3 originalPosition;
    private bool isDragging = false;

    private Animal animal;
    private List<BiomeSlot> nearbySlots = new List<BiomeSlot>();

    private void Start()
    {
        animal = GetComponent<Animal>();
        originalScale = transform.localScale;

        if (animal.parentSlot != null)
        {
            originalPosition = animal.parentSlot.transform.position;
        }
        else
        {
            originalPosition = transform.position; // fallback
        }

        targetPosition = originalPosition;
    }
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
    }

    private void OnMouseDown()
    {
        if (!animal.selectable) return;
        isDragging = true;
        animal.aAnimator.OnMoodSwap(Mood.Grabed);
        StartpickUpEffect(zoomScale);
        LevelManager.instance.HighLightAll(true);
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        targetPosition = Camera.main.ScreenToWorldPoint(mousePos) - (Vector3.down.normalized * grabOffset);
    }

    private void OnMouseUp()
    {
        if (!isDragging) return;
        isDragging = false;
        LevelManager.instance.HighLightAll(false);

        BiomeSlot closestSlot = GetClosestSlot();

        if (closestSlot == null) //No slot aprop
        {
            targetPosition = animal.parentSlot != null ? animal.parentSlot.transform.position : originalPosition;
            if (animal.parentSlot != null)
                animal.SetMood(animal.CheckIfHappy());
            else
                animal.aAnimator.OnMoodSwap(Mood.Idle);

            StartpickUpEffect(originalScale.x);
            return;
        }
        if (!closestSlot.SlotPle)
        {
            if (animal.parentSlot != null)
                animal.parentSlot.SlotPle = false;

            animal.SetParentSlot(closestSlot, false);
            closestSlot.SlotPle = true;

            animal.SetParentSlot(closestSlot);
            targetPosition = closestSlot.transform.position;
            originalPosition = targetPosition;
            animal.SetMood(animal.CheckIfHappy());
        }
        else if (!animal.isOnQueue)
        {
            Debug.Log("On");
            Animal otherAnimal = closestSlot.GetComponentInChildren<Animal>();
            BiomeSlot newSlot = otherAnimal != null ? otherAnimal.parentSlot : null;
            BiomeSlot oldSlot = animal.parentSlot;
            AnimalDragHandler otherDrag = otherAnimal != null ? otherAnimal.GetComponent<AnimalDragHandler>() : null;

            if (otherAnimal != null && otherAnimal != animal && !otherAnimal.isOnQueue)
            {
                Debug.Log(1);
                animal.SetParentSlot(newSlot,false);
                otherAnimal.SetParentSlot(oldSlot,false);

                newSlot.SlotPle = true;
                oldSlot.SlotPle = true;

                targetPosition = animal.parentSlot != null ? animal.parentSlot.transform.position : originalPosition;
                originalPosition = targetPosition;

                otherDrag.targetPosition = otherAnimal.parentSlot != null ? otherAnimal.parentSlot.transform.position : otherDrag.originalPosition;

                animal.SetMood(animal.CheckIfHappy());
                otherAnimal.SetMood(otherAnimal.CheckIfHappy());

            }
            else
            {
                animal.SetMood(animal.CheckIfHappy());
                targetPosition = animal.parentSlot != null ? animal.parentSlot.transform.position : originalPosition;
            }
        }
        else
        {
            targetPosition = animal.parentSlot != null ? animal.parentSlot.transform.position : originalPosition;
        }

        StartpickUpEffect(originalScale.x);
    }


    BiomeSlot GetClosestSlot()
    {
        BiomeSlot closest = null;
        float minDist = Mathf.Infinity;
        foreach (var slot in nearbySlots)
        {
            float dist = Vector2.Distance(transform.position, slot.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = slot;
            }
        }
        return closest;
    }
    public void ResetPosition()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition;
    }
    private void StartpickUpEffect(float targetScale)
    {
        if (scaleRoutine != null)
            StopCoroutine(scaleRoutine);
        scaleRoutine = StartCoroutine(pickUpEffect(targetScale));
    }
    IEnumerator pickUpEffect(float targetScale)
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = originalScale * targetScale;
        float elapsed = 0f;

        while (elapsed < zoomDuration)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, elapsed / zoomDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = endScale;
        scaleRoutine = null;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        var slot = collision.GetComponent<BiomeSlot>();
        if (slot != null && !nearbySlots.Contains(slot))
            nearbySlots.Add(slot);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        var slot = collision.GetComponent<BiomeSlot>();
        if (slot != null)
            nearbySlots.Remove(slot);
    }
}

