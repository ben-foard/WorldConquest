using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{

    //Gets a random dice roll value between 1 and 6 
    public int getDiceValue(){
        return Random.Range(1,7);
    }

    public void StartDiceRollAnimation(List<Sprite> dice, Image display)
    {
        StartCoroutine(DiceRollAnimation(dice, display));
    }

    private IEnumerator DiceRollAnimation(List<Sprite> dice, Image display)
    {
        int numberOfImages = dice.Count; 
        float delayInSeconds = 0.5f; 
        for (int i = 0; i < 12; i++)
        {
            int value = Random.Range(0,6);
            display.sprite = dice[value];
            yield return new WaitForSeconds(delayInSeconds);
        }
    }
}