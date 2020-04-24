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

    public class Player
    {        
        private readonly int initialMoney = 1500;

        // What piece represents this player.
        private string piece;

        // Which player is this ie: player 1
        private int playerNum;

        // Current amount of money.
        private int money;

        // properties owned by the player.
        private List<Property> properties;

        public Player(int playNum, string chosenPiece)
        {
            this.playerNum = playNum;
            this.money = this.initialMoney;
            this.piece = chosenPiece;
        }

        public string Piece
        {
            get { return this.piece; }
        }

        public List<Property> Properties
        {
            get { return this.properties; }
        }

        public int Money
        {
            get { return this.money; }
            set { this.money += value; }
        }

        // Add property to list of owned properties
        private void BuyProperty(Property prop)
        {
            this.properties.Add(prop);
        }
    }
}
