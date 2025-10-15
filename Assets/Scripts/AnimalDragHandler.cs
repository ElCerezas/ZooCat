using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animal))]
public class AnimalDragHandler : MonoBehaviour
{
    [Header("Drag settings")]
    public float followSpeed = 10f;       // Cuanto más alto, más rápido sigue al ratón
    public float snapBackSpeed = 8f;      // Velocidad al volver al slot original

    private Vector3 targetPosition;
    private Vector3 originalPosition;
    private bool isDragging = false;

    private Animal animal;
    private List<BiomeSlot> nearbySlots = new List<BiomeSlot>();

    private void Start()
    {
        animal = GetComponent<Animal>();

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
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        targetPosition = Camera.main.ScreenToWorldPoint(mousePos);
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
        }
        else
        {
            // Snap back
            targetPosition = animal.parentSlot != null  ? animal.parentSlot.transform.position : originalPosition;
        }
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

