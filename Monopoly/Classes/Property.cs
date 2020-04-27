//-----------------------------------------------------------------------
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
        /// Price of railroads
        /// </summary>
        private readonly int railroadPrice = 200;

        /// <summary>
        /// Price of utiliies
        /// </summary>
        private readonly int utilityPrice = 150;

        /// <summary>
        /// Initializes a new instance of the Property class
        /// </summary>
        /// <param name="Name">The name of the property</param>
        /// <param name="Price">The price of the property</param>
        /// <param name="HousePrice">The price of buying a house</param>
        /// <param name="BaseRent">The rent price of the property</param>
        /// <param name="OneHouseRent">The rent price of the property with one house</param>
        /// <param name="TwoHouseRent">The rent price of the property with two houses</param>
        /// <param name="ThreeHouseRent">The rent price of the property with three houses</param>
        /// <param name="FourHouseRent">The rent price of the property with four houses</param>
        /// <param name="HotelRent">The rent price of the property with a hotel</param>
        /// <param name="Group">The group the property belongs to</param>
        public Property(string Name, int Price, int HousePrice, int BaseRent, int OneHouseRent, int TwoHouseRent, int ThreeHouseRent, int FourHouseRent, int HotelRent, string Group)
        {
            this.name = Name;
            this.price = Price;
            this.housePrice = HousePrice;
            this.baseRent = BaseRent;
            this.oneHouseRent = OneHouseRent;
            this.twoHouseRent = TwoHouseRent;
            this.threeHouseRent = ThreeHouseRent;
            this.fourHouseRent = FourHouseRent;
            this.hotelRent = HotelRent;
            this.group = Group;
            this.owned = false;
            this.mortgage = Price / 2;
        }

        /// <summary>
        /// Initializes a new instance of the Property class for railroads or utilities
        /// </summary>
        /// <param name="Name">The name of the property</param>
        /// <param name="Price">The price of the property</param>
        public Property(string Name, string type)
        {
            if(type == "railroad")
            {
                this.name = Name;
                this.price = railroadPrice;
                this.owned = false;
                this.mortgage = railroadPrice / 2;
            }
            else
            {
                this.name = Name;
                this.price = utilityPrice;
                this.owned = false;
                this.mortgage = utilityPrice / 2;
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
        /// Gets or sets
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
