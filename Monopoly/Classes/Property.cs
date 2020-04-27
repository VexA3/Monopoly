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
        private double price;

        /// <summary>
        /// Keeps track if the property is owned.
        /// </summary>
        private bool owned;

        /// <summary>
        /// Color of property on the board.
        /// </summary>
        private string color;

        /// <summary>
        /// Property name.
        /// </summary>
        private string propName;

        /// <summary>
        /// Tax fo the property when another player lands on here
        /// </summary>
        private double tax;

        /// <summary>
        /// Number of houses on this property
        /// </summary>
        private int houses;

        /// <summary>
        /// List of properties.
        /// </summary>
        private List<Property> properties;

        /// <summary>
        /// Initializes a new instance of the Property class
        /// </summary>
        /// <param name="PropName">The name of the property</param>
        /// <param name="Price">The price of the property</param>
        public void NewProperty(string PropName, double Price, double Tax, string Color)
        {
            this.propName = PropName;
            this.price = Price;
            this.tax = Tax;
            this.color = Color;
            this.owned = true;
        }
        /// <summary>
        /// Gets or sets the price of the property.
        /// </summary>
        public double Price
        {
            get { return this.price; }
            set { this.price += value; }
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
        /// Gets the Color of the property
        /// </summary>
        public string Color
        {
            get { return this.color; }
        }

        /// <summary>
        /// Gets the properties name.
        /// </summary>
        public string PropName
        {
            get { return this.propName; }
        }

        /// <summary>
        /// Gets or sets the tax of this property.
        /// </summary>
        public double Tax
        {
            get { return this.tax; }
            set { this.tax += value; }
        }

        /// <summary>
        /// Gets or sets the number of houses on the property
        /// </summary>
        public int House
        {
            get { return this.houses; }
            set { this.houses += value; }
        }
    }
}
