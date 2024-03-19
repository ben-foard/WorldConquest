using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] public Button continueButton;
    [SerializeField] private Button confirmButton;

    //Updates if you can interact with the button
    public void InteractableUpdater(bool EndGame)
    {      
        continueButton.interactable = EndGame;
    }

    //Update if the confirm button is visible to the user 
    public void UpdateConfirmVisibility(bool isVisible){
        confirmButton.gameObject.SetActive(false);
    }

    //Returns the confirm button that is on the game
    public Button getConfirmButton(){
        return this.confirmButton;
    }

}

