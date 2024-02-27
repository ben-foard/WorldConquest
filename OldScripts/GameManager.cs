using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    private int PlayerIndex = 0;
    private List<Player> CurrentPlayers = new List<Player>();
    private Territory t1;
    private Territory t2;
    private Player p1;
    private Player p2;
    private string[] currentPhase = new string[2];
    
    void Awake() 
    {
        t1 = new Territory("territory1");
        t2 = new Territory("territory2");
        p1 = new Player("player1", t1);
        p2 = new Player("player2", t2);
        currentPhase[0] = "Deploy";
        currentPhase[1] = "Attack";
        currentPhase[2] = "Fortify";
    }
    enum gamePhases {
        Deploy,
        Attack,
        Fortify
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void StartGame()
    {
        CurrentPlayers.Add(p1);
        CurrentPlayers.Add(p2);
        GameLoop();
    }
    private void GameLoop()
    {
        
        if(CheckWin())
        {
            Debug.Log("Player 1 wins");       
        }
        else
        {
            // switch(currentPhase)
            // {
            //     case gamePhases.Deploy:         
            //         return; 
            //     case gamePhases.Attack:
            //         CurrentPlayers[PlayerIndex].attackTerritory();
            //         return;
            //     case gamePhases.Fortify:
            //         return;
            // }
            for(int i = 0; i < currentPhase.Length; i++)
            {
                switch(i)
                {
                    case 0:         
                      return; 
                    case 1:
                      CurrentPlayers[PlayerIndex].attackTerritory();
                      return;
                    case 2:
                       return;
                }
            }
            
        }

    }
    private void EndPlayerTurn()
    {
        PlayerIndex = (PlayerIndex+1)%2; //2 will take the place of the amount of players in the future so that it only goes through the two indicies 
        GameLoop();                
    }

    private bool CheckWin()
    {
        if(p2.GetTroopTotal() == 0)
        {
            return true;
        }
        return false;
    }
}
    /**createPlayers()
        //Player name  = player1
        //Player name  = player2
    **/    
    /**gameloop():
        // check win
        // startPlayer turn
        // runPhases
        // end player turn
    **/

    /**runPhases()
        //player.addTroops()
        //player.attack()
        //player.fortify()
    **/