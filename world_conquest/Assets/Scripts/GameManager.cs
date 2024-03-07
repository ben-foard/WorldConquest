using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using System;
using Unity.VisualScripting;
using System.Linq;
using System.Security.Cryptography;
public class GameManager : MonoBehaviour
{   
    public static GameManager Instance {get; private set;}
 
   
    [SerializeField] private List<Territory> allTerritories;
    [SerializeField] private TextMeshProUGUI currentPhaseText;
    [SerializeField] private TextMeshProUGUI currentPlayerText;
    [SerializeField] private List<Player> CurrentPlayers = new List<Player>();
    [SerializeField] private TextMeshProUGUI TroopsToDeployText;
        
    private gamePhases currentGamePhase = gamePhases.Start;
    private List<Color32> playerColours = new List<Color32> { new Color32(229,19,19,255), new Color32(255,225,9,255),  new Color32(0,21,255,255), new Color32(6,171,30,255), new Color32(100,6,171,255), new Color32(171,6,154,255)};
    private int PlayerIndex = 0;
    private Dice gameDice = new Dice();
    private SliderScript slider;
    private ButtonManager buttonManager;
    private Territory previousSelectedTerritory;

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
        Player p1 = new Player("player1");
        Player p2 = new Player("player2");
        p1.SetPlayerColour(playerColours[0]);
        p2.SetPlayerColour(playerColours[1]);
        for(int i = 0; i<allTerritories.Count;i++)
        {
            if(i%2==0)
            {
                p1.AddTerritory(allTerritories[i]);
                allTerritories[i].SetOwner(p1);
                continue;
            }
            p2.AddTerritory(allTerritories[i]);
            allTerritories[i].SetOwner(p2);
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
        Debug.Log(CurrentPlayers[0].GetAllTerritories().Count());
        Debug.Log(CurrentPlayers[1].GetAllTerritories().Count());
        CurrentPlayers.RemoveAll(p => p.GetAllTerritories().Count() == 0);
        
        return CurrentPlayers.Count == 1;
    }

    private void EndGame()
    {
        PlayerIndex = 0;
        UpdateUI();
        currentPlayerText.text = "Winner: " + CurrentPlayers[0].GetPlayerName().ToString();   
    }

    void UpdateUI()
    {
        slider.SetSliderActive(currentGamePhase == gamePhases.Deploy);
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

    public void PerformAttack(Territory defendingCountry)
    {  
        if(previousSelectedTerritory.GetTerritoryTroopCount() > 1){
            int attackValue = gameDice.getDiceValue(1);
            int defendValue = gameDice.getDiceValue(1);
            CurrentPlayers[PlayerIndex].AttackTerritory(previousSelectedTerritory, defendingCountry, attackValue, defendValue);
        }
        UpdateUI();
        previousSelectedTerritory = null;

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
                    AdvancePhase(); 
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
            previousSelectedTerritory = selectedTerritory;
            foreach(Territory t in selectedTerritory.GetNeighbours())
            {
                if(!CurrentPlayers[PlayerIndex].GetAllTerritories().Contains(t)){
                    t.HighlightTerritory();
                }
            }
            
        }
        else{
           previousSelectedTerritory = null; 
        }
        
    }
    public Territory GetPreviousSelectedTerritory(){
        return this.previousSelectedTerritory;
    }
    public void SetPreviousSelectedTerritory(Territory t){
        this.previousSelectedTerritory = t;
    }
}
