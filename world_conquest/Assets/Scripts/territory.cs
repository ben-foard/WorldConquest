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
    [SerializeField] private TextMeshProUGUI territoryName;
    [SerializeField] private List<Territory> neighbours;

    void Awake() 
    {
        territoryButton.onClick.AddListener(OnTerritoryButtonClick);
    }

    void OnTerritoryButtonClick() {
        
        if (GameManager.Instance.GetCurrentPhase() == "Start" || GameManager.Instance.GetCurrentPhase() == "Deploy") {
           
            GameManager.Instance.DeployTroops(this);
        }

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
    public void AddTroops(int amount)
    {
        this.troopCount += amount;
    }

    public void RemoveTroops(int amount)
    {
        troopCount -= amount;
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
