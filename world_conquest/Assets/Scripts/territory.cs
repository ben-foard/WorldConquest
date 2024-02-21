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
    private string territoryName;
    public Territory(string name)
    {
        this.territoryName = name;
        Debug.Log(troopCount);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(troopCount);
        UpdateTroopCountUI();
    }
    void Update()
    {
        
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
        troopCount = amount;
        UpdateTroopCountUI();
    }

    public void removeTroops(int amount)
    {
        troopCount -= amount;
        UpdateTroopCountUI();
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
