using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

///<summary>
/// Represents a territory in the game.
///</summary>
public class Territory : MonoBehaviour
{
    ///<summary>
    /// The text displaying the troop count on the territory.
    ///</summary>
    [SerializeField] private TextMeshProUGUI troopText;

    ///<summary>
    /// The button associated with the territory.
    ///</summary>
    [SerializeField] private Button territoryButton;

    ///<summary>
    /// The outline of the territory button.
    ///</summary>
    [SerializeField] private SpriteRenderer territoryButtonOutline;

    ///<summary>
    /// The name of the territory.
    ///</summary>
    [SerializeField] private string territoryName;

    ///<summary>
    /// The list of neighboring territories.
    ///</summary>
    [SerializeField] private List<Territory> neighbours;

    ///<summary>
    /// The color of the territory.
    ///</summary>
    private Color32 territoryColour;

    ///<summary>
    /// The number of troops on the territory.
    ///</summary>
    private int troopCount = 0;

    ///<summary>
    /// The player who owns the territory.
    ///</summary>
    private Player territoryOwner;

    ///<summary>
    /// The continent to which the territory belongs.
    ///</summary>
    public Continent continent;

    ///<summary>
    /// Initializes the territory, adds the listener to the territory button, sets the troop count to 0 and the outline to default
    ///</summary>
    void Awake() 
    {
        territoryColour = territoryButtonOutline.color;
        //Adds a listener to the territory for when it is selected by the user 
        territoryButton.onClick.AddListener(OnTerritoryButtonClick);
        troopText.text = "0";
    }

    ///<summary>
    /// Updates the troop count displayed on the territory UI.
    ///</summary>
    void Update()
    {
        UpdateTroopCountUI();
    }

    ///<summary>
    /// Returns the troop count on the territory.
    ///</summary>
    ///<returns>The troop count.</returns>
    public int GetTerritoryTroopCount()
    {
        return troopCount;
    }

    ///<summary>
    /// Updates the troop count UI.
    ///</summary>
    public void UpdateTroopCountUI()
    {
        if(troopText != null)
        {
            troopText.text = troopCount.ToString();
        }
    }

    ///<summary>
    /// Adds troops to the territory.
    ///</summary>
    ///<param name="amount">The number of troops to add.</param>
    public void AddTroops(int amount)
    {
        this.troopCount += amount;
    }

    ///<summary>
    /// Removes troops from the territory.
    ///</summary>
    ///<param name="amount">The number of troops to remove.</param>
    public void RemoveTroops(int amount)
    {
        troopCount -= amount;
    }

    ///<summary>
    /// RChecks if the territory has enough troops to be able to foritfy
    ///</summary>
    ///<returns>The ammount of troops taht can be used for fortify</returns>
    public int AvailableTroops(){
        if(troopCount > 1){
            return troopCount - 1;
        }
        return 0;
    }
    ///<summary>
    /// Returns the button attached to the territory.
    ///</summary>
    ///<returns>The territory button.</returns>
    public Button GetTerritoryButton()
    {
        return this.territoryButton;
    }

    ///<summary>
    /// Returns all the neighbors of this territory.
    ///</summary>
    ///<returns>The list of neighboring territories.</returns>
    public List<Territory> GetNeighbours()
    {
        return this.neighbours;
    }

    ///<summary>
    /// Highlights the color of the territory (in green).
    ///</summary>
    public void HighlightTerritory()
    {
        territoryButtonOutline.color = new Color32(0, 255, 0, 255);
    }

    ///<summary>
    /// Reverts the highlight back to its original color.
    ///</summary>
    public void RevertHighlight()
    {
        territoryButtonOutline.color = this.territoryColour;
    }

    ///<summary>
    /// Sets the color of the territory.
    ///</summary>
    ///<param name="c">The color to set.</param>
    public void SetColour(Color32 c)
    {
        this.territoryColour = c;
        territoryButtonOutline.color = c;
    }

    ///<summary>
    /// Sets the owner of the territory.
    ///</summary>
    ///<param name="p">The player who owns the territory.</param>
    public void SetOwner(Player p)
    {
        this.territoryOwner = p; 

        // Checks if player owns all countries in continent
        p.PlayerOwnsContinent(this.GetContinent());
    }

    ///<summary>
    /// Returns the owner of the current territory.
    ///</summary>
    ///<returns>The owner of the territory.</returns>
    public Player GetOwner()
    {
        return this.territoryOwner;
    }

    ///<summary>
    /// Gets the name of the territory.
    ///</summary>
    ///<returns>The territory name.</returns>
    public string GetTerritoryName()
    {
        return territoryName;
    }

    ///<summary>
    /// Changes the owner of the current territory.
    ///</summary>
    ///<param name="p">The new owner of the territory.</param>
    public void ChangeOwner(Player p)
    {
        this.territoryOwner.RemoveTerritory(this);
        SetOwner(p);
        this.territoryColour = p.GetPlayerColour();
        SetColour(this.territoryColour);
        p.AddTerritory(this);
    }

    ///<summary>
    /// Gets the continent to which the territory belongs.
    ///</summary>
    ///<returns>The continent.</returns>
    public Continent GetContinent()
    {
        return continent;
    }

    ///<summary>
    /// Handles the click event of the territory button.
    ///</summary>
    // Method triggered when a territory button is clicked
    void OnTerritoryButtonClick() {

        // Gets the current territory and the previously selected territory
        Territory currentTerritory = this;
        Territory prevTerritory = GameManager.Instance.GetPreviousSelectedTerritory();

        // Gets the current game phase
        string phase = GameManager.Instance.GetCurrentPhase();

        // Switch statement to handle different game phases
        switch(phase) {
            case "Start":
                // If the current territory has no troops and it's the start phase, initiate troop deployment
                if(currentTerritory.GetTerritoryTroopCount() == 0){
                    GameManager.Instance.StartPhaseDeploy(currentTerritory);
                } 
                // If all territories are owned by players and the current territory belongs to the current player, initiate troop deployment
                else if(GameManager.Instance.allTerritoriesOwned() && currentTerritory.GetOwner() == GameManager.Instance.getCurrentPlayer()){
                    GameManager.Instance.StartPhaseDeploy(currentTerritory);
                }
                break;
            case "Deploy":
                // If the current territory belongs to the current player and not all troops have been deployed, highlight it for deployment
                if(currentTerritory.GetOwner() == GameManager.Instance.getCurrentPlayer() && !GameManager.Instance.CheckDeployedAllTroops(GameManager.Instance.getCurrentPlayer())){
                    // Revert the highlight of the previous territory, if any
                    if(prevTerritory != null){
                        prevTerritory.RevertHighlight();
                    }
                    // Highlight the current territory for deployment
                    this.HighlightTerritory();
                    // Set the current territory as the selected one
                    GameManager.Instance.SetCurrentSelectedTerritory(this);
                    // Update UI visibility for confirming deployment
                    GameManager.Instance.UpdateConfirmVisbility(true);                  
                } 
                // Set the current territory as the previous selected one
                GameManager.Instance.SetPreviousSelectedTerritory(this);
                break;
            case "Attack":
                // Initiates an attack if two territories have been selected by the user
                if(prevTerritory != null){
                    // Get the attacking territory
                    Territory attackingTerritory = prevTerritory;
                    // If the two selected territories are both neighbours and the attacker is not the defender's owner, start the attack
                    if(attackingTerritory.GetNeighbours().Contains(currentTerritory) && !attackingTerritory.GetOwner().GetAllTerritories().Contains(this)){
                        GameManager.Instance.StartAttack(currentTerritory);                 
                    }
                    else{
                        // If not, display the neighbours of the current territory
                        GameManager.Instance.DisplayNeighbours(currentTerritory);
                    } 
                    return;
                }
                // Display neighbours of the current territory as nothing has been selected to attack  
                GameManager.Instance.DisplayNeighbours(currentTerritory);
                break;
            case "Fortify":
                // Initiates fortification if the previous selected territory is not null, is a neighbour, and is owned by the current player
                if (prevTerritory != null && prevTerritory.GetNeighbours().Contains(currentTerritory) && prevTerritory.GetOwner().CheckTerritories(currentTerritory)) {
                    // Update UI visibility for confirming fortification
                    GameManager.Instance.UpdateConfirmVisbility(true);
                    // Set the current territory as the selected one
                    GameManager.Instance.SetCurrentSelectedTerritory(this);
                    // Set the previous territory as the previous selected one
                    GameManager.Instance.SetPreviousSelectedTerritory(prevTerritory);
                    // Highlight the previous territory for fortification
                    prevTerritory.HighlightTerritory();
                } else {
                    // Update UI visibility for cancelling fortification
                    GameManager.Instance.UpdateConfirmVisbility(false);
                    // Revert the highlight of the previous territory, if any
                    if(prevTerritory != null){
                        prevTerritory.RevertHighlight();
                    }
                    
                    // If the current territory has no available troops, hide the slider and set the previous selected territory as null
                    if (this.AvailableTroops() == 0) {
                        GameManager.Instance.UpdateSliderVisibility(false);
                        GameManager.Instance.SetPreviousSelectedTerritory(null);
                    } 
                    // Otherwise, display the amount of troops available for fortification to another territory
                    else {
                        // Highlight the current territory for fortification
                        currentTerritory.HighlightTerritory();
                        // Update UI visibility for the slider
                        GameManager.Instance.UpdateSliderVisibility(true);
                        // Set the previous selected territory as the current territory
                        GameManager.Instance.SetPreviousSelectedTerritory(this);
                        // Update slider values based on the available troops for fortification
                        GameManager.Instance.UpdateSliderValues(this.AvailableTroops());
                    }
                }
                break;
        }
    }

}