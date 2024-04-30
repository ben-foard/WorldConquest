///<summary>
/// Represents a card used in a game.
///</summary>
public class Card
{
    ///<summary>
    /// The name of the territory associated with the card.
    ///</summary>
    private string territory;

    ///<summary>
    /// The type of army associated with the card.
    ///</summary>
    private string armyType;

    ///<summary>
    /// The type of the card.
    ///</summary>
    private string cardType;

    ///<summary>
    /// Constructor for creating territory cards.
    ///</summary>
    ///<param name="territory">The name of the territory.</param>
    ///<param name="armyType">The type of army associated with the territory.</param>
    ///<param name="cardType">The type of the card.</param>
    public Card(string territory, string armyType, string cardType)
    {
        this.territory = territory;
        this.armyType = armyType;
        this.cardType = cardType;
    }

    ///<summary>
    /// Overloaded constructor for creating mission or wild cards.
    ///</summary>
    ///<param name="cardType">The type of the card.</param>
    public Card(string cardType)
    {
        this.territory = "empty";
        this.armyType = "empty";
        this.cardType = cardType;
    }

    ///<summary>
    /// Returns the name of the territory associated with the card.
    ///</summary>
    ///<returns>The name of the territory.</returns>
    public string GetTerritoryName()
    {
        return this.territory;
    }

    ///<summary>
    /// Returns the type of army associated with the card.
    ///</summary>
    ///<returns>The type of army.</returns>
    public string GetArmyType()
    {
        return this.armyType;
    }

    ///<summary>
    /// Returns the type of the card.
    ///</summary>
    ///<returns>The type of the card.</returns>
    public string GetCardType()
    {
        return this.cardType;
    }
}
