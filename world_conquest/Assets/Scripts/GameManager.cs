using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using System.Data;
using JetBrains.Annotations;

public class GameManager : MonoBehaviour
{   
    //Singleton instance of GameManager
    public static GameManager Instance;
 
   
    //Fields for inspector accessibilty using [SerializeField]
    [SerializeField] private List<Territory> allTerritories;
    [SerializeField] private TextMeshProUGUI currentPhaseText;
    [SerializeField] private TextMeshProUGUI currentPlayerText;
    [SerializeField] private TextMeshProUGUI GameInfoText;
    [SerializeField] private TextMeshProUGUI AttackDiceText;
    [SerializeField] private TextMeshProUGUI DefendDiceText;
    [SerializeField] private Canvas diceRollCanvas;
    [SerializeField] private List<TextMeshProUGUI> diceRollText = new List<TextMeshProUGUI>();
    [SerializeField] private TextMeshProUGUI[] playerTextNames = new TextMeshProUGUI[6]; 

    
    //private variables for managing game state
    Dictionary<Player, int> initialDiceRoll = new Dictionary<Player, int>();
    private List<Player> CurrentPlayers = new List<Player>();    
    private Deck mainDeck;
    List<Card> discardPile = new List<Card>();
    private gamePhases currentGamePhase = gamePhases.Start;
    private int PlayerIndex = 0;
    private Dice gameDice;
    private SliderScript slider;
    private ButtonManager buttonManager;
    private Territory previousSelectedTerritory;
    private Territory currentSelectedTerritory;
    private int amountOfAttackDice;
    private int amountOfDefendDice;
    private int amountOfDiceRolled = 0;
    private int amountOfSetsTraded = 0;
    private int [] tradeInValues = {4,6,8,10,12,15};

    //Enum defining different phases of the game
    enum gamePhases {
        Start,
        Deploy,
        Attack,
        Fortify,
        EndGame
    }

    //Awake method called when the script instance is being loaded 
    void Awake() 
    {
        //Initialize singleton instance
        Instance = this;
        StartGame(MenuController.Instance.GetHumanPlayers(), MenuController.Instance.GetAIPlayers(), 
        MenuController.Instance.GetPlayerNames(), MenuController.Instance.GetPlayerColours());
    }
    
    //Method to start game called from the menuController
    public void StartGame(int amountOfHumans, int amountOfAI, string[] playerNames, Color32[] playerColours)
    {
        print(playerColours[0]);
        print(playerColours[1]);
        for(int i = 0; i < amountOfHumans; i++){
            Player nextPlayer = gameObject.AddComponent<Player>();
            nextPlayer.SetPlayerText(playerTextNames[i]);
            nextPlayer.SetPlayerColour(playerColours[i]);
            nextPlayer.SetPlayerName(playerNames[i]);
            CurrentPlayers.Add(nextPlayer);
        }

        mainDeck = gameObject.AddComponent<Deck>();
        gameDice = gameObject.AddComponent<Dice>();

        // Find and initialize slider and button manager       
        slider = FindObjectOfType<SliderScript>();
        buttonManager = FindObjectOfType<ButtonManager>();

        //Update User inteface and add lister to continue button 
        buttonManager.getContinueButton().onClick.AddListener(AdvancePhase);
        buttonManager.UpdateConfirmVisibility(false);
        buttonManager.UpdateConfirmVisibility(false);

        //Intial methods that run before game start
        //mainDeck.PopulateDeck();
        InitialDiceRoll(amountOfHumans, amountOfAI);
        UpdateUI();

    }
    
    //Method to get the player order when the game starts
    private void InitialDiceRoll(int amountOfHumans,int amountOfAI){

        diceRollCanvas.enabled = true;
        buttonManager.getRollButton().onClick.AddListener(RollDice);
        diceRollText[0].color = new Color32(0, 255, 0, 255);
        for(int i = 0; i < CurrentPlayers.Count; i++){
            diceRollText[i].text = CurrentPlayers[i].GetPlayerName() + " rolled: ";
        }     
    }    

    //Method is ran when the roll button is clicked 
    private void RollDice(){
        //Reverts the highlight
        diceRollText[amountOfDiceRolled].color  = new Color32(255,255,255,255);
        int diceValue = gameDice.getDiceValue();
        initialDiceRoll.Add(CurrentPlayers[amountOfDiceRolled], diceValue);
        diceRollText[amountOfDiceRolled].text += diceValue;

        amountOfDiceRolled++;
        if(amountOfDiceRolled >= CurrentPlayers.Count){
            FinishDiceRoll();
        }
        else{
            //highlights the next player
            diceRollText[amountOfDiceRolled].color  = new Color32(0, 255, 0, 255);
        }
    }

    private void FinishDiceRoll(){
        CurrentPlayers.Clear();
        buttonManager.UpdateRollVisibility(false);
        //Shows the order of everyone playing
        diceRollText[6].text = "Game playing order:\n";

        //Orders based on the key value (dice roll)
        foreach(KeyValuePair<Player, int> order in initialDiceRoll.OrderByDescending(key => key.Value)){
            CurrentPlayers.Add(order.Key);
            //Update with the order of playing
            diceRollText[6].text += order.Key.GetPlayerName() +"\n";
        }
        
        //Has a 3 second wait until the screen disappears
        StartCoroutine(UpdateDiceRoll());
    }

    //Waits 3 seconds to hide the screen
    IEnumerator UpdateDiceRoll()
    {
        yield return new WaitForSeconds(3);

        diceRollCanvas.enabled = false;
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
            buttonManager.getConfirmButton().onClick.RemoveAllListeners();
            //Execute actions based on current game phase
            switch (currentGamePhase)
            {
                case gamePhases.Deploy:
                    getCurrentPlayer().AlterTroopsToDeploy(3);
                    string playerName = getCurrentPlayer().GetPlayerName();
                    buttonManager.getConfirmButton().onClick.AddListener(DeployTroops);
                    bool containsSet = hasSet(playerName);
                    
                    if (containsSet && getCurrentPlayer().GetPlayerDeck().getSize() <= 4)
                    {
                         //TODO: give option to trade in cards 
                    }
                    else {
                        List<Card> setToTrade = GetSetToTrade(playerName);
                    }
                    //TODO: Check amount of (matching) sets player owns
                    //Allowed if 3 matching cards
                    //MUST if amount of cards => 5 must trade in atleast one set
                    //TODO: keep track of sets been traded
                    //TODO: occupied territory matching card sets get 2 extra armies
                                        
                    break;
                case gamePhases.Attack:
                    break;
                case gamePhases.Fortify:
                    buttonManager.getConfirmButton().onClick.AddListener(FortifyPositions);
                    //getCurrentPlayer().GetPlayerDeck().AddCard(mainDeck.DrawCard());
                    if (getCurrentPlayer().GetPlayerDeck().getSize() >= 5) 
                    {  
                    }
                    break;
            }
            
        }  
        UpdateUI();
    }

    //Method to end the current players's turn 
    private void EndPlayerTurn()
    {
        PlayerIndex = (PlayerIndex+1)%CurrentPlayers.Count;
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

        //Changes the visibility of the button and slider
        slider.SetSliderActive(currentGamePhase == gamePhases.Deploy || currentGamePhase == gamePhases.Fortify);
        buttonManager.InteractableUpdater(currentGamePhase == gamePhases.Attack || currentGamePhase == gamePhases.Fortify);
        //buttonManager.UpdateConfirmVisibility(false);
        RevertHighlight();

        if(currentGamePhase == gamePhases.Deploy || currentGamePhase == gamePhases.Start){
            GameInfoText.text = "Troops to deploy: " + getCurrentPlayer().GetTroopsToDeploy().ToString();
            slider.UpdateRange(getCurrentPlayer().GetTroopsToDeploy());
        } else {GameInfoText.text = "";}
        currentPhaseText.text = currentGamePhase.ToString();
        currentPlayerText.text = "Current Turn: " + getCurrentPlayer().GetPlayerName();
    }    

    // Method for attacking
    public void StartAttack(Territory defendingCountry)
    {  
        //checks whether attacker territory count has more troops than 1 before allowing attack
        if(previousSelectedTerritory.GetTerritoryTroopCount() > 1){
            
            //Updates UI elements to select  the territory
            slider.SetSliderActive(true);
            buttonManager.UpdateConfirmVisibility(true);

            currentSelectedTerritory=defendingCountry;
            if(previousSelectedTerritory.GetTerritoryTroopCount() > 3){
                slider.UpdateRange(3);
            }
            else{
                slider.UpdateRange(previousSelectedTerritory.GetTerritoryTroopCount() - 1);
            }

            GameInfoText.text = previousSelectedTerritory.GetOwner().GetPlayerName() + " select the amount of dice to attack with: ";
            getAttackDiceAmount();
        }

    }


    //Once the amount of dice per player selected, the attack is performed
    private void PerformAttack()
    {
        //Updates UI elements
        GameInfoText.text = "";
        amountOfDefendDice = slider.GetAmount();
        buttonManager.getConfirmButton().onClick.RemoveAllListeners();
        buttonManager.UpdateConfirmVisibility(false);
        AttackDiceText.text = "Attacker rolled: ";
        DefendDiceText.text = "Defender rolled:  ";

        int[] attackValues = new int[amountOfAttackDice];
        int[] defendValues = new int[amountOfDefendDice];

        //Puts dice values into the array
        for(int i = 0; i <amountOfAttackDice; i++){
            attackValues[i] = gameDice.getDiceValue();
            AttackDiceText.text += attackValues[i] + " ";
        }

        for(int j = 0; j< amountOfDefendDice;j++){
            defendValues[j] = gameDice.getDiceValue();
            DefendDiceText.text += defendValues[j] + " ";
        }

        //Sorts the arrays based on highest to lowest
        Array.Sort(attackValues, (x, y) => y.CompareTo(x));
        Array.Sort(defendValues, (x, y) => y.CompareTo(x));

        //How many times it needs to compare (3 vs 2 dice is 2 times)
        int numDiceToCompare = Mathf.Min(attackValues.Length, defendValues.Length);

        for(int k = 0; k < numDiceToCompare;k++){
            AttackTerritory(attackValues[k], defendValues[k]);
        }

        RevertHighlight();
        previousSelectedTerritory = null;
    }

    //Method for deploying troops in deploy and starting phase 
    public void DeployTroops() {
        Player currentPlayer = getCurrentPlayer();

        //Will run if the current player has the selected territory and hasnt deployed all troops
        int amount = slider.GetAmount();

        currentSelectedTerritory.AddTroops(amount);
        currentPlayer.AddTroops(amount);
        currentPlayer.AlterTroopsToDeploy(-amount);

        if (CheckDeployedAllTroops(currentPlayer)) {
            AdvancePhase();
        }          

        UpdateUI();
        currentSelectedTerritory.RevertHighlight();
        currentSelectedTerritory = null;
        previousSelectedTerritory = null;
    }

    //The start deploy phase 
    public void StartPhaseDeploy(Territory selectedTerritory){
            
        Player currentPlayer = getCurrentPlayer();

        //If the territory hasnt been owned yet
        if(selectedTerritory.GetTerritoryTroopCount() == 0){
            currentPlayer.AddTerritory(selectedTerritory);
            selectedTerritory.SetOwner(currentPlayer);
        }
        
        selectedTerritory.AddTroops(1);
        currentPlayer.AddTroops(1);
        currentPlayer.AlterTroopsToDeploy(-1);

        //Goes to the next player
        PlayerIndex = (PlayerIndex + 1) % CurrentPlayers.Count;

        if (AllPlayersDeployed()) {
            //Removes mission cards
            mainDeck.RemoveAllMissionCards();
            mainDeck.shuffleCards();
            currentGamePhase = gamePhases.Attack;
        }

        UpdateUI();
        previousSelectedTerritory = null;
            
    }

    //Method for fortify
    public void FortifyPositions(){
        Player currentPlayer = getCurrentPlayer();
        int amountToMove = slider.GetAmount();
        slider.UpdateRange(previousSelectedTerritory.AvailableTroops());
        currentPlayer.Fortify(previousSelectedTerritory, currentSelectedTerritory, amountToMove);
                
        UpdateUI();
        //currentPlayer.GetPlayerDeck().AddCard(mainDeck.DrawCard());
 
        //DONEISH: REWARD CARD (AT LEAST ONE) 
        //CARD PER TERRITORY??
        previousSelectedTerritory.RevertHighlight();
        currentSelectedTerritory.RevertHighlight();
        buttonManager.UpdateConfirmVisibility(false);
        previousSelectedTerritory = null;
    }

    //Method Checks if all troops have been deployed
    public bool CheckDeployedAllTroops(Player p)
    {       
        return (p.GetTroopsToDeploy() == 0);
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
        if(getCurrentPlayer().GetAllTerritories().Contains(selectedTerritory)){
            previousSelectedTerritory = selectedTerritory;

            //Displays all the neighbours of the current territory
            foreach(Territory t in selectedTerritory.GetNeighbours())
            {
                if(!getCurrentPlayer().GetAllTerritories().Contains(t)){
                    t.HighlightTerritory();
                }
            }
            
        }
        else{
           previousSelectedTerritory = null; 
        }
        
    }

    //Method returns the first territory that was clicked
    public Territory GetPreviousSelectedTerritory(){
        return this.previousSelectedTerritory;
    }

    //Method sets the field t to territory clicked first
    public void SetPreviousSelectedTerritory(Territory t){
        this.previousSelectedTerritory = t;
    }

    public void SetCurrentSelectedTerritory(Territory t){
        this.currentSelectedTerritory = t;
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

    //Returns the currentPlayer
    public Player getCurrentPlayer(){
        return CurrentPlayers[PlayerIndex];
    }

    //Gets is all territories are owned 
    public bool allTerritoriesOwned(){

        foreach(Territory t in allTerritories){
            if(t.GetTerritoryTroopCount() == 0){
                return false;
            }
        }
        return true;
    }

    //Gets the amount of attack dice chosen by the attacker
    private void getAttackDiceAmount(){
        buttonManager.UpdateConfirmVisibility(true);
        buttonManager.getConfirmButton().onClick.AddListener(getDefendDiceAmount);
    }

    //Gets the amount of defend dice chosen by defender
    private void getDefendDiceAmount(){

        amountOfAttackDice = slider.GetAmount();
        GameInfoText.text = currentSelectedTerritory.GetOwner().GetPlayerName() + " select the amount of dice to defend with: ";
        buttonManager.getConfirmButton().onClick.RemoveAllListeners();

        //Updates slider based on amount of trooop its has to defend
        if(previousSelectedTerritory.GetTerritoryTroopCount() > 1){
                slider.UpdateRange(2);
        }
        else{
                slider.UpdateRange(1);
        }
        buttonManager.getConfirmButton().onClick.AddListener(PerformAttack);    
    }

    //UNUSED: Returns a list of cards for the amount of territories captured
    // public List<Card> rewardCards(int capturedAmount, Deck mainDeck){
    //   List<Card> rewardedCards = new List<Card>();
    //   for(int i = 0; i < capturedAmount; i++){
    //         Card drawnCard = mainDeck.DrawCard();
    //         rewardedCards.Add(drawnCard);
    //   }
    //   return rewardedCards;
    // }

   //Method to trade in 3 cards
    public void tradeInCards(List<Card> sets){

        for(int i = 0; i < 3; i++){
            discardPile.Add(sets[i]);
            getCurrentPlayer().GetPlayerDeck().RemoveCard(sets[i]);
        }

    }   

    public int GetTradeValue(){
        int tradesValue=0;
        if(amountOfSetsTraded<6){
            tradesValue = tradeInValues[amountOfSetsTraded];
        }else{
            tradesValue = tradeInValues[5] + ((amountOfSetsTraded - (tradeInValues.Length - 1))*5);
        }
        amountOfSetsTraded++;
        return tradesValue;
    }


    //Checks whether a player has a set
    public bool hasSet(string playerName) {

        List<Card> cards = getCurrentPlayer().GetPlayerDeck().getAllCards();

        if(cards.Count < 3){
            return false;
        } else {
            //Gets the count of all  cards
            int infantyCount = cards.Count(card => card.getArmyType() == "Infantry");
            int cavalryCount = cards.Count(card => card.getArmyType() == "Calvary");
            int artilleryCount = cards.Count(card => card.getArmyType() == "Artillery");
            int wildCardCount = cards.Count(card => card.getCardType() == "Wild Card");

            if(infantyCount >= 3 || cavalryCount >= 3 || artilleryCount >= 3){
                return true;
            } else if(wildCardCount >= 1){
                return true;
            } else if(infantyCount >= 1 & cavalryCount >= 1 & artilleryCount >= 1){
                return true;
            }
            return false;
        }

   
    }

    //Gets the set to trade in if player has over 5 cards 
    public List<Card> GetSetToTrade(string playerName)
    {

        List<Card> setToTrade = new List<Card>();
        List<Card> cards = getCurrentPlayer().GetPlayerDeck().getAllCards();
        
        //Counts amount each of the card types
        int infantyCount = cards.Count(card => card.getArmyType() == "Infantry");
        int cavalryCount = cards.Count(card => card.getArmyType() == "Calvary");
        int artilleryCount = cards.Count(card => card.getArmyType() == "Artillery");
        int wildCardCount = cards.Count(card => card.getCardType() == "Wild Card");

        if(infantyCount >= 3 || cavalryCount >= 3 || artilleryCount >= 3){
            //TODO
        } else if(wildCardCount >= 1){
            setToTrade.AddRange(cards.Where(card => card.getCardType() == "Wild Card").Take(3));

        } else if(infantyCount >= 1 && cavalryCount >= 1 && artilleryCount >= 1){
            setToTrade.AddRange(cards.Where(card => card.getArmyType() == "Infantry" ||
            card.getArmyType() == "Calvary" || card.getArmyType() == "Artillery").Take(3));

        }

        return setToTrade;
    }

    //Method for attacking a territory from this players current territory
    public void AttackTerritory(int attackingValue, int defendingValue)
    {
        
        if (attackingValue > defendingValue)
        {    
            //Moves the troops from the defending to the attacking territory 
            getCurrentPlayer().AddTroops(1);
            currentSelectedTerritory.GetOwner().RemoveTroops(1);

            //Changes the owner of the territory if the defending territory has only 1 troop left
            if (currentSelectedTerritory.GetTerritoryTroopCount() == 1)
            {
                //TODO: check if owner has any more territories, if 0 current player gets all their cards
                //TODO: if cards > 6 must trade in to get to a card count of 4 or fewer
                currentSelectedTerritory.ChangeOwner(getCurrentPlayer());

                if (currentSelectedTerritory.GetOwner().GetAllTerritories().Count == 0)
                {
                    getCurrentPlayer().GetPlayerDeck().AddCards(currentSelectedTerritory.GetOwner().GetPlayerDeck().RemoveAllCards());
                    while (getCurrentPlayer().GetPlayerDeck().getSize() >= 5) {
                        List<Card> setToTrade = GetSetToTrade(getCurrentPlayer().GetPlayerName());
                    }
                    
                }
            }
            //Else will handle the removing of troops from the defending territory
            else
            {
                currentSelectedTerritory.RemoveTroops(1);
                previousSelectedTerritory.AddTroops(1);
            }
        }
        else
        {
            getCurrentPlayer().RemoveTroops(1);
            previousSelectedTerritory.RemoveTroops(1);
        }

    }
}
