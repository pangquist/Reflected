using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillHealthBar : MonoBehaviour
{
    public Health playerHealth;
    public Image fillImage;
    private Slider slider;
    // Start is called before the first frame update
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(slider.value <= slider.minValue)
        {
            fillImage.enabled = false;
        }

        if(slider.value > slider.minValue && !fillImage.enabled)
        {
            fillImage.enabled = true;
        }

        float fillValue = playerHealth.currentHealth / playerHealth.maxHealth;
        //Debug.Log(fillValue);
        //if(fillValue <= slider.maxValue / 3)
        //{
        //    fillImage.color = Color.magenta;
        //}
        //else if(fillValue > slider.maxValue)
        //{
        //    fillImage.color = Color.red;
        //}

        slider.value = fillValue;
    }
}
