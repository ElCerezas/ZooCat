using UnityEngine;

[ExecuteAlways]
class Namer : MonoBehaviour
{
    public bool applyChanges = false;
    string n;
    private void Update()
    {
        if (applyChanges)
        {
            AnimalAnimator aa = GetComponent<AnimalAnimator>();
            aa.headRenderer.sprite = aa.head;
            aa.bodyRenderer.sprite = aa.body;
        }

    }
}

