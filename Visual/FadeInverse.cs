using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInverse : MonoBehaviour
{
    [SerializeField] Image fill = null;
    [SerializeField] float fadeOffset;

    Slider slider;

    private void Start()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    private void Update()
    {
        var val = slider.value / slider.maxValue;
        var invert = 1.0f - val;

        if (invert > 0.97f)
            invert = 1;

        invert = invert;

        var bgc = fill.color;

        if(slider.value != slider.maxValue)
        {
        fill.color = new Color(bgc.r, bgc.g, bgc.b, invert + fadeOffset);
        }
        else
        {
            fill.color = new Color(bgc.r, bgc.g, bgc.b, 0);
        }

    }

}
