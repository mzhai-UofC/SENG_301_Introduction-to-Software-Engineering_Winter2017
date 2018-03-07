using System;
using System.Collections.Generic;
using Frontend2;
using Frontend2.Hardware;
using seng301_asgn2.src;

public class VendingMachineFactory : IVendingMachineFactory {

	private List<VendingMachine> machines;
    //====================================================Initialize a new list
	public VendingMachineFactory()
	{
		machines = new List<VendingMachine>(); //a new list
	}
    //====================================================Creating vending machine
    public int CreateVendingMachine(List<int> coinKinds, int selectionButtonCount, int coinRackCapacity, int popRackCapacity, int receptacleCapacity)
    {
		int[] convertedCoinKinds = new int[coinKinds.Count];
		int index = 0;
		foreach(int kind in coinKinds)
			convertedCoinKinds[index++] = kind;
		VendingMachine newMachine = new VendingMachine(convertedCoinKinds, selectionButtonCount, coinRackCapacity, popRackCapacity, receptacleCapacity);
		machines.Add(newMachine);
		VendingMachineLogic newLogic = new VendingMachineLogic(newMachine, coinKinds);
        return 0;
        /*//Test
        Console.WriteLine("Test for create vending machine:");
        for (int i = 0; i < coinKinds.Count; i++)
        {
            Console.Write(coinKinds[i] + ",");
        }
        Console.WriteLine(selectionButtonCount);*/
    }
    //===================================================Configure Vending Machine
    public void ConfigureVendingMachine(int vmIndex, List<string> popNames, List<int> popCosts) {
		machines[vmIndex].Configure(popNames, popCosts);
        /*//Test
        Console.WriteLine("Test for ConfigureVendingMachine:");
        Console.Write("[" + vmIndex + "] ");
        while (itN.MoveNext() && itC.MoveNext())
        {
            //Test
            Console.Write(itN.Current + "," + itC.Current + ";");
            machines[vmIndex].setButton(itN.Current, itC.Current);
        }
        //Test
        Console.WriteLine("   ");*/
    }
    //====================================================Load coins
    public void LoadCoins(int vmIndex, int coinKindIndex, List<Coin> coins) {
		machines[vmIndex].CoinRacks[coinKindIndex].LoadCoins(coins);
        /*for (int i = 0; i < coins.Count; i++)
        {
            if (coins[i] == null)
            {
                Console.WriteLine("coin cannot be null");
            }
            machines[vmIndex].addCoin(coinKindIndex, coins[i]);
            //TEST
            Console.WriteLine("Test for load coin:");
            Console.Write("[" + vmIndex + "] ");
            Console.Write(coinKindIndex + ",");
            Console.Write(coins[i] + ",");
            Console.WriteLine(coins.Count + ";");
        }*/
    }
    //=====================================================Load pops
    public void LoadPops(int vmIndex, int popKindIndex, List<PopCan> pops) {
		machines[vmIndex].PopCanRacks[popKindIndex].LoadPops(pops);
       /* for (int i = 0; i < pops.Count; i++)
        {
            if (pops[i] == null)
            {
                throw new System.NullReferenceException("pop cannot be null");
            }
            machines[vmIndex].addPop(popKindIndex, pops[i]);
            //TEST
            Console.WriteLine("Test for load pops:");
            Console.Write("[" + vmIndex + "] ");
            Console.Write(popKindIndex + ",");
            Console.Write(pops[i] + ",");
            Console.WriteLine(pops.Count + ";");
        }*/
    }
    //====================================================Insert coins
    public void InsertCoin(int vmIndex, Coin coin) {
		machines[vmIndex].CoinSlot.AddCoin(coin);
    }
    //===================================================press button
    public void PressButton(int vmIndex, int value) {
		machines[vmIndex].SelectionButtons[value].Press();
    }
    //===================================================Extract from delivery chute
    public List<IDeliverable> ExtractFromDeliveryChute(int vmIndex) {
		List<IDeliverable> extracted = new List<IDeliverable>();
		foreach(IDeliverable item in machines[vmIndex].DeliveryChute.RemoveItems())
			extracted.Add(item);

        return extracted;
    }
    //===========================================================New added
    public VendingMachineStoredContents UnloadVendingMachine(int vmIndex) {
		VendingMachineStoredContents unloaded = new VendingMachineStoredContents();

		// Unload every coin rack
		foreach(CoinRack rack in machines[vmIndex].CoinRacks)
			unloaded.CoinsInCoinRacks.Add(rack.Unload());

		// Unload every pop can rack
		foreach(PopCanRack rack in machines[vmIndex].PopCanRacks)
			unloaded.PopCansInPopCanRacks.Add(rack.Unload());
		
		// Unload storage bin coins
		foreach(Coin coin in machines[vmIndex].StorageBin.Unload())
			unloaded.PaymentCoinsInStorageBin.Add(coin);

        return unloaded;
    }
}