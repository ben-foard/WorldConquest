using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using System;
using System.Linq;
using System.Data;

/// <summary>
/// Represents the main controller class for managing the game logic and user interface interactions in a Risk-like game.
/// </summary>
public class GameManager : MonoBehaviour
{   
    /// <summary>
    /// Singleton instance of GameManager
    /// </summary>
    public static GameManager Instance;

    /// <summary>
    /// Text for displaying the current game phase
    /// </summary>
    [SerializeField] private TextMeshProUGUI currentPhaseText;

    /// <summary>
    /// Text for displaying the current player's name
    /// </summary>
    [SerializeField] private TextMeshProUGUI currentPlayerText;

    /// <summary>
    /// Text for displaying game-related information
    /// </summary>
    [SerializeField] private TextMeshProUGUI GameInfoText;

    /// <summary>
    /// Text for displaying the size of the deck
    /// </summary>
    [SerializeField] private TextMeshProUGUI deckSize;

    /// <summary>
    /// Text for displaying the name of the next player
    /// </summary>
    [SerializeField] private TextMeshProUGUI nextPlayerText;

    /// <summary>
    /// Array of text objects representing player names
    /// </summary>
    [SerializeField] private TextMeshProUGUI[] playerTextNames = new TextMeshProUGUI[6];

    /// <summary>
    /// List of text objects used in attack canvas
    /// </summary>
    [SerializeField] private List<TextMeshProUGUI> attackCanvasText = new List<TextMeshProUGUI>();

    /// <summary>
    /// List of text objects representing dice roll results
    /// </summary>
    [SerializeField] private List<TextMeshProUGUI> diceRollText = new List<TextMeshProUGUI>();

    /// <summary>
    /// List of text objects used in card trading
    /// </summary>
    [SerializeField] private List<TextMeshProUGUI> cardTradeText = new List<TextMeshProUGUI>();

    /// <summary>
    /// Canvas for displaying dice rolls
    /// </summary>
    [SerializeField] private Canvas diceRollCanvas;

    /// <summary>
    /// Canvas for displaying attack-related information
    /// </summary>
    [SerializeField] private Canvas attackCanvas;

    /// <summary>
    /// Canvas for displaying information about the next player
    /// </summary>
    [SerializeField] private Canvas nextPlayerCanvas;

    /// <summary>
    /// Canvas for displaying card trading information
    /// </summary>
    [SerializeField] private Canvas cardTradeCanvas;

    /// <summary>
    /// List of all territories in the game
    /// </summary>
    [SerializeField] private List<Territory> allTerritories;

    /// <summary>
    /// List of images representing attack dice
    /// </summary>
    [SerializeField] private List<Sprite> attackDiceImages = new List<Sprite>();

    /// <summary>
    /// List of images representing regular dice
    /// </summary>
    [SerializeField] private List<Sprite> diceImages = new List<Sprite>();

    /// <summary>
    /// List of images for displaying attack dice
    /// </summary>
    [SerializeField] private List<Image> attackDice = new List<Image>();

    /// <summary>
    /// List of images for displaying defend dice
    /// </summary>
    [SerializeField] private List<Image> defendDice = new List<Image>();

    /// <summary>
    /// Image for displaying the initial dice
    /// </summary>
    [SerializeField] private Image InitialDice;

    /// <summary>
    /// Button for card operations
    /// </summary>
    [SerializeField] private Button cardButton;

    /// <summary>
    /// List of images representing cards
    /// </summary>
    [SerializeField] private List<Image> cardImages;

    /// <summary>
    /// Dictionary to store initial dice rolls of players
    /// </summary>
    Dictionary<Player, int> initialDiceRoll = new Dictionary<Player, int>();

    /// <summary>
    /// List of current players in the game
    /// </summary>
    private List<Player> CurrentPlayers = new List<Player>();

    /// <summary>
    /// Main deck of cards
    /// </summary>
    private Deck mainDeck;

    /// <summary>
    /// List of cards in the discard pile
    /// </summary>
    List<Card> discardPile = new List<Card>();

    /// <summary>
    /// Current phase of the game
    /// </summary>
    private gamePhases currentGamePhase = gamePhases.Start;

    /// <summary>
    /// Index of the current player
    /// </summary>
    private int PlayerIndex = 0;

    /// <summary>
    /// Dice object for managing dice rolls
    /// </summary>
    private Dice gameDice;

    /// <summary>
    /// SliderScript object for managing sliders
    /// </summary>
    private SliderScript slider;

    /// <summary>
    /// ButtonManager object for managing buttons
    /// </summary>
    private ButtonManager buttonManager;

    /// <summary>
    /// Previously selected territory
    /// </summary>
    private Territory previousSelectedTerritory;

    /// <summary>
    /// Currently selected territory
    /// </summary>
    private Territory currentSelectedTerritory;

    /// <summary>
    /// Number of attack dice
    /// </summary>
    private int amountOfAttackDice;

    /// <summary>
    /// Number of defend dice
    /// </summary>
    private int amountOfDefendDice;

    /// <summary>
    /// Number of dice rolled
    /// </summary>
    private int amountOfDiceRolled = 0;

    /// <summary>
    /// Number of card sets traded
    /// </summary>
    private int amountOfSetsTraded = 0;

    /// <summary>
    /// Values required for trading cards
    /// </summary>
    private int [] tradeInValues = {4,6,8,10,12,15};

    /// <summary>
    /// Indicates if a territory has been captured
    /// </summary>
    private bool hasCapturedTerritory = false;


     /// <summary>
    /// Enum defining different phases of the game
    /// </summary>
    enum gamePhases {
        Start,
        Deploy,
        Attack,
        Fortify,
        EndGame
    }

    /// <summary>
    /// Start method called when the script instance is being loaded
    /// </summary>
    void Start() 
    {
        // Initialize singleton instance
        Instance = this;
        StartGame(MenuController.Instance.GetHumanPlayers(), MenuController.Instance.GetAIPlayers(), 
        MenuController.Instance.GetPlayerNames(), MenuController.Instance.GetPlayerColours());
    }
    
    /// <summary>
    /// Method to start the game called from the menuController
    /// </summary>
    /// <param name="amountOfHumans">Number of human players</param>
    /// <param name="amountOfAI">Number of AI players</param>
    /// <param name="playerNames">Names of the players</param>
    /// <param name="playerColours">Colors of the players</param>
    public void StartGame(int amountOfHumans, int amountOfAI, string[] playerNames, Color32[] playerColours)
    {
        
        // Create and initialize the main deck
        mainDeck = gameObject.AddComponent<Deck>();
        mainDeck.PopulateDeck();
        mainDeck.RemoveAllMissionCards();
        mainDeck.ShuffleCards();

        // Calculate the initial number of troops each player receives
        int amountOfInitialTroops = 40 - ((amountOfHumans + amountOfAI - 2) * 5);
        
        // Create and initialize human players
        for (int i = 0; i < amountOfHumans; i++)
        {
            Player nextPlayer = gameObject.AddComponent<Player>();
            nextPlayer.SetPlayerText(playerTextNames[i]);
            nextPlayer.SetPlayerColour(playerColours[i]);
            nextPlayer.SetPlayerName(playerNames[i]);
            CurrentPlayers.Add(nextPlayer);
            nextPlayer.AlterTroopsToDeploy(amountOfInitialTroops);
        }

        // Create and initialize the game dice
        gameDice = gameObject.AddComponent<Dice>();

        // Find and initialize slider and button manager       
        slider = FindObjectOfType<SliderScript>();
        buttonManager = FindObjectOfType<ButtonManager>();

        // Set up card button and listener for trading in card sets
        cardButton.onClick.AddListener(tradeInSetClick);
        cardButton.interactable = false;

        // Set up continue button and listener for advancing game phase
        buttonManager.getContinueButton().onClick.AddListener(AdvancePhase);
        buttonManager.UpdateConfirmVisibility(false);
        buttonManager.UpdateConfirmVisibility(false);

        // Execute initial methods before game start
        InitialDiceRoll(amountOfHumans, amountOfAI);
        UpdateUI();

    }
    
    /// <summary>
    /// Method to get the player order when the game starts
    /// </summary>
    /// <param name="amountOfHumans">Number of human players</param>
    /// <param name="amountOfAI">Number of AI players</param>
    private void InitialDiceRoll(int amountOfHumans,int amountOfAI){

        diceRollCanvas.enabled = true;
        buttonManager.getRollButton().onClick.AddListener(RollDice);
        diceRollText[0].color = new Color32(0, 255, 0, 255);
        for(int i = 0; i < CurrentPlayers.Count; i++){
            diceRollText[i].text = CurrentPlayers[i].GetPlayerName() + " rolled: ";
        }     
    }    

    /// <summary>
    /// Method is run when the roll button is clicked 
    /// </summary>
    private void RollDice(){
        buttonManager.getRollButton().interactable = false;
        
        //Reverts the highlight
        diceRollText[amountOfDiceRolled].color  = new Color32(255,255,255,255);
        int diceValue = gameDice.GetDiceValue();
        initialDiceRoll.Add(CurrentPlayers[amountOfDiceRolled], diceValue);

        if(!gameDice.GetIsRolling()){
            buttonManager.getRollButton().interactable = false;
            gameDice.StartDiceRollAnimation(diceImages, InitialDice);
            StartCoroutine(WaitForInitialDiceRoll(diceValue));
        }
        InitialDice.sprite = diceImages[diceValue - 1];

    }

    /// <summary>
    /// Coroutine to wait for the initial dice roll animation to finish
    /// </summary>
    /// <param name="diceValue">The value obtained from the dice roll</param>
    IEnumerator WaitForInitialDiceRoll(int diceValue)
    {
        yield return new WaitWhile(() => gameDice.GetIsRolling()); // Wait while dice are rolling
        
        // Show the button again once rolling stops
        buttonManager.getRollButton().interactable = true;
        
        //Updates the dice roll UI elements
        diceRollText[amountOfDiceRolled].text += diceValue;
        InitialDice.sprite = diceImages[diceValue - 1];
        amountOfDiceRolled++;
        if(amountOfDiceRolled >= CurrentPlayers.Count){
            FinishDiceRoll();
        }
        else{
            // Highlights the next player
            diceRollText[amountOfDiceRolled].color  = new Color32(0, 255, 0, 255);
        }
    }

    /// <summary>
    /// Pauses after dice roll animation and resets UI
    /// </summary>
    IEnumerator pauseDiceRoll(){

        yield return new WaitForSeconds(3);
        attackCanvas.enabled = false;
        slider.SetSliderActive(false);

        for(int i = 0; i < 3; i++){
            attackDice[i].enabled = false;
            if(i!= 2){
                defendDice[i].enabled = false;
            }
        }

        RevertHighlight();
        previousSelectedTerritory = null;
    }

    /// <summary>
    /// Method to finalize the initial dice roll and determine player order
    /// </summary>
    private void FinishDiceRoll(){
        CurrentPlayers.Clear();
        buttonManager.UpdateRollVisibility(false);
        // Shows the order of everyone playing
        diceRollText[6].text = "Game playing order:\n";

        // Orders based on the key value (dice roll)
        foreach(KeyValuePair<Player, int> order in initialDiceRoll.OrderByDescending(key => key.Value)){
            CurrentPlayers.Add(order.Key);
            // Update with the order of playing
            diceRollText[6].text += order.Key.GetPlayerName() +"\n";
        }
        
        // Has a 3-second wait until the screen disappears
        StartCoroutine(UpdateInitialDiceRoll());
    }

    /// <summary>
    /// Coroutine to wait 3 seconds to hide the screen after initial dice roll
    /// </summary>
    IEnumerator UpdateInitialDiceRoll()
    {
        yield return new WaitForSeconds(3);

        diceRollCanvas.enabled = false;
    } 

    
    /// <summary>
    /// Method to advance turn to the next phase
    /// </summary>
    private void AdvancePhase()
    {
        // Advance turn to the next phase unless fortify is current phase
        if(currentGamePhase != gamePhases.Fortify){
            currentGamePhase = (gamePhases)(((int)currentGamePhase + 1) % System.Enum.GetValues(typeof(gamePhases)).Length);
            GameLoop();
        }
        else{
            EndPlayerTurn();
        }
    }

    /// <summary>
    /// Executes the main game loop logic, which includes checking for win condition, handling player actions based on the current game phase, and updating the UI.
    /// </summary>
    private void GameLoop()
    { 
        // Get the current player
        Player currentPlayer = getCurrentPlayer();
        
        // Check for win condition 
        if(CheckWin())
        {
            // If win condition is met, set the current game phase to EndGame and execute EndGame method
            currentGamePhase = gamePhases.EndGame;
            EndGame();    
        }
        else
        {   
            // Remove any existing event listeners from the confirm button
            buttonManager.getConfirmButton().onClick.RemoveAllListeners();
            
            // Execute actions based on the current game phase
            switch (currentGamePhase)
            {
                case gamePhases.Deploy:
                    // Set the troops to deploy for the current player
                    currentPlayer.AlterTroopsToDeploy(currentPlayer.GetAmountOfTroopsToDeploy());
                    string playerName = currentPlayer.GetPlayerName();
                    
                    // Add a listener to the confirm button to deploy troops
                    buttonManager.getConfirmButton().onClick.AddListener(DeployTroops);
                    
                    // Check if the current player has a card set to trade in
                    bool containsSet = HasSet(playerName);
                    
                    // If the player has a set and the deck size is less than or equal to 4, enable the card trade button
                    if (containsSet && currentPlayer.GetPlayerDeck().GetSize() <= 4)
                    {
                        cardButton.interactable = true;
                        cardButton.GetComponent<Image>().color = new Color32(255, 255, 0, 255);   
                        foreach(Image im in cardImages)
                        {
                            im.color = new Color32(255, 255, 0, 255);   
                        }                 
                    }
                    // If the player has a set, trade in the cards and update troops to deploy
                    else if(containsSet)
                    {
                        List<Card> setToTrade = GetSetToTrade(playerName);
                        bool bonus = TradeInCards(setToTrade);
                        if(!bonus)
                        {
                            currentPlayer.AlterTroopsToDeploy(2);
                            currentPlayer.SetReceivedBonusTroops(true);
                        }
                        currentPlayer.AlterTroopsToDeploy(GetTradeValue());
                    }
                    break;
                case gamePhases.Attack:
                    break;
                case gamePhases.Fortify:
                    // Add a listener to the confirm button to fortify positions
                    buttonManager.getConfirmButton().onClick.AddListener(FortifyPositions);
                    break;
            }
        }  
        // Update the user interface elements
        UpdateUI();
    }

    /// <summary>
    /// Ends the current player's turn by performing necessary actions such as adding a card to the player's deck if a territory was captured, resetting bonus troops status, updating player index, and displaying the next player's turn.
    /// </summary>
    private void EndPlayerTurn()
    {
        // If a territory was captured during the turn, add a card from the main deck to the current player's deck
        if(hasCapturedTerritory)
        {
            getCurrentPlayer().GetPlayerDeck().AddCard(mainDeck.DrawCard());
        }
        
        // Reset bonus troops status for the current player
        getCurrentPlayer().SetReceivedBonusTroops(false);
        
        // Reset territory capture status
        hasCapturedTerritory = false;
        
        // Update player index to move to the next player in the list of current players
        PlayerIndex = (PlayerIndex + 1) % CurrentPlayers.Count;
        
        // Enable the canvas displaying the next player's turn and update the text to show the current player's name
        nextPlayerCanvas.enabled = true;
        nextPlayerText.text = getCurrentPlayer().GetPlayerName() + "'s turn!"; 
        
        // Set the current game phase to Deploy, indicating the start of the next player's turn
        currentGamePhase = gamePhases.Deploy;
        
        // Start coroutine to wait for a short duration before hiding the canvas, allowing players to see whose turn it is
        StartCoroutine(waitForNextPlayerCanvas());
    }

    /// <summary>
    /// Method to check for win condition
    /// </summary>
    private bool CheckWin()
    {
        // Currently this code removes any player who has 0 territories 
        CurrentPlayers.RemoveAll(p => p.GetAllTerritories().Count() == 0);
        
        return CurrentPlayers.Count == 1;
    }

    /// <summary>
    /// Method handles end game scenario
    /// </summary>
    private void EndGame()
    {
        PlayerIndex = 0;
        UpdateUI();
        currentPlayerText.text = "Winner: " + CurrentPlayers[0].GetPlayerName().ToString();   
    }


    /// <summary>
    /// Coroutine to wait for the next player canvas to be disabled before continuing the game loop
    /// </summary>
    IEnumerator waitForNextPlayerCanvas(){
        yield return new WaitForSeconds(3);
        nextPlayerCanvas.enabled = false;
        GameLoop();
        UpdateUI();    
    }

    /// <summary>
    /// Updates the User Interface elements based on the current game phase and player status.
    /// </summary>
    void UpdateUI()
    {
        // Changes the visibility of the slider based on the current game phase
        slider.SetSliderActive(currentGamePhase == gamePhases.Deploy || currentGamePhase == gamePhases.Fortify);

        // Updates the interactability of UI elements based on the current game phase
        buttonManager.InteractableUpdater(currentGamePhase == gamePhases.Attack || currentGamePhase == gamePhases.Fortify);

        // Reverts any highlighted territories on the game board
        RevertHighlight();

        // Updates the game information text based on the current game phase and player status
        if(currentGamePhase == gamePhases.Deploy || currentGamePhase == gamePhases.Start)
        {
            // Display the number of troops to deploy and update the slider range accordingly
            GameInfoText.text = "Troops to deploy: " + getCurrentPlayer().GetTroopsToDeploy().ToString();
            slider.UpdateRange(getCurrentPlayer().GetTroopsToDeploy());
        }
        else
        {
            // Clear the game information text if not in Deploy or Start phase
            GameInfoText.text = "";
        }

        // Updates the current game phase text to display the phase name
        currentPhaseText.text = currentGamePhase.ToString();

        // Updates the current player's name displayed on the UI
        currentPlayerText.text = "Current Turn: " + getCurrentPlayer().GetPlayerName();

        // Updates the size of the player's deck displayed on the UI
        deckSize.text = getCurrentPlayer().GetPlayerDeck().GetSize().ToString();
    }
    

    /// <summary>
    /// Method for attacking a territory and moving the troops from one territory to another.
    /// </summary>
    /// <param name="attackingValue">The value of the dice roll for the attacking player.</param>
    /// <param name="defendingValue">The value of the dice roll for the defending player.</param>
    public void AttackTerritory(int attackingValue, int defendingValue)
    {
        Player currentPlayer = getCurrentPlayer();

        // Check if the attacking value is greater than the defending value
        if (attackingValue > defendingValue)
        {    
            // Move one troop from the defending territory to the attacking territory 
            currentPlayer.AddTroops(1);
            currentSelectedTerritory.GetOwner().RemoveTroops(1);

            // Check if the defending territory has only one troop left
            if (currentSelectedTerritory.GetTerritoryTroopCount() == 1)
            {
                // Change the owner of the territory to the attacking player
                currentSelectedTerritory.ChangeOwner(currentPlayer);

                // Set the flag indicating that a territory has been captured
                hasCapturedTerritory = true;

                // Check if the defeated player has lost all territories
                if (currentSelectedTerritory.GetOwner().GetAllTerritories().Count == 0)
                {
                    // Transfer all cards from the defeated player's deck to the attacking player's deck
                    currentPlayer.GetPlayerDeck().AddCards(currentSelectedTerritory.GetOwner().GetPlayerDeck().RemoveAllCards());

                    // Trade in cards if the attacking player's deck size exceeds 4 cards
                    while (currentPlayer.GetPlayerDeck().GetSize() >= 4)
                    {
                        List<Card> setToTrade = GetSetToTrade(currentPlayer.GetPlayerName());
                        bool bonus = TradeInCards(setToTrade);
                        if (!bonus)
                        {
                            currentPlayer.AlterTroopsToDeploy(2);
                            currentPlayer.SetReceivedBonusTroops(true);
                        }
                        currentPlayer.AlterTroopsToDeploy(GetTradeValue());
                    }
                }
            }
            // If the defending territory has more than one troop left, remove one troop from each territory
            else
            {
                currentSelectedTerritory.RemoveTroops(1);
                previousSelectedTerritory.AddTroops(1);
            }
        }
        // If the defending value is greater than or equal to the attacking value, remove one troop from each territory
        else
        {
            currentPlayer.RemoveTroops(1);
            previousSelectedTerritory.RemoveTroops(1);
        }
    }


    /// <summary>
    /// Method to start an attack once the territories that are attack and defending has been established.
    /// </summary>
    /// <param name="defendingCountry">The territory being attacked.</param>
    public void StartAttack(Territory defendingCountry)
    {  
        // Checks whether the attacker territory has more troops than 1 before allowing the attack
        if(previousSelectedTerritory.GetTerritoryTroopCount() > 1)
        {
            // Updates UI elements to select the territory
            slider.SetSliderActive(true);
            buttonManager.UpdateConfirmVisibility(true);
            attackCanvasText[1].text = previousSelectedTerritory.GetOwner().GetPlayerName() + " rolled:";
            attackCanvasText[2].text = defendingCountry.GetOwner().GetPlayerName() + " rolled:";
            attackCanvasText[0].text = previousSelectedTerritory.GetOwner().GetPlayerName() + " is attacking " + defendingCountry.GetTerritoryName();
            currentSelectedTerritory = defendingCountry;

            // Adjusts the slider range based on the number of troops available for attacking
            if(previousSelectedTerritory.GetTerritoryTroopCount() > 3)
            {
                slider.UpdateRange(3);
            }
            else
            {
                slider.UpdateRange(previousSelectedTerritory.GetTerritoryTroopCount() - 1);
            }

            // Displays instructions for the attacker to select the amount of dice to attack with
            GameInfoText.text = previousSelectedTerritory.GetOwner().GetPlayerName() + " select the amount of dice to attack with: ";
            getAttackDiceAmount();
        }
    }


    /// <summary>
    /// Method to perform the attack and updating the UI elements
    /// </summary>
    private void PerformAttack()
    {
        // Update UI elements
        GameInfoText.text = "";
        attackCanvas.enabled = true;
        amountOfDefendDice = slider.GetAmount();
        buttonManager.getConfirmButton().onClick.RemoveAllListeners();
        buttonManager.UpdateConfirmVisibility(false);

        int[] attackValues = new int[amountOfAttackDice];
        int[] defendValues = new int[amountOfDefendDice];

        StartCoroutine(PerformSequentialDiceRolls(attackValues, defendValues));
    }

    /// <summary>
    /// Gets the amount of attack dice chosen by the attacker
    /// </summary>
    private void getAttackDiceAmount(){
        buttonManager.UpdateConfirmVisibility(true);
        buttonManager.getConfirmButton().onClick.AddListener(getDefendDiceAmount);
    }


    /// <summary>
    /// Gets the amount of defend dice chosen by the defender
    /// </summary>
    private void getDefendDiceAmount(){

        amountOfAttackDice = slider.GetAmount();
        GameInfoText.text = currentSelectedTerritory.GetOwner().GetPlayerName() + " select the amount of dice to defend with: ";
        buttonManager.getConfirmButton().onClick.RemoveAllListeners();

        // Updates slider based on amount of troops it has to defend
        if(currentSelectedTerritory.GetTerritoryTroopCount() > 1){
                slider.UpdateRange(2);
        }
        else{
                slider.UpdateRange(1);
        }
        buttonManager.getConfirmButton().onClick.AddListener(PerformAttack);    
    }

    /// <summary>
    /// Coroutine to perform sequential dice rolls for attack and defense
    /// </summary>
    private IEnumerator PerformSequentialDiceRolls(int[] attackValues, int[] defendValues)
    {
        // Roll attack dice
        for(int i = 0; i < amountOfAttackDice; i++)
        {
            attackValues[i] = gameDice.GetDiceValue();
            attackDice[i].enabled = true;
            yield return StartCoroutine(gameDice.DiceRollAnimation(attackDiceImages, attackDice[i]));
            attackDice[i].sprite = attackDiceImages[attackValues[i] - 1];
        }

        // Roll defend dice
        for(int j = 0; j < amountOfDefendDice; j++)
        {
            defendValues[j] = gameDice.GetDiceValue();
            defendDice[j].enabled = true;
            yield return StartCoroutine(gameDice.DiceRollAnimation(diceImages, defendDice[j]));
            defendDice[j].sprite = diceImages[defendValues[j] - 1];
        }

        // Sort the arrays of attack and defend values
        Array.Sort(attackValues, (x, y) => y.CompareTo(x));
        Array.Sort(defendValues, (x, y) => y.CompareTo(x));

        //Compare dice values and resolve attacks
        int numDiceToCompare = Mathf.Min(attackValues.Length, defendValues.Length);
        for(int k = 0; k < numDiceToCompare; k++)
        {
            AttackTerritory(attackValues[k], defendValues[k]);
        }

        StartCoroutine(pauseDiceRoll());
    }

    /// <summary>
    /// Method for deploying troops during the deploy and starting phases.
    /// </summary>
    public void DeployTroops() {
        Player currentPlayer = getCurrentPlayer();

        int amount = slider.GetAmount();

        // Add troops to the selected territory and the current player's troops
        currentSelectedTerritory.AddTroops(amount);
        currentPlayer.AddTroops(amount);
        currentPlayer.AlterTroopsToDeploy(-amount);

        // Check if all troops have been deployed by the current player
        if (CheckDeployedAllTroops(currentPlayer)) {
            AdvancePhase();
        }          

        UpdateUI();
        // Revert highlight and reset selected territories
        currentSelectedTerritory.RevertHighlight();
        currentSelectedTerritory = null;
        previousSelectedTerritory = null;
        // Update confirm button visibility
        UpdateConfirmVisbility(false);
    }

    /// <summary>
    /// Start the deploy phase.
    /// </summary>
    /// <param name="selectedTerritory">The territory selected to deploy troops to.</param>
    public void StartPhaseDeploy(Territory selectedTerritory){
        Player currentPlayer = getCurrentPlayer();

        // If the territory hasn't been owned yet
        if(selectedTerritory.GetTerritoryTroopCount() == 0){
            currentPlayer.AddTerritory(selectedTerritory);
            selectedTerritory.SetOwner(currentPlayer);
        }

        selectedTerritory.AddTroops(1);
        currentPlayer.AddTroops(1);
        currentPlayer.AlterTroopsToDeploy(-1);

        // Move to the next player
        PlayerIndex = (PlayerIndex + 1) % CurrentPlayers.Count;

        // Check if all players have deployed troops
        if (AllPlayersDeployed()) {
            // Remove mission cards, shuffle the deck, and switch to the attack phase
            mainDeck.RemoveAllMissionCards();
            mainDeck.ShuffleCards();
            currentGamePhase = gamePhases.Attack;
        }

        UpdateUI();
        previousSelectedTerritory = null;
    }


    /// <summary>
    /// Method for fortifying positions by moving troops from one territory to another.
    /// </summary>
    public void FortifyPositions(){
        Player currentPlayer = getCurrentPlayer();
        int amountToMove = slider.GetAmount();

        // Update the slider range based on the available troops in the previous selected territory
        slider.UpdateRange(previousSelectedTerritory.AvailableTroops());

        // Fortify the selected territories by moving troops
        currentPlayer.Fortify(previousSelectedTerritory, currentSelectedTerritory, amountToMove);
                
        // Update the user interface
        UpdateUI(); 

        // Revert highlight of selected territories and hide the confirm button
        previousSelectedTerritory.RevertHighlight();
        currentSelectedTerritory.RevertHighlight();
        buttonManager.UpdateConfirmVisibility(false);
        previousSelectedTerritory = null;
    }


    /// <summary>
    /// Method checks if all troops have been deployed
    /// </summary>
    public bool CheckDeployedAllTroops(Player p)
    {       
        return p.GetTroopsToDeploy() == 0;
    }

    /// <summary>
    /// Method checks if all players have been deployed
    /// </summary>
    public bool AllPlayersDeployed()
    {
        return CurrentPlayers.All(p => CheckDeployedAllTroops(p));
    }

    /// <summary>
    /// Method returns the current phase as a string
    /// </summary>
    public string GetCurrentPhase()
    {
        return currentGamePhase.ToString();
    }

    /// <summary>
    /// Method to display the neighbors of the selected territory.
    /// </summary>
    /// <param name="selectedTerritory">The territory for which neighbors are to be displayed.</param>
    public void DisplayNeighbours(Territory selectedTerritory)
    {
        // Update the user interface
        UpdateUI();

        // Check if the selected territory belongs to the current player
        if(getCurrentPlayer().GetAllTerritories().Contains(selectedTerritory)){
            previousSelectedTerritory = selectedTerritory;

            // Display all the neighbors of the current territory
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


    /// <summary>
    /// Method returns the first territory that was clicked
    /// </summary>
    public Territory GetPreviousSelectedTerritory(){
        return this.previousSelectedTerritory;
    }

    /// <summary>
    /// Method sets the field t to territory clicked first
    /// </summary>
    public void SetPreviousSelectedTerritory(Territory t){
        this.previousSelectedTerritory = t;
    }

    /// <summary>
    /// Method sets the current selected territory
    /// </summary>
    public void SetCurrentSelectedTerritory(Territory t){
        this.currentSelectedTerritory = t;
    }
    
    /// <summary>
    /// Method will remove highlight from all territories
    /// </summary>
    public void RevertHighlight(){
        foreach(Territory t in allTerritories){
            t.RevertHighlight();
        }
    }

    /// <summary>
    /// Method updates slider range
    /// </summary>
    public void UpdateSliderValues(int amount){
        slider.UpdateRange(amount);
    }

    /// <summary>
    /// Method turns on/off confirm button visibility
    /// </summary>
    public void UpdateConfirmVisbility(bool isVisible){
        buttonManager.UpdateConfirmVisibility(isVisible);
    }

    /// <summary>
    /// Method turns on/off slider visibility
    /// </summary>
    public void UpdateSliderVisibility(bool isVisible){
        slider.SetSliderActive(isVisible);
    }

    /// <summary>
    /// Returns the current player
    /// </summary>
    public Player getCurrentPlayer(){
        return CurrentPlayers[PlayerIndex];
    }

    /// <summary>
    /// Gets whether all territories are owned 
    /// </summary>
    public bool allTerritoriesOwned(){

        foreach(Territory t in allTerritories){
            if(t.GetTerritoryTroopCount() == 0){
                return false;
            }
        }
        return true;
    }



    /// <summary>
    /// Method to trade in three cards for bonus troops.
    /// </summary>
    /// <param name="sets">The list of three cards to trade in.</param>
    /// <returns>True if the player receives bonus troops, otherwise false.</returns>
    public bool TradeInCards(List<Card> sets)
    {
        // Initialize a variable to track whether the player receives bonus troops
        bool gotBonus = true;
        
        // Enable the card trade canvas
        cardTradeCanvas.enabled = true;
        
        // Iterate through the list of card sets
        for(int i = 0; i < 3; i++)
        {
            // Check if the territory of the card set is owned by the current player
            if(getCurrentPlayer().GetAllTerritoryNames().Contains(sets[i].GetTerritoryName()))
            {
                // Check if the player has already received bonus troops
                if(!getCurrentPlayer().GetReceivedBonusTroops())
                {
                    gotBonus = false;
                }
            }
            
            // Update the card trade text based on the type of card
            if(sets[i].GetCardType().Trim() != "Wild Card")
            {
                cardTradeText[i].text = sets[i].GetTerritoryName().Trim() + "\n" + sets[i].GetArmyType().Trim();
            } 
            else
            {
                cardTradeText[i].text = sets[i].GetCardType().Trim();
            }
            
            // Add the card set to the discard pile and remove it from the player's deck
            discardPile.Add(sets[i]);
            getCurrentPlayer().GetPlayerDeck().RemoveCard(sets[i]);
        }
        
        // Increment the count of sets traded
        amountOfSetsTraded++;
        
        // Update the card trade text to display the current bonus troops
        cardTradeText[3].text = "Current bonus troops: " + GetTradeValue().ToString();
        
        // Start the coroutine to wait for the card canvas
        StartCoroutine(waitForCardCanvas());
        
        // Update the deck size text
        deckSize.text = getCurrentPlayer().GetPlayerDeck().GetSize().ToString();
        
        return gotBonus;
    }


    IEnumerator waitForCardCanvas(){
        yield return new WaitForSeconds(5);
        cardTradeCanvas.enabled = false;
    }

    /// <summary>
    /// Gets the trade value based on how many have been traded in the game
    /// </summary>
    public int GetTradeValue(){
        int tradesValue;
        if (amountOfSetsTraded<6)
        {
            tradesValue = tradeInValues[amountOfSetsTraded];
        }else
        {
            tradesValue = tradeInValues[5] + ((amountOfSetsTraded - (tradeInValues.Length))*5);
        }
        return tradesValue;
    }

    /// <summary>
    /// Checks whether a player has a set of cards that can be traded in.
    /// </summary>
    /// <param name="playerName">The name of the player to check.</param>
    /// <returns>True if the player has a set of cards that can be traded in, otherwise false.</returns>
    public bool HasSet(string playerName)
    {
        // Check if the player's deck is empty
        if (getCurrentPlayer().GetPlayerDeck().GetSize() == 0)
        {
            return false;        
        }

        // Get all cards in the player's deck
        List<Card> cards = getCurrentPlayer().GetPlayerDeck().GetAllCards();

        // If the player has less than 3 cards, they cannot have a set
        if (cards.Count < 3)
        {
            return false;
        }
        else
        {
            // Count the number of each type of card
            int infantryCount = cards.Count(card => card.GetArmyType().Trim() == "Infantry");
            int cavalryCount = cards.Count(card => card.GetArmyType().Trim() == "Cavalry");
            int artilleryCount = cards.Count(card => card.GetArmyType().Trim() == "Artillery");
            int wildCardCount = cards.Count(card => card.GetCardType().Trim() == "Wild Card");

            // Check if the player has at least 3 cards of the same type or one of each type
            if (infantryCount >= 3 || cavalryCount >= 3 || artilleryCount >= 3)
            {
                return true;
            }
            else if (wildCardCount >= 1)
            {
                return true;
            }
            else if (infantryCount >= 1 && cavalryCount >= 1 && artilleryCount >= 1)
            {
                return true;
            }
            return false;
        }
    }


    /// <summary>
    /// Gets the set of cards to trade in if the player has over 5 cards.
    /// </summary>
    /// <param name="playerName">The name of the player whose set of cards is being retrieved.</param>
    /// <returns>A list of cards representing the set of cards to trade in.</returns>
    public List<Card> GetSetToTrade(string playerName)
    {
        List<Card> setToTrade = new List<Card>();
        List<Card> cards = getCurrentPlayer().GetPlayerDeck().GetAllCards();
        
        // Count the number of each type of card
        int infantryCount = cards.Count(card => card.GetArmyType().Trim() == "Infantry");
        int cavalryCount = cards.Count(card => card.GetArmyType().Trim() == "Cavalry");
        int artilleryCount = cards.Count(card => card.GetArmyType().Trim() == "Artillery");
        int wildCardCount = cards.Count(card => card.GetCardType().Trim() == "Wild Card");

        // If the player has at least 3 cards of the same type, add them to the set to trade
        if (infantryCount >= 3 || cavalryCount >= 3 || artilleryCount >= 3)
        {
            if (infantryCount >= 3)
            {
                setToTrade.AddRange(cards.Where(card => card.GetArmyType().Trim() == "Infantry").Take(infantryCount));
            }
            else if (cavalryCount >= 3)
            {
                setToTrade.AddRange(cards.Where(card => card.GetArmyType().Trim() == "Cavalry").Take(cavalryCount));
            }
            else if (artilleryCount >= 3)
            {
                setToTrade.AddRange(cards.Where(card => card.GetArmyType().Trim() == "Artillery").Take(artilleryCount));
            }
        } 
        // If the player has at least 1 wild card, add it to the set to trade
        else if (wildCardCount >= 1)
        {
            setToTrade.AddRange(cards.Where(card => card.GetCardType().Trim() == "Wild Card").Take(wildCardCount));
            setToTrade.AddRange(cards.Take(3 - wildCardCount));
        } 
        // If the player has at least 1 card of each type, add one of each type to the set to trade
        else if (infantryCount >= 1 && cavalryCount >= 1 && artilleryCount >= 1)
        {
            setToTrade.AddRange(cards.Where(card => card.GetArmyType().Trim() == "Infantry").Take(1));
            setToTrade.AddRange(cards.Where(card => card.GetArmyType().Trim() == "Cavalry").Take(1));
            setToTrade.AddRange(cards.Where(card => card.GetArmyType().Trim() == "Artillery").Take(1));
        }

        return setToTrade;
    }


    /// <summary>
    /// Executes the trade-in action when the trade-in button is clicked.
    /// </summary>
    public void tradeInSetClick()
    {
        // Retrieve the set of cards to trade in for the current player
        List<Card> setToTrade = GetSetToTrade(getCurrentPlayer().GetPlayerName());

        // Perform the trade-in action
        TradeInCards(setToTrade);

        // Update UI elements
        cardButton.interactable = false;
        cardButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        foreach (Image im in cardImages)
        {
            im.color = new Color32(255, 255, 255, 255);
        }
        deckSize.text = getCurrentPlayer().GetPlayerDeck().GetSize().ToString();
    }

}
