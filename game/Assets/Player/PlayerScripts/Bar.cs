using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public Image BarImage;
    private float oldBarValue;

    private void Start()
    {
        BarImage = GetComponent<Image>();
        oldBarValue = 1f;
    }

    public void UpdateBar(float currentValue, float maxValue)
    {
        oldBarValue = Mathf.Lerp(oldBarValue, Mathf.Clamp(currentValue / maxValue, 0, 1), 0.05f);
        BarImage.fillAmount = oldBarValue;
    }
}
