using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] public Button continueButton;
    [SerializeField] private List<Button> territoryButtons;
    private GameManager gameManager;
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        
    }


    public void InteractableUpdater(bool EndGame)
    {      
        continueButton.interactable = EndGame;
    }
    
    //public void
}

