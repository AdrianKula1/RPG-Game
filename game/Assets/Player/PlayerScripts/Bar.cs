using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public Image BarImage;

    private void Start()
    {
        BarImage = GetComponent<Image>();
    }

    public void UpdateBar(float currentValue, float maxValue)
    {
        BarImage.fillAmount = Mathf.Clamp(currentValue / maxValue, 0, 1);
    }
}
