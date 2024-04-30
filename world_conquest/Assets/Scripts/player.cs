using System.Collections.Generic;
using UnityEngine;
using TMPro;


///<summary>
/// Represents a player in the game.
///</summary>
public class Player : MonoBehaviour
{
    
    ///<summary>
    /// The name of the player.
    ///</summary>
    private string playerName;

    ///<summary>
    /// The total count of troops the player has.
    ///</summary>
    protected int troopCount = 0;

    ///<summary>
    /// The number of troops the player has left to deploy.
    ///</summary>
    private int troopsToDeploy = 0;

    ///<summary>
    /// The list of territories owned by the player.
    ///</summary>
    private List<Territory> ownedTerritories = new List<Territory>();

    ///<summary>
    /// The color representing the player.
    ///</summary>
    private Color32 playerColour;

    ///<summary>
    /// The deck of cards owned by the player.
    ///</summary>
    private Deck cards = new Deck();


    ///<summary>
    /// The text displaying the player's name.
    ///</summary>
    private TextMeshProUGUI playerNameText;

    ///<summary>
    /// Indicates whether the player has received bonus troops.
    ///</summary>
    private bool receivedBonusTroops = false;

    ///<summary>
    /// The list of continents owned by the player.
    ///</summary>
    private List<Continent> ownedContinents = new List<Continent>();

    ///<summary>
    /// Adds a given amount of troops to the player's troop count.
    ///</summary>
    ///<param name="amount">The amount of troops to add.</param>
    public void AddTroops(int amount)
    {
        troopCount += amount;
    }

    ///<summary>
    /// Removes a given amount of troops from the player's troop count.
    ///</summary>
    ///<param name="amount">The amount of troops to remove.</param>
    public void RemoveTroops(int amount)
    {
        troopCount -= amount;
    }

    ///<summary>
    /// Handles territory fortifying.
    ///</summary>
    ///<param name="fromTerritory">The territory to fortify from.</param>
    ///<param name="toTerritory">The territory to fortify to.</param>
    ///<param name="numOfTroops">The number of troops to fortify with.</param>
    public void Fortify(Territory fromTerritory, Territory toTerritory, int numOfTroops)
    {
        fromTerritory.RemoveTroops(numOfTroops);
        toTerritory.AddTroops(numOfTroops);
    }

    ///<summary>
    /// Returns the amount of troops the player has left to deploy.
    ///</summary>
    ///<returns>The remaining troops to deploy.</returns>
    public int GetTroopsToDeploy()
    {
        return this.troopsToDeploy;
    }

    ///<summary>
    /// Modifies the amount of troops to deploy based on the amount.
    ///</summary>
    ///<param name="amount">The amount to adjust by.</param>
    public void AlterTroopsToDeploy(int amount)
    {
        this.troopsToDeploy += amount;
    }

    ///<summary>
    /// Returns the total amount of troops of the player.
    ///</summary>
    ///<returns>The total troop count.</returns>
    public int GetTroopTotal()
    {
        return troopCount;
    }

    ///<summary>
    /// Returns the player's name.
    ///</summary>
    ///<returns>The player's name.</returns>
    public string GetPlayerName()
    {
        return this.playerName;
    }

    ///<summary>
    /// Returns all territories owned by the player.
    ///</summary>
    ///<returns>The list of owned territories.</returns>
    public List<Territory> GetAllTerritories()
    {
        return this.ownedTerritories;
    }

    ///<summary>
    /// Checks if the player owns a territory.
    ///</summary>
    ///<param name="territory">The territory to check ownership of.</param>
    ///<returns>True if the player owns the territory, false otherwise.</returns>
    public bool CheckTerritories(Territory territory)
    {
        return this.ownedTerritories.Contains(territory);
    }

    ///<summary>
    /// Adds a territory to the player's owned territories.
    ///</summary>
    ///<param name="t">The territory to add.</param>
    public void AddTerritory(Territory t)
    {
        this.ownedTerritories.Add(t);
        t.SetColour(this.playerColour);
    }

    ///<summary>
    /// Removes a territory from the player's owned territories.
    ///</summary>
    ///<param name="t">The territory to remove.</param>
    public void RemoveTerritory(Territory t)
    {
        this.ownedTerritories.Remove(t);
    }

    ///<summary>
    /// Changes the player's playing color on the UI.
    ///</summary>
    ///<param name="c">The new color.</param>
    public void SetPlayerColour(Color32 c)
    {
        this.playerColour = c;
        playerNameText.color = playerColour;
    }

    ///<summary>
    /// Gets the current player's color.
    ///</summary>
    ///<returns>The player's color.</returns>
    public Color32 GetPlayerColour()
    {
        return this.playerColour;
    }

    ///<summary>
    /// Sets the player's name.
    ///</summary>
    ///<param name="name">The new name.</param>
    public void SetPlayerName(string name)
    {
        this.playerName = name;
        playerNameText.text = playerName;
    }

    ///<summary>
    /// Gets the player's deck of cards.
    ///</summary>
    ///<returns>The player's deck.</returns>
    public Deck GetPlayerDeck()
    {
        return cards;
    }  

    ///<summary>
    /// Sets the player's text component.
    ///</summary>
    ///<param name="text">The text component to set.</param>
    public void SetPlayerText(TextMeshProUGUI text)
    {
        playerNameText = text;
    }

    ///<summary>
    /// Returns all territory names owned by the player.
    ///</summary>
    ///<returns>The list of territory names.</returns>
    public List<string> GetAllTerritoryNames()
    {
        List<string> allTerritoryNames = new List<string>();
        foreach(Territory t in ownedTerritories)
        {
            allTerritoryNames.Add(t.GetTerritoryName());
        }
        return allTerritoryNames;
    }

    ///<summary>
    /// Sets whether the player has received bonus troops.
    ///</summary>
    ///<param name="received">True if received, false otherwise.</param>
    public void SetReceivedBonusTroops(bool received)
    {
        receivedBonusTroops = received;
    }

    ///<summary>
    /// Returns whether the player has received bonus troops.
    ///</summary>
    ///<returns>True if received, false otherwise.</returns>
    public bool GetReceivedBonusTroops()
    {
        return receivedBonusTroops;
    }

    ///<summary>
    /// Calculates the amount of troops to deploy for the current turn.
    ///</summary>
    ///<returns>The number of troops to deploy.</returns>
    public int GetAmountOfTroopsToDeploy()
    {
        // Gets the amount of troops based on how many territories owned (minimum 3)
        int baseTroops = Mathf.Max(this.ownedTerritories.Count / 3, 3);

        // Checks for continent bonuses
        foreach(Continent c in ownedContinents)
        {
            baseTroops += c.GetBonusTroops();
        }
        return baseTroops;
    }

    ///<summary>
    /// Checks if the player owns all countries in a continent.
    ///</summary>
    ///<param name="c">The continent to check.</param>
    public void PlayerOwnsContinent(Continent c)
    {
        foreach(Territory t in c.GetCountriesInContinent())
        {
            if(!ownedTerritories.Contains(t))
            {
                return;
            }
        }
        ownedContinents.Add(c);
    }
}
