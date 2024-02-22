using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class ButtonManager : MonoBehaviour
{
    public Button deployButton; 
    public Button attackButton; 
    public Button fortifyButton; 
    public Button continueButton;
    private GameManager gameManager;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        attackButton.onClick.AddListener(gameManager.PerformAttack);
    }

    void Update()
    {
        
    }


    public void InteractableUpdater(bool deploy, bool attack, bool fortify, bool EndGame)
    {
        deployButton.interactable = deploy;
        attackButton.interactable = attack;
        fortifyButton.interactable = fortify;
        continueButton.interactable = EndGame;
    }
    //public void
}

