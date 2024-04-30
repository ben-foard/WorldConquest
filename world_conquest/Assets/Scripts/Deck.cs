using System.Collections.Generic;
using TMPro;
using UnityEngine;


///<summary>
/// Represents a deck of cards used in the game.
///</summary>
public class Deck : MonoBehaviour
{   
    ///<summary>
    /// The list of cards in the deck.
    ///</summary>
    private List<Card> deck = new List<Card>();

    ///<summary>
    /// The current size of the deck.
    ///</summary>
    private int deckSize = 0;

    ///<summary>
    /// Text component displaying the deck size.
    ///</summary>
    public TMP_Text countText;
   
    ///<summary>
    /// Populates the main deck with cards.
    ///</summary>
    public void PopulateDeck()
    {
        // Loads the types (Territory, wild, or mission) and army types from a resources file
        string[] types = LoadFile("types");
        string[] armyTypes = LoadFile("armyTypes");

        for (int i = 0; i < types.Length; i++) {
            Card newCard;
            
            if (types[i] == "Mission" || types[i] == "Wild Card" || i >= armyTypes.Length) {
                newCard = new Card(types[i]);  // No army type associated with these card types
            } else {
                newCard = new Card(types[i], armyTypes[i], "Territory");  // Use army type only for territory cards
            }
            deck.Add(newCard);
        }

        deckSize = deck.Count;

    }

    ///<summary>
    /// Adds multiple cards to the deck.
    ///</summary>
    ///<param name="cards">The list of cards to add.</param>
    public void AddCards(List<Card> cards)
    {
        for(int i = 0; i < cards.Count; i++) {
            this.deck.Add(cards[i]);
        }
        deckSize = deck.Count;
    }

    ///<summary>
    /// Adds a single card to the deck.
    ///</summary>
    ///<param name="card">The card to add.</param>
    public void AddCard(Card card)
    {
        this.deck.Add(card);
        deckSize = deck.Count;
    }

    ///<summary>
    /// Draws a card from the top of the deck.
    ///</summary>
    ///<returns>The drawn card.</returns>
    public Card DrawCard()
    {
        if (deck.Count == 0){
            Debug.LogWarning("Deck is empty!");
            return null;
        }
        // Draw the card from the top
        Card drawnCard = deck[0];
        deck.RemoveAt(0);

        // Update the deck size
        deckSize = deck.Count;
        return drawnCard;
    }

    ///<summary>
    /// Shuffles the cards in the deck.
    ///</summary>
    public void ShuffleCards()
    {
        Card container;

        for(int i = 0; i < deck.Count; i++){
            // Switches two cards between the current index and a random index
            container = deck[i];
            int randomIndex = Random.Range(i, deckSize);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = container;
        }
    }

    ///<summary>
    /// Returns the size of the deck.
    ///</summary>
    ///<returns>The size of the deck.</returns>
    public int GetSize()
    {
        deckSize = deck.Count;
        return deckSize;
    }

    ///<summary>
    /// Returns the list of cards in the deck for testing purposes.
    ///</summary>
    ///<returns>The list of cards.</returns>
    public List<Card> GetAllCards()
    {
        return deck;
    }

    ///<summary>
    /// Loads a file from the resources folder.
    ///</summary>
    ///<param name="fileName">The name of the file to load.</param>
    ///<returns>An array containing the data from the file.</returns>
    string[] LoadFile(string fileName)
    {
        TextAsset asset = Resources.Load<TextAsset>(fileName);
        if(asset != null){
            return asset.text.Split(',');
        }
        else{
            Debug.LogError("Failed to load data from file: " + fileName);
            return null;
        }
    }

    ///<summary>
    /// Removes all mission cards from the deck.
    ///</summary>
    public void RemoveAllMissionCards()
    {
        foreach(Card c in this.GetAllCards()){
            if(c.GetCardType() == "Mission"){
                this.RemoveCard(c);
            }
        }
    }

    ///<summary>
    /// Removes a single card from the deck.
    ///</summary>
    ///<param name="c">The card to remove.</param>
    public void RemoveCard(Card c)
    {
        this.deck.Remove(c);
    }

    ///<summary>
    /// Clears the current deck and removes all cards.
    ///</summary>
    ///<returns>A list containing all removed cards.</returns>
    public List<Card> RemoveAllCards()
    {
        List<Card> removedCards = new List<Card>();
        foreach (Card c in deck) {
            removedCards.Add(DrawCard());
        }

        return removedCards;
    }
}
