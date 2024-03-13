using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using System;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    private string playerName;
    private int troopCount = 0;
    private int troopsToDeploy = 5;
    private List<Territory> ownedTerritories;
    private Color32 playerColour;

    public Player(string name)
    {
        this.playerName = name;
        this.ownedTerritories = new List<Territory>();
    }

    public void AddTroops(int amount)
    {
        troopCount += amount;
    }
    public void RemoveTroops(int amount)
    {
        troopCount -= amount;
    }

    public void AttackTerritory(Territory territoryAttacking, Territory territoryDefending, int attackingValue, int defendingValue)
    {
        if(attackingValue > defendingValue){
            this.AddTroops(1);
            territoryDefending.GetOwner().RemoveTroops(1);
            if(territoryDefending.GetTerritoryTroopCount() == 1){
                territoryDefending.ChangeOwner(this);
            }
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

    public int GetTroopsToDeploy()
    {
        return this.troopsToDeploy;
    }

    public void AlterTroopsToDeploy(int amount)
    {
        this.troopsToDeploy += amount;
    }

    public int GetTroopTotal()
    {
        return troopCount;
    }
    public string GetPlayerName()
    {
        return this.playerName;
    }
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

    public void AddTerritory(Territory t)
    {
        this.ownedTerritories.Add(t);
        t.SetColour(this.playerColour);
    }
    public void RemoveTerritory(Territory t)
    {
        this.ownedTerritories.Remove(t);
    }
    public void SetPlayerColour(Color32 c)
    {
        this.playerColour = c;
    }
    public Color32 GetPlayerColour()
    {
        return this.playerColour;
    }

}
