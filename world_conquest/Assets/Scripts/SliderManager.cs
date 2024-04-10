using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using System;
using Unity.VisualScripting;
public class SliderScript : MonoBehaviour
{
    //Fields that are serialized from the game objects on screen
    [SerializeField] private TextMeshProUGUI sliderText;
    [SerializeField] private Slider slider;
    
    void Start()
    {
        //Adds an onlistener to the slider so that it updates the slider text to the value selected
        slider.onValueChanged.AddListener((v) => {
            int intValue = Mathf.RoundToInt(v); // Round the float value to the nearest integer
            sliderText.text = intValue.ToString();
        });
    }

    //Returns the value from the slider 
    public int GetAmount(){
        return Mathf.RoundToInt(slider.value);
    }

    //Updates the range of the slider values
    public void UpdateRange(int maxValue)
    {
        slider.maxValue = maxValue;
        if(slider.maxValue == 1){
            slider.interactable = false;
        }
        else{
            slider.interactable = true;
        }
    }

    //Updates whether the slider is displayed to the user 
    public void SetSliderActive(bool isActive)
    {
        slider.gameObject.SetActive(isActive);
        sliderText.gameObject.SetActive(isActive);
    }
}