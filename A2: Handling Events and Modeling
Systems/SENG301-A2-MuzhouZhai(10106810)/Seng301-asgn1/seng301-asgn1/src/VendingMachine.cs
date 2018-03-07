using System;
using System.Collections.Generic;
using System.Collections;
using Frontend1;
namespace seng301_asgn1
{
    class VendingMachine
    {
        private String name;
        private int cost;
        //private List<int> buttons;
        private List<int> coinKinds;
        private String[][] popKinds;

        public int selectionButtonCount;

        private List<Coin>[] coinChutes; //the array is the no. of chutes, the queue is the coins in the chute
        private Queue<Pop>[] popChutes; //array is the no. of chutes, the queue 

        private List<Coin> InsertCoins;
        private List<Coin> insideCoins;
        private List<Deliverable> deliveryChute; //the delivery chute can have both coins and pops

        public void VmButton(string name, int cost)
        {
            if (string.ReferenceEquals(name, null) || cost == null)
            {
                throw new System.ArgumentException("arguments may not be null");
            }
            if (name.Length == 0 || name.Length < 3)
            {
                throw new System.ArgumentException("pop name must be at least one character");
            }
            if (cost <= 0)
            {
                throw new System.ArgumentException("pop cost must be positive");
            }
            this.Name = name;
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                this.name = value;
            }
        }

        public int Cost
        {
            get
            {
                return cost;
            }
            set
            {
                this.cost = value;
            }
        }


        public VendingMachine(List<int> coinKinds, int selectionButtonCount)
        {
            //coinSlot = 0;
            // buttons = new List<VmButton>();
            //coinInventory = new Dictionary<int, Dictionary<string, List<Coin>>>();
            //popInventory = new Dictionary<string, List<Pop>>();
            checkCoinKinds(coinKinds); //checkCoinKinds also throws and exception
            checkSelctionButtonCount(selectionButtonCount); //thorws exception if less than 1

            this.coinKinds = coinKinds.GetRange(0, coinKinds.Count); //create a copy
            this.selectionButtonCount = selectionButtonCount;
            popKinds = new String[selectionButtonCount][];
            coinChutes = new List<Coin>[coinKinds.Count];
            createCoinChutes(coinChutes);

            popChutes = new Queue<Pop>[selectionButtonCount]; //there is a chute for each button
            createPopChutes(popChutes);

            InsertCoins = new List<Coin>();
            insideCoins = new List<Coin>();
            deliveryChute = new List<Deliverable>();
        }


        public void configure(List<String> popName, List<int> popCosts)
        {
            popCostsContainsZero(popCosts);

            if (popName.Count != popCosts.Count)
                throw new Exception("Number of pops is not the same as the number of pop costs");
            int popNameIndex = 0;
            int popPriceIndex = 1;

            for (int i = 0; i < popKinds.Length; i++)
            {
                popKinds[i] = new string[2];
                popKinds[i][popNameIndex] = popName[i]; //set the name for the chute i
                popKinds[i][popPriceIndex] = Convert.ToString(popCosts[i]); //set the price of the pop for the chute 
            }
        }
        public void loadCoins(int coinKindIndex, List<Coin> coins)
        {
            foreach (Coin coin in coins)
            {
                if (ifCoinAccepted(coin))
                    coinChutes[coinKindIndex].Add(coin);
                else
                {
                    throw new Exception("ERROR: Cannot load coin of value " + coin.Value + " into vending machine. Not an " +
                        "acceptable coin value");
                }
            }
        }
        public int CoinTypeCount
        {
            get
            {
                return coinKinds.Count;

            }
        }
        public bool ifCoinAccepted(Coin coin)
        {
            foreach (int coin1 in coinKinds)
            {
                if (coin1 == coin.Value)
                    return true;
            }
            return false;
        }

        public void loadPops(int popKindIndex, List<Pop> pops)
        {
            foreach (Pop pop in pops)
            {
                if (ifPopAccepted(pop))
                {
                    popChutes[popKindIndex].Enqueue(pop);
                }
                else
                {
                    //Firstly I use "Throw new Exception"", but in order to show the full result for the scripts, I use WriteLine.
                    Console.WriteLine("Cannot load pop with name " + pop.Name + " .");
                }
            }
        }

        public void insertCoin(Coin coin)
        {
            if (ifCoinAccepted(coin))
                //InsertCoins.Add(coin);
                insideCoins.Add(coin);
            else //return invalid coin
                deliveryChute.Add(coin);
        }

        public void pressButton(int btn)
        {

            if (btn >= selectionButtonCount) //btn is 0-indexed based while selctionButtonCount is not
            {
                throw new Exception("Vending Machine only has " + selectionButtonCount + " buttons");
            }
            else
            {
                int popCostIndex = 1;
                int popValue = Convert.ToInt32(popKinds[btn][popCostIndex]);
                int insideCoinsValue = sumInsertCoins(insideCoins);

                if (insideCoinsValue >= popValue)
                {
                    if (popChutes[btn].Count > 0)
                    {
                        deliveryChute.Add(popChutes[btn].Dequeue()); //take the pop from the fron of pop chute and add to delivery chute
                        int diff = insideCoinsValue - popValue;
                        List<Coin> change = calcChange(diff);

                        foreach (Coin c in change)
                            deliveryChute.Add(c);

                        //transfer from limbo to revenue
                        while (insideCoins.Count > 0)
                        {
                            InsertCoins.Add(insideCoins[0]);
                            insideCoins.RemoveAt(0);
                        }
                    }
                }
            }
        }

        private int sumInsertCoins(List<Coin> coins)
        {
            int sum = 0;
            foreach (Coin c in coins)
            {
                sum += Convert.ToInt32(c.Value);
            }
            return sum;
        }

        private List<Coin> calcChange(int diff)
        {
            List<Coin> change = new List<Coin>();
            List<int> coinKindsSorted = sortMethod(coinKinds);

            int Index = 0;
            int sum = 0;

            while (sum != diff && Index < coinKindsSorted.Count)
            {
                for (int i = 0; i < coinKinds.Count; i++)
                {
                    if (coinKinds[i] == coinKindsSorted[Index] && coinKinds[i] + sum <= diff
                        && coinChutes[i].Count > 0)
                    {
                        while (coinChutes[i].Count > 0 && coinKinds[i] + sum <= diff)
                        {
                            Coin c = coinChutes[i][0];
                            change.Add(c);
                            sum += coinKinds[i]; //can't add value of coin because wrong coin could be loaded to chute
                            coinChutes[i].RemoveAt(0);
                        }
                    }
                }
                Index++;
            }
            return change;
        }
        //bubble sort
        private List<int> sortMethod(List<int> coinKinds)
        {
            bool sorted = false;
            List<int> coinKindsSorted = coinKinds.GetRange(0, coinKinds.Count); //shallow copy

            //bubble sort from largeset to smalled
            while (!sorted)
            {
                int i = 0;
                bool swapped = false;
                while (i < coinKindsSorted.Count - 1)
                {
                    int val1 = coinKindsSorted[i];
                    int val2 = coinKindsSorted[i + 1];

                    if (val1 < val2)
                    { //swap
                        int temp = coinKindsSorted[i + 1];
                        coinKindsSorted[i + 1] = coinKindsSorted[i];
                        coinKindsSorted[i] = temp;
                        swapped = true;
                    }
                    i++;
                }
                if (!swapped) //break out of loop if we didn't swap anything on a run
                    sorted = true;
            }
            return coinKindsSorted;
        }

        public bool ifPopAccepted(Pop pop)
        {
            int popNameIndex = 0;
            foreach (String[] p in popKinds)
            {
                if (p[popNameIndex] == pop.Name)
                    return true;
            }
            return false;
        }

        public void createCoinChutes(List<Coin>[] coinChutes)
        {
            for (int i = 0; i < coinChutes.Length; i++)
            {
                coinChutes[i] = new List<Coin>();
            }
        }

        public void createPopChutes(Queue<Pop>[] popChutes)
        {
            for (int i = 0; i < popChutes.Length; i++)
            {
                popChutes[i] = new Queue<Pop>();
            }
        }

        //checks if the list containts a 0 or any duplicates
        private void checkCoinKinds(List<int> coinKinds)
        {
            List<int> coinKindsCopy = coinKinds.GetRange(0, coinKinds.Count); //shallow copy
            coinKindsCopy.Sort();

            int baseValue = coinKindsCopy[0];

            if (baseValue == 0)
                throw new Exception("coinKind list contains invalid coin kind: 0");

            for (int i = 0; i < coinKindsCopy.Count - 1; i++)
            {

                int next = coinKindsCopy[i + 1];

                if (baseValue != next) //the current two indices aren't duplicates
                    baseValue = next;
                else//found duplicate. If there are two duplicates then they'll be next to eachother beacuse list is sorted
                {
                    throw new Exception("coinKind list containts duplicate values: " + baseValue);
                }
            }
        }

        private void checkSelctionButtonCount(int selectionButtonCount)
        {
            if (selectionButtonCount < 1)
                throw new Exception("selectionButtonCount is less than 1");
        }
        private void popCostsContainsZero(List<int> popCosts)
        {
            foreach (int cost in popCosts)
            {
                if (cost == 0)
                {
                    throw new Exception("price of pop is 0");
                }
            }
        }

        private bool ifListEqual(List<String> popNames, List<int> popCosts)
        {
            if (popNames.Count != popCosts.Count)
            {
                throw new Exception("The number of pops is not the same as the number of pop prices");
            }
            return true;
        }

        public List<Deliverable> extractFromDeliveryChute()
        {
            //empty the delivery chute
            List<Deliverable> clone = new List<Deliverable>();
            while (deliveryChute.Count > 0)
            {
                clone.Add(deliveryChute[0]);
                deliveryChute.RemoveAt(0);
            }
            return clone;
        }

        public List<IList> unloadVendingMachine()
        {
            List<Coin> loadedCoins = new List<Coin>();
            List<Coin> InsertCoinsClone = new List<Coin>();
            List<Pop> loadedPops = new List<Pop>();
            foreach (List<Coin> chute in coinChutes)
            {
                while (chute.Count > 0)
                {
                    loadedCoins.Add(chute[0]); //add the first coint
                    chute.RemoveAt(0); //remove the first coint
                }
            }
            foreach (Queue<Pop> chute in popChutes)
            {
                while (chute.Count > 0)
                {
                    loadedPops.Add(chute.Dequeue());
                }
            }
            while (InsertCoins.Count > 0)
            {
                InsertCoinsClone.Add(InsertCoins[0]);
                InsertCoins.RemoveAt(0);
            }

            List<IList> unloaded = new List<IList>();
            return new List<IList>()
            {
                loadedCoins, InsertCoinsClone, loadedPops
            };
        }

        private void copyToSingleList(List<Deliverable> target, List<Deliverable>[] original)
        {
            foreach (List<Deliverable> chute in original)
            {
                foreach (Deliverable d in chute)
                {
                    target.Add(d);
                }
            }
        }
    }
}