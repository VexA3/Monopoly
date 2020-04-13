using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public int numPlayers = 0;

        int moveTotal;
        int diceTotal;
        int doublesCount;
        public int playersPiece = -1;
        public bool pickedAPiece = false;

        const int SIZE = 8;
        // array that holds all the Player pieces.
        string[] arrPlayerPieces = new string[SIZE] { "hatPiece","boatPiece", "carPiece", "dogPiece",
                                                    "bootPiece","wheelbarrowPiece", "ironPiece", "thimblePiece" };
        
        private void StartGame(Button button)
        {
            //change text to restart
            button.Content = "Restart";

            //hide radio buttons
            foreach (RadioButton b in GridControls.Children.OfType<RadioButton>())
            {
                b.Visibility = Visibility.Hidden;
            }

            //Create player classes here depending on number of players

        }
        private void RestartGame(Button button)
        {
            //Change text to Start
            button.Content = "Start";

            //Set number of players radiobuttons to unchecked and visibile again
            numPlayers = 0;
            foreach(RadioButton b in GridControls.Children.OfType<RadioButton>())
            {
                b.IsChecked = false;
                b.Visibility = Visibility.Visible;
            }

            playersPiece = -1;
            VisibleOrHide(true);
            ChangeImage("default", imgCurrentPlayer);
            Opacity1();
            btnConfirmPlayerPiece.Visibility = Visibility.Visible;
            pickedAPiece = false;
            lblPick.Visibility = Visibility.Visible;
            diceTotal = 0;

            //Reset any variables to starting amounts.
        }
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            //Check if numplayers has been set
            if(numPlayers != 0 && pickedAPiece == true)
            {            
                //Check if Starting or restarting the game
                if (button.Content.ToString() == "Start")
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
                MessageBox.Show("Please choose number of players and confirm a piece!");
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
            if (playersPiece != -1)
            {
                // Changes imgCurrentPiece to the selected piece.
                CheckImage(imgCurrentPlayer);

                // Put player 1 on the board.
                CheckImage(imgPlayer1);

                // Method used to hide all images
                VisibleOrHide(false);

                // Make the piece selected visible
                imgCurrentPlayer.Visibility = Visibility.Visible;
                
                pickedAPiece = true;

                btnConfirmPlayerPiece.Visibility = Visibility.Hidden;
                lblPick.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Please pick a piece.");
            }
        }

        private void BtnRoll_Click(object sender, RoutedEventArgs e)
        {
            int die1, die2;

            Random rnd = new Random();
            if(playersPiece != -1 /*&& numPlayer != 0*/)
            {
                // Get random numbers for the user.
                die1 = rnd.Next(1, 7);
                die2 = rnd.Next(1, 7);

                // Set the DiceTotal
                diceTotal = die1 + die2;

                // Set labels to let the user know what they rolled.
                lblDie1.Content = die1.ToString();
                lblDie2.Content = die2.ToString();
                lblTotal.Content = diceTotal.ToString();

                // Check if the user rolled doubles 3 times in a row.
                if (doublesCount != 3)
                {
                    if (die1 == die2)
                    {
                        MessageBox.Show("You Rolled Doubles, Roll Again.");
                        doublesCount++;
                    }
                    else
                    {
                        doublesCount = 0;
                    }
                }
                else
                {
                    MessageBox.Show("Go to jail.");
                }

                MovePiece();
            }
            else
            {
                MessageBox.Show("Please pick a Piece.");
            }
        }

        private void ImgMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Use the last Image as sender.
            Image img = sender as Image;

            // Show the user what image they clicked
            if (img.Opacity == 1)
            {
                Opacity1();
                img.Opacity = 0.5;

                // Uses the selected image's tag for the playerPiece variable.
                playersPiece = Convert.ToInt32(img.Tag.ToString());
            }
            else
            {
                // show that the piece was deselected
                playersPiece = -1;
                img.Opacity = 1;
            }
        }

        public void ChangeImage(string p, Image img)
        {
            // Change the current Player image.
            img.Source = new BitmapImage(new Uri(@"/Images/" + p + ".jpg", UriKind.Relative));
        }

        public void CheckImage(Image img)
        {
            int i = 0;
            while (playersPiece >= i)
            {
                if(playersPiece == i)
                {
                    // Change the current image to the Image the user selected.
                    ChangeImage(arrPlayerPieces[i], img);
                }
                i++;
            }
        }
        
        public void VisibleOrHide(bool v)
        {
            // used to either make every image visible or hidden.
            if(v == true)
            {
                // Foreach loop that makes every image Visible.
                foreach (Image b in grdPlayerPiece.Children.OfType<Image>())
                {
                    b.Visibility = Visibility.Visible;
                }
            }
            else
            {
                // Foreach loop that makes every image hidden.
                foreach (Image b in grdPlayerPiece.Children.OfType<Image>())
                {
                    b.Visibility = Visibility.Hidden;
                }
            }
        }

        public void Opacity1()
        {
            // Foreach loop that makes every image's opacity 1.
            foreach (Image b in grdPlayerPiece.Children.OfType<Image>())
            {
                b.Opacity = 1;
            }
        }
        
        public void MovePiece()
        {
            // TODO: find the correct ratio.
            int moveAmount = diceTotal * 10;

            moveTotal += moveAmount;
            
            imgPlayer1.Margin = new Thickness(640, moveTotal, 0,0);

            // TODO: find the max movement downward then start moving left.
        }

    }
}
