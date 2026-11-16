using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBarSlider;

    public void GiveFullHealth(float health)
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = health;
            healthBarSlider.value = health;
        }
    }

    public void SetHealth(float health)
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.value = health;
        }
    }
}