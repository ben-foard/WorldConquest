using System.Collections.Generic;
using UnityEngine;


///<summary>
/// Represents a continent in the game.
///</summary>
public class Continent : MonoBehaviour
{

    ///<summary>
    /// The list of countries contained within the continent.
    ///</summary>
    [SerializeField] private List<Territory> countriesInContinent = new List<Territory>();

    /// <summary>
    /// The bonus troops awarded for controlling the entire continent.
    /// </summary>    
    [SerializeField] private int bonusTroops;

    /// <summary>
    /// The name of the continent.
    /// </summary>    
    [SerializeField] private string continentName;

    ///<summary>
    /// Returns the list of countries within the continent.
    ///</summary>
    ///<returns>The list of countries.</returns>
    public List<Territory> GetCountriesInContinent()
    {
        return countriesInContinent;
    }

    ///<summary>
    /// Returns the bonus troops awarded for controlling the continent.
    ///</summary>
    ///<returns>The bonus troops.</returns>
    public int GetBonusTroops()
    {
        return bonusTroops;
    }

    ///<summary>
    /// Returns the name of the continent.
    ///</summary>
    ///<returns>The name of the continent.</returns>
    public string GetContinentName()
    {
        return continentName;
    }
}
