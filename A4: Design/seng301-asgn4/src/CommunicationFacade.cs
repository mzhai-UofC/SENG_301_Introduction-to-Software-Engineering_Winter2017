using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend4;
using Frontend4.Hardware;
/*SENG301_Winter2017_Assignment4
 * Author: Alejandro Garcia (T06), Muzhou Zhai (T06)
 * Submission date: Apirl, 2nd, 2017
 */
namespace seng301_asgn4.src
{
    //This class will talk to the HardwareFacade and nothing else
    public class CommunicationFacade
    {
        private HardwareFacade Hardware;
        public EventHandler buttonPressEvent;
        public int index { get; private set; }
        //=========This facade using Hardware settings
        public CommunicationFacade(HardwareFacade hardware)
        {
            this.Hardware = hardware;
            for (int i = 0; i < Hardware.SelectionButtons.Count(); i++)
            {
                Hardware.SelectionButtons[i].Pressed += new EventHandler(notifyButtonPressed);
            }
        }
        //====This is a function that justify if a button is being pressed
        public void notifyButtonPressed(object sender, EventArgs args)
        {
            this.buttonPressEvent(sender, args);
        }
        //====This function will get the product's index number according the button that the user selected 
        public void Buttonselection(object sender, EventArgs args)
        {
            var button = (SelectionButton)sender;//Get the button's information from hardware
            if (button.Enabled)//if there is a button selected
            {
                var buttonIndex = this.Hardware.SelectionButtons.ToList().FindIndex(b => b.Equals(button));//get the button index
                this.index = buttonIndex;//the product's index
            }
        }
        //===Not enough funds message
        public void notEnoughFunds()
        {
            this.Hardware.Display.DisplayMessage("Not enough funds");
        }
        //===product dispensed message
        public void productDispensed()
        {
            this.Hardware.Display.DisplayMessage("Thank you for your purchase");
        }
        //==not enough products message
        public void displayNotEnoughProduct()
        {
            this.Hardware.Display.DisplayMessage("We are out of that product, sorry for the inconvenience");
        }
        public void disableButton(int index)
        {
            this.Hardware.SelectionButtons[index].Disable();
        }
        public void enableButton(int index)
        {
            this.Hardware.SelectionButtons[index].Enable();
        }
    }
}
