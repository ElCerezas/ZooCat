using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordWobble : MonoBehaviour
{
    [SerializeField] private bool wobbleByCharacter = false;
    [SerializeField] private float wobbleSpeed = 3f;
    [SerializeField] private float wobbleIntensity = 1f;

    TMP_Text textMesh;
    Mesh mesh;
    Vector3[] vertices;
    List<int> wordIndexes;
    List<int> wordLengths;

    public Gradient rainbow;

    void Start()
    {
        textMesh = GetComponent<TMP_Text>();

        wordIndexes = new List<int> { 0 };
        wordLengths = new List<int>();

        string s = textMesh.text;
        for (int index = s.IndexOf(' '); index > -1; index = s.IndexOf(' ', index + 1))
        {
            wordLengths.Add(index - wordIndexes[wordIndexes.Count - 1]);
            wordIndexes.Add(index + 1);
        }
        wordLengths.Add(s.Length - wordIndexes[wordIndexes.Count - 1]);
    }

    void Update()
    {
        textMesh.ForceMeshUpdate();
        mesh = textMesh.mesh;
        vertices = mesh.vertices;
        Color[] colors = mesh.colors;

        if (wobbleByCharacter)
        {
            for (int i = 0; i < textMesh.textInfo.characterCount; i++)
            {
                if (!textMesh.textInfo.characterInfo[i].isVisible)
                    continue;

                TMP_CharacterInfo c = textMesh.textInfo.characterInfo[i];
                int index = c.vertexIndex;

                Vector3 offset = Wobble(Time.time + i);
                ApplyOffsetAndColor(i, offset, colors);
            }
        }
        else
        {
            for (int w = 0; w < wordIndexes.Count; w++)
            {
                int wordIndex = wordIndexes[w];
                Vector3 offset = Wobble(Time.time + w);

                for (int i = 0; i < wordLengths[w]; i++)
                {
                    int charIndex = wordIndex + i;
                    if (!textMesh.textInfo.characterInfo[charIndex].isVisible)
                        continue;

                    ApplyOffsetAndColor(charIndex, offset, colors);
                }
            }
        }

        mesh.vertices = vertices;
        mesh.colors = colors;
        textMesh.canvasRenderer.SetMesh(mesh);
    }

    Vector2 Wobble(float time)
    {
        return new Vector2(
            Mathf.Sin(time * wobbleSpeed),
            Mathf.Cos(time * wobbleSpeed)
        ) * wobbleIntensity;
    }

    void ApplyOffsetAndColor(int charIndex, Vector3 offset, Color[] colors)
    {
        TMP_CharacterInfo c = textMesh.textInfo.characterInfo[charIndex];
        int index = c.vertexIndex;

        for (int j = 0; j < 4; j++)
        {
            colors[index + j] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index + j].x * 0.001f, 1f));
            vertices[index + j] += offset;
        }
    }
}
