using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using System;
using Unity.VisualScripting;
using System.Linq;
public class GameManager : MonoBehaviour
{   
    public static GameManager Instance {get; private set;}
 
    private SliderScript slider;
    [SerializeField] private List<Territory> allTerritories;
    [SerializeField] private TextMeshProUGUI currentPhaseText;
    //Will be a list in the future
    [SerializeField] private TextMeshProUGUI currentPlayerText;
    private int PlayerIndex = 0;
    [SerializeField] private List<Player> CurrentPlayers = new List<Player>();
    [SerializeField] private TextMeshProUGUI TroopsToDeployText;
    private Player p1;
    private Player p2;
    private ButtonManager buttonManager;
    private gamePhases currentGamePhase = gamePhases.Start;

    enum gamePhases {
        Start,
        Deploy,
        Attack,
        Fortify,
        EndGame
    }
    
    void Awake() 
    {
        Instance = this;
        p1 = new Player("player1");
        p2 = new Player("player2");
        for(int i = 0; i<allTerritories.Count;i++)
        {
            if(i%2==0)
            {
                p1.AddTerritory(allTerritories[i]);
                continue;
            }
            p2.AddTerritory(allTerritories[i]);
        }
        CurrentPlayers.Add(p1);
        CurrentPlayers.Add(p2);
    }
    
    void Start()
    {           
        slider = FindObjectOfType<SliderScript>();
        buttonManager = FindObjectOfType<ButtonManager>();
        StartGame();
    }

    void Update()
    {

    }
    
    private void StartGame()
    {
        UpdateUI();
        buttonManager.continueButton.onClick.AddListener(AdvancePhase);
    }
    private void AdvancePhase()
    {
        if(currentGamePhase != gamePhases.Fortify){
            currentGamePhase = (gamePhases)(((int)currentGamePhase + 1) % System.Enum.GetValues(typeof(gamePhases)).Length);
        }
        else{
            EndPlayerTurn();
        }
        
        GameLoop();
    }
    private void GameLoop()
    { 

        if(CheckWin())
        {
            currentGamePhase = gamePhases.EndGame;
            EndGame();    
        }

        else
        {
            switch (currentGamePhase)
            {
                case gamePhases.Deploy:
                    CurrentPlayers[PlayerIndex].AlterTroopsToDeploy(3);                    
                    break;
                case gamePhases.Attack:
                    break;
                case gamePhases.Fortify:
                    break;
            }
            UpdateUI();
        }  
    }
    private void EndPlayerTurn()
    {
        PlayerIndex = (PlayerIndex+1)%2;
        currentGamePhase = gamePhases.Deploy;//2 will take the place of the amount of players in the future so that it only goes through the two indicies 
        UpdateUI();              
    }

    private bool CheckWin()
    {
        foreach(Player p in CurrentPlayers)
        {
            if(p.GetTroopTotal() == 0)
            {
                CurrentPlayers.Remove(p);
            }
        }
        if(CurrentPlayers.Count==1){
            return true;
        }
        return false;
    }

    private void EndGame()
    {
        UpdateUI();
        currentPlayerText.text = "Winner: " + CurrentPlayers[0].GetPlayerName().ToString();   
    }

    void UpdateUI()
    {
        slider.SetSliderActive(false);
        buttonManager.InteractableUpdater(currentGamePhase == gamePhases.Attack || currentGamePhase == gamePhases.Fortify);
        foreach(Territory t in allTerritories){
            t.RevertHighlight();
        }
        if(currentGamePhase == gamePhases.Deploy || currentGamePhase == gamePhases.Start){
            TroopsToDeployText.text = "Troops to deploy: " + CurrentPlayers[PlayerIndex].GetTroopsToDeploy().ToString();
        } else {TroopsToDeployText.text = "";}
        currentPhaseText.text = currentGamePhase.ToString();
        currentPlayerText.text = "Current Turn: " + CurrentPlayers[PlayerIndex].GetPlayerName();
    }    

    public void PerformAttack()
    {  
        foreach(Territory t in CurrentPlayers[PlayerIndex].GetAllTerritories()){
            t.GetTerritoryButton().onClick.AddListener(ShowNeighbouringCountries);
        }
        //CurrentPlayers[PlayerIndex].attackTerritory(attackedTerritory);
        AdvancePhase();
    }

    public void ShowNeighbouringCountries()
    {

    }
    
    public void DeployTroops(Territory currentTerritory)
    {
        Player current = CurrentPlayers[PlayerIndex];
        if(current.GetAllTerritories().Contains(currentTerritory) && !CheckDeployedAllTroops(current)){

            int amount = slider.GetAmount();
            if(currentGamePhase == gamePhases.Start){
                amount = 1;
            }      
            currentTerritory.AddTroops(amount);      
            current.AddTroops(amount);
            current.AlterTroopsToDeploy(-amount); 
            if(currentGamePhase == gamePhases.Start){
                PlayerIndex = (PlayerIndex + 1) % CurrentPlayers.Count;
            }
            if(CheckDeployedAllTroops(current))
            {
                if (currentGamePhase == gamePhases.Start)
                {
                    if(AllPlayersDeployed())
                    {
                        
                        currentGamePhase = gamePhases.Attack; 
                    }
                }
                else if (currentGamePhase != gamePhases.Start)
                {
                    AdvancePhase(); // Advance phase if it's not the Start phase and current player has deployed all troops
                }
            }
        }
        UpdateUI();
        
    }
    public bool CheckDeployedAllTroops(Player p)
    {       
        return p.GetTroopsToDeploy() == 0;
    }
    public bool AllPlayersDeployed()
    {
        return CurrentPlayers.All(p => CheckDeployedAllTroops(p));
    }
    public string GetCurrentPhase()
    {
        return currentGamePhase.ToString();
    }
    public void DisplayNeighbours(Territory selectedTerritory)
    {
        UpdateUI();
        if(CurrentPlayers[PlayerIndex].GetAllTerritories().Contains(selectedTerritory)){
            foreach(Territory t in selectedTerritory.GetNeighbours())
            {
                if(!CurrentPlayers[PlayerIndex].GetAllTerritories().Contains(t)){
                    t.HighlightTerritory();
                }
            }
        }
        
    }
}
