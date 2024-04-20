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
    
    private List<Card> deck;
    private int deckSize = 0;
    public TMP_Text countText;
    


    //empty constructor for player decks
    void Awake(){
        deck = new List<Card>();
        updateText();   
    }

    //constructor for populating main deck
    public void PopulateDeck(){
        string[] territories = LoadFile("territories");
        string[] armyTypes = LoadFile("armyTypes");
        for(int i = 0; i < territories.Length;i++){
            Card newCard;
            if(territories[i] == "Mission"){
                newCard = new Card("Mission");
            }
            else if(territories[i] == "Wild Card"){
                newCard = new Card("Wild Card");
            }
            else{
                newCard = new Card(territories[i], armyTypes[i], "Territory");
            }
            deck.Add(newCard);
        }

        // for(int i = 0; i < size; i++) {
        //     Card card = new Card();
        //     this.deck.Add(card);
        // }
        deckSize = deck.Count;
        updateText();
    }

    public void updateText(){
        string text = deckSize.ToString();
        //countText.SetText(text);
    }

    //This function is for poulating a deck of cards
    public void AddCards(List<Card> cards){

        for(int i = 0; i < cards.Count; i++) {
            this.deck.Add(cards[i]);
        }
        deckSize = deck.Count;
        updateText();
    }

    //Add single cards to the draw pile
    public void AddCard(Card card) {
        this.deck.Add(card);
        deckSize = deck.Count;
        updateText();
    }

    //DrawCards
    public Card DrawCard() {
        if (deck.Count == 0){
            Debug.LogWarning("Deck is empty!");
            return null;
        }

        Card drawnCard = deck[0];
        deck.RemoveAt(0);
        deckSize = deck.Count;
        updateText();
        return drawnCard;
    }

    public int getCount(){
        return deckSize;
    }

    //ShuffleCards
    public void shuffleCards(){
        Card container;

        for(int i = 0; i < deck.Count; i++){

            container = deck[i];
            int randomIndex = Random.Range(i, deckSize);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = container;
        }
    }

    public int getSize(){
        return deckSize;
    }

    //returns the list of cards for testing 
    public List<Card> getAllCards(){
        return deck;
    }

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
    public void RemoveAllMissionCards(){
        foreach(Card c in this.getAllCards()){
            if(c.getCardType() == "Mission"){
                this.RemoveCard(c);
            }
        }
    }
    public void RemoveCard(Card c){
        this.deck.Remove(c);
    }
    public List<Card> RemoveAllCards() {
        List<Card> removedCards = new List<Card>();
        foreach (Card c in deck) {
            removedCards.Add(DrawCard());
        }

        return removedCards;
    }

}
