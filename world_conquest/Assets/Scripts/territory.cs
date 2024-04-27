using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using System;
using Unity.VisualScripting;

public class Territory : MonoBehaviour
{
    //Fields serialized from the gameobjects from the UI
    [SerializeField] private TextMeshProUGUI troopText;
    [SerializeField] private Button territoryButton; 
    [SerializeField] private SpriteRenderer territoryButtonOutline;
    [SerializeField] private string territoryName;
    [SerializeField] private List<Territory> neighbours;

    //The fields of the territory
    private Color32 territoryColour;
    private int troopCount = 0;
    private Player territoryOwner;
    public Continent continent;

    void Awake() 
    {
        territoryColour = territoryButtonOutline.color;
        //Adds a listener to the territory for when it is selected by the user 
        territoryButton.onClick.AddListener(OnTerritoryButtonClick);
        troopText.text = "0";
    }

    //Method is run when a territory is selected
    void OnTerritoryButtonClick() {

        //Gets the two territories selected by the user 
        Territory currentTerritory = this;
        Territory prevTerritory = GameManager.Instance.GetPreviousSelectedTerritory();

        //Gets the current game phase
        string phase = GameManager.Instance.GetCurrentPhase();

        switch(phase) {
            case "Start":
                if(currentTerritory.GetTerritoryTroopCount() == 0){
                    GameManager.Instance.StartPhaseDeploy(currentTerritory);
                } else if(GameManager.Instance.allTerritoriesOwned() && currentTerritory.GetOwner() == GameManager.Instance.getCurrentPlayer()){
                    GameManager.Instance.StartPhaseDeploy(currentTerritory);
                }
                break;
            case "Deploy":

                if(currentTerritory.GetOwner() == GameManager.Instance.getCurrentPlayer() && !GameManager.Instance.CheckDeployedAllTroops(GameManager.Instance.getCurrentPlayer())){
                    if(prevTerritory != null){
                        prevTerritory.RevertHighlight();
                    }
                    this.HighlightTerritory();
                    GameManager.Instance.SetCurrentSelectedTerritory(this);
                    GameManager.Instance.UpdateConfirmVisbility(true);
                    
                } 
                GameManager.Instance.SetPreviousSelectedTerritory(this);

                break;

            case "Attack":
                //Will run if two territories have been selected by the user
                if(prevTerritory != null){
                
                    Territory attackingTerritory = prevTerritory;
                    //If the two territories selected are both neighbours of eachother and the owner of attacker is not owner of the defender
                    if(attackingTerritory.GetNeighbours().Contains(currentTerritory) && !attackingTerritory.GetOwner().GetAllTerritories().Contains(this)){
                        GameManager.Instance.StartAttack(currentTerritory);                 
                    }
                    else{
                        GameManager.Instance.DisplayNeighbours(currentTerritory);
                    } 
                    return;
                }
                //Display neighbours of the current selected as nothing has been selected to attack  
                GameManager.Instance.DisplayNeighbours(currentTerritory);
                break;
                
            case "Fortify":

                //Will run fortify if the previous selected is not null, is a neighbour and is owned by the currently selected
                if (prevTerritory != null && prevTerritory.GetNeighbours().Contains(currentTerritory) && prevTerritory.GetOwner().checkTerritories(currentTerritory)) {
                    GameManager.Instance.UpdateConfirmVisbility(true);
                    GameManager.Instance.SetCurrentSelectedTerritory(this);
                    GameManager.Instance.SetPreviousSelectedTerritory(prevTerritory);
                    prevTerritory.HighlightTerritory();
                } else {
                    GameManager.Instance.UpdateConfirmVisbility(false);
                    if(prevTerritory != null){
                        prevTerritory.RevertHighlight();
                    }
                    
                    if (this.AvailableTroops() == 0) {
                        GameManager.Instance.UpdateSliderVisibility(false);
                        GameManager.Instance.SetPreviousSelectedTerritory(null);
                    } 

                    //Displays the amount of troops it has to fortify to another territory
                    else {
                        currentTerritory.HighlightTerritory();
                        GameManager.Instance.UpdateSliderVisibility(true);
                        GameManager.Instance.SetPreviousSelectedTerritory(this);
                        GameManager.Instance.UpdateSliderValues(this.AvailableTroops());
                    }
                }
                break;
        }
    }

    void Update()
    {
        UpdateTroopCountUI();
    }

    //Returns amount of troops on the current territory
    public int GetTerritoryTroopCount()
    {
        return troopCount;
    }

    //Updates the amount of troops displayed on the territory
    public void UpdateTroopCountUI()
    {
        if(troopText != null)
        {
            troopText.text = troopCount.ToString();
        }
    }

    //Adds troops to the current territory count
    public void AddTroops(int amount)
    {
        this.troopCount += amount;
    }

    //Removes troops from the current territory count
    public void RemoveTroops(int amount)
    {
        troopCount -= amount;
    }
    
    //Method returns total amount of troops
    public int TotalTroops(){
        return this.troopCount;
    }

    //Method returns available troops for fortification 
    public int AvailableTroops(){
        if(troopCount > 1){
            return troopCount - 1;
        }
        return 0;
    }

    //Returns the button attacked to the territory
    public Button GetTerritoryButton()
    {
        return this.territoryButton;
    }

    //Returns all the neighbours of this territory 
    public List<Territory> GetNeighbours()
    {
        return this.neighbours;
    }

    //Highlights the colour of the territory (in green)
    public void HighlightTerritory()
    {
        territoryButtonOutline.color = new Color32(0, 255, 0, 255);
    }

    //Reverts the highlight back to its original colour
    public void RevertHighlight()
    {
        territoryButtonOutline.color = this.territoryColour;
    }

    //Sets the colour of the territory
    public void SetColour(Color32 c)
    {
        this.territoryColour = c;
        territoryButtonOutline.color = c;
    }

    //Sets the owner of the territory
    public void SetOwner(Player p)
    {
        this.territoryOwner = p; 

        //Checks if player owns all countries in continent
        p.PlayerOwnsContinent(this.getContinent());
    }

    //Returns the owner of the current territory 
    public Player GetOwner()
    {
        return this.territoryOwner;
    }

    //Gets the name of territory
    public string GetTerritoryName(){
        return territoryName;
    }
    //Alters the owner of the current territory to the Player p
    public void ChangeOwner(Player p)
    {
        this.territoryOwner.RemoveTerritory(this);
        SetOwner(p);
        this.territoryColour = p.GetPlayerColour();
        SetColour(this.territoryColour);
        p.AddTerritory(this);
    }

    public Continent getContinent(){
        return continent;
    }
}