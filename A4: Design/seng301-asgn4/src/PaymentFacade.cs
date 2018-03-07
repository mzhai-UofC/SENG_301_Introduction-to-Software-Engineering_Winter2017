using System;
using System.Collections.Generic;
using System.Linq;
using Frontend4;
using Frontend4.Hardware;
/*SENG301_Winter2017_Assignment4
 * Author: Alejandro Garcia (T06), Muzhou Zhai (T06)
 * Submission date: Apirl, 2nd, 2017
 */
namespace seng301_asgn4.src
{
    //==========This calss will talk to the HardwareFacade and nothing else
    public class PaymentFacade
    {
        public HardwareFacade Hardware;
        public Cents funds;
        public List<Cents> coinKinds;
        //=========This facade using Hardware settings and the CoinKinds list
        public PaymentFacade(HardwareFacade hardware, List<Cents> newCoinKinds)
        {
            this.Hardware = hardware;
            this.coinKinds = newCoinKinds;
            funds = new Cents(0);//initialize funds, funds is the amount of coins that inside the vending machine
            Hardware.CoinSlot.CoinAccepted += new EventHandler<CoinEventArgs>(coinAccepted);//Coin accepted event
        }
        //=========This function Add the cents that user inserted and calculate it as funds
        private void coinAccepted(object sender, EventArgs args)
        {
            CoinEventArgs coinEventArgs = args as CoinEventArgs;//Set the coins event
            if (coinEventArgs != null)//if there are valid coins inserted
            {
                this.addFunds(coinEventArgs.Coin.Value);//Add the amount of the fund and record them in the coin slot
            }
        }
        //=========Add funds, maybe with a credit card? coins?, doesn't matter
        public void addFunds(Cents value)
        {
            this.funds += value;
        }
        //=========This function using the settings inside the Hardware,and the coinRacks to make change and update the avaliable funds (if possible)
        public void makeChange(Cents change)
        {
            this.Hardware.CoinReceptacle.StoreCoins();  //store coins in the coin racks
            if (change.Value > 0) //if the value of change is valid
            {
                //Get the list of the coins in a certain coins kind.
                List<Cents> coinskind = this.coinKinds.OrderBy(coin => coin.Value).ToList();
                for (int i = coinskind.Count - 1; i >= 0; i--)
                {
                    var coins = coinskind[i];//current coins in a certain coins kind
                                             //if the amount of change is greater than the amount of the current coin kind, get the coin rack for the coin kind 
                    if (change.Value >= coins.Value)
                    {
                        var coinRack = this.Hardware.GetCoinRackForCoinKind(coins);
                        if (coinRack.Count != 0)//if there are some coins of this kind left
                        {
                            coinRack.ReleaseCoin();//release the coins
                            change -= coinskind[i];//remove the coins that just being released
                            i++; //inc.
                        }
                    }
                    else
                    {
                        coinskind.RemoveAt(i);//Otherwise directly remove the coin at the list 
                    }
                }

                funds = change; //the funds left in machine
            }
        }
    }
}




