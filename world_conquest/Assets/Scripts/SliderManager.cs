using UnityEngine;
using UnityEngine.UI;
using TMPro;


///<summary>
/// Controls the behavior of a slider in the game.
///</summary>
public class SliderScript : MonoBehaviour
{
    ///<summary>
    /// The text displaying the slider value.
    ///</summary>
    [SerializeField] private TextMeshProUGUI sliderText;

    ///<summary>
    /// The slider component.
    ///</summary>
    [SerializeField] private Slider slider;
    
    ///<summary>
    /// Initializes the slider behavior.
    ///</summary>
    void Start()
    {
        // Adds an on listener to the slider to update the slider text to the selected value
        slider.onValueChanged.AddListener((v) => {
            int intValue = Mathf.RoundToInt(v); // Round the float value to the nearest integer
            sliderText.text = intValue.ToString();
        });
    }

    ///<summary>
    /// Returns the value from the slider.
    ///</summary>
    ///<returns>The value from the slider.</returns>
    public int GetAmount()
    {
        return Mathf.RoundToInt(slider.value);
    }

    ///<summary>
    /// Updates the range of the slider values.
    ///</summary>
    ///<param name="maxValue">The maximum value of the slider.</param>
    public void UpdateRange(int maxValue)
    {
        slider.maxValue = maxValue;
        if(slider.maxValue == 1)
        {
            slider.interactable = false;
        }
        else
        {
            slider.interactable = true;
        }
    }

    ///<summary>
    /// Updates whether the slider is displayed to the user.
    ///</summary>
    ///<param name="isActive">Whether the slider should be active or not.</param>
    public void SetSliderActive(bool isActive)
    {
        slider.gameObject.SetActive(isActive);
        sliderText.gameObject.SetActive(isActive);
    }
}
