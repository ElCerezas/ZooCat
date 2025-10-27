using System;
using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    bool animalHappy = false;
    Vector3 originalPosition;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("moving");
        transform.position += new Vector3(10, 5, 0) * Time.deltaTime;
        if (transform.position.x > 0 || transform.position.y > 0)
        {
            transform.position = originalPosition;
            //wait
        }
        if (animalHappy) this.enabled = false;
    }
    
    
}
