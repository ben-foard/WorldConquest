using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using System;
public class Territory : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI troopText;
    [SerializeField] private Image territoryBackground;
    [SerializeField] private Button territoryButton; 
    [SerializeField] private TextMeshProUGUI territoryName;
    [SerializeField] private List<Territory> neighbours;
    private Color32 territoryColour;
    private int troopCount = 0;
    private Player territoryOwner;
    
    void Awake() 
    {
        territoryColour = territoryBackground.color;
        territoryButton.onClick.AddListener(OnTerritoryButtonClick);
    }

    void OnTerritoryButtonClick() {
        Territory currentTerritory = this;
        Territory prevTerritory = GameManager.Instance.GetPreviousSelectedTerritory();
        string phase = GameManager.Instance.GetCurrentPhase();

        if (phase == "Start" || phase == "Deploy") {
           
            GameManager.Instance.DeployTroops(currentTerritory);
        }
        else if(phase == "Attack"){

            if(prevTerritory != null){
                
                Territory attackingTerritory = prevTerritory;
                if(attackingTerritory.GetNeighbours().Contains(currentTerritory) && !attackingTerritory.GetOwner().GetAllTerritories().Contains(this)){
                    GameManager.Instance.PerformAttack(currentTerritory);
                }     
                else{
                    GameManager.Instance.DisplayNeighbours(currentTerritory);
                }
                return;
            }
            GameManager.Instance.DisplayNeighbours(currentTerritory);            
        }
        else if (phase == "Fortify"){
            
            if(prevTerritory != null) {
                Territory fromTerritory = prevTerritory ;
                Territory toTerritory = currentTerritory;
                if(fromTerritory.GetNeighbours().Contains(toTerritory) && fromTerritory.GetOwner().checkTerritories(toTerritory)){
                    GameManager.Instance.UpdateConfirmVisbility(true);
                    GameManager.Instance.FortifyPositions(fromTerritory, toTerritory);
                }
                       
            }
            else {
                if(this.AvailableTroops() == 0){
                    GameManager.Instance.UpdateSliderVisibility(false);
                    GameManager.Instance.SetPreviousSelectedTerritory(null);
                    return;
                }
                GameManager.Instance.UpdateSliderVisibility(true);
                GameManager.Instance.SetPreviousSelectedTerritory(this);
                GameManager.Instance.UpdateSliderValues(this.AvailableTroops());
            }
            
        }
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
    
    //Method returns total amount of troops
    public int TotalTroops(){
        return this.troopCount;
    }

    //Method returns available troops for fortification 
    public int AvailableTroops(){
        if(troopCount > 1){
            return troopCount - 1;
        }
        return 0;
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

}
