﻿using System;
using System.Collections.Generic;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Monopoly
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //Make a list of players to add the current players to.
        public static List<Player> currentPlayers = new List<Player>();
        // create enum for player turn order
        public List<Player>.Enumerator currentPlayersEnum = currentPlayers.GetEnumerator();

        public int numPlayers = 0;

        // currentChoice is used for looping through the piece allocation
        public int currentChoice = 1;

        //Dice result is an array with 2 values. Those 2 values are your 2 dice rolls.
        public int[] diceResult = new int[2];

        // This holds the name of a piece to assign it to a player temporarily
        public string playersPiece = null;

        //
        int moveTotal;
        int diceTotal;
        int doublesCount;

        private void ChoosePieces()
        {
            // Make confirm button visible
            btnConfirmPlayerPiece.Visibility = Visibility.Visible;

            // Change display of label to indicate to player who's turn it is to choose a piece.
            lblDisplayTurnOrChoice.Content = "Player " + currentChoice.ToString() + " please choose a piece.";
        }
        private void StartGame(Button button)
        {
            //change text to restart
            TextBlockStartRestart.Text = "Restart";

            //hide radio buttons
            foreach (RadioButton b in StkRadioButtons.Children.OfType<RadioButton>())
            {
                b.Visibility = Visibility.Hidden;
            }
            //set the Icon select buttons to visible
            VisibleOrHide(true);
            // Run choose pieces method which will make player classes with chosen pieces.
            ChoosePieces();
        }
        private void StartGame()
        {
            // Update enum for player order
            UpdateEnum();

            // hide pieces to be chosen.
            VisibleOrHide(false);

            // hide confirm piece button.
            btnConfirmPlayerPiece.Visibility = Visibility.Hidden;


            ShowPlayerControls();


            //Place player images on the board on go / 10,10
            foreach (Player p in currentPlayers)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(@"/Images/" + p.Piece + ".png", UriKind.Relative));
                GetWrapPanel(10, 10).Children.Add(img);
                img.Name = p.Piece + "Img";
            }

        }
        private void ShowPlayerControls()
        {
            // Set Control buttons to visible. Dice, Trade, etc...
            lblDisplayTurnOrChoice.Content = "Roll Your Dice";
            lblCurrentPlayer.Visibility = Visibility.Visible;
            lblDisplayTurnOrChoice.Visibility = Visibility.Visible;
            imgCurrentPlayer.Visibility = Visibility.Visible;
            imgDice.Visibility = Visibility.Visible;

            // Display first player's turn.
            ChangeCurrentImage(currentPlayersEnum.Current.Piece);

            //Make dice labels visible
            foreach (Viewbox vb in GridControls.Children.OfType<Viewbox>())
            {
                if (vb.Child is Label)
                {
                    Label l = vb.Child as Label;
                    if (l.Tag != null && l.Tag.ToString() == "DiceLabel")
                        l.Visibility = Visibility.Visible;
                }
            }
        }
        private void RestartGame(Button button)
        {
            //Change text to Start
            TextBlockStartRestart.Text = "Start";

            //Set number of players radiobuttons to unchecked and visibile again            
            foreach(RadioButton b in StkRadioButtons.Children.OfType<RadioButton>())
            {
                b.IsChecked = false;
                b.Visibility = Visibility.Visible;
            }

            // Hide labels and buttons that are not needed when at menu.
            imgCurrentPlayer.Visibility = Visibility.Hidden;
            btnEndTurn.Visibility = Visibility.Hidden;
            btnConfirmPlayerPiece.Visibility = Visibility.Hidden;

            // Delete all player image pieces on the board.
            foreach (Player p in currentPlayers)
            {
                RemovePiece(p);
            }

            //Reset any variables to starting amounts.
            playersPiece = "null";
            diceResult[0] = 0;
            diceResult[1] = 0;
            VisibleOrHide(false);
            ChangeCurrentImage("default");
            Opacity1();
            numPlayers = 0;
            currentChoice = 1;
            currentPlayers.Clear();
            UpdateEnum();

            

        }
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            //Check if numplayers has been set
            if(numPlayers != 0)
            {            
                //Check if Starting or restarting the game
                if (TextBlockStartRestart.Text == "Start")
                {
                    StartGame(button);                
                }
                else
                {
                    RestartGame(button);
                }
            }
            else
            {
                MessageBox.Show("Please choose number of players");
            }
        }

        private void BtnQuit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Developers: Johnathan DeLeeuw, Brian Herman, Kyler Lambert, Billy Rintamaki \n");
        }

        private void BtnRules_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("THE PLAY...Starting with the Banker, each player in turn throws thedice. The player with the highest total starts the play: \n Place your tokenon the corner marked “GO,”  throw the dice and move your token inthe direction of the arrow the number of spaces \nindicated by the dice.After you have completed your play, the turn passes to the left.Thetokens remain on the spaces occupied and \n proceed from that point onthe player’s next turn.Two or more tokens may rest on the same spaceat the same time.According to the\n space your token reaches, you may be entitled tobuy real estate or other properties — or obliged to pay rent, pay taxes, draw a \n  Chance or Community Chest card, “Go to Jail®,” etc.If you throw doubles, you move your token as usual, the sum of thetwo dice, \n and are subject to any privileges or penalties pertaining tothe space on which you land.Retaining the dice, throw again andmove \n your token as before.If you throw doubles three times insuccession, move your token immediately to the space marked “InJail”(see JAIL).\n “GO”...Each time a player’s token lands on or passes over GO, whether by throwing the dice or drawing a card, the Banker \n  payshim / her a $200 salary.The $200 is paid only once each time around the board. However, ifa player passing GO on the throw \n   of the dice lands 2 spaces beyond iton Community Chest, or 7 spaces beyond it on Chance, and draws the“Advance to GO”  card, \n he / she collects $200 for passing GO the firsttime and another $200 for reaching it the second time by instructionson the card.\n   BUYING PROPERTY...Whenever you land on an unowned propertyyou may buy that property from the Bank at its printed price. \n     You receive the Title Deed card showing ownership; place it face up infront of you.If you do not wish to buy the property, \n   the Banker sells it at auctionto the highest bidder. The buyer pays the Bank the amount of the bidin cash and receives the \n   Title Deed card for that property. Any player, including the one who declined the option to buy it at the printedprice, may \n     bid.Bidding may start at any price.PAYING RENT...When you land on property owned by anotherplayer, the owner collects rent \n     from you in accordance with the listprinted on its Title Deed card.If the property is mortgaged, no rent can be collected.\n      When aproperty is mortgaged, its Title Deed card is placed face down in frontof the owner.It is an advantage to hold all \n      the Title Deed cards in a color - group(e.g., Boardwalk and Park Place; or Connecticut, Vermont and OrientalAvenues) because \n      the owner may then charge double rent forunimproved properties in that color-group.This rule applies tounmortgaged properties \n     even if another property in that color-groupis mortgaged.It is even more advantageous to have houses or hotels on properties \n     because rents are much higher than for unimproved properties.The owner may not collect the rent if he / she fails to ask for it beforethe second player following throws the dice.“CHANCE” \n  AND “COMMUNITY CHEST”...When you land oneither of these spaces, take the top card from the deck indicated, follow the instructions \n  and return the card face down to the bottom ofthe deck.The “Get Out of Jail Free”  card is held until used and then returned to \n  the bottom of the deck. If the player who draws it does not wish touse it, he / she may sell it, at any time, to another player \n    at a priceagreeable to both.“INCOME TAX”...If you land here you have two options: You mayestimate your tax at $200 and pay \n    the Bank, or you may pay 10 % ofyour total worth to the Bank.Your total worth is all your cash onhand, printed prices of \n    mortgaged and unmortgaged properties andcost price of all buildings you own.You must decide which option you will take before you add upyour total worth.“JAIL”...You land in Jail when...(1) your token lands on the spacemarked “Go to Jail”; (2) you draw a card marked “Go to Jail”; or(3) you throw doubles three times in succession.When you are sent to Jail you cannot collect your $200 salary in thatmove since, regardless of where your token is on the board, you mustmove it directly into Jail. Yours turn ends when you are sent to Jail.If you are not “sent”  to Jail but in the ordinary course of play landon that space, you are “Just Visiting,”  you incur no penalty, and youmove ahead in the usual manner on your next turn.You get out of Jail by...(1) throwing doubles on any of your nextthree turns; if you succeed in doing this you immediately moveforward the number of spaces shown by your doubles throw; eventhough you had thrown doubles, you do not take another turn; (2) using the “Get Out of Jail Free”  card if you have it; (3) purchasingthe “Get Out of Jail Free”  card from another player and playing it; (4) paying a fine of $50 before you roll the dice on either of your nexttwo turns.If you do not throw doubles by your third turn, you must pay the$50 fine.You then get out of Jail and immediately move forward thenumber of spaces shown by your throw.Even though you are in Jail, you may buy and sell property, buyand sell houses and hotels and collect rents.“FREE PARKING”...A player landing on this place does not receiveany money, property or reward of any kind. This is just a “free”resting place.HOUSES...When you own all the properties in a color-group youmay buy houses from the Bank and erect them on those properties.If you buy one house, you may put it on any one of thoseproperties.The next house you buy must be erected on one of theunimproved properties of this or any other complete color - group youmay own.The price you must pay the Bank for each house is shown on yourTitle Deed card for the property on which you erect the house.      The owner still collects double rent from an opponent who lands onthe unimproved properties of his / her complete color - group. \n      Following the above rules, you may buy and erect at any time asmany houses as your judgement and financial standing will allow. \n      Butyou must build evenly, i.e., you cannot erect more than one house onany one property of any color - group until you have \n      built one house onevery property of that group.You may then begin on the second rowof houses, and so on, up to a limit \n      of four houses to a property.Forexample, you cannot build three houses on one property if you haveonly one house on \n     another property of that group.As you build evenly, you must also break down evenly if you sellhouses back to the Bank \n     (see SELLING PROPERTY).HOTELS...When a player has four houses on each property of acomplete color - group, he / she may buy a \n         hotel from the Bank anderect it on any property of the color - group.He / she returns the fourhouses from that property to \n            the Bank and pays the price for the hotelas shown on the Title Deed card.Only one hotel may be erected onany one property.\n            BUILDING SHORTAGES...When the Bank has no houses to sell, players wishing to build must wait for some player to return or sell \nhis / her houses to the Bank before building. If there are a limitednumber of houses and hotels available and two or more \n  players wishto buy more than the Bank has, the houses or hotels must be sold atauction to the highest bidder.\n\nSELLING PROPERTY...Unimproved properties, railroads andutilities(but not buildings) may be sold to any player as a \nprivate transaction for any amount the owner can get; however, no propertycan be sold to another player if buildings \nare standing on anyproperties of that color-group.Any buildings so located must be soldback to the Bank before the \nowner can sell any property of that color-group.Houses and hotels may be sold back to the Bank at any time \nfor one-half the price paid for them.All houses on one color-group must be sold one by one, evenly, \nin reverse of the manner in which they were erected.All hotels on one color-group may be sold at once, or \nthey may besold one house at a time (one hotel equals five houses), evenly, inreverse of the manner in which \nthey were erected.MORTGAGES...Unimproved properties can be mortgaged throughthe Bank at any time.Before an \nimproved property can be mortgaged, all the buildings on all the properties of its color-group must be \nsold back to the Bank at half price.The mortgage value is printed on eachTitle Deed card.No rent can be \ncollected on mortgaged properties or utilities, butrent can be collected on unmortgaged properties in the same \ngroup.In order to lift the mortgage, the owner must pay the Bank theamount of the mortgage plus 10% interest. \nWhen all the properties of acolor-group are no longer mortgaged, the owner may begin to buyback houses at full \nprice.The player who mortgages property retains possession of it and noother player may secure it by lifting \nthe mortgage from the Bank. However, the owner may sell this mortgaged property to anotherplayer at any agreed\nprice. If you are the new owner, you may lift themortgage at once if you wish by paying off the mortgage \nplus 10%interest to the Bank.If the mortgage is not lifted at once, you must paythe Bank 10% interest \nwhen you buy the property and if you lift themortgage later you must pay the Bank an additional 10% interest \nas well as the amount of the mortgage.BANKRUPTCY...You are declared bankrupt if you owe more thanyou can pay either \nto another player or to the Bank. If your debt is toanother player, you must turn over to that player all that \nyou have ofvalue and retire from the game.In making this settlement, if you ownhouses or hotels, you must return \nthese to the Bank in exchange formoney to the extent of one-half the amount paid for them; this cash isgiven to \nthe creditor.If you have mortgaged property you also turnthis property over to your creditor but the new owner \nmust at oncepay the Bank the amount of interest on the loan, which is 10% of thevalue of the property. \nThe new owner who does this may then, athis/her option, pay the principal or hold the property until some \nlater turn, then lift the mortgage. If he/she holds property in this way untila later turn, he/she must pay \nthe interest again upon lifting themortgage.Should you owe the Bank, instead of another player, more than you \ncan pay (because of taxes or penalties) even by selling off buildingsand mortgaging property, you must turn over \nall assets to the Bank. Inthis case, the Bank immediately sells by auction all property so taken,except buildings. \nA bankrupt player must immediately retire from thegame. The last player left in the game wins.\n\nMISCELLANEOUS...Money can be loaned to a player only by theBank and then only by mortgaging property. \nNo player may borrowfrom or lend money to another player");
        }

        private void RadioBtnPlayer_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            numPlayers = Convert.ToInt32(button.Tag.ToString());
        }

        private void BtnConfirmPlayerPiece_Click(object sender, RoutedEventArgs e)
        {
            // Checks if the user picked a piece.
            if (playersPiece != null)
            {
                //Make a new player class with the number of that player and their chosen piece. Add them to the list of players.
                currentPlayers.Add(new Player(currentChoice, playersPiece));


                // Changes imgCurrentPiece to the selected piece. TODO We will have next player turn button or a next player function that will either call this function or cycle image itself.
                //CheckImage(imgCurrentPlayer);

                // Put player 1 on the board. //TODO Move this to STARTGAME() and create the img of it dynamically instead of always having an imgPlayer1
                //CheckImage(imgPlayer1);


                // make chosen piece hidden.                
                foreach (Viewbox vb in GridControls.Children.OfType<Viewbox>())
                {
                    if (vb.Child is Image)
                    {
                        Image i = vb.Child as Image;
                        if (i.Tag != null && i.Tag.ToString() == playersPiece)
                            i.Visibility = Visibility.Hidden;
                    }

                }

                // Start the game if all players have chosen a piece.
                if (currentChoice == numPlayers)
                {
                    StartGame();
                }
                // If more players have pieces to choose increment the current players turn for choosing a piece.
                else
                {
                    currentChoice++;
                    ChoosePieces();
                    playersPiece = null;
                }
            }
            else
            {
                MessageBox.Show("Please pick a piece.");
            }
            
        }

        // returns the wrappanel at a given coord from the gridboard.
        private WrapPanel GetWrapPanel(int x, int y)
        {
            foreach(WrapPanel w in GridBoard.Children)
            {
                if(w.Tag.ToString() == x+","+y)
                {
                    return w;
                }
            }
            return null;
        }

        // return wp with given name
        private WrapPanel GetWrapPanel(string wrapPanel)
        {
            foreach (WrapPanel w in GridBoard.Children)
            {
                if (w.Name == wrapPanel)
                {
                    return w;
                }
            }
            return null;
        }

        public void ChangeCurrentImage(string p)
        {
            // Change the current Player image.
            imgCurrentPlayer.Source = new BitmapImage(new Uri(@"/Images/"+ p + ".png", UriKind.Relative));
        }

        public void UpdateEnum()
        {
            // Update enum so it can be used after players were added to list.
            currentPlayersEnum = currentPlayers.GetEnumerator();
            currentPlayersEnum.MoveNext();
        }

        private void PieceImgMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Use the last Image as sender.
            Image img = sender as Image;

            // Show the user what image they clicked
            if (img.Opacity == 1)
            {
                //set all other images opacity back to 1
                Opacity1();
                img.Opacity = 0.5;

                // Uses the selected image's tag for the playerPiece variable.
                playersPiece = img.Tag.ToString();

                
            }
            else if (img.Opacity == 0.5)
            {
                // show that the piece was deselected
                playersPiece = null;
                img.Opacity = 1;
            }
        }
        private void DiceImgMouseDown(object sender, MouseButtonEventArgs e)
        {       
            // use DiceRoll to generate 2 die rolls.
            DiceRoll();
            bool goneToJail = false;
            // Set the DiceTotal
            diceTotal = diceResult[0] + diceResult[1];

            // Set labels to let the user know what they rolled.
            lblDie1.Content = diceResult[0].ToString();
            lblDie2.Content = diceResult[1].ToString();
            lblTotal.Content = diceTotal.ToString();

            // Check if the user rolled doubles 3 times in a row.
            if (doublesCount != 2)
            {
                if (diceResult[0] == diceResult[1])
                {
                    MessageBox.Show("You Rolled Doubles, Roll Again.");
                    doublesCount++;
                }
                else
                {
                    // If they haven't rolled doubles then their turn is over. Hide dice and show end turn button.
                    imgDice.Visibility = Visibility.Hidden;
                    btnEndTurn.Visibility = Visibility.Visible;
                }
            }
            else
            {
                //If they hit 3 doubles in a row send them to jail.
                MessageBox.Show("Go to jail.");
                MovePiece(true);
                goneToJail = true;
                // You can't continue moving from jail so hide dice, and show end turn button.
                imgDice.Visibility = Visibility.Hidden;
                btnEndTurn.Visibility = Visibility.Visible;
            }
            // if you go to jail their piece is already moved to jail so do not move them the dicetotal as well.
            if(!goneToJail)
                MovePiece(false);
        }
        
        public void VisibleOrHide(bool v)
        {
            // used to either make every piece image hidden or visible.
            if(v == true)
            {
                // Foreach loop that makes every image Visible.

                foreach (Viewbox vb in GridControls.Children.OfType<Viewbox>())
                {
                    if(vb.Child is Image)
                    {
                        Image i = vb.Child as Image;
                        if (i.Tag != null && i.Tag.ToString() != "CurrentPlayer" && i.Tag.ToString() !="Dice")
                            i.Visibility = Visibility.Visible;
                    }
                    
                }
            }
            else
            {
                // Foreach loop that makes every image hidden.
                foreach (Viewbox vb in GridControls.Children.OfType<Viewbox>())
                {
                    if (vb.Child is Image)
                    {
                        Image i = vb.Child as Image;
                        if (i.Tag != null && i.Tag.ToString() != "CurrentPlayer")
                            i.Visibility = Visibility.Hidden;
                    }

                }

            }
            // Also set labels visible/hidden
            if (v == true)
            {
                // Foreach loop that makes every Label Visible.
                foreach (Viewbox vb in GridControls.Children.OfType<Viewbox>())
                {
                    if(vb.Child is Label)
                    {
                        Label l = vb.Child as Label;
                        if (l.Tag != null)
                        {
                            if (l.Tag.ToString() != "CurrentPlayer" && l.Tag.ToString() != "DiceLabel")
                                l.Visibility = Visibility.Visible;
                        }
                            
                    }
                }
            }
            else
            {
                // Foreach loop that makes every Label hidden.
                foreach (Viewbox vb in GridControls.Children.OfType<Viewbox>())
                {
                    if (vb.Child is Label)
                    {
                        Label l = vb.Child as Label;
                        if(l.Tag == null)
                            l.Visibility = Visibility.Hidden;
                        if(l.Tag != null)
                        {
                            if (l.Tag.ToString() != "display")
                                l.Visibility = Visibility.Hidden;
                            if (l.Tag.ToString() == "display")
                                l.Content = "Choose number of Players then press Start.";

                        }
                        
                    }
                }
            }

        }

        public void Opacity1()
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
        private void DiceRoll()
        {
            // Make a new random seed
            Random roll = new Random();           

            // Add two rolls to diceResult array
            diceResult[0] = roll.Next(1, 7);
            diceResult[1] = roll.Next(1, 7);
        }

        public void ChangeImage(string p, Image img)
        {
            // Change the current Player image.
            img.Source = new BitmapImage(new Uri(@"/Images/" + p + ".jpg", UriKind.Relative));
        }
        
        public Player GetCurrentPlayer()
        {
            return currentPlayersEnum.Current;
        }

        private void PassGo()
        {
            //Add 200 to current player's money value
            currentPlayersEnum.Current.Money = 200;            
        }
        //Move Piece to jail on false, else move to new location on true.
        public void MovePiece(bool goToJail)
        {
            int nextLocation = -1;
            bool imageFound = false;
            if (goToJail)
            {
                //Find the image for the current player.
                // We must first look for each wrap panel in gridboard, and then each of those wrap panels.
                foreach (WrapPanel wp in GridBoard.Children)
                {
                    foreach (Image i in wp.Children)
                    {
                        if (i.Name == currentPlayersEnum.Current.Piece + "Img")
                        {
                            // once we find the image for the players piece move it to jail.                            
                            imageFound = true;
                        }
                        if (imageFound)
                        {

                            //remove image from current location
                            wp.Children.Remove(i);

                            //Add to new location
                            GetWrapPanel("WrapPanelJail11").Children.Add(i);
                            break;
                        }
                    }
                    if (imageFound)
                        break;
                }
            }
            else
            {
                //Find the image for the current player.
                // We must first look for each wrap panel in gridboard, and then each of those wrap panels.
                foreach (WrapPanel wp in GridBoard.Children)
                {
                    foreach (Image i in wp.Children)
                    {
                        if (i.Name == currentPlayersEnum.Current.Piece + "Img")
                        {
                            // once we find the image for the players piece move it the dice result.
                            //Find current location of the image by the name of the wrappanel stripped of chars except numbers converted to int                      
                            int currentLocation = Convert.ToInt32(Regex.Replace(wp.Name, "[^0-9]", ""));

                            // Add dicetotal to currentLocation
                            nextLocation = currentLocation + diceTotal;

                            //Check if passing go.
                            if (nextLocation > 40)
                            {
                                nextLocation = nextLocation - 40;
                                PassGo();
                            }
                            imageFound = true;

                        }
                        if (imageFound)
                        {

                            //remove image from current location
                            wp.Children.Remove(i);

                            //Add to new location
                            GetWrapPanel("WrapPanel" + nextLocation).Children.Add(i);
                            break;
                        }
                    }
                    if (imageFound)
                        break;
                }
            }

        }

        private void BtnRules_EndTurn(object sender, RoutedEventArgs e)
        {            
            //increment enum of current players, if false or end of enum then reupdate enum.
            if (currentPlayersEnum.MoveNext())
            {
                Console.WriteLine(currentPlayersEnum.Current.Piece);
            }
            else
            {
                UpdateEnum();
            }
            ShowPlayerControls();

            //reset doubles count
            doublesCount = 0;
            // Hide end turn button
            btnEndTurn.Visibility = Visibility.Hidden;
        }
        private void RemovePiece(Player p)
        {            
            //Find the image for the current player.
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

                        //remove image from current location
                        wp.Children.Remove(i);
                        break;
                    }
                }
                if (imageFound)
                    break;
            }
        
        }
    }
}
