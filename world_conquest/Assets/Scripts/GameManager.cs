using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For working with UI Button
using TMPro;
public class GameManager : MonoBehaviour
{   
    public Territory t1; 
    public Territory t2; 
    public Button deployButton; 
    public Button attackButton; 
    public Button fortifyButton; 
    public Button continueButton;

    private int PlayerIndex = 0;
    private List<Player> CurrentPlayers = new List<Player>();
    private Player p1;
    private Player p2;
    enum gamePhases {
        Deploy,
        Attack,
        Fortify
    }

    private gamePhases currentGamePhase = gamePhases.Deploy;
    void Awake() 
    {
        
        t1 = new Territory("territory1");
        t2 = new Territory("territory2");
        p1 = new Player("player1", t1);
        p2 = new Player("player2", t2);
        
    }
    
    void Start()
    {   
        
        CurrentPlayers.Add(p1);
        CurrentPlayers.Add(p2);
        StartGame();
    }
    void Update()
    {

    }
    
    private void StartGame()
    {
        UpdateUI();
        continueButton.onClick.AddListener(AdvancePhase);
    }
    private void AdvancePhase()
    { 
        Debug.Log(PlayerIndex);
        if(CheckWin())
        {
            Debug.Log("Player 1 has won!");   
            return;    
        }
        if(currentGamePhase == gamePhases.Fortify)
        {
            EndPlayerTurn();
        }
        else
        {
            currentGamePhase = (gamePhases)(((int)currentGamePhase + 1) % System.Enum.GetValues(typeof(gamePhases)).Length);
            //This does the same as the get current player math where it goes through how ever long the enum is
        }
        
        UpdateUI();
        switch (currentGamePhase)
        {
            case gamePhases.Deploy:               
                break;
            case gamePhases.Attack:                
                attackButton.interactable = true;
                attackButton.onClick.AddListener(CurrentPlayers[PlayerIndex].attackTerritory);
                break;
            case gamePhases.Fortify:             
                break;
        }

    }
    private void EndPlayerTurn()
    {
        PlayerIndex = (PlayerIndex+1)%2;
        currentGamePhase = gamePhases.Deploy; //2 will take the place of the amount of players in the future so that it only goes through the two indicies 
        UpdateUI();              
    }

    private bool CheckWin()
    {
        if(p2.GetTroopTotal() == 0)
        {
            return true;
        }
        return false;
    }
    void UpdateUI()
    {
        deployButton.interactable = currentGamePhase == gamePhases.Deploy;
        attackButton.interactable = currentGamePhase == gamePhases.Attack;
        fortifyButton.interactable = currentGamePhase == gamePhases.Fortify;
    }
}
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
    /**createPlayers()
        //Player name  = player1
        //Player name  = player2
    **/