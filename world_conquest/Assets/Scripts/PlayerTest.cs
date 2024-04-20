using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerTest : MonoBehaviour
{       
    /*
        THIS CLASS IS CURRENTLY OUT OF USE
    */
    
    private Territory t1;
    private Territory t2;
    private Player p1;
    private Player p2;

       void Awake() 
        {
            t1 = new Territory();
            t2 = new Territory();
            
           // p1 = new Player("player1", );
           // p2 = new Player("player2");
        }
       public bool addTroops() 
       {
            p1.AddTroops(5);
            if(p1.GetTroopTotal() == 5)
            {
                return true;
            }
            return false;
       }
       public bool attackTerritory()
       {
          
            if(t1.GetTerritoryTroopCount() == 5 && t2.GetTerritoryTroopCount() == 4)
            {
                return true;
            }
            return false;
       }
}



