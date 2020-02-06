using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TasteIt : MonoBehaviour
{

    [SerializeField] float shiftTime;
    [SerializeField] TextMeshProUGUI tmpObj = null;
    [SerializeField] float colorChangeTime = 2.0f;
    [SerializeField] Color[] colors = new Color[6];

    float timer = 0.0f;
    int currentIndex = 0;
    int nextIndex;

    void Start()
    {
        nextIndex = (currentIndex + 1) % colors.Length;
    }


    void Update()
    {
        timer += Time.deltaTime;

        if (timer > colorChangeTime)
        {
            currentIndex = (currentIndex + 1) % colors.Length;
            nextIndex = (currentIndex + 1) % colors.Length;
            timer = 0.0f;
            
        }
        tmpObj.color = Color.Lerp(colors[currentIndex], colors[nextIndex], timer / colorChangeTime);

    }
}
