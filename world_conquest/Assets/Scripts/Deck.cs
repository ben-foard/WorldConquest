using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{   
    
    private List<Card> deck = new List<Card>();
    private int deckSize = 0;
    public TMP_Text countText;
   
    //empty constructor for player decks
    void Awake(){
        updateText();   
    }

    //constructor for populating main deck
    public void PopulateDeck(){

        //Loads the types (Territory, wild or mission) and army types from a resources file
        string[] types = LoadFile("types");
        string[] armyTypes = LoadFile("armyTypes");

        for (int i = 0; i < types.Length; i++) {
            Card newCard;
            
            if (types[i] == "Mission" || types[i] == "Wild Card"|| i >= armyTypes.Length) {
                newCard = new Card(types[i]);  // No army type associated with these card types
            } else {
                newCard = new Card(types[i], armyTypes[i], "Territory");  // Use army type only for territory cards
            }
            deck.Add(newCard);
        }

        deckSize = deck.Count;

        updateText();
    }

    //Updates the deck size text
    public void updateText(){
        string text = deckSize.ToString();
    }

    //Adding multiple cards to a deck
    public void AddCards(List<Card> cards){

        for(int i = 0; i < cards.Count; i++) {
            this.deck.Add(cards[i]);
        }
        deckSize = deck.Count;
        updateText();
    }

    //Add single card to the deck
    public void AddCard(Card card) {
        this.deck.Add(card);
        deckSize = deck.Count;
        updateText();
    }

    //Will draw a card from the top of the deck
    public Card DrawCard() {
        if (deck.Count == 0){
            Debug.LogWarning("Deck is empty!");
            return null;
        }
        //Draws the card from top
        Card drawnCard = deck[0];
        deck.RemoveAt(0);

        //Updates the deck size
        deckSize = deck.Count;
        updateText();
        return drawnCard;
    }


    //shuffles the cards in the deck
    public void shuffleCards(){
        Card container;

        for(int i = 0; i < deck.Count; i++){
            
            //Switches two cards between the current index and a random index
            container = deck[i];
            int randomIndex = Random.Range(i, deckSize);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = container;
        }
    }

    //Returns the size of the deck
    public int getSize(){
        return deckSize;
    }

    //returns the list of cards for testing 
    public List<Card> getAllCards(){
        return deck;
    }

    //Loads a inputted file from the resources folder
    string[] LoadFile(string fileName){
        TextAsset asset = Resources.Load<TextAsset>(fileName);
        if(asset != null){
            return asset.text.Split(',');

        }
        else{
            Debug.LogError("Failed to load data from file: " + fileName);
            return null;
        }
    }

    //Removes the mission cards from the deck, used at start of game
    public void RemoveAllMissionCards(){
        foreach(Card c in this.getAllCards()){
            if(c.getCardType() == "Mission"){
                this.RemoveCard(c);
            }
        }
    }

    //Removes a single card from deck
    public void RemoveCard(Card c){
        this.deck.Remove(c);
    }

    //Clears the current deck and removes all cards
    public List<Card> RemoveAllCards() {
        List<Card> removedCards = new List<Card>();
        foreach (Card c in deck) {
            removedCards.Add(DrawCard());
        }

        return removedCards;
    }

}
