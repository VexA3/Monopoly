//-----------------------------------------------------------------------
// <copyright file="Player.cs" company="CompanyName">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace Monopoly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// A class to hold player data
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Number to hold starting money value
        /// </summary>
        private readonly int initialMoney = 1500;

        /// <summary>
        /// Piece that represents the player
        /// </summary>
        private string piece;

        /// <summary>
        /// The numeric value of the player 1-8
        /// </summary>
        private int playerNum;

        /// <summary>
        /// Number of turns spent in jail. 
        /// </summary>
        private int jailTurnCount;

        /// <summary>
        /// How much money the player has.
        /// </summary>
        private int money;

        /// <summary>
        /// list of properties owned by the player
        /// </summary>
        private List<Property> properties;

        /// <summary>
        /// list of get out of jail free cards owned by the player
        /// </summary>
        private List<Card> getOutOfJailCards;

        /// <summary>
        /// Initializes a new instance of the Player class
        /// </summary>
        /// <param name="playNum">The number for the player 1-8</param>
        /// <param name="chosenPiece">The name of the piece that player chosen</param>
        public Player(int playNum, string chosenPiece)
        {
            this.playerNum = playNum;
            this.money = this.initialMoney;
            this.piece = chosenPiece;
            this.jailTurnCount = 0;
            this.getOutOfJailCards = new List<Card>();
            this.properties = new List<Property>();
    }

        /// <summary>
        /// Gets the piece for the player
        /// </summary>
        public string Piece
        {
            get { return this.piece; }
        }

        /// <summary>
        /// Gets the list of properties owned by the player
        /// </summary>
        public List<Property> Properties
        {
            get { return this.properties; }
        }

        /// <summary>
        /// Gets the get out of jail card objects
        /// </summary>
        public List<Card> GetOutOfJailCards
        {
            get { return this.getOutOfJailCards; }
        }

        /// <summary>
        /// Gets or sets the money value of the player
        /// </summary>
        public int Money
        {
            get { return this.money; }
            set { this.money += value; }
        }

        /// <summary>
        /// Gets or sets the value of number of turns in jail.
        /// </summary>
        public int JailTurnCount
        {
            get { return this.jailTurnCount; }
            set { this.jailTurnCount = value; }
        }

        /// <summary>
        /// Add property to list of owned properties
        /// </summary>
        /// <param name="prop"> The property to add to the players owned properties </param>
        /// <param name="owner"> The player that is the new owner of the property</param>
        public void BuyProperty(Property prop, Player owner)
        {
            this.money = this.money - prop.Price;
            prop.Owner = owner;
            this.properties.Add(prop);
        }

        /// <summary>
        /// Add card to list of owned cards
        /// </summary>
        /// <param name="card"> The card to add to the players owned card </param>
        public void DrawCard(Card card)
        {
            this.getOutOfJailCards.Add(card);
        }
    }
}
