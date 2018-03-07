using System.Collections;
using System.Collections.Generic;
using System;
using Frontend1;

namespace seng301_asgn1
{
    /// <summary>
    /// Represents the concrete virtual vending machine factory that you will implement.
    /// This implements the IVendingMachineFactory interface, and so all the functions
    /// are already stubbed out for you.
    /// 
    /// Your task will be to replace the TODO statements with actual code.
    /// 
    /// Pay particular attention to extractFromDeliveryChute and unloadVendingMachine:
    /// 
    /// 1. These are different: extractFromDeliveryChute means that you take out the stuff
    /// that has already been dispensed by the machine (e.g. pops, money) -- sometimes
    /// nothing will be dispensed yet; unloadVendingMachine is when you (virtually) open
    /// the thing up, and extract all of the stuff -- the money we've made, the money that's
    /// left over, and the unsold pops.
    /// 
    /// 2. Their return signatures are very particular. You need to adhere to this return
    /// signature to enable good integration with the other piece of code (remember:
    /// this was written by your boss). Right now, they return "empty" things, which is
    /// something you will ultimately need to modify.
    /// 
    /// 3. Each of these return signatures returns typed collections. For a quick primer
    /// on typed collections: https://www.youtube.com/watch?v=WtpoaacjLtI -- if it does not
    /// make sense, you can look up "Generic Collection" tutorials for C#.
    /// </summary>
    public class VendingMachineFactory : IVendingMachineFactory
    {
        private List<VendingMachine> machines; //stores the vending machines

        public VendingMachineFactory()
        {
            machines = new List<VendingMachine>();
        }
        //=======================================Create vending Machine
        public int createVendingMachine(List<int> coinKinds, int selectionButtonCount)
        {
            machines.Add(new VendingMachine(coinKinds, selectionButtonCount));
            //Test the output
            /*Console.WriteLine("Test for create vending machine:");
            for (int i = 0; i < coinKinds.Count; i++)
            {
                Console.Write(coinKinds[i] + ",");
            }
            Console.WriteLine(selectionButtonCount);*/

            return machines.Count - 1;//return as the vmindex value
        }
        //=====================================Configure Vending Machine
        public void configureVendingMachine(int vmIndex, List<string> popNames, List<int> popCosts)
        {
            if (vmIndex < 0 || vmIndex > machines.Count - 1)
            {
                throw new Exception("the selected vending machine is not yet constructed");
            }
            if (popNames == null || popCosts == null)
            {
                throw new Exception("arguments may not be null");
            }
            if (popNames.Count != machines[vmIndex].selectionButtonCount || popCosts.Count != machines[vmIndex].selectionButtonCount)
            {
                throw new Exception("data lists do not match the number of buttons constructed");
            }
            machines[vmIndex].configure(popNames, popCosts);
            //Test
            /*Console.WriteLine("Test for ConfigureVendingMachine:");
            Console.Write("["+ vmIndex +"] ");
            for (int i = 0; i < popKinds.Length; i++)
            {
                Console.Write(popName[i]+popCost[i]);
            }
            Console.WriteLine("   ");*/
        }
        //======================================Load Coins
        public void loadCoins(int vmIndex, int coinKindIndex, List<Coin> coins)
        {

            if (vmIndex < 0 || vmIndex > machines.Count - 1)
            {
                throw new Exception("the selected vending machine is not yet constructed");
            }
            if (coinKindIndex < 0 || coinKindIndex > machines[vmIndex].CoinTypeCount - 1)
            {
                throw new Exception("the coinKindIndex is out of bounds");
            }
            if (coins == null)
            {
                throw new Exception("coin array cannot be null");
            }

            for (int i = 0; i < coins.Count; i++)
            {
                if (coins[i] == null)
                {
                    throw new Exception("coin cannot be null");
                }
                //TEST
                /*Console.WriteLine("Test for load coin:");
                Console.Write("[" + vmIndex + "] ");
                Console.Write(coinKindIndex + ",");
                Console.Write(coins[i] + ",");
                Console.WriteLine(coins.Count + ";");*/
            }
            machines[vmIndex].loadCoins(coinKindIndex, coins);
        }
        //========================================Load Pops
        public void loadPops(int vmIndex, int popKindIndex, List<Pop> pops)
        {
            if (vmIndex < 0 || vmIndex > machines.Count - 1)
            {
                throw new System.IndexOutOfRangeException("the selected vending machine is not yet constructed");
            }
            if (popKindIndex < 0 || popKindIndex > machines[vmIndex].selectionButtonCount - 1)
            {
                throw new System.IndexOutOfRangeException("the selected pop kind does not exist");
            }
            if (pops == null)
            {
                throw new System.NullReferenceException("pop array cannot be null");
            }
            for (int i = 0; i < pops.Count; i++)
            {
                if (pops[i] == null)
                {
                    throw new System.NullReferenceException("pop cannot be null");
                }
                //TEST
                /*Console.WriteLine("Test for load pops:");
                Console.Write("[" + vmIndex + "] ");
                Console.Write(popKindIndex + ",");
                Console.Write(pops[i] + ",");
                Console.WriteLine(pops.Count + ";");*/
            }
            machines[vmIndex].loadPops(popKindIndex, pops);

        }
        //========================================Insert Coins
        public void insertCoin(int vmIndex, Coin coin)
        {
            if (vmIndex < 0 || vmIndex > machines.Count - 1)
            {
                throw new System.IndexOutOfRangeException("the selected vending machine is not yet constructed");
            }
            if (coin == null)
            {
                throw new System.NullReferenceException("coin cannot be null");
            }
            machines[vmIndex].insertCoin(coin);
            //Test
            /*for (int i = 0; i < vmIndex; i++)
            {
                Console.Write(vmIndex + coin[i]);
            }
            Console.WriteLine("   "); */
        }
        //==========================================Press Button
        public void pressButton(int vmIndex, int value)
        {
            if (vmIndex < 0 || vmIndex > machines.Count - 1)
            {
                throw new System.IndexOutOfRangeException("the selected vending machine is not yet constructed");
            }
            if (value < 0 || value > machines[vmIndex].selectionButtonCount - 1)
            {
                throw new System.IndexOutOfRangeException("the button does not exist");
            }
            machines[vmIndex].pressButton(value);
            //Test
            /*for (int i = 0; i < vmIndex; i++)
            {
                Console.Write(vmIndex + value);
            }
            Console.WriteLine("   "); */
        }
        //===================================================Extract From Delivery Chute
        public List<Deliverable> extractFromDeliveryChute(int vmIndex)
        {
            if (vmIndex < 0 || vmIndex > machines.Count - 1)
            {
                throw new System.IndexOutOfRangeException("the selected vending machine is not yet constructed");
            }
            return machines[vmIndex].extractFromDeliveryChute();
            //Test
            // Console.WriteLine(machines[vmIndex].extractFromDeliveryChute());
        }
        //===================================================Unload Vending Machine
        public List<IList> unloadVendingMachine(int vmIndex)
        {
             if (vmIndex < 0 || vmIndex > machines.Count - 1)
             {
                 throw new System.IndexOutOfRangeException("the selected vending machine is not yet implemented");
             }
            return machines[vmIndex].unloadVendingMachine();
        }
    }
}