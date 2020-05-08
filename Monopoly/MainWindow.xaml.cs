//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="CompanyName">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace Monopoly
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Effects;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// List of the current players in the game
        /// </summary>
        private static List<Player> currentPlayers = new List<Player>();

        /// <summary>
        /// List of the properties in the game
        /// </summary>
        private static List<Property> allProperties = new List<Property>();

        /// <summary>
        /// List of the chance cards in the game
        /// </summary>
        private List<Card> chanceDeck = new List<Card>();

        /// <summary>
        /// List of the community chest cards in the game
        /// </summary>
        private List<Card> communityChestDeck = new List<Card>();        

        /// <summary>
        /// Enum for the list of current players
        /// </summary>
        private List<Player>.Enumerator currentPlayersEnum = currentPlayers.GetEnumerator();

        /// <summary>
        /// Number of players
        /// </summary>
        private int numPlayers = 0;

        /// <summary>
        /// Current player by numeric indicator for choosing of their piece before game start
        /// </summary>
        private int currentChoice = 1;

        /// <summary>
        /// An array that can hold 2 dice rolls.
        /// </summary>
        private int[] diceResult = new int[2];

        /// <summary>
        /// Used to hold the name of a piece in order to assign it to a player
        /// </summary>
        private string playerPieces = null;

        /// <summary>
        /// The sum of the two dice rolls
        /// </summary>
        private int diceTotal;

        /// <summary>
        /// Number of doubles rolled for the current players turn
        /// </summary>
        private int doublesCount;

        /// <summary>
        /// Initializes a new instance of the MainWindow class
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            // populate starting data for decks and properties
            this.PopulatePropertiesData();
            this.PopulateDeck("Chance");
            this.PopulateDeck("CommunityChest");
        }

        /// <summary>
        /// Gets or sets the current list of players
        /// </summary>
        public static List<Player> CurrentPlayers
        {
            get => currentPlayers;
            set => currentPlayers = value;
        }

        /// <summary>
        /// Send the current player to jail
        /// </summary>
        private void GoToJail()
        {
            Image currentPlayerImageLocation = this.FindCurrentPlayerImage();

            // Set the value of jailTurnCount to 1 for the current player.
            this.currentPlayersEnum.Current.JailTurnCount = 1;

            // remove image from current location
            this.FindCurrentPlayerWrapPanel().Children.Remove(currentPlayerImageLocation);

             // Add to new location
            this.GetWrapPanel("Jail").Children.Add(currentPlayerImageLocation);

            // When a player is sent to jail there turn is over.
            this.UpdateGui("doneMoving");
        }

        /// <summary>
        /// Do the action specified by a chance or community card.
        /// </summary>
        /// <param name="cardDrawn">The card object drawn</param>
        private void Action(Card cardDrawn)
        {
            // - money
            int totalAmount = 0;
            switch (cardDrawn.Action)
            {
                //// Community chest and Chance card methods. Chance and community chest cards will hold these methods 
                //// Each card will be constructed with CommunityChest("Card text here", "actionString") where action string is which case below to run.
                //// When drawing a card we call action(Carddrawn.action) which goes to this function action("") and the string parameter decides which case to run.

                case "jail":
                    this.GoToJail();
                    break;

                case "getOutOfJail":
                    this.currentPlayersEnum.Current.DrawCard(cardDrawn);
                    this.UpdateGui("cardDrawn");
                    break;

                case "nextRailroad":

                    // move piece to next railroad on the board if pass go PassGo()
                    this.MovePiece(this.NextRailroad());
                    break;

                // Move pieces to specified location.
                case "advanceToGo":
                    this.MovePiece("Go");
                    break;

                case "advanceToBoardwalk":
                    this.MovePiece("Boardwalk");
                    break;

                case "advanceToReadingRailroad":
                    this.MovePiece("Reading Railroad");
                    break;

                case "advanceToStCharlesPlace":
                    this.MovePiece("St Charles Place");
                    break;

                case "nextUtility":
                    // move to next utility if pass go passgo()
                    this.MovePiece(this.NextUtility());
                    break;

                case "advanceToIllinoisAvenue":
                    this.MovePiece("Illinois Avenue");
                    break;

                case "backThreeSpaces":
                    // move back three spaces
                    this.MovePiece(this.GetWrapPanel(this.NumbersFromString(this.FindCurrentPlayerWrapPanel().Name) - 3).Name);                    
                    break;

                // + money
                case "gain10":
                    this.Pay(10);
                    break;
                case "gain20":
                    this.Pay(20);
                    break;
                case "gain25":
                    this.Pay(25);
                    break;
                case "gain45":
                    this.Pay(45);
                    break;
                case "gain50":
                    this.Pay(50);
                    break;
                case "gain100":
                    this.Pay(100);
                    break;
                case "gain150":
                    this.Pay(150);
                    break;
                case "gain200":
                    this.Pay(200);
                    break;
                case "gain50PerPlayer":
                    foreach (Player p in currentPlayers)
                    {
                        if (p != this.currentPlayersEnum.Current)
                        {
                            p.Money -= 50;
                            this.Pay(50);
                        }
                    }

                    MessageBox.Show("You recieved $50 from each player");
                    break;
                
                case "lose15":
                    this.Tax(15);
                    break;
                case "lose50":
                    this.Tax(50);
                    break;
                case "lose100":
                    this.Tax(100);
                    break;
                case "lose50PerPlayer":
                    foreach (Player p in currentPlayers)
                    {
                        if (p != this.currentPlayersEnum.Current)
                        {
                            this.Tax(50);
                            p.Money += 50;
                        }
                    }

                    MessageBox.Show("You paid each player $50");
                    break;
                case "lose40PerHouse115PerHotel":
                    totalAmount = 0;
                    foreach (Property p in this.currentPlayersEnum.Current.Properties)
                    {
                        if (p.Houses == 4)
                        {
                            totalAmount += 115;
                        }
                        else
                        {
                            totalAmount += p.Houses * 40;
                        }
                    }

                    this.Tax(totalAmount);
                    MessageBox.Show("You were taxed for $" + totalAmount.ToString());
                    break;

                case "lose25PerHouse100PerHotel":
                    totalAmount = 0;
                    foreach (Property p in this.currentPlayersEnum.Current.Properties)
                    {
                        if (p.Houses == 4)
                        {
                            totalAmount += 100;
                        }
                        else
                        {
                            totalAmount += p.Houses * 25;
                        }
                    }

                    this.Tax(totalAmount);
                    MessageBox.Show("You were taxed for $" + totalAmount.ToString());
                    break;

                // add more cases as needed to do each chance card and community chest. There are 16 of each with a single duplice so we should have 31 cases here in total.
                default:
                    break;
            }

            this.UpdateGui("spentOrReceivedMoney");
        }

        /// <summary>
        /// Move the current players piece to a location that is diceTotal away
        /// </summary>
        private void MovePiece()
        {
            int nextLocation = -1;
            Image currentPlayerImage = this.FindCurrentPlayerImage();

            // Find current location of the image by the name of the wrappanel stripped of chars except numbers converted to int                      
            int currentLocation = this.NumbersFromString(this.FindCurrentPlayerWrapPanel().Name);

            // Add dicetotal to currentLocation
            nextLocation = currentLocation + this.diceTotal;

            // Check if passing go. Change next location by -40 to put them back relative to go(0).
            if (nextLocation >= 41)
            {
                nextLocation = nextLocation - 40;
                this.PassGo();
            }

            // remove image from current location
            this.FindCurrentPlayerWrapPanel().Children.Remove(currentPlayerImage);

            // Add to new location
            this.GetWrapPanel(nextLocation).Children.Add(currentPlayerImage);

            // Call the method related with the location we landed on.
            this.LandOnSpace(this.GetWrapPanel(nextLocation));
        }

        /// <summary>
        /// Move the current players piece to a location that is diceTotal away
        /// </summary>
        /// <param name ="whereTo"> location to move to. </param>
        private void MovePiece(string whereTo)
        {
            Image currentPlayerImage = this.FindCurrentPlayerImage();

            // Get location value of new location and current location
            int nextLocationInt = this.NumbersFromString(this.GetWrapPanel(whereTo).Name);
            int currentLocationInt = this.NumbersFromString(this.FindCurrentPlayerWrapPanel().Name);

            // if the current location value is bigger than the next location then you are going to pass go.
            if (currentLocationInt > nextLocationInt)
            {
                this.PassGo();
            }

            // remove image from current location
            this.FindCurrentPlayerWrapPanel().Children.Remove(currentPlayerImage);

            // Add to new location
            this.GetWrapPanel(whereTo).Children.Add(currentPlayerImage);

            // Call the method related with the location we landed on.
            this.LandOnSpace(this.GetWrapPanel(whereTo));            
        }

        /// <summary>
        /// Finds the wrap panel object of the current players piece
        /// </summary>
        /// <returns>The wrap panel object containing the current player's piece</returns>
        private WrapPanel FindCurrentPlayerWrapPanel()
        {
            //// Find the image for the current player.
            //// We must first look for each wrap panel in gridboard, and then each of those iamges in the wrappanels.
            foreach (WrapPanel wp in GridBoard.Children)
            {
                foreach (Image i in wp.Children)
                {
                    if (i.Name == this.currentPlayersEnum.Current.Piece + "Img")
                    {
                        return wp;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the image object of the current players piece
        /// </summary>
        /// <returns>The image object containing of current player's piece</returns>
        private Image FindCurrentPlayerImage()
        {
            //// Find the image for the current player.
            //// We must first look for each wrap panel in gridboard, and then each of those iamges in the wrappanels.
            foreach (WrapPanel wp in GridBoard.Children)
            {
                foreach (Image i in wp.Children)
                {
                    if (i.Name == this.currentPlayersEnum.Current.Piece + "Img")
                    {
                        return i;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Determines what to do after landing on a location.
        /// </summary>
        /// <param name="locationLandedOn">What location on the board was landed on</param>
        private void LandOnSpace(WrapPanel locationLandedOn)
        {
            string location = this.FormatString(locationLandedOn.Name);
            switch (location)
            {
                // Cards
                case "Community Chest":
                    this.DrawCard("Community Chest");
                    break;
                case "Chance":
                    this.DrawCard("Chance");
                    break;

                // Jail
                case "Go To Jail":
                    this.GoToJail();
                    break;

                // Taxes
                case "Income Tax":
                    this.Tax(200);
                    break;

                case "Luxury Tax":
                    this.Tax(100);
                    break;

                // "Do Nothings"
                case "Just Visiting":
                    break;
                case "Free Parking":
                    break;
                case "Jail":
                    break;

                // we already check if you land or pass go when moving the piece. 
                case "Go":
                    break;

                // If you landed on a space other than the previous ones then you must be landing on a property. So LandOnProperty deals with buying that property or paying the player who owns it their correct amount.
                default:
                    this.LandOnProperty(location);
                    break;
            }
        }

        /// <summary>
        /// Formats string to have no numbers and change underscore to spaces. cull wrap panel from Beginning of string as well.
        /// </summary>
        /// <param name="text">Text to be formatted</param>\
        /// <returns>A string that is formatted</returns>
        private string FormatString(string text)
        {
            string formattedText;
            if (text.Contains("WrapPanel"))
            {
                formattedText = Regex.Replace(text.Remove(0, 9), @"[\d]", string.Empty);
            }
            else
            {
                formattedText = Regex.Replace(text, @"[\d]", string.Empty);
            }

            formattedText = Regex.Replace(formattedText, @"[_]", " ");
            return formattedText;
        }

        /// <summary>
        /// Finds an integer value in a string
        /// </summary>
        /// <param name="text">Text searched for an integer</param>\
        /// <returns>An integer of the numbers located in the string</returns>
        private int NumbersFromString(string text)
        {
            int foundNumber = Convert.ToInt32(Regex.Replace(text, "[^0-9]", string.Empty));
            return foundNumber;
        }        

        /// <summary>
        /// Draws the specified card by displaying the card text and calling Action(card.Action)
        /// </summary>
        /// <param name="cardType">Which card type to draw</param>
        private void DrawCard(string cardType)
        {
            // No need to shuffle decks if you choose the card at random.
            Random shuffle = new Random();
            int randomCard;
            if (cardType == "Chance")
            {
                // Make sure deck isn't empty first
                if (this.chanceDeck.Count == 0)
                {
                    this.PopulateDeck("Chance");
                }

                // Choose card at random, call action passing through the card object. Display the text from the card.
                randomCard = shuffle.Next(0, this.chanceDeck.Count - 1);
                MessageBox.Show(this.chanceDeck[randomCard].CardText);
                this.Action(this.chanceDeck[randomCard]);
                this.chanceDeck.RemoveAt(randomCard);
            }
            else
            {
                // Make sure deck isn't empty first
                if (this.communityChestDeck.Count == 0)
                {
                    this.PopulateDeck("CommunityChest");
                }

                // Choose card at random, call action passing through the card object. Display the text from the card.
                randomCard = shuffle.Next(0, this.communityChestDeck.Count - 1);
                MessageBox.Show(this.communityChestDeck[randomCard].CardText);
                this.Action(this.communityChestDeck[randomCard]);
                this.communityChestDeck.RemoveAt(randomCard);
            }
        }

        /// <summary>
        /// Subtract the amount of tax from current players money
        /// </summary>
        /// <param name="taxAmount">Amount of money to subtract</param>
        private void Tax(int taxAmount)
        {
            this.currentPlayersEnum.Current.Money = -taxAmount;
            this.UpdateGui("spentOrReceivedMoney");
        }

        /// <summary>
        /// Adds the amount of pay to current players money
        /// </summary>
        /// <param name="payAmount">Amount of money to subtract</param>
        private void Pay(int payAmount)
        {
            this.currentPlayersEnum.Current.Money = payAmount;
            this.UpdateGui("spentOrReceivedMoney");
        }

        /// <summary>
        /// Change menu to allow players to choose pieces or restart the game
        /// </summary>
        /// <param name="button"> The button that was pressed </param>
        private void StartGame(Button button)
        {
            // set the Icon select buttons to visible
            this.UpdateGui("choosePieces");
        }

        /// <summary>
        /// After player pieces are chosen, initialize the enum and put player pieces on the board. Start first players turn. Add cards to their respect decks and create default list of property.
        /// </summary>
        private void StartGame()
        {
            // Update enum for player order
            this.UpdateEnum();
            this.UpdateGui("piecesChosen");
            this.UpdateGui("startOfTurn");            
        }

        /// <summary>
        /// Reset any variables and data to their starting values. Change menu to original menu.
        /// </summary>
        /// <param name="button"> The button that was pressed </param>
        private void RestartGame(Button button)
        {
            // Reset any variables to starting amounts.
            this.playerPieces = "null";
            this.diceResult[0] = 0;
            this.diceResult[1] = 0;
            this.UpdateGui("freshWindow");
            this.numPlayers = 0;
            this.currentChoice = 1;
            currentPlayers.Clear();
            this.UpdateEnum();
        }

        /// <summary>
        /// Handles start/restart button presses. Checks for player number choice and then starts or restarts depending on if the game has already started or not
        /// </summary>
        /// <param name="sender">The object that initiated the event.</param>
        /// <param name="e">The event arguments for the event.</param>
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            // Check if numplayers has been set
            if (this.numPlayers != 0)
            {
                // Check if Starting or restarting the game
                if (TextBlockStartRestart.Text == "Start")
                {
                    this.StartGame(button);
                }
                else
                {
                    this.RestartGame(button);
                }
            }
            else
            {
                MessageBox.Show("Please choose number of players");
            }
        }

        /// <summary>
        /// Button to handle quit button presses
        /// </summary>
        /// <param name="sender">The object that initiated the event.</param>
        /// <param name="e">The event arguments for the event.</param>
        private void BtnQuit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles info button presses
        /// </summary>
        /// <param name="sender">The object that initiated the event.</param>
        /// <param name="e">The event arguments for the event.</param>
        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Developers: Johnathan DeLeeuw, Brian Herman, Kyler Lambert, Billy Rintamaki \n");
        }

        /// <summary>
        /// Handles rules button presses
        /// </summary>
        /// <param name="sender">The object that initiated the event.</param>
        /// <param name="e">The event arguments for the event.</param>
        private void BtnRules_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("THE PLAY...Starting with the Banker, each player in turn throws thedice. The player with the highest total starts the play: \n Place your tokenon the corner marked “GO,”  throw the dice and move your token inthe direction of the arrow the number of spaces \nindicated by the dice.After you have completed your play, the turn passes to the left.Thetokens remain on the spaces occupied and \n proceed from that point onthe player’s next turn.Two or more tokens may rest on the same spaceat the same time.According to the\n space your token reaches, you may be entitled tobuy real estate or other properties — or obliged to pay rent, pay taxes, draw a \n  Chance or Community Chest card, “Go to Jail®,” etc.If you throw doubles, you move your token as usual, the sum of thetwo dice, \n and are subject to any privileges or penalties pertaining tothe space on which you land.Retaining the dice, throw again andmove \n your token as before.If you throw doubles three times insuccession, move your token immediately to the space marked “InJail”(see JAIL).\n “GO”...Each time a player’s token lands on or passes over GO, whether by throwing the dice or drawing a card, the Banker \n  payshim / her a $200 salary.The $200 is paid only once each time around the board. However, ifa player passing GO on the throw \n   of the dice lands 2 spaces beyond iton Community Chest, or 7 spaces beyond it on Chance, and draws the“Advance to GO”  card, \n he / she collects $200 for passing GO the firsttime and another $200 for reaching it the second time by instructionson the card.\n   BUYING PROPERTY...Whenever you land on an unowned propertyyou may buy that property from the Bank at its printed price. \n     You receive the Title Deed card showing ownership; place it face up infront of you.If you do not wish to buy the property, \n   the Banker sells it at auctionto the highest bidder. The buyer pays the Bank the amount of the bidin cash and receives the \n   Title Deed card for that property. Any player, including the one who declined the option to buy it at the printedprice, may \n     bid.Bidding may start at any price.PAYING RENT...When you land on property owned by anotherplayer, the owner collects rent \n     from you in accordance with the listprinted on its Title Deed card.If the property is mortgaged, no rent can be collected.\n      When aproperty is mortgaged, its Title Deed card is placed face down in frontof the owner.It is an advantage to hold all \n      the Title Deed cards in a color - group(e.g., Boardwalk and Park Place; or Connecticut, Vermont and OrientalAvenues) because \n      the owner may then charge double rent forunimproved properties in that color-group.This rule applies tounmortgaged properties \n     even if another property in that color-groupis mortgaged.It is even more advantageous to have houses or hotels on properties \n     because rents are much higher than for unimproved properties.The owner may not collect the rent if he / she fails to ask for it beforethe second player following throws the dice.“CHANCE” \n  AND “COMMUNITY CHEST”...When you land oneither of these spaces, take the top card from the deck indicated, follow the instructions \n  and return the card face down to the bottom ofthe deck.The “Get Out of Jail Free”  card is held until used and then returned to \n  the bottom of the deck. If the player who draws it does not wish touse it, he / she may sell it, at any time, to another player \n    at a priceagreeable to both.“INCOME TAX”...If you land here you have two options: You mayestimate your tax at $200 and pay \n    the Bank, or you may pay 10 % ofyour total worth to the Bank.Your total worth is all your cash onhand, printed prices of \n    mortgaged and unmortgaged properties andcost price of all buildings you own.You must decide which option you will take before you add upyour total worth.“JAIL”...You land in Jail when...(1) your token lands on the spacemarked “Go to Jail”; (2) you draw a card marked “Go to Jail”; or(3) you throw doubles three times in succession.When you are sent to Jail you cannot collect your $200 salary in thatmove since, regardless of where your token is on the board, you mustmove it directly into Jail. Yours turn ends when you are sent to Jail.If you are not “sent”  to Jail but in the ordinary course of play landon that space, you are “Just Visiting,”  you incur no penalty, and youmove ahead in the usual manner on your next turn.You get out of Jail by...(1) throwing doubles on any of your nextthree turns; if you succeed in doing this you immediately moveforward the number of spaces shown by your doubles throw; eventhough you had thrown doubles, you do not take another turn; (2) using the “Get Out of Jail Free”  card if you have it; (3) purchasingthe “Get Out of Jail Free”  card from another player and playing it; (4) paying a fine of $50 before you roll the dice on either of your nexttwo turns.If you do not throw doubles by your third turn, you must pay the$50 fine.You then get out of Jail and immediately move forward thenumber of spaces shown by your throw.Even though you are in Jail, you may buy and sell property, buyand sell houses and hotels and collect rents.“FREE PARKING”...A player landing on this place does not receiveany money, property or reward of any kind. This is just a “free”resting place.HOUSES...When you own all the properties in a color-group youmay buy houses from the Bank and erect them on those properties.If you buy one house, you may put it on any one of thoseproperties.The next house you buy must be erected on one of theunimproved properties of this or any other complete color - group youmay own.The price you must pay the Bank for each house is shown on yourTitle Deed card for the property on which you erect the house.      The owner still collects double rent from an opponent who lands onthe unimproved properties of his / her complete color - group. \n      Following the above rules, you may buy and erect at any time asmany houses as your judgement and financial standing will allow. \n      Butyou must build evenly, i.e., you cannot erect more than one house onany one property of any color - group until you have \n      built one house onevery property of that group.You may then begin on the second rowof houses, and so on, up to a limit \n      of four houses to a property.Forexample, you cannot build three houses on one property if you haveonly one house on \n     another property of that group.As you build evenly, you must also break down evenly if you sellhouses back to the Bank \n     (see SELLING PROPERTY).HOTELS...When a player has four houses on each property of acomplete color - group, he / she may buy a \n         hotel from the Bank anderect it on any property of the color - group.He / she returns the fourhouses from that property to \n            the Bank and pays the price for the hotelas shown on the Title Deed card.Only one hotel may be erected onany one property.\n            BUILDING SHORTAGES...When the Bank has no houses to sell, players wishing to build must wait for some player to return or sell \nhis / her houses to the Bank before building. If there are a limitednumber of houses and hotels available and two or more \n  players wishto buy more than the Bank has, the houses or hotels must be sold atauction to the highest bidder.\n\nSELLING PROPERTY...Unimproved properties, railroads andutilities(but not buildings) may be sold to any player as a \nprivate transaction for any amount the owner can get; however, no propertycan be sold to another player if buildings \nare standing on anyproperties of that color-group.Any buildings so located must be soldback to the Bank before the \nowner can sell any property of that color-group.Houses and hotels may be sold back to the Bank at any time \nfor one-half the price paid for them.All houses on one color-group must be sold one by one, evenly, \nin reverse of the manner in which they were erected.All hotels on one color-group may be sold at once, or \nthey may besold one house at a time (one hotel equals five houses), evenly, inreverse of the manner in which \nthey were erected.MORTGAGES...Unimproved properties can be mortgaged throughthe Bank at any time.Before an \nimproved property can be mortgaged, all the buildings on all the properties of its color-group must be \nsold back to the Bank at half price.The mortgage value is printed on eachTitle Deed card.No rent can be \ncollected on mortgaged properties or utilities, butrent can be collected on unmortgaged properties in the same \ngroup.In order to lift the mortgage, the owner must pay the Bank theamount of the mortgage plus 10% interest. \nWhen all the properties of acolor-group are no longer mortgaged, the owner may begin to buyback houses at full \nprice.The player who mortgages property retains possession of it and noother player may secure it by lifting \nthe mortgage from the Bank. However, the owner may sell this mortgaged property to anotherplayer at any agreed\nprice. If you are the new owner, you may lift themortgage at once if you wish by paying off the mortgage \nplus 10%interest to the Bank.If the mortgage is not lifted at once, you must paythe Bank 10% interest \nwhen you buy the property and if you lift themortgage later you must pay the Bank an additional 10% interest \nas well as the amount of the mortgage.BANKRUPTCY...You are declared bankrupt if you owe more thanyou can pay either \nto another player or to the Bank. If your debt is toanother player, you must turn over to that player all that \nyou have ofvalue and retire from the game.In making this settlement, if you ownhouses or hotels, you must return \nthese to the Bank in exchange formoney to the extent of one-half the amount paid for them; this cash isgiven to \nthe creditor.If you have mortgaged property you also turnthis property over to your creditor but the new owner \nmust at oncepay the Bank the amount of interest on the loan, which is 10% of thevalue of the property. \nThe new owner who does this may then, athis/her option, pay the principal or hold the property until some \nlater turn, then lift the mortgage. If he/she holds property in this way untila later turn, he/she must pay \nthe interest again upon lifting themortgage.Should you owe the Bank, instead of another player, more than you \ncan pay (because of taxes or penalties) even by selling off buildingsand mortgaging property, you must turn over \nall assets to the Bank. Inthis case, the Bank immediately sells by auction all property so taken,except buildings. \nA bankrupt player must immediately retire from thegame. The last player left in the game wins.\n\nMISCELLANEOUS...Money can be loaned to a player only by theBank and then only by mortgaging property. \nNo player may borrowfrom or lend money to another player");
        }

        /// <summary>
        /// Handles radio button presses
        /// </summary>
        /// <param name="sender">The object that initiated the event.</param>
        /// <param name="e">The event arguments for the event.</param>
        private void RadioBtnPlayer_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            this.numPlayers = Convert.ToInt32(button.Tag.ToString());
        }

        /// <summary>
        /// Handles press of confirm piece button. Add the player piece selected to the current player and disallow others from selecting that same piece.
        /// </summary>
        /// <param name="sender">The object that initiated the event.</param>
        /// <param name="e">The event arguments for the event.</param>
        private void BtnConfirmPlayerPiece_Click(object sender, RoutedEventArgs e)
        {
            // Checks if the user picked a piece.
            if (this.playerPieces != null)
            {
                // Make a new player class with the number of that player and their chosen piece. Add them to the list of players.
                currentPlayers.Add(new Player(this.currentChoice, this.playerPieces));

                // Start the game if all players have chosen a piece.
                // else if more players have pieces to choose increment the current players turn for choosing a piece.
                if (this.currentChoice == this.numPlayers)
                {
                    this.StartGame();
                }
                else
                {
                    this.currentChoice++;
                    this.UpdateGui("chooseNextPiece");
                    this.playerPieces = null;
                }
            }
            else
            {
                MessageBox.Show("Please pick a piece.");
            }
        }

        /// <summary>
        /// Finds the wrap panel at a given name on the gridBoard
        /// </summary>
        /// <param name="wrapPanel">The name of the wrap panel to find</param>
        /// <returns> The requested wrap panel object</returns>
        private WrapPanel GetWrapPanel(string wrapPanel)
        {
            foreach (WrapPanel wp in GridBoard.Children)
            {
                string wrapPanelTrimmedName = this.FormatString(wp.Name);
                if (this.FormatString(wrapPanel) == wrapPanelTrimmedName)
                {
                    return wp;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the wrap panel at a board distance on the gridBoard
        /// </summary>
        /// <param name="distanceFromGo">The WrapPanel location on the board.</param>
        /// <returns> The requested wrap panel object</returns>
        private WrapPanel GetWrapPanel(int distanceFromGo)
        {
            if (distanceFromGo < 0)
            {
                distanceFromGo = distanceFromGo + 40;
            }

            foreach (WrapPanel wp in GridBoard.Children)
            {
                // We use a temporary variable to check if wp is the right distance from go/ the wrap panel we are looking for.
                int potentialDistanceFromGo = this.NumbersFromString(wp.Name);
                if (potentialDistanceFromGo == distanceFromGo && wp.Name != "WrapPanelJail11")
                {
                    return wp;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the Property Object with the given name
        /// </summary>
        /// <param name="propertyName">The name of the Property to find</param>
        /// <returns> The requested wrap panel object</returns>
        private Property GetProperty(string propertyName)
        {
            foreach (Property property in allProperties)
            {
                if (property.Name == propertyName)
                {
                    return property;
                }
            }

            return null;
        }

        /// <summary>
        /// initialize the currentPlayersEnum for use.
        /// </summary>
        private void UpdateEnum()
        {
            // Update enum so it can be used after players were added to list.
            this.currentPlayersEnum = currentPlayers.GetEnumerator();
            this.currentPlayersEnum.MoveNext();
        }

        /// <summary>
        /// Handles presses of the pieces buttons. Show they are selected/deselected.
        /// </summary>
        /// <param name="sender">The object that initiated the event.</param>
        /// <param name="e">The event arguments for the event.</param>
        private void PieceImgMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Use the last Image as sender.
            Image img = sender as Image;

            // Show the user what image they clicked
            if (img.Opacity == 1)
            {
                // set all other images opacity back to 1
                this.Opacity1();
                img.Opacity = 0.5;

                // Uses the selected image's tag for the playerPiece variable.
                this.playerPieces = img.Tag.ToString();
            }
            else if (img.Opacity == 0.5)
            {
                // show that the piece was deselected
                this.playerPieces = null;
                img.Opacity = 1;
            }
        }

        /// <summary>
        /// Handles presses of the dice button. Call DiceRoll() and keep track of doubles counts
        /// </summary>
        /// <param name="sender">The object that initiated the event.</param>
        /// <param name="e">The event arguments for the event.</param>
        private void DiceImgMouseDown(object sender, MouseButtonEventArgs e)
        {
            // use DiceRoll to generate 2 die rolls.
            this.DiceRoll();
            bool goneToJail = false;

            // Check if the player is in jail and if they are not on their third and final turn in jail.
            if (this.currentPlayersEnum.Current.JailTurnCount != 0 && this.currentPlayersEnum.Current.JailTurnCount != 3)
            {
                // If they rolled doubles, leave jail.
                if (this.diceResult[0] == this.diceResult[1])
                {
                    // if you roll doubles you may leave jail, so we set jailTurnCount to 0
                    this.currentPlayersEnum.Current.JailTurnCount = 0;

                    // You don't keep moving with a doubles from jail.
                    this.UpdateGui("leftJail");
                    this.UpdateGui("doneMoving");
                    this.MovePiece();
                }                
                else
                {
                    // Increment the count for number of turns they have spent in jail and end their movement.
                    this.currentPlayersEnum.Current.JailTurnCount++;
                    this.UpdateGui("doneMoving");
                }
            }
            else if (this.currentPlayersEnum.Current.JailTurnCount == 3)
            {
                // If you are on your 3rd turn in jail reset counter to 0 as you have to leave it this turn.
                this.currentPlayersEnum.Current.JailTurnCount = 0;

                // if it is their 3rd turn in jail they need to pay the $50 fine. they continue moving as normal but can't go again due to doubles.
                this.Tax(50);
                this.UpdateGui("doneMoving");
                this.MovePiece();
            }
            else
            {
                // Check if the user rolled doubles and if so if it is 3 times in a row.
                if (this.diceResult[0] == this.diceResult[1])
                {
                    if (this.doublesCount == 2)
                    {
                        // If they hit 3 doubles in a row send them to jail.
                        MessageBox.Show("Go to jail.");
                        this.GoToJail();
                        goneToJail = true;

                        // You can't continue moving from jail so hide dice, and show end turn button.
                        this.UpdateGui("doneMoving");
                    }
                    else
                    {
                        MessageBox.Show("You Rolled Doubles, Roll Again.");
                        this.doublesCount++;
                    }
                }
                else
                {
                    // If they haven't rolled doubles then their turn is over. Hide dice and show end turn button.
                    this.UpdateGui("doneMoving");
                }

                // if you go to jail their piece is already moved to jail so do not move them the dicetotal as well.
                if (!goneToJail)
                {
                    this.MovePiece();
                }
            }            
        }

        /// <summary>
        /// Handles what happens when landing on a property. Either buy, auction, pay rent
        /// </summary>
        /// <param name="nameOfProperty"> The name of the property landed on</param>
        private void LandOnProperty(string nameOfProperty)
        {            
            bool owned = false;
            Player owner;
            Property property = this.GetProperty(nameOfProperty);

            // Check if property is owned.
            foreach (Player p in currentPlayers)
            {
                if (p.Properties.Contains(property))
                {
                    owned = true;
                    owner = p;
                }
            }

            if (!owned)
            {
                // Configure the message box to be displayed
                string messageBoxText = "Do you want to buy " + nameOfProperty + " for the price of $" + property.Price.ToString() + "?\nIf you do not buy this property, it will be put up for auction.";
                string caption = "Buy Phase";
                MessageBoxButton button = MessageBoxButton.YesNo;

                // Display message box
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button);

                // Process message box results
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        this.PurchaseProperty(property);
                        break;

                    case MessageBoxResult.No:
                        // TODO Auction
                        break;
                }
            }
            else
            {
                this.Tax(property.GetRentAmount(this.diceTotal));
                MessageBox.Show("You paid $" + property.GetRentAmount(this.diceTotal).ToString() + " in rent.");
            }            
        }

        /// <summary>
        /// If Yes was Pressed, Run this method that adds the property to there owned properties list.
        /// </summary>
        /// <param name="property">The property to be purchased</param>
        private void PurchaseProperty(Property property)
        {
            this.currentPlayersEnum.Current.BuyProperty(property, this.currentPlayersEnum.Current);
            this.UpdateGui("boughtProperty");
        }

        /// <summary>
        /// Based on the sent in instruction we will modify the GUI to show relevant controls to the current state requested.
        /// </summary>
        /// <param name="currentPartOfTurn">The step in turn the game is at.</param>
        private void UpdateGui(string currentPartOfTurn)
        {
            switch (currentPartOfTurn)
            {
                case "leftJail":
                    btnPayJailFee.Visibility = Visibility.Hidden;
                    lblDisplayTurnOrChoice.Content = "Roll Your Dice";
                    break;
                case "startOfTurn":
                    // Get rid of previous players jail button
                    this.UpdateGui("leftJail");

                    // Get rid of previous buy/sell house buttons
                    this.UpdateGui("propertyUnselected");

                    // remove get out of jail cards from previous players
                    imgGetOutOfJailChance.Visibility = Visibility.Hidden;
                    imgGetOutOfJailCommunityChest.Visibility = Visibility.Hidden;

                    // remove previous dice labels.
                    foreach (Viewbox vb in GridControls.Children.OfType<Viewbox>())
                    {
                        if (vb.Child is Label)
                        {
                            Label l = vb.Child as Label;
                            if (l.Tag != null && l.Tag.ToString() == "DiceLabel")
                            {
                                l.Visibility = Visibility.Hidden;
                            }
                        }
                    }

                    // Show buttons needed for a typical turn.
                    // Set Control buttons to visible. Dice, Trade, etc...
                    lblDisplayTurnOrChoice.Content = "Roll Your Dice";
                    lblCurrentPlayer.Visibility = Visibility.Visible;
                    lblDisplayTurnOrChoice.Visibility = Visibility.Visible;
                    imgCurrentPlayer.Visibility = Visibility.Visible;
                    imgDice.Visibility = Visibility.Visible;

                    // Check if the player is in jail. If so show the button for paying the fee.
                    if (this.currentPlayersEnum.Current.JailTurnCount > 0)
                    {
                        lblDisplayTurnOrChoice.Content = "Either pay the fee to leave jail or attempt to roll doubles.";
                        btnPayJailFee.Visibility = Visibility.Visible;
                    }

                    if (this.currentPlayersEnum.Current.JailTurnCount == 3)
                    {
                        btnPayJailFee.Visibility = Visibility.Hidden;
                        lblDisplayTurnOrChoice.Content = "Roll your dice, You will automatically pay the $50 Jail Fee.";
                    }

                    
                    // trade with other player button.
                    // Hide end turn button
                    btnEndTurn.Visibility = Visibility.Hidden;

                    // Display player's turn by changing imgCurrentPlayer
                    this.ChangeImage(this.currentPlayersEnum.Current.Piece, this.imgCurrentPlayer);
                    
                    // Fill in data
                    lblBalance.Visibility = Visibility.Visible;
                    lblMoney.Visibility = Visibility.Visible;
                    lblMoney.Content = this.currentPlayersEnum.Current.Money.ToString();

                    // ListBox of owned properties.
                    if (this.currentPlayersEnum.Current.Properties.Count != 0)
                    {
                        ListBoxPropertiesOwned.Visibility = Visibility.Visible;
                        ListBoxPropertiesOwned.ItemsSource = from p in this.currentPlayersEnum.Current.Properties
                                                             orderby p.Group.ToString()
                                                             select p;
                        lblPropertiesYouOwn.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        ListBoxPropertiesOwned.Visibility = Visibility.Hidden;
                        lblPropertiesYouOwn.Visibility = Visibility.Hidden;
                    }

                    if (this.currentPlayersEnum.Current.GetOutOfJailCards.Count != 0)
                    {
                        foreach (Card c in this.currentPlayersEnum.Current.GetOutOfJailCards)
                        {
                            if (c.CardText == "This card may be kept until needed or sold.  Get out of jail free.")
                            {
                                imgGetOutOfJailChance.Visibility = Visibility.Visible;
                            }
                            else if (c.CardText == "Get out of Jail, free.  This card may be kept until needed or sold.")
                            {
                                imgGetOutOfJailCommunityChest.Visibility = Visibility.Visible;
                            }
                        }
                    }
                    
                    break;
                case "doneMoving":
                    // Hide dice and show end turn button
                    imgDice.Visibility = Visibility.Hidden;
                    btnEndTurn.Visibility = Visibility.Visible;
                    break;
                case "rolledDice":
                    // Make dice labels visible
                    foreach (Viewbox vb in GridControls.Children.OfType<Viewbox>())
                    {
                        if (vb.Child is Label)
                        {
                            Label l = vb.Child as Label;
                            if (l.Tag != null && l.Tag.ToString() == "DiceLabel")
                            {
                                l.Visibility = Visibility.Visible;
                            }
                        }
                    }

                    lblDisplayTurnOrChoice.Content = "Trade with other players, or End your turn.";
                    break;

                case "freshWindow":
                    // Change text to Start
                    TextBlockStartRestart.Text = "Start";

                    // Delete all player image pieces on the board.
                    foreach (Player p in currentPlayers)
                    {
                        this.RemovePiece(p);
                    }

                    this.ChangeImage("default", this.imgCurrentPlayer);

                    // Hide labels and buttons that are not needed when at menu.
                    imgCurrentPlayer.Visibility = Visibility.Hidden;
                    btnEndTurn.Visibility = Visibility.Hidden;
                    btnConfirmPlayerPiece.Visibility = Visibility.Hidden;
                    lblCurrentPlayer.Visibility = Visibility.Hidden;
                    lblBalance.Visibility = Visibility.Hidden;
                    lblMoney.Visibility = Visibility.Hidden;
                    imgDice.Visibility = Visibility.Hidden;
                    lblDiceTotal.Visibility = Visibility.Hidden;
                    lblDie1.Visibility = Visibility.Hidden;
                    lblDie2.Visibility = Visibility.Hidden;
                    lblDieOne.Visibility = Visibility.Hidden;
                    lblDieTwo.Visibility = Visibility.Hidden;
                    lblTotal.Visibility = Visibility.Hidden;
                    lblDisplayTurnOrChoice.Visibility = Visibility.Hidden;
                    btnPayJailFee.Visibility = Visibility.Hidden;
                    ListBoxPropertiesOwned.Visibility = Visibility.Hidden;
                    lblPropertiesYouOwn.Visibility = Visibility.Hidden;
                    imgGetOutOfJailChance.Visibility = Visibility.Hidden;
                    imgGetOutOfJailCommunityChest.Visibility = Visibility.Hidden;

                    // Foreach loop that makes every piece choice image hidden.
                    foreach (Viewbox vb in GridControls.Children.OfType<Viewbox>())
                    {
                        if (vb.Child is Image)
                        {
                            Image i = vb.Child as Image;
                            if (i.Tag != null && i.Tag.ToString() != "CurrentPlayer")
                            {
                                i.Visibility = Visibility.Hidden;
                            }
                        }
                    }

                    // Show buttons needed at main menu/restarted game
                    // Set number of players radiobuttons to unchecked and visibile again            
                    foreach (RadioButton b in StkRadioButtons.Children.OfType<RadioButton>())
                    {
                        b.IsChecked = false;
                        b.Visibility = Visibility.Visible;
                    }

                    // set opacity back to normal for piece images.
                    this.Opacity1();
                    break;

                case "choosePieces":
                    // hide radio buttons
                    foreach (RadioButton b in StkRadioButtons.Children.OfType<RadioButton>())
                    {
                        b.Visibility = Visibility.Hidden;
                    }

                    // change text to restart
                    TextBlockStartRestart.Text = "Restart";                    

                    // Display buttons for choosing pieces.
                    // Foreach loop that makes every Choose piece image Visible.
                    foreach (Viewbox vb in GridControls.Children.OfType<Viewbox>())
                    {
                        if (vb.Child is Image)
                        {
                            Image i = vb.Child as Image;
                            if (i.Tag != null && i.Tag.ToString() != "CurrentPlayer" && i.Tag.ToString() != "Dice")
                            {
                                i.Visibility = Visibility.Visible;
                            }
                        }
                    }

                    // Make confirm button visible
                    btnConfirmPlayerPiece.Visibility = Visibility.Visible;

                    // Change display of label to indicate to player who's turn it is to choose a piece.
                    lblDisplayTurnOrChoice.Content = "Player " + this.currentChoice.ToString() + " please choose a piece.";
                    break;

                case "chooseNextPiece":
                    // make chosen piece hidden.                
                    foreach (Viewbox vb in GridControls.Children.OfType<Viewbox>())
                    {
                        if (vb.Child is Image)
                        {
                            Image i = vb.Child as Image;
                            if (i.Tag != null && i.Tag.ToString() == this.playerPieces)
                            {
                                i.Visibility = Visibility.Hidden;
                            }
                        }
                    }

                    // Change display of label to indicate to player who's turn it is to choose a piece.
                    lblDisplayTurnOrChoice.Content = "Player " + this.currentChoice.ToString() + " please choose a piece.";
                    break;

                case "piecesChosen":
                    // Place player images on the board on go / 10,10
                    foreach (Player p in currentPlayers)
                    {
                        Image img = new Image();
                        img.Source = new BitmapImage(new Uri(@"/Images/" + p.Piece + ".png", UriKind.Relative));
                        img.Effect = new DropShadowEffect
                        {
                            Color = new Color { A = 255, R = 0, G = 0, B = 0 },
                            Direction = 270,
                            ShadowDepth = 5,
                            Opacity = 1,
                            BlurRadius = 15
                        }; 
                        this.GetWrapPanel("Go").Children.Add(img);
                        img.Name = p.Piece + "Img";
                    }

                    // Foreach loop that makes every piece choice image hidden.
                    foreach (Viewbox vb in GridControls.Children.OfType<Viewbox>())
                    {
                        if (vb.Child is Image)
                        {
                            Image i = vb.Child as Image;
                            if (i.Tag != null && i.Tag.ToString() != "CurrentPlayer")
                            {
                                i.Visibility = Visibility.Hidden;
                            }
                        }
                    }

                    // hide confirm piece button.
                    btnConfirmPlayerPiece.Visibility = Visibility.Hidden;
                    break;

                case "boughtProperty":                    
                    ListBoxPropertiesOwned.Visibility = Visibility.Visible;
                    ListBoxPropertiesOwned.ItemsSource = null;
                    ListBoxPropertiesOwned.Items.Clear();
                    ListBoxPropertiesOwned.ItemsSource = from p in this.currentPlayersEnum.Current.Properties
                                                         orderby p.Group.ToString()
                                                         select p;
                    lblPropertiesYouOwn.Visibility = Visibility.Visible;
                    this.UpdateGui("spentOrReceivedMoney");
                    this.UpdateGui("drawHouses");
                    break;

                case "spentOrReceivedMoney":
                    lblMoney.Content = this.currentPlayersEnum.Current.Money.ToString();
                    break;

                case "cardDrawn":
                    // First clear any ones already there.
                    imgGetOutOfJailChance.Visibility = Visibility.Hidden;
                    imgGetOutOfJailCommunityChest.Visibility = Visibility.Hidden;

                    foreach (Card c in this.currentPlayersEnum.Current.GetOutOfJailCards)
                    {
                        if (c.CardText == "Get out of Jail, free.  This card may be kept until needed or sold.")
                        {
                            imgGetOutOfJailCommunityChest.Visibility = Visibility.Visible;
                        }
                        else if (c.CardText == "This card may be kept until needed or sold.  Get out of jail free.")
                        {
                            imgGetOutOfJailChance.Visibility = Visibility.Visible;
                        }
                    }                      
                    
                    break;
                case "propertySelected":
                    btnPurchaseHouse.Visibility = Visibility.Visible;
                    btnSellHouseOrMortgage.Visibility = Visibility.Visible;
                    Property selectedProperty = (ListBoxPropertiesOwned.SelectedItem as Property);
                    if(selectedProperty.Name.Contains("Railroad") || selectedProperty.Name.Contains("Utility"))
                    {
                        btnPurchaseHouse.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        btnPurchaseHouse.Content = "Purchase a house for $" + selectedProperty.HousePrice;
                    }                    
                    break;

                case "propertyUnselected":
                    btnPurchaseHouse.Visibility = Visibility.Hidden;
                    btnSellHouseOrMortgage.Visibility = Visibility.Hidden;
                    break;
                case "clearHouses":
                    foreach (Property p in allProperties)
                    {
                        if (GetWrapPanel(p.Name) != null)
                        {
                            List<Image> imagesToRemove = new List<Image>();
                            foreach (Image i in GetWrapPanel(p.Name).Children)
                            {
                                if (!i.Name.Contains("Piece"))
                                {
                                    imagesToRemove.Add(i);
                                }
                            }
                            foreach (Image i in imagesToRemove)
                            {
                                GetWrapPanel(p.Name).Children.Remove(i);
                            }
                        }
                    }
                    break;
                case "drawHouses":
                    // First clear any previous images.                    
                    UpdateGui("clearHouses");

                    foreach (Player p in currentPlayers)
                    {
                        foreach(Property prop in p.Properties)
                        {
                            if (0 < prop.Houses && prop.Houses < 5)
                            {
                                for(int i = 0; i <prop.Houses; i++)
                                {
                                    Image newImage = new Image();
                                    newImage.Source = new BitmapImage(new Uri(@"/Images/house.png", UriKind.Relative));
                                    GetWrapPanel(prop.Name).Children.Add(newImage);
                                }                                
                            }
                            else if(prop.Houses == 5)
                            {
                                Image newImage = new Image();
                                newImage.Source = new BitmapImage(new Uri(@"/Images/Hotel.png", UriKind.Relative));
                                GetWrapPanel(prop.Name).Children.Add(newImage);
                            }
                        }
                    }
                    break;
                case "Auction":
                    // Show text boxes for bidding with for each player.
                    btnBid.Visibility = Visibility.Visible;
                    break;
            }
        }

        /// <summary>
        /// Based on the sent in instruction we will modify the GUI to show relevant controls to the current state requested.
        /// </summary>
        /// <param name="playerToTradeWith">The player object to trade with</param>
        private void UpdateGui(Player playerToTradeWith)
        {
            // Show gui needed for trade using the requested player to trade with.
        }

        /// <summary>
        /// Sets opacity of images to 1
        /// </summary>
        private void Opacity1()
        {
            // Foreach loop that makes every image's opacity 1.
            foreach (Viewbox vb in GridControls.Children.OfType<Viewbox>())
            {
                if (vb.Child is Image)
                {
                    Image i = vb.Child as Image;
                    i.Opacity = 1;
                }
            }
        }

        /// <summary>
        /// Sets the two values of diceResult to two dice rolls chosen at random
        /// </summary>
        private void DiceRoll()
        {
            // Make a new random seed
            Random roll = new Random();

            // Add two rolls to diceResult array
            this.diceResult[0] = roll.Next(1, 7);
            this.diceResult[1] = roll.Next(1, 7);

            // Set the DiceTotal
            this.diceTotal = this.diceResult[0] + this.diceResult[1];

            // Set labels to let the user know what they rolled.
            lblDie1.Content = this.diceResult[0].ToString();
            lblDie2.Content = this.diceResult[1].ToString();
            lblTotal.Content = this.diceTotal.ToString();
            this.UpdateGui("rolledDice");
        }

        /// <summary>
        /// Change an images source
        /// </summary>
        /// <param name="p">New image source name</param>
        /// <param name="img">Image to be changed</param>
        private void ChangeImage(string p, Image img)
        {
            // Change the current Player image.
            img.Source = new BitmapImage(new Uri(@"/Images/" + p + ".png", UriKind.Relative));
        }

        /// <summary>
        /// Gets the current player from the currentPlayersEnum
        /// </summary>
        /// <returns>The current player</returns>
        private Player GetCurrentPlayer()
        {
            return this.currentPlayersEnum.Current;
        }

        /// <summary>
        /// Gives the current player 200 dollars for passing go
        /// </summary>
        private void PassGo()
        {
            // Add 200 to current player's money value
            this.currentPlayersEnum.Current.Money = 200;
        }

        /// <summary>
        /// Handles presses of the end turn button. Update menu for next player, reset any variables. Loop through turn order.
        /// </summary>
        /// <param name="sender">The object that initiated the event.</param>
        /// <param name="e">The event arguments for the event.</param>
        private void BtnRules_EndTurn(object sender, RoutedEventArgs e)
        {
            // increment enum of current players, if false or end of enum then reupdate enum.
            if (!this.currentPlayersEnum.MoveNext())
            {
                this.UpdateEnum();
            }

            this.UpdateGui("startOfTurn");

            // reset doubles count
            this.doublesCount = 0;
        }

        /// <summary>
        /// Remove the image for the current players piece from the board
        /// </summary>
        /// <param name="p">The current player</param>
        private void RemovePiece(Player p)
        {
            // Find the image for the current player.
            // We must first look for each wrap panel in gridboard, and then each of those wrap panels.
            foreach (WrapPanel wp in GridBoard.Children)
            {
                bool imageFound = false;
                foreach (Image i in wp.Children)
                {
                    if (i.Name == p.Piece + "Img")
                    {
                        // once we find an image with a playerpiece name, remove it.
                        imageFound = true;
                    }

                    if (imageFound)
                    {
                        // remove image from current location
                        wp.Children.Remove(i);
                        break;
                    }
                }

                if (imageFound)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Populates the allProperties list with all the properties. This list is never modified afterwards except whether properties are owned. 
        /// </summary>
        private void PopulatePropertiesData()
        {
            string[] propertiesData = File.ReadAllLines(@"TextDataFiles\Properties.txt");
            foreach (string s in propertiesData)
            {
                if (s.Contains('/'))
                {
                    string[] splitS = s.Split('/');
                    allProperties.Add(new Property(splitS[0], splitS[1]));
                }
                else
                {
                    string[] splitS = s.Split('_');
                    allProperties.Add(new Property(splitS[0], Convert.ToInt32(splitS[1]), Convert.ToInt32(splitS[2]), Convert.ToInt32(splitS[3]), Convert.ToInt32(splitS[4]), Convert.ToInt32(splitS[5]), Convert.ToInt32(splitS[6]), Convert.ToInt32(splitS[7]), Convert.ToInt32(splitS[8]), new SolidColorBrush((Color)ColorConverter.ConvertFromString(splitS[10]))));
                }
            }
        }

        /// <summary>
        /// Populates the Chance deck with all the cards.
        /// </summary>
        /// <param name="typeOfDeck"> The type of deck to populate</param>
        private void PopulateDeck(string typeOfDeck)
        {
            List<string> cardsToSkip = new List<string>();

            // Check if any player owns any card.
            if (currentPlayers.Count > 0)
            {
                foreach (Player p in currentPlayers)
                {
                    if (p.GetOutOfJailCards.Count > 0)
                    {
                        // if they own any add all of them to the list of cards to not reshuffle into the deck.
                        foreach (Card c in p.GetOutOfJailCards)
                        {
                            cardsToSkip.Add(c.CardText);
                        }
                    }
                }
            }
            // Check which deck to add it to.
            if (typeOfDeck == "Chance")
            {
                string[] card = File.ReadAllLines(@"TextDataFiles\chanceDeck.txt");
                foreach (string s in card)
                {
                    string[] splitS = s.Split('/');

                    // Skip adding the card to the new deck if it should be skipped.
                    if (!cardsToSkip.Contains(splitS[0]))
                    {                        
                        this.chanceDeck.Add(new Card(splitS[0], splitS[1]));
                    }
                }
            }
            else
            {
                string[] card = File.ReadAllLines(@"TextDataFiles\communityChestDeck.txt");
                foreach (string s in card)
                {
                    string[] splitS = s.Split('/');

                    // Skip adding the card to the new deck if it should be skipped.
                    if (!cardsToSkip.Contains(splitS[0]))
                    {
                        this.communityChestDeck.Add(new Card(splitS[0], splitS[1]));
                    }
                }
            }            
        }

        /// <summary>
        /// Get the next railroad in front of the player
        /// </summary>
        /// <returns> name of railroad to travel to.</returns>
        private string NextRailroad()
        {
            int playerLocation = this.NumbersFromString(this.FindCurrentPlayerWrapPanel().Name);
            int closestRailroad = 100;
            int possibleShortestDistanceSoFar;
            int shortestDistanceSoFar = 100;
            foreach (WrapPanel wp in GridBoard.Children)
            {
                if (wp.Name.Contains("Railroad"))
                {
                    possibleShortestDistanceSoFar = this.NumbersFromString(wp.Name) - playerLocation;
                    if (possibleShortestDistanceSoFar < shortestDistanceSoFar && possibleShortestDistanceSoFar > 0)
                    {
                        shortestDistanceSoFar = possibleShortestDistanceSoFar;
                        closestRailroad = this.NumbersFromString(wp.Name);
                    }
                }
            }

            if (closestRailroad != 100)
            {
                return this.GetWrapPanel(closestRailroad).Name;
            }

            return "Reading Railroad";
        }

        /// <summary>
        /// Get the next utility in front of the player
        /// </summary>
        /// <returns> name of utility to travel to.</returns>
        private string NextUtility()
        {
            int playerLocation = this.NumbersFromString(this.FindCurrentPlayerWrapPanel().Name);
            int closestUtility = 100;
            int possibleShortestDistanceSoFar;
            int shortestDistanceSoFar = 100;
            foreach (WrapPanel wp in GridBoard.Children)
            {
                if (wp.Name.Contains("Utility"))
                {
                    possibleShortestDistanceSoFar = this.NumbersFromString(wp.Name) - playerLocation;
                    if (possibleShortestDistanceSoFar < shortestDistanceSoFar && possibleShortestDistanceSoFar > 0)
                    {
                        shortestDistanceSoFar = possibleShortestDistanceSoFar;
                        closestUtility = this.NumbersFromString(wp.Name);
                    }
                }
            }

            if (closestUtility != 100)
            {
                return this.GetWrapPanel(closestUtility).Name;
            }

            return "Water Works Utility";
        }

        /// <summary>
        /// Return whether the player can buy a house or hotel for the selected property group.
        /// <param name="selectedProp"> The property the player is attempting to upgrade</param>
        /// <returns> A boolean value of whether you can buy the upgrade or not.</returns>
        private bool canBuyUpgrade(Property selectedProp)
        {
            int groupCount = 0;
            int playerGroupCount = 0;
            String groupColor = selectedProp.Group.ToString();

            // First we check if the player owns all properties in the group.
            foreach(Property p in allProperties)
            {
                if(p.Group.ToString() == groupColor)
                {
                    groupCount++;
                }
            }

            foreach(Property p in currentPlayersEnum.Current.Properties)
            {
                if (p.Group.ToString() == groupColor)
                {
                    playerGroupCount++;
                }
            }
            
            // If they do own all properties in the group, we check if they are buying a house for a property in that group with the lowest amount of houses.
            if(playerGroupCount == groupCount)
            {
                foreach (Property p in currentPlayersEnum.Current.Properties)
                {
                    if (p.Group.ToString() == groupColor)
                    {
                        if (selectedProp.Houses > p.Houses)
                        {
                            MessageBox.Show("You must first upgrade the other properties in this group to the same amount of houses.");
                            return false;                            
                        }                        
                    }                
                }
            }
            else
            {
                MessageBox.Show("You do not own all properties in that group");
                return false;
            }

            return true;            
        }

        /// <summary>
        /// A button to be used for development purposes
        /// </summary>
        /// /// <param name="sender">The object that initiated the event.</param>
        /// <param name="e">The event arguments for the event.</param>
        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            /* TESTDEV*/
            // use this to test events.

            // GoToJail();
            //DrawCard("Community Chest");
            // DrawCard("CommunityChest");            
            currentPlayersEnum.Current.BuyProperty(allProperties[2], currentPlayersEnum.Current);
            currentPlayersEnum.Current.BuyProperty(allProperties[3], currentPlayersEnum.Current);
            currentPlayersEnum.Current.BuyProperty(allProperties[4], currentPlayersEnum.Current);        
        }

        private void BtnTest2_Click(object sender, RoutedEventArgs e)
        {
            MovePiece("Vermont Avenue");
        }

        /// <summary>
        /// Pays the fee for jail and moves the player out of jail.
        /// </summary>
        /// /// <param name="sender">The object that initiated the event.</param>
        /// <param name="e">The event arguments for the event.</param>
        private void BtnPayJailFee_Click(object sender, RoutedEventArgs e)
        {
            this.Tax(50);
            this.currentPlayersEnum.Current.JailTurnCount = 0;
            this.MovePiece("Just Visiting");
            this.UpdateGui("leftJail");
        }

        /// <summary>
        /// Pays the fee for jail and moves the player out of jail.
        /// </summary>
        /// /// <param name="sender">The object that initiated the event.</param>
        /// <param name="e">The event arguments for the event.</param>
        private void ImgGetOutOfJail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Only run this if the player is in jail.
            if (this.currentPlayersEnum.Current.JailTurnCount != 0)
            {
                String cardType = (sender as Image).Name;


                this.currentPlayersEnum.Current.JailTurnCount = 0;
                this.MovePiece("Just Visiting");
                this.UpdateGui("leftJail");

                // Remove the card from players owned cards.
                foreach (Card c in this.currentPlayersEnum.Current.GetOutOfJailCards)
                {
                    if (c.CardText == "Get out of Jail, free.  This card may be kept until needed or sold." && cardType.Contains("Community"))
                    {
                        this.currentPlayersEnum.Current.GetOutOfJailCards.Remove(c);
                        break;
                    }
                    else if (c.CardText == "This card may be kept until needed or sold.  Get out of jail free." && cardType.Contains("Chance"))
                    {
                        this.currentPlayersEnum.Current.GetOutOfJailCards.Remove(c);
                        break;
                    }
                }
                UpdateGui("cardDrawn");
            }
            
        }

        private void BtnSellHouse_Click(object sender, RoutedEventArgs e)
        {
            //This will sell house for selected property ListboxPropertiesOwned.Selected
            // mortgage if no houeses.
        }

        private void BtnPurchaseHouse_Click(object sender, RoutedEventArgs e)
        {
            Property selectedProperty = (ListBoxPropertiesOwned.SelectedItem as Property);
            if(selectedProperty !=null)
                {
                    SolidColorBrush selectedPropertyGroup = selectedProperty.Group;
            
            


                if ( currentPlayersEnum.Current.Money >= selectedProperty.HousePrice)
                {
                    if(canBuyUpgrade(selectedProperty))
                    {
                        selectedProperty.Houses = 1;
                        Tax(selectedProperty.HousePrice);
                    }             
                } 
                else
                {
                    MessageBox.Show("You do not have enough money to purchase a house.");
                }
            }
            UpdateGui("boughtProperty");
            // purchase house for selected property ListboxPropertiesOwned.Selected
            // only works if you aren't adding a house that is 2 above lowest number of houses for a property group/color others for example 1, 1, 0 you have to put a 1 on the 0.
        }

        private void BtnBid_Click(object sender, RoutedEventArgs e)
        {
            //TODO Extra Credit
            // This will be used in conjunction with a textinput to allow players to bid for a property. Check if bid is greater than previous bid. Show current bid, cycle to next player.

        }

        private void ListBoxPropertiesOwned_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // The property in the listbox that is selected
            Property selectedProperty = (ListBoxPropertiesOwned.SelectedItem as Property);
            if(selectedProperty != null)
            {
                UpdateGui("propertySelected");
            }            
        }

        
    }
}
