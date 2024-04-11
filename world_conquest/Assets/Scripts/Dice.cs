using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    
    //Gets a random dice roll value between 1 and 6 
    public int getDiceValue(){
        return Random.Range(1,7);
    }
    
}