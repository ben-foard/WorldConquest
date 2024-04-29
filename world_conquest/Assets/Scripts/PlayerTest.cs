using UnityEngine;

///<summary>
/// Unit tests for the Player class.
///</summary>
public class PlayerTest : MonoBehaviour
{       
    /*
        THIS CLASS IS CURRENTLY OUT OF USE
    */
    
    private Territory t1;
    private Territory t2;
    private Player p1;
    private Player p2;

    ///<summary>
    /// Initializes the test environment.
    ///</summary>
    void Awake() 
    {
        t1 = new Territory();
        t2 = new Territory();
        
        // p1 = new Player("player1", );
        // p2 = new Player("player2");
    }

    ///<summary>
    /// Tests the AddTroops method of the Player class.
    ///</summary>
    ///<returns>True if the test passes, false otherwise.</returns>
    public bool AddTroopsTest() 
    {
        p1.AddTroops(5);
        return p1.GetTroopTotal() == 5;
    }

    ///<summary>
    /// Tests the attackTerritory method.
    ///</summary>
    ///<returns>True if the test passes, false otherwise.</returns>
    public bool AttackTerritoryTest()
    {
        return t1.GetTerritoryTroopCount() == 5 && t2.GetTerritoryTroopCount() == 4;
    }
}
