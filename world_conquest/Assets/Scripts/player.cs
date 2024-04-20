using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using System;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    //Field that is serialized from the current scene
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TMP_Text tradedInSets;
    

    //The properties of the player
    private string playerName;
    private int troopCount = 0;
    private int troopsToDeploy = 5;
    private List<Territory> ownedTerritories = new List<Territory>();
    private Color32 playerColour;
    private Deck cards = new Deck();


    //Adds a given amount of troops to the players troop count
    public void AddTroops(int amount)
    {
        troopCount += amount;
    }

    //Removes a given amount of troops from the players troop count 
    public void RemoveTroops(int amount)
    {
        troopCount -= amount;
    }

    //Method for attacking a territory from this players current territory
    public void AttackTerritory(Territory territoryAttacking,
        Territory territoryDefending, int attackingValue, int defendingValue)
    {
        if(attackingValue > defendingValue){

            //Moves the troops from the defending to the attacking territory 
            this.AddTroops(1);           
            territoryDefending.GetOwner().RemoveTroops(1);

            //Changes the owner of the territory if the defending territory has only 1 troop left
            if(territoryDefending.GetTerritoryTroopCount() == 1){
                //TODO: check if owner has any more territories, if 0 current player gets all their cards
                //TODO: if cards > 6 must trade in to get to a card count of 4 or fewer
                territoryDefending.ChangeOwner(this);
                
                if (territoryDefending.GetOwner().GetAllTerritories().Count == 0) {
                    this.GetPlayerDeck().AddCards(territoryDefending.GetOwner().GetPlayerDeck().RemoveAllCards());
                    if (this.GetPlayerDeck().getSize() >= 5) { 
                    
                    }
                    
                }
            }
            //Else will handle the removing of troops from the defending territory
            else{
                territoryDefending.RemoveTroops(1);
                territoryAttacking.AddTroops(1);
            }
       
        }

        else{
            this.RemoveTroops(1);
            territoryAttacking.RemoveTroops(1);
        }
        
    }

    //Method handles territory fortifying 
    public void Fortify(Territory fromTerritory, Territory toTerritory, int numOfTroops){
        fromTerritory.RemoveTroops(numOfTroops);
        toTerritory.AddTroops(numOfTroops);
    }

    //Returns amount of troops the current player has left to deploy
    public int GetTroopsToDeploy()
    {
        return this.troopsToDeploy;
    }

    //Alters the amount of troops to deploy based on the amount
    public void AlterTroopsToDeploy(int amount)
    {
        this.troopsToDeploy += amount;
    }

    //Returns the total amount of troops of the player
    public int GetTroopTotal()
    {
        return troopCount;
    }

    //Returns the players name
    public string GetPlayerName()
    {
        return this.playerName;
    }

    //Returns all territories owned by the player
    public List<Territory> GetAllTerritories()
    {
        return this.ownedTerritories;
    }

    //Method checks territory ownership
    public bool checkTerritories(Territory territory){
        if(this.ownedTerritories.Contains(territory)){
            return true;
        }
        return false;
    }

    //Adds a territory to the players owned territories
    public void AddTerritory(Territory t)
    {
        this.ownedTerritories.Add(t);
        t.SetColour(this.playerColour);
    }

    //Removes a territory from the players owned territories 
    public void RemoveTerritory(Territory t)
    {
        this.ownedTerritories.Remove(t);
    }

    //Changes the players playing colour on the UI
    public void SetPlayerColour(Color32 c)
    {
        this.playerColour = c;
    }

    //Gets the current players colour
    public Color32 GetPlayerColour()
    {
        return this.playerColour;
    }
    public void SetPlayerName(string name){
        this.playerName = name;
    }

    public Deck GetPlayerDeck() {
        return cards;
    }  

}