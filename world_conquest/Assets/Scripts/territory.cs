using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
public class Territory : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI troopText;
    [SerializeField] private Image territoryBackground;
    [SerializeField] private Button territoryButton; 
    private Color32 territoryColour;
    private int troopCount = 0;
    [SerializeField] private TextMeshProUGUI territoryName;
    [SerializeField] private List<Territory> neighbours;
    private Player territoryOwner;

    void Awake() 
    {
        territoryColour = territoryBackground.color;
        territoryButton.onClick.AddListener(OnTerritoryButtonClick);
    }

    void OnTerritoryButtonClick() {

        
        if (GameManager.Instance.GetCurrentPhase() == "Start" || GameManager.Instance.GetCurrentPhase() == "Deploy") {
           
            GameManager.Instance.DeployTroops(this);
        }
        else if(GameManager.Instance.GetCurrentPhase() == "Attack"){
            
            GameManager.Instance.DisplayNeighbours(this);
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
    public List<Territory> GetNeighbours()
    {
        return this.neighbours;
    }
    public void HighlightTerritory()
    {
        territoryBackground.color = new Color32(0, 255, 0, 255);
    }
    public void RevertHighlight()
    {
        territoryBackground.color = this.territoryColour;
    }
    public void SetColour(Color32 c)
    {
        this.territoryColour = c;
        territoryBackground.color = c;
    }
    public void SetOwner(Player p)
    {
        this.territoryOwner = p;
    }
    public Player GetOwner()
    {
        return this.territoryOwner;
    }
    public void ChangeOwner(Player p)
    {
        this.territoryOwner.RemoveTerritory(this);
        this.territoryOwner = p;
        this.territoryColour = p.GetPlayerColour();
        SetColour(this.territoryColour);
        p.AddTerritory(this);
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
