using UnityEngine;
[ExecuteAlways]
public class AlignFeetToOrigin : MonoBehaviour
{
    [SerializeField] private Transform visual;
    [SerializeField] private SpriteRenderer body;

    private void Reset()
    {
        visual = transform.Find("Visual");
        if (visual != null)
            body = visual.Find("Cuerpo")?.GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        if (visual == null || body == null) return;

        // calcula la distancia desde el pivot del Visual hasta el pie en coordenadas locales
        float footLocalY = body.transform.localPosition.y + body.sprite.bounds.min.y;

        // movemos Visual para que el pie quede en y=0 del prefab
        visual.localPosition = new Vector3(0, -footLocalY, 0);
    }
}

