using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class Card
{
    private string territory;
    private string armyType;
    private string cardType;

    void Awake(){
        
    }

    public Card(string territory, string armyType, string cardType){
        this.territory = territory;
        this.armyType = armyType;
        this.cardType = cardType;
    }
    public Card(string cardType){
        this.territory = null;
        this.armyType = null;
        this.cardType=cardType;
    }
    public string GetTerritoryName(){
        return this.territory;
    }

    public string getArmyType(){
        return this.armyType;
    }

    public string getCardType(){
        return this.cardType;
    }

}


