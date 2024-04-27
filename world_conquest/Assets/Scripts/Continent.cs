using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using System;
using Unity.VisualScripting;

public class Continent : MonoBehaviour
{
    [SerializeField] private List<Territory> countriesInContinent = new List<Territory>();
    [SerializeField] private int bonusTroops;
    [SerializeField] private string continentName;

    public List<Territory> getCountriesInContinent(){
        return countriesInContinent;
    }

    public int getBonusTroops(){
        return bonusTroops;
    }

    public string getContinentName(){
        return continentName;
    }

}
