using System;
using System.Collections.Generic;
using Frontend2.Hardware;

namespace seng301_asgn2.src
{
	class VendingMachineLogic
	{
		private VendingMachine hardware;    //Create an new machine	
		private int insertedValue;			
		private List<int> coinKinds;		
		private List<int> indexDescending;	

		public VendingMachineLogic(VendingMachine hardware, List<int> coinKinds)
		{
			// Initialize variables
			this.hardware = hardware;
			this.insertedValue = 0;
			this.coinKinds = coinKinds;
			sortCoinKinds(coinKinds);

			// CoinReceptacle
			this.hardware.CoinReceptacle.CoinAdded += new EventHandler<CoinEventArgs>(coinAdded);

			// Selection Buttons
			foreach(SelectionButton button in this.hardware.SelectionButtons)
				button.Pressed += new EventHandler(pressed);
		}
		
		private void coinAdded(object sender, CoinEventArgs e)
		{
			// Increment counter
			insertedValue += e.Coin.Value;
		}
		
		private void pressed(object sender, EventArgs e)
		{
			int buttonIndex = Array.IndexOf(this.hardware.SelectionButtons, sender);
			if(insertedValue >= this.hardware.PopCanCosts[buttonIndex] && this.hardware.PopCanRacks[buttonIndex].Count > 0)
			{
				this.hardware.PopCanRacks[buttonIndex].DispensePopCan();// Send pop to delivery
				this.hardware.CoinReceptacle.StoreCoins();				// Attempt to store coins
				int difference = insertedValue - this.hardware.PopCanCosts[buttonIndex];
				insertedValue = 0;
				foreach(int value in indexDescending)
				{
					while(this.hardware.CoinRacks[value].Count > 0)
					{
						if(coinKinds[value] <= difference)
						{
							this.hardware.CoinRacks[value].ReleaseCoin();
							difference -= coinKinds[value];
						}
						else
							break;
					}
				}
			}				
		}

		private void sortCoinKinds(List<int> coinKinds)
		{
			indexDescending = new List<int>();
			while(indexDescending.Count != coinKinds.Count)
				indexDescending.Add(getIndexOfMax(indexDescending, coinKinds));
		}

		private int getIndexOfMax(List<int> exclude, List<int> coinKinds)
		{
			int indexOfMax = new int();
			int max = 0;
			foreach(int value in coinKinds)
			{
				if(!exclude.Contains(coinKinds.IndexOf(value)))	// Check if index is excluded
				{
					if(value > max)		// New largest value
					{
						max = value;	// Reset max
						indexOfMax = coinKinds.IndexOf(value);	// Reset indexOfMax
					}
				}
			}
			return indexOfMax;
		}

	}
}
