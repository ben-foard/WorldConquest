using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] public Button continueButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private List<Button> territoryButtons;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public void InteractableUpdater(bool EndGame)
    {      
        continueButton.interactable = EndGame;
    }
    public void UpdateConfirmVisibility(bool isVisible){
        confirmButton.gameObject.SetActive(false);
    }

    public Button getConfirmButton(){
        return this.confirmButton;
    }
    
    //public void
}

