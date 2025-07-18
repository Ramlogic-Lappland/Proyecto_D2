using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image fillImage;

    public void SetHealth(int health)
    {
        if (slider != null)
            slider.value = health;
    }

    void Awake()
    {
        if (slider == null)
            slider = GetComponent<Slider>();
    }
}
