﻿//-----------------------------------------------------------------------
// <copyright file="CommunityChest.cs" company="CompanyName">
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
    /// A class to hold community chest data
    /// </summary>
    public class CommunityChest
    {
        private string cardText;

        // The method to run when this card is drawn
        private string action;

        /// <summary>
        /// Initializes a new instance of the CommunityChest class
        /// </summary>
        /// <param name="crdTxt">The text on the card</param>
        /// <param name="act">The method to be performed for the card</param>
        public CommunityChest(string crdTxt, string act)
        {
            this.cardText = crdTxt;
            this.action = act;
        }

        public string CardText
        {
            get { return cardText; }
        }

        public string Action
        {
            get { return action; }
        }        
    }
}
