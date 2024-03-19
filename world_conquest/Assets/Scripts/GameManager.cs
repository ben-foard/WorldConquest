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
    //Singleton instance of GameManager
    public static GameManager Instance {get; private set;}
 
   
    //Fields for inspector accessibilty using [SerializeFiedl]
    [SerializeField] private List<Territory> allTerritories;
    [SerializeField] private TextMeshProUGUI currentPhaseText;
    [SerializeField] private TextMeshProUGUI currentPlayerText;
    [SerializeField] private List<Player> CurrentPlayers = new List<Player>();
    [SerializeField] private TextMeshProUGUI TroopsToDeployText;
    [SerializeField] private TextMeshProUGUI AttackDiceText;
    [SerializeField] private TextMeshProUGUI DefendDiceText;
    

    //private variables for managing game state
    private gamePhases currentGamePhase = gamePhases.Start;
    private List<Color32> playerColours = new List<Color32> { 
        new Color32(229,19,19,255),
        new Color32(255,225,9,255),  
        new Color32(0,21,255,255),
        new Color32(6,171,30,255), 
        new Color32(100,6,171,255),
        new Color32(171,6,154,255)};

    private int PlayerIndex = 0;
    private Dice gameDice = new Dice();
    private SliderScript slider;
    private ButtonManager buttonManager;
    private Territory previousSelectedTerritory;

    //Enum defining different phases of the game
    enum gamePhases {
        Start,
        Deploy,
        Attack,
        Fortify,
        EndGame
    }
    

    //Awake method called when the script is instance is being loaded 
    void Awake() 
    {
        //Initialize singleton instance 
        Instance = this;

        //Create players, set colors and assign territories 
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
    
    //start methiod called before the first frame update 
    void Start()        
    {     
        // Find and initialize slider and button manager       
        slider = FindObjectOfType<SliderScript>();
        buttonManager = FindObjectOfType<ButtonManager>();

        // StartGame
        StartGame();
    }

    //Method to start game game
    private void StartGame()
    {
        //Update User inteface and add lister to continue button
        UpdateUI();
        buttonManager.continueButton.onClick.AddListener(AdvancePhase);
        buttonManager.UpdateConfirmVisibility(false);
    }

    // Method to advance turn to next phasee
    private void AdvancePhase()
    {

        // Advance turn to the next phase unless fortify is current phase
        if(currentGamePhase != gamePhases.Fortify){
            currentGamePhase = (gamePhases)(((int)currentGamePhase + 1) % System.Enum.GetValues(typeof(gamePhases)).Length);
        }
        else{
            EndPlayerTurn();
        }
        
        GameLoop();
    }

    //Main game loop method
    private void GameLoop()
    { 
        //Check for win condition 
        if(CheckWin())
        {
            currentGamePhase = gamePhases.EndGame;
            EndGame();    
        }
        else
        {
            //Execute actions based on current game phase
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
    //Method to end the current players's turn 
    private void EndPlayerTurn()
    {
        PlayerIndex = (PlayerIndex+1)%2;
        currentGamePhase = gamePhases.Deploy;//2 will take the place of the amount of players in the future so that it only goes through the two indicies 
        UpdateUI();              
    }

    //Method to check for win condition
    private bool CheckWin()
    {
        //Currently this code removes any player who has 0 territories 
        CurrentPlayers.RemoveAll(p => p.GetAllTerritories().Count() == 0);
        
        return CurrentPlayers.Count == 1;
    }

    //Method handles end game scenario
    private void EndGame()
    {
        PlayerIndex = 0;
        UpdateUI();
        currentPlayerText.text = "Winner: " + CurrentPlayers[0].GetPlayerName().ToString();   
    }

    // Method updates User Inteface elements
    void UpdateUI()
    {
        //Hides the dice roll text
        AttackDiceText.text = "";
        DefendDiceText.text = "";

        //Changes the visibilit of the button and slider
        slider.SetSliderActive(currentGamePhase == gamePhases.Deploy || currentGamePhase == gamePhases.Fortify);
        buttonManager.InteractableUpdater(currentGamePhase == gamePhases.Attack || currentGamePhase == gamePhases.Fortify);
        
        //buttonManager.UpdateConfirmVisibility(false);
        RevertHighlight();

        if(currentGamePhase == gamePhases.Deploy || currentGamePhase == gamePhases.Start){
            TroopsToDeployText.text = "Troops to deploy: " + CurrentPlayers[PlayerIndex].GetTroopsToDeploy().ToString();
            slider.UpdateRange(CurrentPlayers[PlayerIndex].GetTroopsToDeploy());
        } else {TroopsToDeployText.text = "";}

        currentPhaseText.text = currentGamePhase.ToString();
        currentPlayerText.text = "Current Turn: " + CurrentPlayers[PlayerIndex].GetPlayerName();
    }    

    // Method for attacking
    public void PerformAttack(Territory defendingCountry)
    {  
        //checks whether attacker territory count has more troops than 1 before allowing attack
        if(previousSelectedTerritory.GetTerritoryTroopCount() > 1){
            int attackValue = gameDice.getDiceValue(1);
            AttackDiceText.text = "Rolled: " + attackValue;
            int defendValue = gameDice.getDiceValue(1);
            DefendDiceText.text = "Rolled: " + defendValue;
            CurrentPlayers[PlayerIndex].AttackTerritory(previousSelectedTerritory, defendingCountry, attackValue, defendValue);
        }
        RevertHighlight();
        previousSelectedTerritory = null;

    }
    
    //Method for deploying troops in deploy and starting phase 
    public void DeployTroops(Territory selectedTerritory) {
        Player currentPlayer = CurrentPlayers[PlayerIndex];

        //Will run if the current player has the selected territory and hasnt deployed all troops
        if (currentPlayer.GetAllTerritories().Contains(selectedTerritory) && !CheckDeployedAllTroops(currentPlayer)) {

            int amount = slider.GetAmount();

            //Defaults amount to deploy to 1 if in the start phase
            if (currentGamePhase == gamePhases.Start) {
                amount = 1;
            }
            

            selectedTerritory.AddTroops(amount);
            currentPlayer.AddTroops(amount);
            currentPlayer.AlterTroopsToDeploy(-amount);


            if (currentGamePhase == gamePhases.Start) {

                //Goes to the next player in the sequence until it has deployed all the starting troops
                PlayerIndex = (PlayerIndex + 1) % CurrentPlayers.Count;
                if (AllPlayersDeployed()) {
                    currentGamePhase = gamePhases.Attack;
                }
            } else {
                if (CheckDeployedAllTroops(currentPlayer)) {
                    AdvancePhase();
                }
            }
            
            UpdateUI();
            previousSelectedTerritory = null;
        }
    }


    //Method for fortifying territories
    public void FortifyPositions(Territory fromTerritory, Territory toTerritory){

       Player currentPlayer = CurrentPlayers[PlayerIndex];

        int amountToMove = slider.GetAmount();
        slider.UpdateRange(fromTerritory.AvailableTroops());
        currentPlayer.Fortify(fromTerritory, toTerritory, amountToMove);
                
        UpdateUI();

        //Defaults this to 3 for the deploy stage, will change in the future
        UpdateSliderValues(3);
        AdvancePhase();
       
       previousSelectedTerritory = null;
    }



    //Method Checks if all troops have been deployed
    public bool CheckDeployedAllTroops(Player p)
    {       
        return p.GetTroopsToDeploy() == 0;
    }

    //Method checks if all players have been deployed
    public bool AllPlayersDeployed()
    {
        return CurrentPlayers.All(p => CheckDeployedAllTroops(p));
    }

    //Method returns the current phase as a string
    public string GetCurrentPhase()
    {
        return currentGamePhase.ToString();
    }

    //Method Displays Neigbours 
    public void DisplayNeighbours(Territory selectedTerritory)
    {
        UpdateUI();
        if(CurrentPlayers[PlayerIndex].GetAllTerritories().Contains(selectedTerritory)){
            previousSelectedTerritory = selectedTerritory;

            //Displays all the neighbours of the current territory
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

    // Mehtod returns the first territory that was clicked
    public Territory GetPreviousSelectedTerritory(){
        return this.previousSelectedTerritory;
    }

    //Method sets the field t to territory clicked first
    public void SetPreviousSelectedTerritory(Territory t){
        this.previousSelectedTerritory = t;
    }

    //Method will remove highlight from territory
    public void RevertHighlight(){
        foreach(Territory t in allTerritories){
            t.RevertHighlight();
        }
    }

    //Method updates slider range
    public void UpdateSliderValues(int amount){
        slider.UpdateRange(amount);
    }

    //Method turns on/off confirm button visibilty
    public void UpdateConfirmVisbility(bool isVisible){
        buttonManager.UpdateConfirmVisibility(isVisible);
    }

    //Method turns on/off slider 
    public void UpdateSliderVisibility(bool isVisible){
        slider.SetSliderActive(isVisible);
    }
}