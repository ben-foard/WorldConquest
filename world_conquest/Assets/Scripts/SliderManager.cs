using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
public class SliderScript : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI sliderText;
    [SerializeField] private Slider slider;

    void Start()
    {
        
        slider.onValueChanged.AddListener((v) => {
            int intValue = Mathf.RoundToInt(v); // Round the float value to the nearest integer
            sliderText.text = intValue.ToString();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
