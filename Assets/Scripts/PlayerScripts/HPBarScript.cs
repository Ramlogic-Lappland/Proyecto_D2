using UnityEngine;
using UnityEngine.UI;
public class HPBarScript : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetCurrentHealth(int health)
    {
        slider.value = health;
    }
    
}
