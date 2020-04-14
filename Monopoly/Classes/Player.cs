using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly
{
    public class Player
    {
        public Player(int playNum, string chosenPiece)
        {
            playerNum = playNum;
            money = initialMoney;
            piece = chosenPiece;
        }
        private readonly int initialMoney = 1500;
        //What piece represents this player.
        private string piece;
        //Which player is this ie: player 1
        private int playerNum;
        //Current amount of money.
        private int money;
        //properties owned by the player.
        private List<Property> properties;

        public string Piece
        {
            get { return piece; }
        }
        public List<Property> Properties
        {
            get { return properties; }
        }

        //Add property to list of owned properties
        private void buyProperty(Property prop)
        {
            properties.Add(prop);
        }
    }
}
