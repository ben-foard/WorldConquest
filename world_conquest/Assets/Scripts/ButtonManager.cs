using UnityEngine;
using UnityEngine.UI; 

/// <summary>
/// Manages button interactions for the game, enabling or disabling buttons based on game state.
/// </summary>
public class ButtonManager : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button rollButton;

    /// <summary>
    /// Updates the interactability of the continue button.
    /// </summary>
    /// <param name="EndGame">If set to true, the button becomes interactable; otherwise, it becomes non-interactable.</param>
    public void InteractableUpdater(bool EndGame)
    {      
        continueButton.interactable = EndGame;
    }

    /// <summary>
    /// Updates the visibility of the confirm button.
    /// </summary>
    /// <param name="isVisible">If true, the confirm button is made visible; otherwise, it is hidden.</param>
    public void UpdateConfirmVisibility(bool isVisible){
        confirmButton.gameObject.SetActive(isVisible);
    }

    /// <summary>
    /// Updates the visibility of the roll button.
    /// </summary>
    /// <param name="isVisible">If true, the roll button is made visible; otherwise, it is hidden.</param>
    public void UpdateRollVisibility(bool isVisible){
        rollButton.gameObject.SetActive(isVisible);
    }

    /// <summary>
    /// Retrieves the confirm button.
    /// </summary>
    /// <returns>Returns the confirm button component.</returns>
    public Button getConfirmButton(){
        return this.confirmButton;
    }

    /// <summary>
    /// Retrieves the continue button.
    /// </summary>
    /// <returns>Returns the continue button component.</returns>
    public Button getContinueButton(){
        return this.continueButton;
    }

    /// <summary>
    /// Retrieves the roll button.
    /// </summary>
    /// <returns>Returns the roll button component.</returns>
    public Button getRollButton(){
        return this.rollButton;
    }

}