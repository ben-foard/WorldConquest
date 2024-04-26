using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button rollButton;
    [SerializeField] private Button cardButton;
    //Updates if you can interact with the button
    public void InteractableUpdater(bool EndGame)
    {      
        continueButton.interactable = EndGame;
    }

    //Update if the confirm button is visible to the user 
    public void UpdateConfirmVisibility(bool isVisible){
        confirmButton.gameObject.SetActive(isVisible);
    }

    //Update the visibility of the roll button
    public void UpdateRollVisibility(bool isVisible){
        rollButton.gameObject.SetActive(isVisible);
    }

    //Returns the confirm button that is on the game
    public Button getConfirmButton(){
        return this.confirmButton;
    }

    //Returns the coninute button
    public Button getContinueButton(){
        return this.continueButton;
    }

    //returns the roll button
    public Button getRollButton(){
        return this.rollButton;
    }

}

