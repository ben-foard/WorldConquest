using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    private string playerName;
    private Territory currentTerritory;
    private int troopCount = 40;
    private int troopsToDeploy;
    private List<Territory> ownedTerritories;

    public Player(string name, Territory ter)
    {
        this.playerName = name;
        this.currentTerritory = ter;
    }

    // Start is called before the first frame update
    void Start()
    {
      
    } 

    public void AddTroops(int amount)
    {
        troopCount += amount;
    }

    public void attackTerritory(Territory ter)
    {
        ter.RemoveTroops(1);
        
    }
    public int getTroopsToDeploy()
    {
        return this.troopsToDeploy;
    }
    public void Fortify()
    {
        return;
    }
    public int GetTroopTotal()
    {
        return troopCount;
    }
    public string getPlayerName()
    {
        return this.playerName;
    }
    public List<Territory> getAllTerritories()
    {
        return this.ownedTerritories;
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
