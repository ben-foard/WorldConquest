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
    public static GameManager Instance {get; private set;}
 
   
    //Fields for inspector accessibilty using [SerializeField]
    [SerializeField] private List<Territory> allTerritories;
    [SerializeField] private TextMeshProUGUI currentPhaseText;
    [SerializeField] private TextMeshProUGUI currentPlayerText;
    [SerializeField] private TextMeshProUGUI GameInfoText;
    [SerializeField] private TextMeshProUGUI AttackDiceText;
    [SerializeField] private TextMeshProUGUI DefendDiceText;
    [SerializeField] private Canvas diceRollCanvas;
    [SerializeField] private List<TextMeshProUGUI> diceRollText = new List<TextMeshProUGUI>();
    [SerializeField] private Deck missionCardPile;

    //private variables for managing game state
    private List<Player> CurrentPlayers = new List<Player>();    
    private Deck mainDeck;
    private int tradeValue = 0;
    private gamePhases currentGamePhase = gamePhases.Start;
    private List<Color32> playerColours = new List<Color32> { 
        new Color32(229,19,19,255),
        new Color32(255,225,9,255),  
        new Color32(0,21,255,255),
        new Color32(6,171,30,255), 
        new Color32(100,6,171,255),
        new Color32(171,6,154,255)};

    private Dictionary<string, List<Card>> playerCards = new Dictionary<string,List<Card>>();
    private int PlayerIndex = 0;
    private Dice gameDice;
    private SliderScript slider;
    private ButtonManager buttonManager;
    private Territory previousSelectedTerritory;
    private Territory currentSelectedTerritory;
    private int amountOfAttackDice;
    private int amountOfDefendDice;
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
        mainDeck = gameObject.AddComponent<Deck>();
        gameDice = gameObject.AddComponent<Dice>();
        Player p1 = gameObject.AddComponent<Player>();
        Player p2 = gameObject.AddComponent<Player>();
        p1.SetPlayerColour(playerColours[0]);
        p2.SetPlayerColour(playerColours[1]);
        p1.SetPlayerName("Player 1");
        p2.SetPlayerName("Player 2");
        playerCards.Add(p1.GetPlayerName(), new List<Card>());
        playerCards.Add(p2.GetPlayerName(), new List<Card>());
        
        CurrentPlayers.Add(p1);
        CurrentPlayers.Add(p2);
    }
    
    //start method called before the first frame update 
    void Start()        
    {     
        // Find and initialize slider and button manager       
        slider = FindObjectOfType<SliderScript>();
        buttonManager = FindObjectOfType<ButtonManager>();
        //mainDeck.PopulateDeck();
        for(int i = 0; i < 5; i++){
            //Card card = mainDeck.DrawCard();
        }
        StartGame();
    }

    //Method to start game game
    private void StartGame()
    {
        //Update User inteface and add lister to continue button
        buttonManager.getContinueButton().onClick.AddListener(AdvancePhase);
        buttonManager.UpdateConfirmVisibility(false);
        buttonManager.UpdateConfirmVisibility(false);
        InitialDiceRoll();
        UpdateUI();

    }
 
    private void InitialDiceRoll(){
        Dictionary<Player, int> diceRolls = new Dictionary<Player, int>();
        int diceValue;
        diceRollCanvas.enabled = true;
        for(int i = 0; i < CurrentPlayers.Count; i++){
            diceValue = gameDice.getDiceValue();
            diceRolls.Add(CurrentPlayers[i],diceValue);
            diceRollText[i].text = CurrentPlayers[i].GetPlayerName() + " rolled: " + diceValue;
            
        }

        CurrentPlayers.Clear();
        diceRollText[6].text = "Game playing order:\n";
        foreach(KeyValuePair<Player, int> order in diceRolls.OrderBy(key => key.Value)){
            CurrentPlayers.Add(order.Key);
            diceRollText[6].text += order.Key.GetPlayerName() +"\n";
        }

       StartCoroutine(UpdateDiceRoll());
        
    }    
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
            //Execute actions based on current game phase
            switch (currentGamePhase)
            {
                /*Nothing happens here at the moment*/
                case gamePhases.Deploy:
                    string playerName = CurrentPlayers[PlayerIndex].GetPlayerName();
                    bool containsSet = hasSet(playerName);
                    buttonManager.getConfirmButton().onClick.AddListener(DeployTroops);
                    if (containsSet && playerCards[playerName].Count <= 4) { 
                        //TODO: give option to trade in cards 
                    }
                    //TODO: Check amount of (matching) sets player owns
                    //Allowed if 3 matching cards
                    //MUST if amount of cards => 5 must trade in atleast one set
                    //TODO: keep track of sets been traded
                    //TODO: occupied territory matching card sets get 2 extra armies
                    getCurrentPlayer().AlterTroopsToDeploy(3);                    
                    break;
                case gamePhases.Attack:
                    break;
                case gamePhases.Fortify:
                    //CurrentPlayers[PlayerIndex].GetPlayerDeck().AddCard(mainDeck.DrawCard());
                    //if (CurrentPlayers[PlayerIndex].GetPlayerDeck().getSize() >= 5) 
                    //{ 
                        
                    //}
                    break;
            }
            UpdateUI();
        }  
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

        //Changes the visibilit of the button and slider
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
    private void PerformAttack()
    {
        GameInfoText.text = "";
        amountOfDefendDice = slider.GetAmount();
        buttonManager.getConfirmButton().onClick.RemoveAllListeners();
        buttonManager.UpdateConfirmVisibility(false);
        
        int[] attackValues = new int[amountOfAttackDice];
        int[] defendValues = new int[amountOfDefendDice];

        AttackDiceText.text = "Attacker rolled: ";
        DefendDiceText.text = "Defender rolled:  ";

        for(int i = 0; i <amountOfAttackDice; i++){

            attackValues[i] = gameDice.getDiceValue();
            AttackDiceText.text += attackValues[i] + " ";

            if(amountOfDefendDice > i){
                defendValues[i] = gameDice.getDiceValue();
                DefendDiceText.text += defendValues[i] + " ";
            }
        }

        Array.Sort(attackValues, (x, y) => y.CompareTo(x));
        Array.Sort(defendValues, (x, y) => y.CompareTo(x));

        int numDiceToCompare = Mathf.Min(attackValues.Length, defendValues.Length);

        for(int i = 0; i < numDiceToCompare;i++){
            getCurrentPlayer().AttackTerritory(previousSelectedTerritory, currentSelectedTerritory, attackValues[i], defendValues[i]);
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
        UpdateConfirmVisbility(false);
        
    }
    public void StartPhaseDeploy(Territory selectedTerritory){
            
        Player currentPlayer = getCurrentPlayer();

        if(selectedTerritory.GetTerritoryTroopCount() == 0){
            currentPlayer.AddTerritory(selectedTerritory);
            selectedTerritory.SetOwner(currentPlayer);
        }
        
        selectedTerritory.AddTroops(1);
        currentPlayer.AddTroops(1);
        currentPlayer.AlterTroopsToDeploy(-1);

        PlayerIndex = (PlayerIndex + 1) % CurrentPlayers.Count;

        if (AllPlayersDeployed()) {
            for(int i = 0; i < 12;i++){
                //missionCardPile.AddCard(mainDeck.DrawCard());
            }
            //mainDeck.RemoveAllMissionCards();
            //mainDeck.shuffleCards();
            //DONE: SHUFFLE DECK AND REMOVE MISSION CARDS
            currentGamePhase = gamePhases.Attack;
        }

        UpdateUI();
        previousSelectedTerritory = null;
            
    }

    //Method for fortifying territories
    public void FortifyPositions(Territory fromTerritory, Territory toTerritory){

       Player currentPlayer = getCurrentPlayer();

        int amountToMove = slider.GetAmount();
        slider.UpdateRange(fromTerritory.AvailableTroops());
        currentPlayer.Fortify(fromTerritory, toTerritory, amountToMove);
                
        UpdateUI();

        //Defaults this to 3 for the deploy stage, will change in the future
        UpdateSliderValues(3);
        if(playerCards.ContainsKey(currentPlayer.GetPlayerName())){
            playerCards[currentPlayer.GetPlayerName()].Add(mainDeck.DrawCard());
        }
        //DONEISH: REWARD CARD (AT LEAST ONE) 
        //CARD PER TERRITORY??
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
    public Player getCurrentPlayer(){
        return CurrentPlayers[PlayerIndex];
    }
    public bool allTerritoriesOwned(){

        foreach(Territory t in allTerritories){
            if(t.GetTerritoryTroopCount() == 0){
                return false;
            }
        }
        return true;
    }
    private void getAttackDiceAmount(){
        buttonManager.UpdateConfirmVisibility(true);
        buttonManager.getConfirmButton().onClick.AddListener(getDefendDiceAmount);
    }
    private void getDefendDiceAmount(){
        amountOfAttackDice = slider.GetAmount();
        GameInfoText.text = currentSelectedTerritory.GetOwner().GetPlayerName() + " select the amount of dice to defend with: ";
        buttonManager.getConfirmButton().onClick.RemoveAllListeners();

        if(previousSelectedTerritory.GetTerritoryTroopCount() > 1){
                slider.UpdateRange(2);
        }
        else{
                slider.UpdateRange(1);
        }
        buttonManager.getConfirmButton().onClick.AddListener(PerformAttack);    
    }
    //Here i will return a list of cards for the amount of territories captured
   public List<Card> rewardCards(int capturedAmount, Deck mainDeck){
      List<Card> rewardedCards = new List<Card>();
      for(int i = 0; i < capturedAmount; i++){
            Card drawnCard = mainDeck.DrawCard();
            rewardedCards.Add(drawnCard);
      }
      return rewardedCards;
   }

   /**
   * trade in simulation for in game play
   **/
   public int tradeInCards(int setsToExchange, List<Card> discardPile, List<Card> sets){
      int tradesValue = getTrades(8);
      int cardCount = setsToExchange * 3;
      for(int i = 0; i < cardCount; i++) {
         discardPile.Add(sets[i]);
      }

      //increase trade count in player 
      
      return tradesValue * setsToExchange;
   }

   //this will be in the player class either be get trade value
   public int getTrades(int tradesDone){
      return tradesDone;
   }

    public bool hasSet(string playerName) {
        if (playerCards.ContainsKey(playerName)) {

            List<Card> cards = new List<Card>();

            int infantyCount = cards.Count(card => card.getArmyType() == "Infantry");
            int cavalryCount = cards.Count(card => card.getArmyType() == "Calvary");
            int artilleryCount = cards.Count(card => card.getArmyType() == "Artillery");

            return (infantyCount >= 1 && cavalryCount >= 1 && artilleryCount >= 1) ||
                (cards.Count(card => card.getCardType() == "Wild Card") >= 1);
        }
        return false;
    }
    public List<Card> GetSetToTrade(string playerName)
    {

        List<Card> setToTrade = new List<Card>();
        if (playerCards.ContainsKey(playerName))
        {

            List<Card> cards = new List<Card>();

            int infantyCount = cards.Count(card => card.getArmyType() == "Infantry");
            int cavalryCount = cards.Count(card => card.getArmyType() == "Calvary");
            int artilleryCount = cards.Count(card => card.getArmyType() == "Artillery");

            if (infantyCount >= 1 && cavalryCount >= 1 && artilleryCount >= 1)
            {
                setToTrade.AddRange(cards.Where(card => card.getArmyType() == "Infantry" ||
                card.getArmyType() == "Calvary" || card.getArmyType() == "Artillery").Take(3));
            }
            else if (cards.Count(card => card.getCardType() == "Wild Card") >= 2)
            {
                setToTrade.AddRange(cards.Where(card => card.getCardType() == "Wild Card").Take(3));
            }
        }
        return setToTrade;
    }

    public Dictionary<string, List<Card>> getPlayerDecks() {
        return playerCards;
    }

    public void SetCurrentSelectedTerritory(Territory t){
        currentSelectedTerritory = t;
    }
}
