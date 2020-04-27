﻿//-----------------------------------------------------------------------
// <copyright file="Property.cs" company="CompanyName">
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
    /// A class to hold property data
    /// </summary>
    public class Property
    {
        /// <summary>
        /// Price of railroads
        /// </summary>
        private readonly int railroadPrice = 200;

        /// <summary>
        /// Price of utilities
        /// </summary>
        private readonly int utilityPrice = 150;

        /// <summary>
        /// Price of the property.
        /// </summary>
        private int price;

        /// <summary>
        /// Mortgage amount of the property.
        /// </summary>
        private int mortgage;

        /// <summary>
        /// Keeps track if the property is owned.
        /// </summary>
        private bool owned;

        /// <summary>
        /// group of property on the board.
        /// </summary>
        private string group;

        /// <summary>
        /// Property name.
        /// </summary>
        private string name;

        /// <summary>
        /// Cost of purchasing a house for the property
        /// </summary>
        private int housePrice;

        /// <summary>
        /// rent cost for when a player lands on here with no houses/hotels
        /// </summary>
        private int baseRent;

        /// <summary>
        /// rent cost for when a player lands on here for one house
        /// </summary>
        private int oneHouseRent;

        /// <summary>
        /// rent cost for when a player lands on here for two houses
        /// </summary>
        private int twoHouseRent;

        /// <summary>
        /// rent cost for when a player lands on here for three houses
        /// </summary>
        private int threeHouseRent;

        /// <summary>
        /// rent cost for when a player lands on here for four houses
        /// </summary>
        private int fourHouseRent;

        /// <summary>
        /// rent cost for when a player lands on here for when there is a hotel
        /// </summary>
        private int hotelRent;

        /// <summary>
        /// Number of houses on this property
        /// </summary>
        private int houses;

        /// <summary>
        /// Initializes a new instance of the Property class
        /// </summary>
        /// <param name="name">The name of the property</param>
        /// <param name="price">The price of the property</param>
        /// <param name="housePrice">The price of buying a house</param>
        /// <param name="baseRent">The rent price of the property</param>
        /// <param name="oneHouseRent">The rent price of the property with one house</param>
        /// <param name="twoHouseRent">The rent price of the property with two houses</param>
        /// <param name="threeHouseRent">The rent price of the property with three houses</param>
        /// <param name="fourHouseRent">The rent price of the property with four houses</param>
        /// <param name="hotelRent">The rent price of the property with a hotel</param>
        /// <param name="group">The group the property belongs to</param>
        public Property(string name, int price, int housePrice, int baseRent, int oneHouseRent, int twoHouseRent, int threeHouseRent, int fourHouseRent, int hotelRent, string group)
        {
            this.name = name;
            this.price = price;
            this.housePrice = housePrice;
            this.baseRent = baseRent;
            this.oneHouseRent = oneHouseRent;
            this.twoHouseRent = twoHouseRent;
            this.threeHouseRent = threeHouseRent;
            this.fourHouseRent = fourHouseRent;
            this.hotelRent = hotelRent;
            this.group = group;
            this.owned = false;
            this.mortgage = price / 2;
        }

        /// <summary>
        /// Initializes a new instance of the Property class for railroads or utilities
        /// </summary>
        /// <param name="name">The name of the property</param>
        /// <param name="type">The type of the property</param>
        public Property(string name, string type)
        {
            if (type == "railroad")
            {
                this.name = name;
                this.price = this.railroadPrice;
                this.owned = false;
                this.mortgage = this.railroadPrice / 2;
            }
            else
            {
                this.name = name;
                this.price = this.utilityPrice;
                this.owned = false;
                this.mortgage = this.utilityPrice / 2;
            }            
        }

        /// <summary>
        /// Gets the price of the property.
        /// </summary>
        public int Price
        {
            get { return this.price; }
        }

        /// <summary>
        /// Gets mortgage amount of the property
        /// </summary>
        public int Mortage
        {
            get { return this.mortgage; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the property is owned.
        /// </summary>
        public bool Owned
        {
            get { return this.owned; }
            set { this.owned = value; }
        }

        /// <summary>
        /// Gets the group of the property
        /// </summary>
        public string Group
        {
            get { return this.group; }
        }

        /// <summary>
        /// Gets the properties name.
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets or sets the rent of this property.
        /// </summary>
        public int BaseRent
        {
            get { return this.baseRent; }
            set { this.baseRent += value; }
        }

        /// <summary>
        /// Gets or sets the oneHouseRent of this property.
        /// </summary>
        public int OneHouseRent
        {
            get { return this.oneHouseRent; }
            set { this.oneHouseRent += value; }
        }

        /// <summary>
        /// Gets or sets the twoHouseRent of this property.
        /// </summary>
        public int TwoHouseRent
        {
            get { return this.twoHouseRent; }
            set { this.twoHouseRent += value; }
        }

        /// <summary>
        /// Gets or sets the threeHouseRent of this property.
        /// </summary>
        public int ThreeHouseRent
        {
            get { return this.threeHouseRent; }
            set { this.threeHouseRent += value; }
        }

        /// <summary>
        /// Gets or sets the fourHouseRent of this property.
        /// </summary>
        public int FourHouseRent
        {
            get { return this.fourHouseRent; }
            set { this.fourHouseRent += value; }
        }

        /// <summary>
        /// Gets or sets the hotelRent of this property.
        /// </summary>
        public int HotelRent
        {
            get { return this.hotelRent; }
            set { this.hotelRent += value; }
        }

        /// <summary>
        /// Gets or sets the number of houses on the property
        /// </summary>
        public int Houses
        {
            get { return this.houses; }
            set { this.houses += value; }
        }
    }
}
