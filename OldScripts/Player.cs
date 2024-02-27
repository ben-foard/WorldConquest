using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    private string playerName;
    private Territory currentTerritory;
    private int troopCount = 5;
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



    public void attackTerritory()
    {
        if(playerName == "player2")
        {
            currentTerritory.removeTroops(1);
        }
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
        return null;
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
