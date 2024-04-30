using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
/// Represents a dice used in the game.
///</summary>
public class Dice : MonoBehaviour
{
    ///<summary>
    /// Indicates whether the dice is currently rolling.
    ///</summary>
    private bool isRolling = false;

    ///<summary>
    /// Gets a random dice roll value between 1 and 6.
    ///</summary>
    ///<returns>The random dice value.</returns>
    public int GetDiceValue()
    {
        return Random.Range(1, 7);
    }

    ///<summary>
    /// Initiates the dice roll animation.
    ///</summary>
    ///<param name="dice">The list of dice sprites.</param>
    ///<param name="display">The image component displaying the dice.</param>
    public void StartDiceRollAnimation(List<Sprite> dice, Image display)
    {
        isRolling = true;
        StartCoroutine(DiceRollAnimation(dice, display));
    }

    ///<summary>
    /// Coroutine for animating the dice roll.
    ///</summary>
    ///<param name="dice">The list of dice sprites.</param>
    ///<param name="display">The image component displaying the dice.</param>
    public IEnumerator DiceRollAnimation(List<Sprite> dice, Image display)
    {
        float delayInSeconds = 0.25f; 
        for (int i = 0; i < 12; i++)
        {
            int value = Random.Range(0, 6);
            display.sprite = dice[value];
            yield return new WaitForSeconds(delayInSeconds);
        }
        isRolling = false;
    }

    ///<summary>
    /// Returns whether the dice is currently rolling.
    ///</summary>
    ///<returns>True if the dice is rolling, false otherwise.</returns>
    public bool GetIsRolling()
    {
        return isRolling;
    }
}
