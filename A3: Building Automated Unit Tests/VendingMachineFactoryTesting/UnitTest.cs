using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Frontend2;
using Frontend2.Hardware;
/*SENG301_Winter2017_Assignment3
 * Unit Test for the project
 * Created by Muzhou Zhai from T06
 * To do - Add test classes and methods to mimic the included test scripts (21 scripts totally)
 */
namespace UnitTestProject
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        //T01_good_insert_and_press_exact_change
        public void T01_insert_and_press_extract_change()
        {
            int buttonNum = 0;
            //Setting up the input & creating vending machine vm0
            int[] coinKinds = new int[4] { 5, 10, 25, 100 };   //coin kinds from the script
            VendingMachine vm0 = new VendingMachine(coinKinds, 3, 10, 10, 10);     //create a new vending machine vm0
            new VendingMachineLogic(vm0);      //create a new machine logic		
            //Configure vending machine
            List<String> popNames = new List<String>() { "Coke", "water", "stuff" };
            List<int> popPrices = new List<int>() { 250, 250, 205 };
            vm0.Configure(popNames, popPrices);
            //Load coins
            vm0.CoinRacks[0].LoadCoins(new List<Coin>() { new Coin(5) });     //one 5 cent 
            vm0.CoinRacks[1].LoadCoins(new List<Coin>() { new Coin(10) });    //one 10 cent
            vm0.CoinRacks[2].LoadCoins(new List<Coin>() { new Coin(25), new Coin(25) });  //two 25 cents
            vm0.CoinRacks[3].LoadCoins(new List<Coin>(new Coin[] { }));  //0 100 cents
            //Load pops
            vm0.PopCanRacks[0].LoadPops(new List<PopCan>() { new PopCan("Coke") });   // 1 coke
            vm0.PopCanRacks[1].LoadPops(new List<PopCan>() { new PopCan("water") });  // 1 water
            vm0.PopCanRacks[2].LoadPops(new List<PopCan>() { new PopCan("stuff") });  // 1 stuff
            //Insert coins
            vm0.CoinSlot.AddCoin(new Coin(100));
            vm0.CoinSlot.AddCoin(new Coin(100));
            vm0.CoinSlot.AddCoin(new Coin(25));
            vm0.CoinSlot.AddCoin(new Coin(25));
            //Press the button 0
            vm0.SelectionButtons[buttonNum].Press();
            //Extract from the delivery chute
            var insideDeliveryChute = vm0.DeliveryChute.RemoveItems();
            //Check delivery
            int expectedChanges = 0; //Check coin delivery
            Assert.IsTrue(checkCoinDelivery(insideDeliveryChute, expectedChanges));
            var expectedPops = new List<PopCan>() { new PopCan("Coke") };//Check pop delivery
            Assert.IsTrue(checkPopDelivery(insideDeliveryChute, expectedPops));
            //Check unload
            var storedContents = new VendingMachineStoredContents();
            int expectedCoinRackValue = 315;
            Assert.IsTrue(checkUnload(vm0, storedContents, expectedCoinRackValue));
            //Check tear down
            int expectedStorageBinValue = 0;
            var popsInRacks = new List<List<PopCan>>()
            { new List<PopCan>() { }, new List<PopCan>() {new PopCan("water")},new List<PopCan>() {new PopCan("stuff")}};
            Assert.IsTrue(checkTearDown(storedContents, expectedStorageBinValue, popsInRacks));
        }
       
        [TestMethod]
        //T02_good_insert_and_press_change_expected
        public void T02_good_insert_and_press_change_expected()
        {
            int buttonNum = 0;
            //Setting up the input & creating vending machine vm0
            int[] coinKinds = new int[4] { 5, 10, 25, 100 };   //coin kinds from the script
            VendingMachine vm0 = new VendingMachine(coinKinds, 3, 10, 10, 10);     //create a new vending machine vm0
            new VendingMachineLogic(vm0);      //create a new machine logic		
                                                //Configure vending machine
            List<String> popNames = new List<String>() { "Coke", "water", "stuff" };
            List<int> popPrices = new List<int>() { 250, 250, 205 };
            vm0.Configure(popNames, popPrices);
            //Load coins
            vm0.CoinRacks[0].LoadCoins(new List<Coin>() { new Coin(5) });     //one 5 cent 
            vm0.CoinRacks[1].LoadCoins(new List<Coin>() { new Coin(10) });    //one 10 cent
            vm0.CoinRacks[2].LoadCoins(new List<Coin>() { new Coin(25), new Coin(25) });  //two 25 cents
            vm0.CoinRacks[3].LoadCoins(new List<Coin>(new Coin[] { }));  //0 100 cents
            //Load pops
            vm0.PopCanRacks[0].LoadPops(new List<PopCan>() { new PopCan("Coke") });   // 1 coke
            vm0.PopCanRacks[1].LoadPops(new List<PopCan>() { new PopCan("water") });  // 1 water
            vm0.PopCanRacks[2].LoadPops(new List<PopCan>() { new PopCan("stuff") });  // 1 stuff
            //Insert coins
            vm0.CoinSlot.AddCoin(new Coin(100));
            vm0.CoinSlot.AddCoin(new Coin(100));
            vm0.CoinSlot.AddCoin(new Coin(100));
            //Press the button 0
            vm0.SelectionButtons[buttonNum].Press();
            //Extract from delivery chute
            var insideDeliveryChute = vm0.DeliveryChute.RemoveItems();
            //Check delivery
            int expectedChanges = 50; //Check coin delivery
            Assert.IsTrue(checkCoinDelivery(insideDeliveryChute, expectedChanges));
            var expectedPops = new List<PopCan>() { new PopCan("Coke") };//Check pop delivery
            Assert.IsTrue(checkPopDelivery(insideDeliveryChute, expectedPops));
            //Check unload
            var storedContents = new VendingMachineStoredContents();
            int expectedCoinRackValue = 315;
            Assert.IsTrue(checkUnload(vm0, storedContents, expectedCoinRackValue));
            //Check tear down
            int expectedStorageBinValue = 0;
            var popsInRacks = new List<List<PopCan>>()
            { new List<PopCan>() { }, new List<PopCan>() {new PopCan("water")},new List<PopCan>() {new PopCan("stuff")}};
            Assert.IsTrue(checkTearDown(storedContents, expectedStorageBinValue, popsInRacks));
        }

        [TestMethod]
        //Good-Tear down without configure or load
        public void T03_good_tear_down_without_configure_or_load()
        {
            //Setting up the input & creating vending machine vm0
            int[] coinKinds = new int[4] { 5, 10, 25, 100 };   //coin kinds from the script
            VendingMachine vm0 = new VendingMachine(coinKinds, 3, 10, 10, 10);     //create a new vending machine vm0
            new VendingMachineLogic(vm0);      //create a new machine logic	
            //Extract from delivery chute
            var insideDeliveryChute = vm0.DeliveryChute.RemoveItems();
            //Check delivery
            int expectedChanges = 0; //Check coin delivery
            Assert.IsTrue(checkCoinDelivery(insideDeliveryChute, expectedChanges));
            var expectedPops = new List<PopCan>() { };//Check pop delivery
            Assert.IsTrue(checkPopDelivery(insideDeliveryChute, expectedPops));
            //Check unload tear down
            var storedContents = new VendingMachineStoredContents();
            int expectedCoinRackValue = 0;
            Assert.IsTrue(checkUnload(vm0, storedContents, expectedCoinRackValue));
            //Check tear down
            int expectedStorageBinValue = 0;
            var popsInRacks = new List<List<PopCan>>()
            { new List<PopCan>() { }};
            Assert.IsTrue(checkTearDown(storedContents, expectedStorageBinValue, popsInRacks));
        }

        [TestMethod]
        //Good-press without insert
        public void T04_good_press_without_insert()
        {
            //Setting up the input & creating vending machine vm0
            int[] coinKinds = new int[4] { 5, 10, 25, 100 };   //coin kinds from the script
            VendingMachine vm0 = new VendingMachine(coinKinds, 3, 10, 10, 10);     //create a new vending machine vm0
            new VendingMachineLogic(vm0);      //create a new machine logic		
                                                //Configure vending machine
            List<String> popNames = new List<String>() { "Coke", "water", "stuff" };
            List<int> popPrices = new List<int>() { 250, 250, 205 };
            vm0.Configure(popNames, popPrices);
            //Load coins
            vm0.CoinRacks[0].LoadCoins(new List<Coin>() { new Coin(5) });     //one 5 cent 
            vm0.CoinRacks[1].LoadCoins(new List<Coin>() { new Coin(10) });    //one 10 cent
            vm0.CoinRacks[2].LoadCoins(new List<Coin>() { new Coin(25), new Coin(25) });  //two 25 cents
            vm0.CoinRacks[3].LoadCoins(new List<Coin>(new Coin[] { }));  //0 100 cents
            //Load pops
            vm0.PopCanRacks[0].LoadPops(new List<PopCan>() { new PopCan("Coke") });   // 1 coke
            vm0.PopCanRacks[1].LoadPops(new List<PopCan>() { new PopCan("water") });  // 1 water
            vm0.PopCanRacks[2].LoadPops(new List<PopCan>() { new PopCan("stuff") });  // 1 stuff
            //Press button
            vm0.SelectionButtons[0].Press();
            //Extract from the delivery chute
            var insideDeliveryChute = vm0.DeliveryChute.RemoveItems();
            //Check delivery
            int expectedChanges = 0; //Check coin delivery
            Assert.IsTrue(checkCoinDelivery(insideDeliveryChute, expectedChanges));
            var expectedPops = new List<PopCan>() { };//Check pop delivery
            Assert.IsTrue(checkPopDelivery(insideDeliveryChute, expectedPops));
            //Check unload
            var storedContents = new VendingMachineStoredContents();
            int expectedCoinRackValue = 65;
            Assert.IsTrue(checkUnload(vm0, storedContents, expectedCoinRackValue));
            //Check tear down
            int expectedStorageBinValue = 0;
            var popsInRacks = new List<List<PopCan>>()
            { new List<PopCan>() {new PopCan("Coke") }, new List<PopCan>() {new PopCan("water")},new List<PopCan>() {new PopCan("stuff")}};
            Assert.IsTrue(checkTearDown(storedContents, expectedStorageBinValue, popsInRacks));
        }

        [TestMethod]
        //Good-scrambled coin kinds
        public void T05_good_scrambled_coin_kinds()
        {
            //Setting up the input & creating vending machine vm0
            int[] coinKinds = new int[4] { 100, 5, 25, 10 };   //coin kinds from the script
            VendingMachine vm0 = new VendingMachine(coinKinds, 3, 2, 10, 10);     //create a new vending machine vm0
            new VendingMachineLogic(vm0);      //create a new machine logic		
                                                //Configure vending machine
            List<String> popNames = new List<String>() { "Coke", "water", "stuff" };
            List<int> popPrices = new List<int>() { 250, 250, 205 };
            vm0.Configure(popNames, popPrices);
            //Load coin
            vm0.CoinRacks[0].LoadCoins(new List<Coin>(new Coin[] { }));  //0 100 cents
            vm0.CoinRacks[1].LoadCoins(new List<Coin>() { new Coin(5) });     //one 5 cent 
            vm0.CoinRacks[2].LoadCoins(new List<Coin>() { new Coin(25), new Coin(25) });  //two 25 cents
            vm0.CoinRacks[3].LoadCoins(new List<Coin>() { new Coin(10) });    //one 10 cent
            //Load pops
            vm0.PopCanRacks[0].LoadPops(new List<PopCan>() { new PopCan("Coke") });   // 1 coke
            vm0.PopCanRacks[1].LoadPops(new List<PopCan>() { new PopCan("water") });  // 1 water
            vm0.PopCanRacks[2].LoadPops(new List<PopCan>() { new PopCan("stuff") });  // 1 stuff
            //press the button 0
            vm0.SelectionButtons[0].Press();
            //----------------------extract & check dilivery
            var insideDeliveryChute = vm0.DeliveryChute.RemoveItems();
            //Check delivery
            int expectedChanges = 0; //Check coin delivery
            Assert.IsTrue(checkCoinDelivery(insideDeliveryChute, expectedChanges));
            var expectedPops = new List<PopCan>() { };//Check pop delivery
            Assert.IsTrue(checkPopDelivery(insideDeliveryChute, expectedPops));
            //Insert coins
            vm0.CoinSlot.AddCoin(new Coin(100));
            vm0.CoinSlot.AddCoin(new Coin(100));
            vm0.CoinSlot.AddCoin(new Coin(100));
            //press the button 0
            vm0.SelectionButtons[0].Press();
            //Extract from delivery chute
            var insideDeliveryChute1 = vm0.DeliveryChute.RemoveItems();
            int expectedChanges1 = 50; //Check coin delivery
            Assert.IsTrue(checkCoinDelivery(insideDeliveryChute1, expectedChanges1));
            var expectedPops1 = new List<PopCan>() { new PopCan("Coke") };//Check pop delivery
            Assert.IsTrue(checkPopDelivery(insideDeliveryChute1, expectedPops1));
            //Check unload
            var storedContents = new VendingMachineStoredContents();
            int expectedCoinRackValue = 215;
            Assert.IsTrue(checkUnload(vm0, storedContents, expectedCoinRackValue));
            //Check tear down
            int expectedStorageBinValue = 100;
            var popsInRacks = new List<List<PopCan>>()
            { new List<PopCan>() { }, new List<PopCan>() {new PopCan("water")},new List<PopCan>() {new PopCan("stuff")}};
            Assert.IsTrue(checkTearDown(storedContents, expectedStorageBinValue, popsInRacks));
        }

        [TestMethod]
        //good_extract_before_sale
        public void T06_good_extract_before_sale()
        {
            //Setting up the input & creating vending machine vm0
            int[] coinKinds = new int[4] { 100, 5, 25, 10 };   //coin kinds from the script
            VendingMachine vm0 = new VendingMachine(coinKinds, 3, 2, 10, 10);     //create a new vending machine vm0
            new VendingMachineLogic(vm0);      //create a new machine logic		
                                              //Configure vending machine
            List<String> popNames = new List<String>() { "Coke", "water", "stuff" };
            List<int> popPrices = new List<int>() { 250, 250, 205 };
            vm0.Configure(popNames, popPrices);
            //Load coins 
            vm0.CoinRacks[0].LoadCoins(new List<Coin>(new Coin[] { }));  //0 100 cents
            vm0.CoinRacks[1].LoadCoins(new List<Coin>() { new Coin(5) });     //one 5 cent 
            vm0.CoinRacks[2].LoadCoins(new List<Coin>() { new Coin(25), new Coin(25) });  //two 25 cents
            vm0.CoinRacks[3].LoadCoins(new List<Coin>() { new Coin(10) });    //one 10 cent
            //Load pops
            vm0.PopCanRacks[0].LoadPops(new List<PopCan>() { new PopCan("Coke") });   // 1 coke
            vm0.PopCanRacks[1].LoadPops(new List<PopCan>() { new PopCan("water") });  // 1 water
            vm0.PopCanRacks[2].LoadPops(new List<PopCan>() { new PopCan("stuff") });  // 1 stuff
            //press the button 0
            vm0.SelectionButtons[0].Press();
            //----------------------extract & check dilivery
            var insideDeliveryChute = vm0.DeliveryChute.RemoveItems();
            //Check delivery
            int expectedChanges = 0; //Check coin delivery
            Assert.IsTrue(checkCoinDelivery(insideDeliveryChute, expectedChanges));
            var expectedPops = new List<PopCan>() { };//Check pop delivery
            Assert.IsTrue(checkPopDelivery(insideDeliveryChute, expectedPops));
            //Insert coins
            vm0.CoinSlot.AddCoin(new Coin(100));
            vm0.CoinSlot.AddCoin(new Coin(100));
            vm0.CoinSlot.AddCoin(new Coin(100));
            //Extract from delivery chute
            var insideDeliveryChute1 = vm0.DeliveryChute.RemoveItems();
            int expectedChanges1 = 0; //Check coin delivery
            Assert.IsTrue(checkCoinDelivery(insideDeliveryChute1, expectedChanges1));
            var expectedPops1 = new List<PopCan>() { };//Check pop delivery
            Assert.IsTrue(checkPopDelivery(insideDeliveryChute1, expectedPops1));
            //Check unload
            var storedContents = new VendingMachineStoredContents();
            int expectedCoinRackValue = 65;
            Assert.IsTrue(checkUnload(vm0, storedContents, expectedCoinRackValue));
            //Check tear down
            int expectedStorageBinValue = 0;
            var popsInRacks = new List<List<PopCan>>()
            { new List<PopCan>() {new PopCan("Coke")}, new List<PopCan>() {new PopCan("water")},new List<PopCan>() {new PopCan("stuff")}};
            Assert.IsTrue(checkTearDown(storedContents, expectedStorageBinValue, popsInRacks));
        }

        [TestMethod]
        //good changing configure
        public void T07_good_changing_configure()
        {
            int buttonNum = 0;
            //Setting up the input & creating vending machine vm0
            int[] coinKinds = new int[4] { 5, 10, 25, 100 };   //coin kinds from the script
            VendingMachine vm0 = new VendingMachine(coinKinds, 3, 10, 10, 10);     //create a new vending machine vm0
            new VendingMachineLogic(vm0);      //create a new machine logic		
            //Configure vending machine
            List<String> popNames = new List<String>() { "A", "B", "C" };
            List<int> popPrices = new List<int>() { 5, 10, 25 };
            vm0.Configure(popNames, popPrices);
            //Load coins
            vm0.CoinRacks[0].LoadCoins(new List<Coin>() { new Coin(5) });     //one 5 cent 
            vm0.CoinRacks[1].LoadCoins(new List<Coin>() { new Coin(10) });    //one 10 cent
            vm0.CoinRacks[2].LoadCoins(new List<Coin>() { new Coin(25), new Coin(25) });  //two 25 cents
            vm0.CoinRacks[3].LoadCoins(new List<Coin>(new Coin[] { }));  //0 100 cents
            //Load pops
            vm0.PopCanRacks[0].LoadPops(new List<PopCan>() { new PopCan("A") });   // 1 coke
            vm0.PopCanRacks[1].LoadPops(new List<PopCan>() { new PopCan("B") });  // 1 water
            vm0.PopCanRacks[2].LoadPops(new List<PopCan>() { new PopCan("C") });  // 1 stuff
            //Configure
            List<String> popNames1 = new List<String>() { "Coke", "water", "stuff"};
            List<int> popPrices1 = new List<int>() { 250, 250, 205 };
            vm0.Configure(popNames1, popPrices1);
            //Press the button 0
            vm0.SelectionButtons[buttonNum].Press();
            //Extract from the delivery chute
            var insideDeliveryChute = vm0.DeliveryChute.RemoveItems();
            //Check delivery
            int expectedChanges = 0; //Check coin delivery
            Assert.IsTrue(checkCoinDelivery(insideDeliveryChute, expectedChanges));
            //Insert coins
            vm0.CoinSlot.AddCoin(new Coin(100));
            vm0.CoinSlot.AddCoin(new Coin(100));
            vm0.CoinSlot.AddCoin(new Coin(100));
            //press the button 0
            vm0.SelectionButtons[0].Press();
            //Extract from delivery chute
            var insideDeliveryChute1 = vm0.DeliveryChute.RemoveItems();
            int expectedChanges2 = 50; //Check coin delivery
            Assert.IsTrue(checkCoinDelivery(insideDeliveryChute1, expectedChanges2));
            var expectedPops = new List<PopCan>() { new PopCan("A") };//Check pop delivery
            Assert.IsTrue(checkPopDelivery(insideDeliveryChute1, expectedPops));
            //Check unload
            var storedContents = new VendingMachineStoredContents();
            int expectedCoinRackValue = 315;
            Assert.IsTrue(checkUnload(vm0, storedContents, expectedCoinRackValue));
            int expectedStorageBinValue = 0;
            var popsInRacks = new List<List<PopCan>>()
            { new List<PopCan>() { }, new List<PopCan>() {new PopCan("B")},new List<PopCan>() {new PopCan("C")}};
            Assert.IsTrue(checkTearDown(storedContents, expectedStorageBinValue, popsInRacks));
            //Load coins
            vm0.CoinRacks[0].LoadCoins(new List<Coin>() { new Coin(5) });     //one 5 cent 
            vm0.CoinRacks[1].LoadCoins(new List<Coin>() { new Coin(10) });    //one 10 cent
            vm0.CoinRacks[2].LoadCoins(new List<Coin>() { new Coin(25), new Coin(25) });  //two 25 cents
            vm0.CoinRacks[3].LoadCoins(new List<Coin>(new Coin[] { }));  //0 100 cents
            //Load pops
            vm0.PopCanRacks[0].LoadPops(new List<PopCan>() { new PopCan("Coke") });   // 1 coke
            vm0.PopCanRacks[1].LoadPops(new List<PopCan>() { new PopCan("water") });  // 1 water
            vm0.PopCanRacks[2].LoadPops(new List<PopCan>() { new PopCan("stuff") });  // 1 stuff
            //Insert coins
            vm0.CoinSlot.AddCoin(new Coin(100));
            vm0.CoinSlot.AddCoin(new Coin(100));
            vm0.CoinSlot.AddCoin(new Coin(100));
            //press the button 0
            vm0.SelectionButtons[0].Press();
            //Extract from delivery chute
            var insideDeliveryChute3 = vm0.DeliveryChute.RemoveItems();
            int expectedChanges3 = 50; //Check coin delivery
            Assert.IsTrue(checkCoinDelivery(insideDeliveryChute3, expectedChanges3));
            var expectedPops3 = new List<PopCan>() { new PopCan("Coke") };//Check pop delivery
            Assert.IsTrue(checkPopDelivery(insideDeliveryChute3, expectedPops3));
            //Check unload
            var storedContents3 = new VendingMachineStoredContents();
            int expectedCoinRackValue3 = 315;
            Assert.IsTrue(checkUnload(vm0, storedContents3, expectedCoinRackValue3));
            //check tear down
            int expectedStorageBinValue3 = 0;
            var popsInRacks3 = new List<List<PopCan>>()
            { new List<PopCan>() { }, new List<PopCan>() {new PopCan("water")},new List<PopCan>() {new PopCan("stuff")}};
            Assert.IsTrue(checkTearDown(storedContents3, expectedStorageBinValue3, popsInRacks3));
        }

        [TestMethod]
        //T08_good_approximate_change
        public void T08_good_approximate_change()
        {
            int buttonNum = 0;
            //Setting up the input & creating vending machine vm0
            int[] coinKinds = new int[4] { 5, 10, 25, 100 };   //coin kinds from the script
            VendingMachine vm0 = new VendingMachine(coinKinds, 1, 10, 10, 10);     //create a new vending machine vm0
            new VendingMachineLogic(vm0);      //create a new machine logic		
            //Configure vending machine
            List<String> popNames = new List<String>() { "stuff" };
            List<int> popPrices = new List<int>() { 140 };
            vm0.Configure(popNames, popPrices);
            //Load coins
            vm0.CoinRacks[0].LoadCoins(new List<Coin>(new Coin[] { }));     //one 5 cent 
            vm0.CoinRacks[1].LoadCoins(new List<Coin>() { new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10) });    //5 10 cent
            vm0.CoinRacks[2].LoadCoins(new List<Coin>() { new Coin(25)});  //two 25 cents
            vm0.CoinRacks[3].LoadCoins(new List<Coin> { new Coin(100) });  //0 100 cents
            //Load pops
            vm0.PopCanRacks[0].LoadPops(new List<PopCan>() { new PopCan("stuff") });   // 1 stuff
            //Insert coins
            vm0.CoinSlot.AddCoin(new Coin(100));
            vm0.CoinSlot.AddCoin(new Coin(100));
            vm0.CoinSlot.AddCoin(new Coin(100));
            //Press the button 0
            vm0.SelectionButtons[buttonNum].Press();
            //Extract from the delivery chute
            var insideDeliveryChute = vm0.DeliveryChute.RemoveItems();
            //Check delivery
            int expectedChanges = 155; //Check coin delivery
            Assert.IsTrue(checkCoinDelivery(insideDeliveryChute, expectedChanges));
            var expectedPops = new List<PopCan>() { new PopCan("stuff") };//Check pop delivery
            Assert.IsTrue(checkPopDelivery(insideDeliveryChute, expectedPops));
            //Check unload
            var storedContents = new VendingMachineStoredContents();
            int expectedCoinRackValue = 320;
            Assert.IsTrue(checkUnload(vm0, storedContents, expectedCoinRackValue));
            //check tear down
            int expectedStorageBinValue = 0;
            var popsInRacks = new List<List<PopCan>>()
            { new List<PopCan>() { }};
            Assert.IsTrue(checkTearDown(storedContents, expectedStorageBinValue, popsInRacks));
        }
        [TestMethod]
        // T09_good_hard_for_change()
        public void T09_good_hard_for_change()
        {
            int buttonNum = 0;
            //Setting up the input & creating vending machine vm0
            int[] coinKinds = new int[4] { 5, 10, 25, 100 };   //coin kinds from the script
            VendingMachine vm0 = new VendingMachine(coinKinds, 1, 10, 10, 10);     //create a new vending machine vm0
            new VendingMachineLogic(vm0);      //create a new machine logic		
            //Configure vending machine
            List<String> popNames = new List<String>() { "stuff" };
            List<int> popPrices = new List<int>() { 140 };
            vm0.Configure(popNames, popPrices);
            //Load coins
            vm0.CoinRacks[0].LoadCoins(new List<Coin> { new Coin(5) });     //one 5 cent 
            vm0.CoinRacks[1].LoadCoins(new List<Coin>() { new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10) });    //5 10 cent
            vm0.CoinRacks[2].LoadCoins(new List<Coin>() { new Coin(25) });  //two 25 cents
            vm0.CoinRacks[3].LoadCoins(new List<Coin> { new Coin(100) });  //0 100 cents
            //Load pops
            vm0.PopCanRacks[0].LoadPops(new List<PopCan>() { new PopCan("stuff") });   // 1 stuff
            //Insert coins
            vm0.CoinSlot.AddCoin(new Coin(100));
            vm0.CoinSlot.AddCoin(new Coin(100));
            vm0.CoinSlot.AddCoin(new Coin(100));
            //Press the button 0
            vm0.SelectionButtons[buttonNum].Press();
            //Extract from the delivery chute
            var insideDeliveryChute = vm0.DeliveryChute.RemoveItems();
            //Check delivery
            int expectedChanges = 160; //Check coin delivery
            Assert.IsTrue(checkCoinDelivery(insideDeliveryChute, expectedChanges));
            var expectedPops = new List<PopCan>() { new PopCan("stuff") };//Check pop delivery
            Assert.IsTrue(checkPopDelivery(insideDeliveryChute, expectedPops));
            //Check unload
            var storedContents = new VendingMachineStoredContents();
            int expectedCoinRackValue = 330;
            Assert.IsTrue(checkUnload(vm0, storedContents, expectedCoinRackValue));
            //check tear down
            int expectedStorageBinValue = 0;
            var popsInRacks = new List<List<PopCan>>()
            { new List<PopCan>() { }};
            Assert.IsTrue(checkTearDown(storedContents, expectedStorageBinValue, popsInRacks));
        }

        [TestMethod]
        // T10_good_invalid_coin
        public void T10_good_invalid_coin()
        {
            int buttonNum = 0;
            //Setting up the input & creating vending machine vm0
            int[] coinKinds = new int[4] { 5, 10, 25, 100 };   //coin kinds from the script
            VendingMachine vm0 = new VendingMachine(coinKinds, 1, 10, 10, 10);     //create a new vending machine vm0
            new VendingMachineLogic(vm0);      //create a new machine logic		
            //Configure vending machine
            List<String> popNames = new List<String>() { "stuff" };
            List<int> popPrices = new List<int>() { 140 };
            vm0.Configure(popNames, popPrices);
            //Load coins
            vm0.CoinRacks[0].LoadCoins(new List<Coin> { new Coin(5) });     //one 5 cent 
            vm0.CoinRacks[1].LoadCoins(new List<Coin>() { new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10) });    //5 10 cent
            vm0.CoinRacks[2].LoadCoins(new List<Coin>() { new Coin(25) });  //two 25 cents
            vm0.CoinRacks[3].LoadCoins(new List<Coin> { new Coin(100) });  //0 100 cents
            //Load pops
            vm0.PopCanRacks[0].LoadPops(new List<PopCan>() { new PopCan("stuff") });   // 1 stuff
            //Insert coins
            vm0.CoinSlot.AddCoin(new Coin(1));
            vm0.CoinSlot.AddCoin(new Coin(139));
            //Press the button 0
            vm0.SelectionButtons[buttonNum].Press();
            //Extract from the delivery chute
            var insideDeliveryChute = vm0.DeliveryChute.RemoveItems();
            //Check delivery
            int expectedChanges = 140; //Check coin delivery
            Assert.IsTrue(checkCoinDelivery(insideDeliveryChute, expectedChanges));
            var expectedPops = new List<PopCan>() {  };//Check pop delivery
            Assert.IsTrue(checkPopDelivery(insideDeliveryChute, expectedPops));
            //Check unload
            var storedContents = new VendingMachineStoredContents();
            int expectedCoinRackValue = 190;
            Assert.IsTrue(checkUnload(vm0, storedContents, expectedCoinRackValue));
            //check tear down
            int expectedStorageBinValue = 0;
            var popsInRacks = new List<List<PopCan>>()
            { new List<PopCan>() {new PopCan("stuff") }};
            Assert.IsTrue(checkTearDown(storedContents, expectedStorageBinValue, popsInRacks));
        }
        [TestMethod]
        //T11_good_extract_before_sale_comlpete
        public void T11_good_extract_before_sale_comlpete()
        {
            int buttonNum = 0;
            int buttonNum2 = 2;
            //Setting up the input & creating vending machine vm0
            int[] coinKinds = new int[4] { 100, 5, 25, 10 };   //coin kinds from the script
            VendingMachine vm0 = new VendingMachine(coinKinds, 3, 10, 10, 10);     //create a new vending machine vm0
            new VendingMachineLogic(vm0);      //create a new machine logic		
            //Configure vending machine
            List<String> popNames = new List<String>() { "A", "B", "C" };
            List<int> popPrices = new List<int>() { 5, 10, 25 };
            vm0.Configure(popNames, popPrices);
            //Load coins
            vm0.CoinRacks[0].LoadCoins(new List<Coin>(new Coin[] { }));  //0 100 cents
            vm0.CoinRacks[1].LoadCoins(new List<Coin>() { new Coin(5) });     //one 5 cent 
            vm0.CoinRacks[2].LoadCoins(new List<Coin>() { new Coin(25), new Coin(25) });  //two 25 cents
            vm0.CoinRacks[3].LoadCoins(new List<Coin>() { new Coin(10) });    //one 10 cent
            //Load pops
            vm0.PopCanRacks[0].LoadPops(new List<PopCan>() { new PopCan("Coke") });   // 1 coke
            vm0.PopCanRacks[1].LoadPops(new List<PopCan>() { new PopCan("water") });  // 1 water
            vm0.PopCanRacks[2].LoadPops(new List<PopCan>() { new PopCan("stuff") });  // 1 stuff
            //Configure
            List<String> popNames1 = new List<String>() { "Coke", "water", "stuff" };
            List<int> popPrices1 = new List<int>() { 250, 250, 205 };
            vm0.Configure(popNames1, popPrices1);
            //Press the button 0
            vm0.SelectionButtons[buttonNum].Press();
            //Extract from the delivery chute
            var insideDeliveryChute0 = vm0.DeliveryChute.RemoveItems();
            //Check delivery
            int expectedChanges0 = 0; //Check coin delivery
            Assert.IsTrue(checkCoinDelivery(insideDeliveryChute0, expectedChanges0));
            //Insert coins
            vm0.CoinSlot.AddCoin(new Coin(100));
            vm0.CoinSlot.AddCoin(new Coin(100));
            vm0.CoinSlot.AddCoin(new Coin(100));
            //Extract from delivery chute
            var insideDeliveryChute2 = vm0.DeliveryChute.RemoveItems();
            int expectedChanges2 = 0; //Check coin delivery
            Assert.IsTrue(checkCoinDelivery(insideDeliveryChute2, expectedChanges2));
            //Check unload
           var storedContents2 = new VendingMachineStoredContents();
            int expectedCoinRackValue2 = 65;
            Assert.IsTrue(checkUnload(vm0, storedContents2, expectedCoinRackValue2));
            int expectedStorageBinValue2 = 0;
            var popsInRacks2 = new List<List<PopCan>>()
            { new List<PopCan>() {new PopCan("Coke") }, new List<PopCan>() {new PopCan("water")}, new List<PopCan>() {new PopCan("stuff")} };
            Assert.IsTrue(checkTearDown(storedContents2, expectedStorageBinValue2, popsInRacks2));
            //Load coins
            vm0.CoinRacks[0].LoadCoins(new List<Coin>(new Coin[] { }));  //0 100 cents
            vm0.CoinRacks[1].LoadCoins(new List<Coin>() { new Coin(5) });     //one 5 cent 
            vm0.CoinRacks[2].LoadCoins(new List<Coin>() { new Coin(25), new Coin(25) });  //two 25 cents
            vm0.CoinRacks[3].LoadCoins(new List<Coin>() { new Coin(10) });    //one 10 cent
            //Load pops
            vm0.PopCanRacks[0].LoadPops(new List<PopCan>() { new PopCan("Coke") });   // 1 coke
            vm0.PopCanRacks[1].LoadPops(new List<PopCan>() { new PopCan("water") });  // 1 water
            vm0.PopCanRacks[2].LoadPops(new List<PopCan>() { new PopCan("stuff") });  // 1 stuff
            //press the button 0
            vm0.SelectionButtons[0].Press();
            //Extract from delivery chute
            var insideDeliveryChute3 = vm0.DeliveryChute.RemoveItems();
            int expectedChanges3 = 50; //Check coin delivery
            Assert.IsTrue(checkCoinDelivery(insideDeliveryChute3, expectedChanges3));
            var expectedPops3 = new List<PopCan>() { new PopCan("Coke") };//Check pop delivery
            Assert.IsTrue(checkPopDelivery(insideDeliveryChute3, expectedPops3));
            //Check unload
            var storedContents3 = new VendingMachineStoredContents();
            int expectedCoinRackValue3 = 315;
            Assert.IsTrue(checkUnload(vm0, storedContents3, expectedCoinRackValue3));
            //Check tear dowm
            int expectedStorageBinValue3 = 0;
            var popsInRacks3 = new List<List<PopCan>>()
            { new List<PopCan>() { }, new List<PopCan>() {new PopCan("water")},new List<PopCan>() {new PopCan("stuff")}};
            Assert.IsTrue(checkTearDown(storedContents3, expectedStorageBinValue3, popsInRacks3));
            //create a new vending machine vm1
            int[] coinKinds1 = new int[4] { 100, 5, 25, 10 };   //coin kinds from the script
            VendingMachine vm1 = new VendingMachine(coinKinds1, 3, 10, 10, 10);     //create a new vending machine vm0
            new VendingMachineLogic(vm1);      //create a new machine logic		
            //Configure vending machine
            List<String> popNames4 = new List<String>() { "Coke", "water", "stuff" };
            List<int> popPrices4 = new List<int>() { 250, 250, 205};
            vm1.Configure(popNames4, popPrices4);
            List<String> popNames5 = new List<String>() { "A", "B", "C" };
            List<int> popPrices5 = new List<int>() { 5, 10, 25 };
            vm1.Configure(popNames5, popPrices5);
            //Check unload
            var storedContents4= new VendingMachineStoredContents();
            int expectedCoinRackValue4 = 0;
            Assert.IsTrue(checkUnload(vm0, storedContents4, expectedCoinRackValue4));
            //Check tear dowm
            int expectedStorageBinValue4 = 0;
            var popsInRacks4 = new List<List<PopCan>>()
            { new List<PopCan>() { }, new List<PopCan>() { },new List<PopCan>() { }};
            Assert.IsTrue(checkTearDown(storedContents4, expectedStorageBinValue4, popsInRacks4));
            vm1.CoinRacks[0].LoadCoins(new List<Coin>(new Coin[] { }));  //0 100 cents
            vm1.CoinRacks[1].LoadCoins(new List<Coin>() { new Coin(5) });     //one 5 cent 
            vm1.CoinRacks[2].LoadCoins(new List<Coin>() { new Coin(25), new Coin(25) });  //two 25 cents
            vm1.CoinRacks[3].LoadCoins(new List<Coin>() { new Coin(10) });    //one 10 cent
            //Load pops
            vm1.PopCanRacks[0].LoadPops(new List<PopCan>() { new PopCan("A") });   // 1 coke
            vm1.PopCanRacks[1].LoadPops(new List<PopCan>() { new PopCan("B") });  // 1 water
            vm1.PopCanRacks[2].LoadPops(new List<PopCan>() { new PopCan("C") });  // 1 stuff
            //Insert coins
            vm1.CoinSlot.AddCoin(new Coin(10));
            vm1.CoinSlot.AddCoin(new Coin(5));
            vm1.CoinSlot.AddCoin(new Coin(10));
            //Press the button 2
            vm1.SelectionButtons[buttonNum2].Press();
            //Extract from the delivery chute
            var insideDeliveryChute6 = vm1.DeliveryChute.RemoveItems();
            //Check delivery
            int expectedChanges6 = 0; //Check coin delivery
            Assert.IsTrue(checkCoinDelivery(insideDeliveryChute6, expectedChanges6));
           var expectedPops6 = new List<PopCan>() { new PopCan("C") };//Check pop delivery
            Assert.IsTrue(checkPopDelivery(insideDeliveryChute6, expectedPops6));
            //Check unload
            var storedContents6 = new VendingMachineStoredContents();
            int expectedCoinRackValue6 = 90;
            Assert.IsTrue(checkUnload(vm1, storedContents6, expectedCoinRackValue6));
            //Check tear down
            int expectedStorageBinValue6 = 0;
            var popsInRacks6 = new List<List<PopCan>>()
            { new List<PopCan>() { new PopCan("A")}, new List<PopCan>() {new PopCan("B")},new List<PopCan>() {}};
            Assert.IsTrue(checkTearDown(storedContents6, expectedStorageBinValue6, popsInRacks6));
        }

        [TestMethod]
        //T12_good_approximate_change_with_credit
        public void T12_good_approximate_change_with_credit()
        {
            int buttonNum = 0;
            //Setting up the input & creating vending machine vm0
            int[] coinKinds = new int[4] { 5, 10, 25, 100 };   //coin kinds from the script
            VendingMachine vm0 = new VendingMachine(coinKinds, 1, 10, 10, 10);     //create a new vending machine vm0
            new VendingMachineLogic(vm0);      //create a new machine logic		
            //Configure vending machine
            List<String> popNames1 = new List<String>() { "stuff" };
            List<int> popPrices1 = new List<int>() { 140 };
            vm0.Configure(popNames1, popPrices1);
            //Load coins
            vm0.CoinRacks[0].LoadCoins(new List<Coin>(new Coin[] { }));     //one 5 cent 
            vm0.CoinRacks[1].LoadCoins(new List<Coin>() { new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10) });    //5 10 cent
            vm0.CoinRacks[2].LoadCoins(new List<Coin>() { new Coin(25) });  //two 25 cents
            vm0.CoinRacks[3].LoadCoins(new List<Coin>() { new Coin(100) });  //0 100 cents
            //Load pops
            vm0.PopCanRacks[0].LoadPops(new List<PopCan>() { new PopCan("stuff") });   // 1 stuff
            //Insert coins
            vm0.CoinSlot.AddCoin(new Coin(100));
            vm0.CoinSlot.AddCoin(new Coin(100));
            vm0.CoinSlot.AddCoin(new Coin(100));
            //Press the button 0
            vm0.SelectionButtons[buttonNum].Press();
            //Extract from the delivery chute
            var insideDeliveryChute1 = vm0.DeliveryChute.RemoveItems();
            //Check delivery
            int expectedChanges1 = 155; //Check coin delivery
            Assert.IsTrue(checkCoinDelivery(insideDeliveryChute1, expectedChanges1));
            var expectedPops1 = new List<PopCan>() { new PopCan("stuff") };//Check pop delivery
            Assert.IsTrue(checkPopDelivery(insideDeliveryChute1, expectedPops1));
            //Check unload
            var storedContents1 = new VendingMachineStoredContents();
            int expectedCoinRackValue1 = 320;
            Assert.IsTrue(checkUnload(vm0, storedContents1, expectedCoinRackValue1));
            //Check tear down
            int expectedStorageBinValue1 = 0;
            var popsInRacks1 = new List<List<PopCan>>()
            { new List<PopCan>() {}};
            Assert.IsTrue(checkTearDown(storedContents1, expectedStorageBinValue1, popsInRacks1));
            //Load coins
            vm0.CoinRacks[0].LoadCoins(new List<Coin>() { new Coin(5), new Coin(5), new Coin(5), new Coin(5), new Coin(5), new Coin(5), new Coin(5), new Coin(5), new Coin(5), new Coin(5)});     //one 5 cent 
            vm0.CoinRacks[1].LoadCoins(new List<Coin>() { new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10) });    //5 10 cent
            vm0.CoinRacks[2].LoadCoins(new List<Coin>() { new Coin(25), new Coin(25), new Coin(25), new Coin(25), new Coin(25), new Coin(25), new Coin(25), new Coin(25), new Coin(25), new Coin(25) });  //two 25 cents
            vm0.CoinRacks[3].LoadCoins(new List<Coin>() { new Coin(100), new Coin(100), new Coin(100), new Coin(100), new Coin(100), new Coin(100), new Coin(100), new Coin(100), new Coin(100), new Coin(100) });  //0 100 cents
            //Load pops
            vm0.PopCanRacks[0].LoadPops(new List<PopCan>() { new PopCan("stuff") });   // 1 stuff
            //Insert coin                                                                          //Insert coins
            vm0.CoinSlot.AddCoin(new Coin(25));
            vm0.CoinSlot.AddCoin(new Coin(100));
            vm0.CoinSlot.AddCoin(new Coin(10));
            //Press the button 0
            vm0.SelectionButtons[buttonNum].Press();
            //Extract from the delivery chute
            var insideDeliveryChute2 = vm0.DeliveryChute.RemoveItems();
            //Check delivery
            int expectedChanges2 = 0; //Check coin delivery
            Assert.IsTrue(checkCoinDelivery(insideDeliveryChute2, expectedChanges2));
            var expectedPops2 = new List<PopCan>() { new PopCan("stuff") };//Check pop delivery
            Assert.IsTrue(checkPopDelivery(insideDeliveryChute2, expectedPops2));
           //Check unload
            var storedContents2 = new VendingMachineStoredContents();
            int expectedCoinRackValue2 = 1400;
            Assert.IsTrue(checkUnload(vm0, storedContents2, expectedCoinRackValue2));
            //Check tear down
            int expectedStorageBinValue2 = 135;
            var popsInRacks2 = new List<List<PopCan>>()
            { new List<PopCan>() {}};
            Assert.IsTrue(checkTearDown(storedContents2, expectedStorageBinValue2, popsInRacks2));
        }

        [TestMethod]
        //T13_good_need_to_store_payment
        public void T13_good_need_to_store_payment()
        {
            int buttonNum = 0;
            //Setting up the input & creating vending machine vm0
            int[] coinKinds = new int[4] { 5, 10, 25, 100 };   //coin kinds from the script
            VendingMachine vm0 = new VendingMachine(coinKinds, 1, 10, 10, 10);     //create a new vending machine vm0
            new VendingMachineLogic(vm0);      //create a new machine logic		
            //Configure vending machine
            List<String> popNames1 = new List<String>() { "stuff" };
            List<int> popPrices1 = new List<int>() { 135 };
            vm0.Configure(popNames1, popPrices1);
            //Load coins
            vm0.CoinRacks[0].LoadCoins(new List<Coin>() { new Coin(5), new Coin(5), new Coin(5), new Coin(5), new Coin(5), new Coin(5), new Coin(5), new Coin(5), new Coin(5), new Coin(5) });     //one 5 cent 
            vm0.CoinRacks[1].LoadCoins(new List<Coin>() { new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10), new Coin(10) });    //5 10 cent
            vm0.CoinRacks[2].LoadCoins(new List<Coin>() { new Coin(25), new Coin(25), new Coin(25), new Coin(25), new Coin(25), new Coin(25), new Coin(25), new Coin(25), new Coin(25), new Coin(25) });  //two 25 cents
            vm0.CoinRacks[3].LoadCoins(new List<Coin>() { new Coin(100), new Coin(100), new Coin(100), new Coin(100), new Coin(100), new Coin(100), new Coin(100), new Coin(100), new Coin(100), new Coin(100) });  //0 100 cents
            //Load pops
            vm0.PopCanRacks[0].LoadPops(new List<PopCan>() { new PopCan("stuff") });   // 1 stuff
            //Insert coin                                                                          //Insert coins
            vm0.CoinSlot.AddCoin(new Coin(25));
            vm0.CoinSlot.AddCoin(new Coin(100));
            vm0.CoinSlot.AddCoin(new Coin(10));
            //Press the button 0
            vm0.SelectionButtons[buttonNum].Press();
            //Extract from the delivery chute
            var insideDeliveryChute = vm0.DeliveryChute.RemoveItems();
            //Check delivery
            int expectedChanges = 0; //Check coin delivery
            Assert.IsTrue(checkCoinDelivery(insideDeliveryChute, expectedChanges));
            var expectedPops = new List<PopCan>() { new PopCan("stuff") };//Check pop delivery
            Assert.IsTrue(checkPopDelivery(insideDeliveryChute, expectedPops));
            //Check unload
            var storedContents = new VendingMachineStoredContents();
            int expectedCoinRackValue = 1400;
            Assert.IsTrue(checkUnload(vm0, storedContents, expectedCoinRackValue));
            //Check tear down
            int expectedStorageBinValue = 135;
            var popsInRacks = new List<List<PopCan>>()
            { new List<PopCan>() {}};
            Assert.IsTrue(checkTearDown(storedContents, expectedStorageBinValue, popsInRacks));
        }

        //=========================================Bad Test
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        //Trying to configure before creating a vending machine
        public void U01_bad_cofigure_before_construct()
        {
            List<VendingMachine> vms = new List<VendingMachine>();

            List<String> pops = new List<String>();
            pops.Add("Coke"); pops.Add("water"); pops.Add("stuff");
            List<int> costs = new List<int>();
            costs.Add(250); costs.Add(250); costs.Add(205);
            vms[5].Configure(pops, costs);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        //A pop with a cost of 0
        public void U02_bad_costs_list()
        {
            List<VendingMachine> vms = new List<VendingMachine>();

            int[] coinKinds = { 5, 10, 25, 100 };
            vms.Add(new VendingMachine(coinKinds, 3, 10, 10, 10));

            List<String> pops = new List<String>();
            pops.Add("Coke"); pops.Add("water"); pops.Add("stuff");
            List<int> costs = new List<int>();
            costs.Add(250); costs.Add(250); costs.Add(0);
            vms[0].Configure(pops, costs);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        //more buttons than pops
        public void U03_bad_name_list()
        {
            List<VendingMachine> vms = new List<VendingMachine>();

            int[] coinKinds = { 5, 10, 25, 100 };
            vms.Add(new VendingMachine(coinKinds, 3, 10, 10, 10));

            List<String> pops = new List<String>();
            pops.Add("Coke"); pops.Add("water");
            List<int> costs = new List<int>();
            costs.Add(250); costs.Add(250);
            vms[0].Configure(pops, costs);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        //More coin types than 
        public void U04_bad_non_unique_denomination()
        //repeated denomination
        {
            List<VendingMachine> vms = new List<VendingMachine>();

            int[] coinKinds = { 1, 1 };
            vms.Add(new VendingMachine(coinKinds, 1, 10, 10, 10));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        //bad coin type
        public void U05_bad_coin_kind()
        {
            List<VendingMachine> vms = new List<VendingMachine>();

            int[] coinKinds = { 0 };
            vms.Add(new VendingMachine(coinKinds, 1, 10, 10, 10));
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        //Tried to press non-existing button
        public void U06_bad_button_number()
        {
            List<VendingMachine> vms = new List<VendingMachine>();

            int[] coinKinds = { 5, 10, 25, 100 };
            vms.Add(new VendingMachine(coinKinds, 3, 1, 1, 1));
            vms[0].SelectionButtons[3].Press();
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        //Tried to press non-existing button
        public void U07_bad_button_number_2()
        {
            List<VendingMachine> vms = new List<VendingMachine>();

            int[] coinKinds = { 5, 10, 25, 100 };
            vms.Add(new VendingMachine(coinKinds, 3, 1, 1, 1));
            vms[0].SelectionButtons[-1].Press();
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        //Tried to press non-existing button
        public void U08_bad_button_number_3()
        {
            List<VendingMachine> vms = new List<VendingMachine>();

            int[] coinKinds = { 5, 10, 25, 100 };
            vms.Add(new VendingMachine(coinKinds, 3, 1, 1, 1));
            vms[0].SelectionButtons[4].Press();
        }

        //============================== methods being called in Good script
        /*The function checkCoinDelivery basically compare the totally coin value inside the 
          delivery chute and the expexted changes. 
        */
        public bool checkCoinDelivery(IDeliverable[] insideDeliveryChute, int expectedChange)
        {
            int changes = sumCoins(insideDeliveryChute);    //sum all the coins inside the delivery chute
            if (changes != expectedChange)//compare
            {
                return false; //if not equal return false
            }
            else return true; //ohterwise return true
        }
        /*The function checkCoinDelivery basically compare the pops inside the delivery chute and the
          expexted changes. 
        */
        public bool checkPopDelivery(IDeliverable[] insideDeliveryChute, List<PopCan> expectedPops)
        {
            var pops = addPopCans(insideDeliveryChute); //get the actual pop inside the delivery chute
            int count = 0; //an variable that record if the actual pop is same as the expected pop
            //check if the items of pop is same as the expected pops, if not the count should >0
            for (int i = 0; i < expectedPops.Count; i++)
            {
                if (!pops[i].Equals(expectedPops[i]))
                {
                    count += 1;//if the actual pops in the list does not match every expected pops, count != 0
                }
            }
            if (pops.Count != expectedPops.Count)//check if the numbers of pop is same as the expected pops
            {
                return false;   //if the number of actual pops and the expected pops does not match, return false
            }
            if (count != 0)//if the items are not the same, return false
            {
                return false;
            }
            else return true;  //otherwise return true.
        }
        /* This function is for check tear down by checking if the actual pops left in the vending machine matches the 
           the expected pop list
        */ 
        public bool checkTearDown(VendingMachineStoredContents storedContents, int expectedStorageBinValue, List<List<PopCan>>popsInRacks)
        {
            int storageBinValue = sumCoins(storedContents.PaymentCoinsInStorageBin.ToArray());//get the coins inside the storage bin
            int count = 0;
            //check if the items of pop is same as the expected pops, if not the count should >0
            for (int i = 0; i < popsInRacks.Count; i++)
            {
                for (int j = 0; j < popsInRacks[i].Count; j++)
                {
                    if (!popsInRacks[i][j].ToString().Equals(storedContents.PopCansInPopCanRacks[i][j].ToString()))
                    {
                        count += 1;//if the actual pops in the list does not match every expected pops, count != 0
                    }
                }
            }
            if (storageBinValue != expectedStorageBinValue)
            {
                return false;//if the actual coin value inside the storage bin does not match the expected, return false
            }
            if (count != 0)
            {
                return false;//if the items are not the same, return false
            }
            else return true;//otherwise return true.
        }
        /* This function is for check unload items by checking if the coin rack value of the vending machine matches the 
          the expected coin rack value.
        */
        public bool checkUnload(VendingMachine vm0, VendingMachineStoredContents storedContents, int expectedCoinRackValue)
        {
            int coinRackValue = 0;//initialize the coin rack value with 0
            foreach (var rack in vm0.CoinRacks)//calculate the coin rack value by adding the value rack by rack.
            {
                storedContents.CoinsInCoinRacks.Add(rack.Unload());
            }
            foreach (var rack in vm0.PopCanRacks)
            {
                storedContents.PopCansInPopCanRacks.Add(rack.Unload());
            }
            storedContents.PaymentCoinsInStorageBin.AddRange(vm0.StorageBin.Unload());
            foreach (var racks in storedContents.CoinsInCoinRacks)
            {
                coinRackValue += sumCoins(racks.ToArray());
            }
            if (coinRackValue != expectedCoinRackValue)//compare the actual coin rack value and the expected coin rack value.
            {
                return false;//if not match return false
            }
            else return true;//otherwise return true
        }

        //This method is to get the sum of all the coins being loaded. 
        public int sumCoins(IDeliverable[] items)
        {
            int sum = 0;
            foreach (var c in items)
            {
                if (c is Coin)
                {
                    sum += ((Coin)c).Value;
                }
            }
            return sum;
        }
        //Get the pop list inside the vending machine
        public List<PopCan> addPopCans(IDeliverable[] items)
        {
            var popcans = new List<PopCan>();
            foreach (var item in items)
            {
                if (item is PopCan)
                {
                    popcans.Add((PopCan)item);
                }
            }
            return popcans;
        }
    }
}


