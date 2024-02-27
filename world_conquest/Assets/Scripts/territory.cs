using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class Territory : MonoBehaviour
{
 //Name
    //Sections = 1 
    public TextMeshProUGUI troopText;
    public Button territoryButton; 
    private int troopCount = 5;
    public string territoryName;

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
    public void setTroops(int amount = 5)
    {
        this.troopCount = amount;
    }

    public void removeTroops(int amount)
    {
        troopCount -= amount;
    }

    public void SetTerritoryName(string name)
    {
        this.territoryName = name;
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
