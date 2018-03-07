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
    /*This will talk to the CommunicationFacade, PaymentFacade, & ProductFacade
    * This will not talk to the HardwareFacade
    * This is a singleton so that
    * a) Facades can simply use the static instance, without having a class variable.
    * b) Facades can all talk to the same businessRule
    * c) Its simple, and logical to have 1 businessrule per machine.
    */
    public class BusinessRule
    {
        private CommunicationFacade communicationFacade;
        private PaymentFacade paymentFacade;
        private ProductFacade productFacade;


        //=====This Business Rules manage and control the three facades based on the hardwares
        public BusinessRule(CommunicationFacade communicationFacade, PaymentFacade paymentFacade, ProductFacade productFacade)
        {
            this.communicationFacade = communicationFacade;
            this.paymentFacade = paymentFacade;
            this.productFacade = productFacade;
            communicationFacade.buttonPressEvent += buttonPressed;
            productFacade.productsAddedEvent += productsAdded;
        }
        //=====This is the function that tell the vending machine what to do after a button is being pressed.
        public void buttonPressed(object sender, EventArgs args)
        {
            Cents funds = paymentFacade.funds;//funds is the amount of coins that left in the vending machine
            SelectionButton buttonPressed = sender as SelectionButton;//event notified
            if (buttonPressed == null)//if the button is invalid
                throw new Exception("'Button pressed' event didn't return a button");

            Cents cost = productFacade.getCost(this.communicationFacade.index);//get the price of the product
            if (funds >= cost)//if the funds that user provided is greater than the selected product's price
            {
                if (this.productFacade.hasProduct(this.communicationFacade.index))
                {
                    paymentFacade.makeChange(funds - cost);//make the change and set avaliable funds
                    productFacade.vendProduct(this.communicationFacade.index);//dispense the product
                }
                else
                {
                    this.communicationFacade.disableButton(this.communicationFacade.index);
                    this.communicationFacade.displayNotEnoughProduct();
                }
            }
            else
            {
                this.communicationFacade.notEnoughFunds();
            }
        }
        public void productsAdded(object sender, ProductEventArgs args)
        {
            int index = this.productFacade.getIndex((Product)sender);
            this.communicationFacade.enableButton(index);
        }
    }
}
