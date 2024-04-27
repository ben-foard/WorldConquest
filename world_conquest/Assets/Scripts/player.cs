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
    [SerializeField] private TMP_Text tradedInSets;
    
    //The properties of the player
    private string playerName;
    protected int troopCount = 0;
    private int troopsToDeploy = 0;
    private List<Territory> ownedTerritories = new List<Territory>();
    private Color32 playerColour;
    private Deck cards = new Deck();
    private int tradeValue = 0;
    private TextMeshProUGUI playerNameText;
    private bool receivedBonusTroops = false;
    private List<Continent> ownedContinents = new List<Continent>();

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
        playerNameText.color = playerColour;
    }

    //Gets the current players colour
    public Color32 GetPlayerColour()
    {
        return this.playerColour;
    }

    //Sets the players name to the input
    public void SetPlayerName(string name){
        this.playerName = name;
        playerNameText.text = playerName;
    }

    //Gets the deck that the player has
    public Deck GetPlayerDeck() {
        return cards;
    }  

    public void SetPlayerText(TextMeshProUGUI text){
        playerNameText = text;
    }

    public List<string> GetAllTerritoryNames(){
        List<string> allTerritoryNames = new List<string>();
        foreach(Territory t in ownedTerritories){
            allTerritoryNames.Add(t.GetTerritoryName());
        }
        return allTerritoryNames;
    }
    public void setReceivedBonusTroops(bool received){
        receivedBonusTroops = received;
    }
    public bool getReceivedBonusTroops(){
        return receivedBonusTroops;
    }

    public int getAmountOfTroopsToDeploy(){
        //Gets the amount of troops based on how many territories owned (minimum 3)
        int baseTroops = Mathf.Max(this.ownedTerritories.Count / 3, 3);

        //Checks for continent bonuses
        foreach(Continent c in ownedContinents){
            baseTroops += c.getBonusTroops();
        }
        return baseTroops;
    }

    //checks if a player owns all countries in a continent
    public void PlayerOwnsContinent(Continent c)
    {
        foreach(Territory t in c.getCountriesInContinent()){
            if(!ownedTerritories.Contains(t)){
                return;
            }
        }
        ownedContinents.Add(c);
    }
}