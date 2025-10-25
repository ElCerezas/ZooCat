using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Animal))]
public class AnimalDragHandler : MonoBehaviour
{
    [Header("Drag settings")]
    public float followSpeed = 10f;
    public float snapBackSpeed = 8f;
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

        BiomeSlot closestSlot = GetClosestSlot();

        if (closestSlot != null && !closestSlot.SlotPle)
        {
            // Asignar al nuevo slot
            if (animal.parentSlot != null)
                animal.parentSlot.SlotPle = false;

            animal.parentSlot = closestSlot;
            closestSlot.SlotPle = true;

            animal.SetParentSlot(closestSlot);
            targetPosition = closestSlot.transform.position;
            originalPosition = targetPosition;
            animal.SetMood(animal.CheckIfHappy());
        }
        else
        {
            // Snap back
            targetPosition = animal.parentSlot != null  ? animal.parentSlot.transform.position : originalPosition;
            if (animal.parentSlot != null)
            {
                animal.SetMood(animal.CheckIfHappy());
            }
            else
            {
                animal.aAnimator.OnMoodSwap(Mood.Idle);
            }
        }
        StartpickUpEffect(originalScale.x);
    }

    private BiomeSlot GetClosestSlot()
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
    private IEnumerator pickUpEffect(float targetScale)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var slot = collision.GetComponent<BiomeSlot>();
        if (slot != null && !nearbySlots.Contains(slot))
            nearbySlots.Add(slot);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var slot = collision.GetComponent<BiomeSlot>();
        if (slot != null)
            nearbySlots.Remove(slot);
    }
}

