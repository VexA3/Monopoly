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
        /// How much money the player has.
        /// </summary>
        private int money;

        /// <summary>
        /// list of properties owned by the player
        /// </summary>
        private List<Property> properties;

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
        /// Gets or sets the money value of the player
        /// </summary>
        public int Money
        {
            get { return this.money; }
            set { this.money += value; }
        }

        /// <summary>
        /// Add property to list of owned properties
        /// </summary>
        /// <param name="prop"> The property to add to the players owned properties </param>
        private void BuyProperty(Property prop)
        {
            this.properties.Add(prop);
        }
    }
}
