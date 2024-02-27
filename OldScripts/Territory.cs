using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Territory : MonoBehaviour
{
 //Name
    //Sections = 1 
    public TextMeshProUGUI troopText;
    public int troopCount = 0;
    private string territoryName;
    public Territory(string name)
    {
        this.territoryName = name;
    }
    // Start is called before the first frame update
    void Start()
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
    }

    public void removeTroops(int amount)
    {
        troopCount -= amount;
    }
    
    /**setTroops()
        this.troopsPerland = random assigned.
    **/

    /**getTroops()
    **/

    /**getName()

    **/

    // Update is called once per frame
    void Update()
    {
        
    }
}
