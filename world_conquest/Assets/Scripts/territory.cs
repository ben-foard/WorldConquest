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
    public void SetTroops(int amount = 5)
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
    //public Button GetTerritoryButton()
    //{
    //    return this.territoryButton;
   //}
    
    /**setTroops()
        this.troopsPerland = random assigned.
    **/

    /**getTroops()
    **/

    /**getName()

    **/

    // Update is called once per frame

}
