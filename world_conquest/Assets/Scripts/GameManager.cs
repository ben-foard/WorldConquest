using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
public class GameManager : MonoBehaviour
{   
    public Territory t1;
    public Territory t2;
    private Slider slider;
    [SerializeField] private List<Territory> allTerritories;
    //Will be a list in the future
    [SerializeField] private TextMeshProUGUI currentPlayerText;
    private int PlayerIndex = 0;
    [SerializeField] private List<Player> CurrentPlayers = new List<Player>();
    private Player p1;
    private Player p2;
    private ButtonManager buttonManager;
    private int troopsToDeploy = 5;

    enum gamePhases {
        Deploy,
        Attack,
        Fortify,
        EndGame
    }
    
    private gamePhases currentGamePhase = gamePhases.Deploy;

    void Awake() 
    {
        p1 = new Player("player1", t1);
        p2 = new Player("player2", t2);
        CurrentPlayers.Add(p1);
        CurrentPlayers.Add(p2);
    }
    
    void Start()
    {           
        slider = FindObjectOfType<Slider>();
        buttonManager = FindObjectOfType<ButtonManager>();
        StartGame();
    }

    void Update()
    {

    }
    
    private void StartGame()
    {
        UpdateUI();
        buttonManager.continueButton.onClick.AddListener(AdvancePhase);
    }
    private void AdvancePhase()
    { 
        if(CheckWin())
        {
            currentGamePhase = gamePhases.EndGame;
            EndGame();    
        }

        else if(currentGamePhase == gamePhases.Fortify)
        {
            EndPlayerTurn();
        }

        else
        {
            currentGamePhase = (gamePhases)(((int)currentGamePhase + 1) % System.Enum.GetValues(typeof(gamePhases)).Length);
            //This does the same as the get current player math where it goes through how ever long the enum is
            

            UpdateUI();
            switch (currentGamePhase)
            {
                case gamePhases.Deploy:               
                    break;
                case gamePhases.Attack: 
                    slider.gameObject.SetActive(false);              
                    break;
                case gamePhases.Fortify:             
                    break;
            }
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
        if(t2.GetTerritoryTroopCount() == 0)
        {
            return true;
        }
        return false;
    }

    private void EndGame()
    {
        UpdateUI();
        currentPlayerText.text = "Winner: player 1!";   
    }
    void UpdateUI()
    {
        buttonManager.InteractableUpdater(currentGamePhase != gamePhases.EndGame);

        currentPlayerText.text = "Current Turn: " + CurrentPlayers[PlayerIndex].getPlayerName();
    }    
    public void PerformAttack()
    {  
        
        foreach(Territory t in CurrentPlayers[PlayerIndex].getAllTerritories()){
            //t.GetonClick.AddListener(ShowNeighbouringCountries);
        }
        CurrentPlayers[PlayerIndex].attackTerritory(t2);
        AdvancePhase();
    }
    public void ShowNeighbouringCountries()
    {

    }

    // public void PerformDeploy()
    // {
    //     while()
    // }
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