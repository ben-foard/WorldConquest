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

    //Constructor for territory cards
    public Card(string territory, string armyType, string cardType){
        this.territory = territory;
        this.armyType = armyType;
        this.cardType = cardType;
    }

    //Overloaded constructor for mission or wild cards
    public Card(string cardType){
        this.territory = "empty";
        this.armyType = "empty";
        this.cardType=cardType;
    }

    //Returns territory name
    public string GetTerritoryName(){
        return this.territory;
    }

    //Returns the army type
    public string getArmyType(){
        return this.armyType;
    }

    //Returns the card type
    public string getCardType(){
        return this.cardType;
    }

}


