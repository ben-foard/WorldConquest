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
    private int troopsToDeploy = 4;
    private List<Territory> ownedTerritories;

    public Player(string name)
    {
        this.playerName = name;
        this.ownedTerritories = new List<Territory>();
    }

    // Start is called before the first frame update
    void Start()
    {
      
    } 

    public void AddTroops(int amount)
    {
        troopCount += amount;
    }

    public void AttackTerritory(Territory ter)
    {
        ter.RemoveTroops(1);
        
    }
    public int GetTroopsToDeploy()
    {
        return this.troopsToDeploy;
    }
    public void AlterTroopsToDeploy(int amount)
    {
        this.troopsToDeploy += amount;
    }
    public void Fortify()
    {
        return;
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
    public void AddTerritory(Territory t)
    {
        this.ownedTerritories.Add(t);
    }
    public void RemoveTerritory(Territory t)
    {
        this.ownedTerritories.Remove(t);
    }
    /**fortify()
        //for this sprint this phase will just print fortified 
    **/

    /**gettroopsTotal()
        //skips for the moment
    **/

    /**getPlayerNamel()
        //skips for the moment
    **/
}
