//-----------------------------------------------------------------------
// <copyright file="Card.cs" company="CompanyName">
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
    /// A class to hold card data.
    /// </summary>
    public class Card
    {
        /// <summary>
        /// What will be displayed on the card
        /// </summary>
        private string cardText;

        /// <summary>
        /// Used to determine the method to be ran when card is drawn
        /// </summary>
        private string action;

        /// <summary>
        /// Whether or not a player has this card in his possession
        /// </summary>
        private bool inPossession;

        /// <summary>
        /// Initializes a new instance of the Card class
        /// </summary>
        /// <param name="crdTxt">The text on the card</param>
        /// <param name="act">The method to be performed for the card</param>
        public Card(string crdTxt, string act)
        {
            this.cardText = crdTxt;
            this.action = act;
            this.inPossession = false;
        }

        /// <summary>
        /// Gets card text to display
        /// </summary>
        public string CardText
        {
            get { return this.cardText; }
        }

        /// <summary>
        /// Gets the name of the action to be called when card is drawn
        /// </summary>
        public string Action
        {
            get { return this.action; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a player has the card in their possession
        /// </summary>
        public bool InPossession
        {
            get { return this.inPossession; }
            set { this.InPossession = value; }
        }
    }
}