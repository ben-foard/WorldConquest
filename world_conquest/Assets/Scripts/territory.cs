using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class Territory : MonoBehaviour
{
 //Name
    //Sections = 1 
    [SerializeField] private TextMeshProUGUI troopText;
    [SerializeField] private Button territoryButton; 
    private int troopCount = 0;
    [SerializeField] private string territoryName;
    [SerializeField] private List<Territory> neighbours;

    void Awake() 
    {
        territoryButton.onClick.AddListener(OnTerritoryButtonClick);
    }

    void OnTerritoryButtonClick() {
        Debug.Log("test");
        // Assuming GameManager can be accessed to check the current phase and switch players
        //if (GameManager.Instance.CurrentGamePhase == GameManager.gamePhases.Start || GameManager.Instance.CurrentGamePhase == GameManager.gamePhases.Deploy) {
            // Logic to determine the number of troops to deploy, e.g., based on a UI element or a fixed number for this phase
            int troopsToDeploy = 5; // Example fixed number, replace with dynamic logic as needed

            // Deploy troops and update the UI
            SetTroops(troopCount + troopsToDeploy);
            UpdateTroopCountUI();

            // Notify GameManager to switch to the next player
            //GameManager.Instance.SwitchToNextPlayer();
        //}
    }

    void Start()
    {
       
    }
    void Update()
    {
        UpdateTroopCountUI();
    }
    public int GetTerritoryTroopCount()
    {
        return troopCount;
    }

    public void UpdateTroopCountUI()
    {
        if(troopText != null)
        {
            troopText.text = troopCount.ToString();
        }
    }
    public void SetTroops(int amount)
    {
        this.troopCount = amount;
    }

    public void RemoveTroops(int amount)
    {
        troopCount -= amount;
    }

    public void SetTerritoryName(string name)
    {
        this.territoryName = name;
    }
    public Button GetTerritoryButton()
    {
        return this.territoryButton;
    }
    public void OnTerritoryClick()
    {
        
    }
    /**setTroops()
        this.troopsPerland = random assigned.
    **/

    /**getTroops()
    **/

    /**getName()

    **/

    // Update is called once per frame

}
